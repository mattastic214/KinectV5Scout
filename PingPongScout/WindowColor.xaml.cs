using System;
using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PingPongScout
{
    /// <summary>
    /// Interaction logic for WindowColor.xaml
    /// Tracks joints of body.
    /// Color camera view, provides feedback to user.
    /// </summary>
    public partial class WindowColor : Window
    {

        #region Members

        private List<Ellipse> _points = new List<Ellipse>();        // Use this instead of new upping Points?

        private KinectSensor _kinectSensor = null;
        private KinectViewer _kinectViewer = null;
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

        private Body _body = null;                  // Use this if the body is tracked, right?
        private IList<Body> _bodyData = null;

        private WriteableBitmap _bitmap = null;
        private ColorBitmapGenerator _colorBitmapGenerator = null;
        private InfraredBitmapGenerator _infraredBitmapGenerator = null;
        private DepthBitmapGenerator _depthBitmapGenerator = null;

        private ushort[] _depthData = null;
        private ushort[] _infraredData = null;
        private ushort[] _longExposureData = null;
        private byte[] _bodyIndexData = null;

        #endregion

        #region Delegates

        delegate void constructorOperationDelegate();
        private constructorOperationDelegate constructorOperation;

        delegate void initializeFrameDataDelegate(int depthWidth, int depthHeight);

        private delegate void generateBodyDataDelegate();

        #endregion

        #region Delegate Definitions

        private void OpenKinect()
        {
            _kinectSensor.Open();
            _coordinateMapper = _kinectSensor.CoordinateMapper;
            _kinectViewer = new KinectViewer();
            _kinectViewer.InitializeComponent();
            _kinectViewer.Visualization = Visualization.Color;
        }

        private void InitializeMultiSourceReader()
        {
            _multiSourceFrameReader = _kinectSensor
                    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth | FrameSourceTypes.Color);

            _multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;
        }

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            _bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
            camera.Source = _bitmap;

            _colorBitmapGenerator = new ColorBitmapGenerator();
            _infraredBitmapGenerator = new InfraredBitmapGenerator();
            _depthBitmapGenerator = new DepthBitmapGenerator();
        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];
            _infraredData = new ushort[depthArea];
            _depthData = new ushort[depthArea];
            _bodyIndexData = new byte[depthArea];
            _longExposureData = new ushort[depthArea];
        }

        #endregion

        public WindowColor()
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

                // Async 1
                // Async 2
                // using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                using (var colorFrame = reference.ColorFrameReference.AcquireFrame())
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {

                    if (colorFrame != null)
                    {

                        //_infraredBitmapGenerator.Update(colorFrame);
                        _colorBitmapGenerator.Update(colorFrame);
                        _infraredData = _infraredBitmapGenerator.InfraredData;

                        // _bitmap = _infraredBitmapGenerator.Bitmap;
                        _bitmap = _colorBitmapGenerator.Bitmap;
                        // camera.Source = _bitmap;             // Regular camera picture.    Cancel out to reduce feedback noise to user.                    
                    }

                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);

                        var trackedBodies = _bodyData.Where(b => b.IsTracked)
                                                    .Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Color));

                        foreach (BodyWrapper bodyWrapper in trackedBodies)     // There can only be 4 in our case. 6 is most Kinect supports. It's ok.
                        {
                            if (bodyWrapper.IsTracked)
                            {
                                canvas.Children.Clear();

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

                                    canvas.Children.Add(ellipse);
                                }
                            }
                        }
                    }
                }

                // Reference Vitruvius hand draw demo.
                using (var longExposureFrame = reference.LongExposureInfraredFrameReference.AcquireFrame())
                {

                }
            }
        }

        #endregion
    }
}
