using System;
using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;
using System.Linq;
using System.Numerics;
using System.ComponentModel;
using BallDetection;

// Below is from WeightLift Sample:
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PingPongScout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Members
        
        private KinectSensor _kinectSensor = null;
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;
        private BallDetectionEngine _ballDetectionEngine = null;
        private Body _body = null;
        private IList<Body> _bodyData = null;
        private WriteableBitmap _bitmap = null;
        private ushort[] _depthData = null;
        private ushort[] _infraredData = null;
        private ushort[] _longExposureData = null;
        private byte[] _bodyIndexData = null;

        #endregion

        #region Delegates

        delegate void constructorOperationDelegate();
        private constructorOperationDelegate constructorOperation;

        delegate void generateBodyDataDelegate();
        private generateBodyDataDelegate generateBodyData;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();

            constructorOperation += OpenKinect;
            constructorOperation += InitializeMultiSourceReader;
            
            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                this.constructorOperation();
                // Generic start
                //_kinectSensor.Open();
                //_coordinateMapper = _kinectSensor.CoordinateMapper;

                // Setup the multiSourceFrameReader
                //_multiSourceFrameReader = _kinectSensor
                //    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.LongExposureInfrared);
                //_multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;

                // Width and Height from sensor -> to camrera!
                int depthWidth = _kinectSensor.DepthFrameSource.FrameDescription.Width;
                int depthHeight = _kinectSensor.DepthFrameSource.FrameDescription.Height;

                // Get a bitmap with width and height.
                _bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Kinect sensor Data of interest, 3 of 5. 
                _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];
                _infraredData = new ushort[depthWidth * depthHeight];
                _depthData = new ushort[depthWidth * depthHeight];
                _bodyIndexData = new byte[depthWidth * depthHeight];
                _longExposureData = new ushort[depthWidth * depthHeight];

                // Get the BallDetection class set up.
                _ballDetectionEngine = new BallDetectionEngine(_coordinateMapper, depthWidth, depthHeight);
                _ballDetectionEngine.BallDetected += BallDetectEvent;

                // Assign the bitmap to the camera! Of course!!
                camera.Source = _bitmap;
            }
        }

        #endregion

        #region EventHandlers

        public void WindowClosing(object sender, CancelEventArgs e)
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

        private void MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame frame = e.FrameReference.AcquireFrame();

            if (frame != null )
            {
                Console.WriteLine("Initialize all Frames from MultiFrame");

                using (var bodyFrame = frame.BodyFrameReference.AcquireFrame())
                {
                    // Placeholder for refactoring.
                    // Handle all logic in another method. Functional Programming?
                    //bodyFrame?.Dispose();

                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);

                        var bodyIEnum = _bodyData.Where(b => b.IsTracked);
                        var otVitBodies = bodyIEnum.Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Infrared));
                        // Ok, you have the bodies you want. What do you want to do with them now?
                    }


                }

                using (var bodyIndexFrame = frame.BodyIndexFrameReference.AcquireFrame())
                {

                }

                using (var depthFrame = frame.DepthFrameReference.AcquireFrame())
                {

                }

                using (var infraredFrame = frame.InfraredFrameReference.AcquireFrame())
                {

                }

                using (var longExposureFrame = frame.LongExposureInfraredFrameReference.AcquireFrame())
                {

                }                               
            }

        }

        #endregion

        private void BallDetectEvent(object sender, BallDetectionResult e)
        {

        }

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
    }
}
