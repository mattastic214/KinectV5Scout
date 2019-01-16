using System;
using System.IO;
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
using SessionWriter;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PingPongScout
{
    enum CameraType
    {
        Depth = 1,
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

        readonly string FRAMEDATAPATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../..", "KinectDataBase/KinectDataOutput");
        readonly CameraType cameraView = CameraType.Depth;
        Visualization Visualization;

        #endregion

        #region Tasks

        Task depthTask = null;
        Task infraredTask = null;
        Task longExpTask = null;
        Task vitruviusTask = null;
        Task quarternionTask = null;
        private CancellationTokenSource tokenSource = null;

        #endregion

        #region Members

        string[] filePaths =
        {
            @"../../../KinectDataBase/KinectDataOutput/depth/Depth.txt",
            @"../../../KinectDataBase/KinectDataOutput/bodyIndex/BodyIndex.png",
            @"../../../KinectDataBase/KinectDataOutput/infra/Infrared.txt",
            @"../../../KinectDataBase/KinectDataOutput/longExp/LongExposure.txt",
        };

        private DataBaseController DataBaseController = null;
        private SessionController SessionController = null;
        private TimeSpan timeStamp;
        private KinectSensor kinectSensor = null;
        private MultiSourceFrameReader multiSourceFrameReader = null;
        private CoordinateMapper coordinateMapper = null;

        private InfraredBitmapGenerator infraredBitmapGenerator = null;
        private DepthBitmapGenerator depthBitmapGenerator = null;

        private ushort[] depthData = null;
        private ushort[] infraredData = null;
        private byte[] bodyIndexData = null;
        private ushort[] longExposureData = null;

        private byte[] depthPixels = null;
        private byte[] bodyIndexPixels = null;
        private byte[] infraredPixels = null;

        private IList<Body> bodyData = null;

        #endregion

        #region Delegates

        delegate void constructorOperationDelegate();
        private constructorOperationDelegate constructorOperation;

        delegate void endOperationDelegate();
        private endOperationDelegate endOperation;

        delegate void initializeFrameDataDelegate(int depthWidth, int depthHeight);

        private void CloseFrameAndKinect()
        {
            if (multiSourceFrameReader != null)
            {
                multiSourceFrameReader.Dispose();
            }

            if (kinectSensor != null)
            {
                kinectSensor.Close();
            }
        }

        #endregion

        #region Constructors

        private void OpenKinect()
        {
            kinectSensor.Open();
            coordinateMapper = kinectSensor.CoordinateMapper;
        }

        private void InitializeMultiSourceReader()
        {
            multiSourceFrameReader = kinectSensor
                    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.LongExposureInfrared);

            multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;
        }

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            int pixelSizeInBytes = depthHeight * depthWidth * 32;

            infraredBitmapGenerator = new InfraredBitmapGenerator();
            depthBitmapGenerator = new DepthBitmapGenerator();

            infraredPixels = new byte[pixelSizeInBytes];
            depthPixels = new byte[pixelSizeInBytes];
            bodyIndexPixels = new byte[pixelSizeInBytes];

        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            bodyData = new Body[kinectSensor.BodyFrameSource.BodyCount];
            depthData = new ushort[depthArea];
            infraredData = new ushort[depthArea];
            bodyIndexData = new byte[depthArea];
        }

        private void InitializeControllers()
        {
            DataBaseController = new DataBaseController(filePaths, new DataBaseAccess());
            SessionController = new SessionController();
        }

        private void WriteJSON()
        {
            JSONWriter.writeJSONSession();
        }

        private void AssignConstructors(int depthWidth, int depthHeight)
        {
            constructorOperation += OpenKinect;
            constructorOperation += InitializeMultiSourceReader;
            constructorOperation += (() => { InitializeBitmap(depthWidth, depthHeight); });
            constructorOperation += (() => { InitializeFrameData(depthWidth, depthHeight); });
            constructorOperation += InitializeControllers;
        }

        private void AssignEndOperations()
        {
            endOperation += CloseFrameAndKinect;
            endOperation += WriteJSON;
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
            kinectSensor = KinectSensor.GetDefault();

            if (kinectSensor != null)
            {
                int depthWidth = kinectSensor.DepthFrameSource.FrameDescription.Width;
                int depthHeight = kinectSensor.DepthFrameSource.FrameDescription.Height;

                AssignConstructors(depthHeight, depthHeight);
                AssignEndOperations();

                constructorOperation();
            }
        }

        private async void MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame reference = e.FrameReference.AcquireFrame();

            if (reference != null)
            {
                tokenSource = new CancellationTokenSource();
                CancellationToken token = tokenSource.Token;

                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                {
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        timeStamp = depthFrame.RelativeTime;
                        depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                        depthData = depthBitmapGenerator.DepthData;
                        bodyIndexData = depthBitmapGenerator.BodyData;


                        depthTask = DataBaseController.GetDepthData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, depthBitmapGenerator), token, FRAMEDATAPATH + filePaths[0]);

                        if (cameraView == CameraType.Depth)
                        {
                            camera.Source = DepthExtensions.ToLayeredBitmap(depthFrame, bodyIndexFrame);

                            depthPixels = depthBitmapGenerator.Pixels;
                            bodyIndexPixels = depthBitmapGenerator.HighlightedPixels;
                        }
                    }

                }


                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                {
                    if (infraredFrame != null)
                    {
                        timeStamp = infraredFrame.RelativeTime;

                        infraredBitmapGenerator.Update(infraredFrame);

                        infraredData = infraredBitmapGenerator.InfraredData;


                        infraredTask = DataBaseController.GetInfraredData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, infraredBitmapGenerator), token, FRAMEDATAPATH + filePaths[2]);

                        using (var longExposureFrame = reference.LongExposureInfraredFrameReference.AcquireFrame())
                        {
                            if (longExposureFrame != null)
                            {
                                longExposureData = infraredBitmapGenerator.InfraredData;
                                longExpTask = DataBaseController.GetLongExposureData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, infraredBitmapGenerator), token, FRAMEDATAPATH + filePaths[3]);
                            }
                        }

                        if (cameraView == CameraType.Infrared)
                        {
                            camera.Source = InfraredExtensions.ToBitmap(infraredFrame);

                            infraredPixels = infraredBitmapGenerator.Pixels;
                        }
                    }
                }

                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(bodyData);
                        timeStamp = bodyFrame.RelativeTime;

                        var wrappedBodyData = BodyWrapper.Create(bodyData
                                                        .Where(b => b.IsTracked)
                                                        .Closest(), coordinateMapper, this.Visualization);

                        vitruviusTask = SessionController.GetVitruviusSingleData(new KeyValuePair<TimeSpan, BodyWrapper>(timeStamp, wrappedBodyData), token);
                    }
                }

                try
                {
                    if (depthTask != null) { await depthTask; depthTask = null; }
                    if (infraredTask != null) { await infraredTask; infraredTask = null; }
                    if (longExpTask != null) { await longExpTask; longExpTask = null; }
                    if (vitruviusTask != null) { await vitruviusTask; vitruviusTask = null; }
                    if (quarternionTask != null) { await quarternionTask; quarternionTask = null; }
                }
                catch (OperationCanceledException oce)
                {
                    Console.WriteLine(oce.Message);
                }
            }
        }



        //private void ScreenshotButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var frame = kinectSensor.DepthFrameSource;
            
        //    // Then wait for the camera to give it one of its frames.
            
        //    using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
        //    {
        //        string photoLoc = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        //        string time = DateTime.Now.ToString("d MMM yyyy hh - mm - ss");
        //        string path = Path.Combine(photoLoc, "Screenshot " + time + ".png");
        //        Console.WriteLine("Path: " + path);

        //        if (depthFrame != null)
        //        {

        //            BitmapEncoder encoder = new PngBitmapEncoder();

        //            depthBitmapGenerator.Update(depthFrame);

        //            var pic = depthBitmapGenerator.Bitmap;

        //            encoder.Frames.Add(BitmapFrame.Create(pic));

        //            try
        //            {
        //                using (FileStream fs = new FileStream(path, FileMode.Create))
        //                {
        //                    encoder.Save(fs);
        //                }
        //                Console.WriteLine("Picture was saved to: " + path);
        //            }
        //            catch (IOException x)
        //            {
        //                Console.WriteLine(x.Message);

        //            }

        //        }
        //        else
        //        {
        //            Console.WriteLine("depthBitmap was null: " + (depthBitmapGenerator.Bitmap == null) + " at " + path);
        //        }
        //    }

               
        //}

        #endregion
    }
}
