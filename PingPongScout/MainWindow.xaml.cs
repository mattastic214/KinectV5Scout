using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Kinect;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls; // Use for KinectViewer. What does it do?
using System.Threading;
using System.Linq;
using KinectDataBase;
using KinectConstantsBGRA;
using Recorder;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PingPongScout
{
    enum CameraType
    {
        BodyIndex = 1,
        Infrared = 2,
        None = 3
    };
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// DepthFrame
    /// BodyIndexFrame
    /// Gets the highlighted BodyIndexFrame; cancels out table blocking the view of the body.
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Members

        readonly CameraType cameraView = CameraType.BodyIndex;
        //private VideoRecorder videoRecorder = new VideoRecorder();

        Task depthTask = null;
        Task infraredTask = null;
        Task bodyIndexTask = null;
        Task longExpTask = null;
        Task vitruviusTask = null;
        Task bodyBitMapUpdateTask = null;
        private CancellationTokenSource tokenSource = null;

        private DataBaseController DataBaseController = null;
        private PlayerRecorder PlayerRecorder = null;
        private TimeSpan timeStamp;
        private KinectSensor _kinectSensor = null;
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

        delegate void endOperationDelegate();
        private endOperationDelegate endOperation;

        delegate void initializeFrameDataDelegate(int depthWidth, int depthHeight);

        private void endFrameAndKinect()
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
            // constructorOperation += PlayerRecorder.ToggleRecord;
        }

        private void AssignEndOperations()
        {
            endOperation += endFrameAndKinect;
            //endOperation += PlayerRecorder.ToggleRecord;
        }

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();
            WindowState = cameraView == CameraType.None ? WindowState.Minimized : WindowState.Maximized;
        }

        #endregion

        #region EventHandlers

        private void Window_Closed(object sender, EventArgs e)
        {
            endOperation();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                int depthWidth = _kinectSensor.DepthFrameSource.FrameDescription.Width;
                int depthHeight = _kinectSensor.DepthFrameSource.FrameDescription.Height;

                AssignConstructors(depthHeight, depthHeight);
                AssignEndOperations();

                constructorOperation();
            }
        }

        private async void MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame reference = e.FrameReference.AcquireFrame();

            // COORDINATE MAPPING
            if (reference != null)
            {
                tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    if (depthFrame != null)
                    {
                        timeStamp = depthFrame.RelativeTime;
                        _depthBitmapGenerator.Update(depthFrame);

                        depthTask = DataBaseController.GetDepthData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator), token);

                        using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                        {
                            if (bodyIndexFrame != null)
                            {
                                _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                                bodyIndexTask = DataBaseController.GetBodyIndexData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator), token);

                                if (cameraView == CameraType.BodyIndex)
                                {
                                    camera.Source = DepthExtensions.ToBitmap(depthFrame, bodyIndexFrame);
                                }
                            }
                        }
                    }
                }

                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                {
                    if (infraredFrame != null)
                    {
                        timeStamp = infraredFrame.RelativeTime;

                        _infraredBitmapGenerator.Update(infraredFrame);

                        infraredTask = DataBaseController.GetInfraredData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator), token);

                        using (var longExposureFrame = reference.LongExposureInfraredFrameReference.AcquireFrame())
                        {
                            if (longExposureFrame != null)
                            {
                                longExpTask = DataBaseController.GetLongExposureData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator), token);
                            }
                        }

                        if (cameraView == CameraType.Infrared)
                        {
                            camera.Source = InfraredExtensions.ToBitmap(infraredFrame);
                        }
                    }
                }

                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        canvas.Children.Clear();
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        timeStamp = bodyFrame.RelativeTime;

                        //var bodyDataList = _bodyData.Where(b => b.IsTracked)
                        //                            .Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Infrared))
                        //                            .ToList();
                        //vitruviusTask = DataBaseController.GetVitruviusData(new KeyValuePair<TimeSpan, IList<BodyWrapper>>(timeStamp, bodyDataList), token);

                        var bodyData = BodyWrapper.Create(_bodyData
                                                        .Where(b => b.IsTracked)
                                                        .Closest(), _coordinateMapper, Visualization.Infrared);
                        vitruviusTask = DataBaseController.GetVitruviusSingleData(new KeyValuePair<TimeSpan, BodyWrapper>(timeStamp, bodyData), token);
                    }
                }

                try
                {
                    if (depthTask != null) { await depthTask; depthTask = null; }
                    if (infraredTask != null) { await infraredTask; infraredTask = null; }
                    if (bodyBitMapUpdateTask != null) { await bodyBitMapUpdateTask; bodyBitMapUpdateTask = null; }
                    if (bodyIndexTask != null) { await bodyIndexTask; bodyIndexTask = null; }
                    if (longExpTask != null) { await longExpTask; longExpTask = null; }
                    if (vitruviusTask != null) { await vitruviusTask; vitruviusTask = null; }
                }
                catch (OperationCanceledException oce)
                {
                    Console.WriteLine(oce.Message);
                }
            }
        }

        #endregion
    }
}
