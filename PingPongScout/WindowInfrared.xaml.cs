using System;
using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;     // Use for KinectViewer. What does it do?
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PingPongScout
{
    /// <summary>
    /// Interaction logic for WindowInfrared.xaml
    /// InfraredFrame
    /// BodyFrame
    /// Visualization is set to Infrared 512x424
    /// Tracks joints of body.
    /// Detects image and location of ball. (Yet to be implemented) [Too much for one class? Determine distance from Ball to Player Hand? hmmm...] Back to MultiSourceFrameReader...
    /// </summary>
    public partial class WindowInfrared : Window
    {

        #region Members

        //private List<Ellipse> _points = new List<Ellipse>();        // Use this instead of new upping Points?

        private KinectSensor _kinectSensor = null;
        //private KinectViewer _kinectViewer = null;
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;
        private InfraredBitmapGenerator _infraredBitmapGenerator = null;

        private IList<Body> _bodyData = null;
        private ushort[] _infraredData = null;

        #endregion

        #region Delegates

        delegate void constructorOperationDelegate();
        private constructorOperationDelegate constructorOperation;

        #endregion

        #region Initialization and OpenKinect Definitions

        private void OpenKinect()
        {
            _kinectSensor.Open();
            _coordinateMapper = _kinectSensor.CoordinateMapper;
        }

        private void InitializeMultiSourceReader()
        {
            _multiSourceFrameReader = _kinectSensor
                    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.Infrared);

            _multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;
        }

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            _infraredBitmapGenerator = new InfraredBitmapGenerator();
            camera.Source = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);          
        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];
            _infraredData = new ushort[depthArea];
        }

        #endregion

        public WindowInfrared()
        {
            InitializeComponent();
        }

        #region EventHandlers

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_multiSourceFrameReader != null)
            {
                _multiSourceFrameReader.Dispose();
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
                constructorOperation += InitializeMultiSourceReader;
                constructorOperation += (() => { InitializeBitmap(depthWidth, depthHeight); });
                constructorOperation += (() => { InitializeFrameData(depthWidth, depthHeight); });

                constructorOperation();
            }
        }

        private void MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame reference = e.FrameReference.AcquireFrame();

            // COORDINATE MAPPING
            if (reference != null)
            {
                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {

                    if (infraredFrame != null)
                    {
                        _infraredBitmapGenerator.Update(infraredFrame);                        

                        _infraredData = _infraredBitmapGenerator.InfraredData;
                        camera.Source = _infraredBitmapGenerator.Bitmap;             // Looks like night vision.                        
                    }

                    if (bodyFrame != null)
                    {
                        canvas.Children.Clear();
                        bodyFrame.GetAndRefreshBodyData(_bodyData);

                        var trackedBodies = _bodyData.Where(b => b.IsTracked)
                                                    .Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Infrared));

                        // Foreach 1: File IO. Body data also Being done in WindowColor, move somewhere else.
                        foreach (BodyWrapper bodyWrapper in trackedBodies)
                        {
                            Console.WriteLine("Infrared available body:");
                            Console.WriteLine("Tracking ID: " + bodyWrapper.TrackingId);
                            Console.WriteLine("Upper Height: " + bodyWrapper.UpperHeight());
                            // BodyLean, HandConfidence, etc.
                        }
                        // Ok, stash the _bodyData somewhere to File I/O to work with it later. Yay!

                        // Foreach 2: Draw Joint to canvas
                        foreach (BodyWrapper bodyWrapper in trackedBodies)     // There can only be 4 in our case. 6 is most Kinect supports. It's ok.
                        {
                            if (bodyWrapper.IsTracked)
                            {                                                                
                                var trackedJoints = bodyWrapper.TrackedJoints(false);

                                foreach (Joint joint in trackedJoints)
                                {
                                    DepthSpacePoint depthPoint = _kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(joint.Position);

                                    Ellipse ellipse = new Ellipse       // Use List of Ellipses here!
                                    {
                                        Fill = Brushes.Red,
                                        Width = 10,
                                        Height = 10
                                    };

                                    Canvas.SetLeft(ellipse, (depthPoint.X - ellipse.Width / 2)); // Use a forech on the ellipse List with this
                                    Canvas.SetTop(ellipse, (depthPoint.Y - ellipse.Width / 2));                                                                      

                                    canvas.Children.Add(ellipse);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
