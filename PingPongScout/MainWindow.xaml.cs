using System;
using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls; // Use for KinectViewer. What does it do?
using KinectConstantsBGRA;

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

        private KinectSensor _kinectSensor = null;
        //private KinectViewer _kinectViewer = null;                  // What use is this??
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

        private DepthBitmapGenerator _depthBitmapGenerator = null;

        private ushort[] _depthData = null;
        private byte[] _bodyIndexData = null;

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

        private void InitializeMultiSourceReader()
        {
            _multiSourceFrameReader = _kinectSensor
                    .OpenMultiSourceFrameReader(FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth);

            _multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;
        }

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            _depthBitmapGenerator = new DepthBitmapGenerator();
        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            _depthData = new ushort[depthArea];
            _bodyIndexData = new byte[depthArea];
        }

        #endregion

        public MainWindow()
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
            if (reference != null )
            {

                using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    
                    // Dose it cancel out the Table Tennis table? Yes!
                    // Generates highlited bodyIndex frame and displays to camera.Source;
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                        _depthData = _depthBitmapGenerator.DepthData;
                        _bodyIndexData = _depthBitmapGenerator.BodyData;

                        // camera.Source should take the ProjectionInterface result as an argument.
                        // can set any of the running analyzers to be the camera Source.
                        camera.Source = DepthExtensions.ToBitmap(depthFrame, bodyIndexFrame);       // Looks like the Predator. Not needed Modularized version refactor.

                        Console.WriteLine("Depth data: " + _depthData.Length);
                        Console.WriteLine("BodyFrameIndex data: " + _bodyIndexData.Length);
                    }
                }
            }
        }

        #endregion
    }
}
