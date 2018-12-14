using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Threading.Tasks;
using LightBuzz.Vitruvius.Controls; // Use for KinectViewer. What does it do?
using System.Linq;
using KinectDataBase;
using KinectConstantsBGRA;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace PingPongScout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// DepthFrame
    /// BodyIndexFrame
    /// Gets the highlighted BodyIndexFrame; cancels out table blocking the view of the body.
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Members

        readonly CameraType cameraView = CameraType.Skeletal;
        //private VitruviusRecorder VitruviusRecorder = null;

        private DataBaseController DataBaseController = null;
        private TimeSpan timeStamp;
        private KinectSensor _kinectSensor = null;
        //private KinectViewer _kinectViewer = null;                  // What use is this??
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

        private InfraredBitmapGenerator _infraredBitmapGenerator = null;
        private DepthBitmapGenerator _depthBitmapGenerator = null;

        private ushort[] _depthData = null;

        private IList<Body> _bodyData = null;

        #endregion

        #region Delegates

        delegate void constructorOperationDelegate();
        private constructorOperationDelegate constructorOperation;

        delegate void initializeFrameDataDelegate(int depthWidth, int depthHeight);

        #endregion

        #region Constructors

        private void OpenKinect()
        {
            _kinectSensor.Open();
            _coordinateMapper = _kinectSensor.CoordinateMapper;
        }

        private void InitializeMultiSourceReader()
        {
            _multiSourceFrameReader = _kinectSensor
                    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.LongExposureInfrared);

            _multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;
        }

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            _infraredBitmapGenerator = new InfraredBitmapGenerator();
            _depthBitmapGenerator = new DepthBitmapGenerator();
        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];

            _depthData = new ushort[depthWidth * depthHeight];
        }

        private void InitializeDataAccessController()
        {
            DataBaseController = new DataBaseController();
        }

        private void AssignConstructors(int depthWidth, int depthHeight)
        {
            constructorOperation += OpenKinect;
            constructorOperation += InitializeMultiSourceReader;
            constructorOperation += (() => { InitializeBitmap(depthWidth, depthHeight); });
            constructorOperation += (() => { InitializeFrameData(depthWidth, depthHeight); });
            constructorOperation += InitializeDataAccessController;
        }

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();
            WindowState = cameraView == CameraType.None ? WindowState.Minimized : WindowState.Maximized;
            if (cameraView == CameraType.Skeletal)
            {
                Height = 1920;
                Width = 1080;
                grid.Height = 1920;
                grid.Width = 1080; 
            }
        }

        #endregion

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

                AssignConstructors(depthHeight, depthHeight);

                constructorOperation();
            }
        }

        private void MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame reference = e.FrameReference.AcquireFrame();

            // COORDINATE MAPPING
            if (reference != null )
            {                
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    // 1a. Infrared and Object Depth Tracking.
                    if (depthFrame != null)
                    {
                        timeStamp = depthFrame.RelativeTime;
                        _depthBitmapGenerator.Update(depthFrame);

                        _depthData = _depthBitmapGenerator.DepthData;

                        DataBaseController.GetDepthData(new KeyValuePair<TimeSpan, ushort[]>(timeStamp, _depthData));

                        using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                        {
                            if (bodyIndexFrame != null)
                            {
                                _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);
                                DataBaseController.GetBodyIndexData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator));

                                if (cameraView == (int)CameraType.BodyIndex)
                                {
                                    camera.Source = DepthExtensions.ToBitmap(depthFrame, bodyIndexFrame);       // Looks like the Predator.
                                }
                            }
                        }
                    }
                }

                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                {
                    // 1b. Infrared and Object Depth Tracking.
                    // 3. LongExposure Frame Tracking (Infrared)
                    if (infraredFrame != null)
                    {
                        timeStamp = infraredFrame.RelativeTime;

                        _infraredBitmapGenerator.Update(infraredFrame);

                        DataBaseController.GetInfraredData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator));

                        using (var longExposureFrame = reference.LongExposureInfraredFrameReference.AcquireFrame())
                        {
                            if (longExposureFrame != null)
                            {
                                _infraredBitmapGenerator.Update(infraredFrame);

                                DataBaseController.GetLongExposureData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator));
                            }
                        }

                        if (cameraView == CameraType.Infrared)
                        {
                            camera.Source = InfraredExtensions.ToBitmap(infraredFrame);
                        }
                    }
                }

                // 4. Vitruvius Body Wrapper Tracking
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        canvas.Children.Clear();
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        timeStamp = bodyFrame.RelativeTime;

                        var bodyDataList = _bodyData.Where(b => b.IsTracked)
                                                    .Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Infrared))
                                                    .ToList();

                        var trackedBodies = new KeyValuePair<TimeSpan, IList<BodyWrapper>>(timeStamp, bodyDataList);
                        DataBaseController.GetVitruviusData(trackedBodies);

                        if (cameraView == CameraType.Skeletal)
                        {
                            // Foreach 2: Draw Joint to canvas
                            foreach (BodyWrapper bodyWrapper in bodyDataList)
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

                                        canvas.Children.Add(ellipse);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }

    enum CameraType
    {
        BodyIndex,
        Infrared,
        Skeletal,
        None
    };
}
