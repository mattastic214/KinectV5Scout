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

        #region CameraSettings

        readonly string FOLDER_PATH = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../", "video");
        readonly CameraType cameraView = CameraType.BodyIndex;
        VitruviusRecorder _recorder = new VitruviusRecorder();
        Visualization Visualization;

        #endregion

        #region Members

        Task depthTask = null;
        Task infraredTask = null;
        Task bodyIndexTask = null;
        Task longExpTask = null;
        Task vitruviusTask = null;
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

        private void CloseFrameAndKinect()
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

        private void ToggleRecorder()
        {
            if (_recorder.IsRecording)
            {
                _recorder.Stop();
            }
            else
            {
                _recorder.Clear();

                _recorder.Visualization = this.Visualization;
                _recorder.Folder = FOLDER_PATH;
                _recorder.Start();
            }
        }

        private void AssignConstructors(int depthWidth, int depthHeight)
        {
            constructorOperation += OpenKinect;
            constructorOperation += InitializeMultiSourceReader;
            constructorOperation += (() => { InitializeBitmap(depthWidth, depthHeight); });
            constructorOperation += (() => { InitializeFrameData(depthWidth, depthHeight); });
            constructorOperation += InitializeDataAccessController;
            constructorOperation += ToggleRecorder;
        }

        private void AssignEndOperations()
        {
            endOperation += CloseFrameAndKinect;
            endOperation += ToggleRecorder;
        }

        #endregion

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();
            WindowState = cameraView == CameraType.None ? WindowState.Minimized : WindowState.Maximized;
            Visualization = cameraView == CameraType.Infrared ? Visualization.Infrared : Visualization.Depth;
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
            VitruviusFrame recordingFrame = new VitruviusFrame();

            // COORDINATE MAPPING
            if (reference != null)
            {
                tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                // New Task 1:
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                {
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        timeStamp = depthFrame.RelativeTime;
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                        depthTask = DataBaseController.GetDepthData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator), token);

                        bodyIndexTask = DataBaseController.GetBodyIndexData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator), token);

                        if (cameraView == CameraType.BodyIndex)
                        {
                            camera.Source = DepthExtensions.ToBitmap(depthFrame, bodyIndexFrame);
                            recordingFrame.Image = _depthBitmapGenerator.Pixels;
                        }
                    }
                }


                // New Task 2:
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
                            recordingFrame.Image = _infraredBitmapGenerator.Pixels;
                        }
                    }
                }

                // New Task 3:
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        timeStamp = bodyFrame.RelativeTime;


                        var bodyData = BodyWrapper.Create(_bodyData
                                                        .Where(b => b.IsTracked)
                                                        .Closest(), _coordinateMapper, this.Visualization);

                        vitruviusTask = DataBaseController.GetVitruviusSingleData(new KeyValuePair<TimeSpan, BodyWrapper>(timeStamp, bodyData), token);
                        recordingFrame.Body = bodyData;
                    }
                }

                try
                {
                    if (depthTask != null) { await depthTask; depthTask = null; }
                    if (infraredTask != null) { await infraredTask; infraredTask = null; }
                    if (bodyIndexTask != null) { await bodyIndexTask; bodyIndexTask = null; }
                    if (longExpTask != null) { await longExpTask; longExpTask = null; }
                    if (vitruviusTask != null) { await vitruviusTask; vitruviusTask = null; }
                }
                catch (OperationCanceledException oce)
                {
                    Console.WriteLine(oce.Message);
                }

                if (_recorder.IsRecording)
                {
                    _recorder.AddFrame(recordingFrame);
                }
            }
        }

        #endregion
    }
}
