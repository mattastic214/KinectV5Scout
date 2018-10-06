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
        private byte[] _bodyIndexData = null;
        
        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            
            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                _kinectSensor.Open();
                _coordinateMapper = _kinectSensor.CoordinateMapper;

                _multiSourceFrameReader = _kinectSensor
                    .OpenMultiSourceFrameReader(FrameSourceTypes.Body | FrameSourceTypes.BodyIndex | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.LongExposureInfrared);

                _multiSourceFrameReader.MultiSourceFrameArrived += MultiSourceFrameArrived;

                int depthWidth = _kinectSensor.DepthFrameSource.FrameDescription.Width;
                int depthHeight = _kinectSensor.DepthFrameSource.FrameDescription.Height;

                _bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];
                _depthData = new ushort[depthWidth * depthHeight];
                _bodyIndexData = new byte[depthWidth * depthHeight];

                _ballDetectionEngine = new BallDetectionEngine(_coordinateMapper, depthWidth, depthHeight);
                _ballDetectionEngine.BallDetected += BallDetectEvent;

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
                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        _body = _bodyData.Where(b => b.IsTracked).FirstOrDefault();

                        if (_body != null)
                        {
                            Console.WriteLine("Found One Person.");
                        }
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

        private void BallDetectEvent(object sender, BallDetectionResult c)
        {

        }

        
    }
}
