using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Threading.Tasks;
using LightBuzz.Vitruvius.Controls; // Use for KinectViewer. What does it do?
using System.Linq;
using KinectDataBase;

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

        private VitruviusRecorder VitruviusRecorder = null;

        private DataBaseController DataBaseController = null;

        private KinectSensor _kinectSensor = null;
        //private KinectViewer _kinectViewer = null;                  // What use is this??
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

        private InfraredBitmapGenerator _infraredBitmapGenerator = null;
        private DepthBitmapGenerator _depthBitmapGenerator = null;

        private ushort[] _depthData = null;
        private ushort[] _infraredData = null;
        private byte[] _bodyIndexData = null;

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
                    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth | FrameSourceTypes.Infrared);

            _multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;
        }

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            _infraredBitmapGenerator = new InfraredBitmapGenerator();
            _depthBitmapGenerator = new DepthBitmapGenerator();
        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];

            _infraredData = new ushort[depthWidth * depthHeight];
            _depthData = new ushort[depthWidth * depthHeight];
            _bodyIndexData = new byte[depthWidth * depthHeight];
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
                TimeSpan timeStamp;

                // Body Index Tracking.
                using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        // Need and object of both Frames and the depth Bitmap Generator.
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);
                        
                        timeStamp = depthFrame.RelativeTime;

                        //depthFrame.CopyFrameDataToArray(_depthData);
                        //bodyIndexFrame.CopyFrameDataToArray(_bodyIndexData);

                        // Needs new Data structure Kvp<TimeSpan, DataContainer>;
                        DataBaseController.GetBodyIndexData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator));
                    }
                }

                // Infrared and Object Depth Tracking.
                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (infraredFrame != null)
                    {
                        // Need an object of the frame and the bitmap generator

                        timeStamp = infraredFrame.RelativeTime;
                        _infraredBitmapGenerator.Update(infraredFrame);

                        //infraredFrame.CopyFrameDataToArray(_infraredData);

                        // Needs new Data structure Kvp<TimeSpan, DataContainer>;
                        DataBaseController.GetInfraredData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator));
                    }

                    if (depthFrame != null)
                    {
                        // Need an object of the frame and the bitmap generator
                        timeStamp = depthFrame.RelativeTime;
                        _depthBitmapGenerator.Update(depthFrame);

                        //depthFrame.CopyFrameDataToArray(_depthData);

                        // Needs new Data structure Kvp<TimeSpan, DataContainer>; 
                        DataBaseController.GetDepthData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator));
                    }
                }

                // Vitruvius Body Wrapper Tracking
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        timeStamp = bodyFrame.RelativeTime;

                        var bodyDataList = _bodyData.Where(b => b.IsTracked)
                                                    .Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Infrared))
                                                    .ToList<BodyWrapper>();

                        var trackedBodies = new KeyValuePair<TimeSpan, IList<BodyWrapper>>(timeStamp, bodyDataList);
                        DataBaseController.GetVitruviusData(trackedBodies);
                    }
                }
            }
        }

        #endregion
    }
}
