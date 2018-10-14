using System;
using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;     // Use for KinectViewer. What does it do?
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PingPongScout
{
    /// <summary>
    /// Interaction logic for WindowColor.xaml
    /// BodyFrame
    /// Visualization is set to Color 1920x1080
    /// Tracks joints of body.
    /// Color camera view, provides feedback to user.
    /// </summary>
    public partial class WindowColor : Window
    {

        #region Members

        // private List<Ellipse> _points = new List<Ellipse>();        // Use this instead of new upping Points? No, because anywhere between 1 and 6 people at a time; To hard.

        private KinectSensor _kinectSensor = null;
        // private KinectViewer _kinectViewer = null;

        private BodyFrameReader _bodyFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

        private IList<Body> _bodyData = null;

        #endregion

        #region Delegates

        delegate void constructorOperationDelegate();
        private constructorOperationDelegate constructorOperation;

        delegate void initializeFrameDataDelegate(int depthWidth, int depthHeight);

        #endregion

        #region Initialization and OpenKinect Definitions

        private void OpenKinect()
        {
            _kinectSensor.Open();
            _coordinateMapper = _kinectSensor.CoordinateMapper;
        }

        private void InitializeBodySourceReader()
        {
            _bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();
            _bodyFrameReader.FrameArrived += BodyFrameArrived;
        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];
        }

        #endregion

        public WindowColor()
        {
            InitializeComponent();
        }

        #region EventHandlers

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_bodyFrameReader != null)
            {
                _bodyFrameReader.Dispose();
            }

            if (_kinectSensor != null)
            {
                _kinectSensor.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                int depthWidth = _kinectSensor.DepthFrameSource.FrameDescription.Width;
                int depthHeight = _kinectSensor.DepthFrameSource.FrameDescription.Height;

                constructorOperation += OpenKinect;
                constructorOperation += InitializeBodySourceReader;
                constructorOperation += (() => { InitializeFrameData(depthWidth, depthHeight); });

                constructorOperation();
            }
        }

        private void BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {

                if (bodyFrame != null)
                {
                    canvas.Children.Clear();

                    bodyFrame.GetAndRefreshBodyData(_bodyData);

                    var trackedBodies = _bodyData.Where(b => b.IsTracked)
                                                .Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Color));

                    // Foreach 1: File IO. Body data also being done in WindowInfrared. Move somewhere else.
                    foreach (BodyWrapper bodyWrapper in trackedBodies)
                    {
                        Console.WriteLine("Color available body:");
                        Console.WriteLine("Tracking ID: " + bodyWrapper.TrackingId);
                        Console.WriteLine("Upper Height: " + bodyWrapper.UpperHeight());
                        Console.WriteLine("BodyWrapper Color JSON: " + bodyWrapper.ToJSON());
                        // BodyLean, HandConfidence, etc.
                    }
                    // Ok, stash the _bodyData somewhere to File I/O to work with it later. Yay!

                    // Foreach 2: Draw Joint to canvas
                    foreach (BodyWrapper bodyWrapper in trackedBodies)
                    {
                        if (bodyWrapper.IsTracked)
                        {
                            var trackedJoints = bodyWrapper.TrackedJoints(false);

                            foreach (Joint joint in trackedJoints)
                            {
                                DepthSpacePoint depthPoint = _kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);
                                ColorSpacePoint colorPoint = _kinectSensor.CoordinateMapper.MapCameraPointToColorSpace(joint.Position);

                                Ellipse ellipse = new Ellipse
                                {
                                    Fill = Brushes.Red,
                                    Width = 30,
                                    Height = 30
                                };

                                Canvas.SetLeft(ellipse, (colorPoint.X - ellipse.Width / 2));
                                Canvas.SetTop(ellipse, (colorPoint.Y - ellipse.Width / 2));

                                canvas.Children.Add(ellipse);                                   // Not needed Modularized version refactor.
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
