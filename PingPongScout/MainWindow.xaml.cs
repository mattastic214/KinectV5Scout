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
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace PingPongScout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        #region Members

        private List<Ellipse> _points = new List<Ellipse>();

        private KinectSensor _kinectSensor = null;
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;
        private BallDetectionEngine _ballDetectionEngine = null;
        private Body _body = null;
        private IList<Body> _bodyData = null;
        private WriteableBitmap _bitmap = null;
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

        delegate void generateBodyDataDelegate();
        private generateBodyDataDelegate generateBodyData;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();


            _kinectSensor = KinectSensor.GetDefault();

            if (_kinectSensor != null)
            {
                int depthWidth = _kinectSensor.DepthFrameSource.FrameDescription.Width;
                int depthHeight = _kinectSensor.DepthFrameSource.FrameDescription.Height;

                constructorOperation += OpenKinect;
                constructorOperation += InitializeMultiSourceReader;
                constructorOperation += (() => { InitializeBitmap(depthWidth, depthHeight); });
                constructorOperation += (() => { InitializeFrameData(depthWidth, depthHeight); });
                constructorOperation += (() => { InitializeBallDetection(depthWidth, depthHeight); });

                constructorOperation();
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

                // Async 1
                using (var bodyFrame = frame.BodyFrameReference.AcquireFrame())
                {
                    // Placeholder for refactoring.
                    // Handle all logic in another method. Functional Programming?

                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);

                        var bodyIEnum = _bodyData.Where(b => b.IsTracked);
                        var otVitBodies = bodyIEnum.Select(b => BodyWrapper.Create(b, _coordinateMapper, Visualization.Infrared));                        
                        // Ok, you have the bodies you want. What do you want to do with them now?

                        foreach(BodyWrapper bodyWrapper in otVitBodies)
                        {
                            if(bodyWrapper.IsTracked)
                            {
                                Console.WriteLine("Got bodyWrapper.");
                                var joints = bodyWrapper.Joints;

                                var jointsAll = joints.Values;

                                var trackedJoints = jointsAll.Where(j => j.TrackingState != TrackingState.NotTracked);
                                
                                // Using Infrared, not depth hmmmm....
                                foreach(Joint joint in trackedJoints)
                                {
                                    Console.WriteLine("Position of " + joint.JointType.ToString() + ": X=" + joint.Position.X + " Y=" + joint.Position.Y + " Z=" + joint.Position.Z);
                                }
                            }                            
                        }
                    }
                }


                // Async 2
                using (var bodyIndexFrame = frame.BodyIndexFrameReference.AcquireFrame())
                using (var depthFrame = frame.DepthFrameReference.AcquireFrame())
                {
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);
                        
                        // Pick one.
                        _bitmap = depthFrame.ToBitmap(bodyIndexFrame);
                        var bitmapDepth = depthFrame.ToBitmap(bodyIndexFrame);
                        Console.WriteLine("Depth and BodyIndex Present");
                    }

                }
                
                // Async 3
                using (var infraredFrame = frame.InfraredFrameReference.AcquireFrame())
                {
                    // A job for Async?

                    if (infraredFrame != null)
                    {
                        Console.WriteLine("Infrared Present");
                        _infraredBitmapGenerator.Update(infraredFrame);
                        var bitmapInfrared = infraredFrame.ToBitmap();
                    }                    
                }

                // Async 4
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

        private void InitializeBitmap(int depthWidth, int depthHeight)
        {
            _bitmap = new WriteableBitmap(depthWidth, depthHeight, 96.0, 96.0, PixelFormats.Bgr32, null);
            camera.Source = _bitmap;

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

        private void InitializeBallDetection(int depthWidth, int depthHeight)
        {
            _ballDetectionEngine = new BallDetectionEngine(_coordinateMapper, depthWidth, depthHeight);
            _ballDetectionEngine.BallDetected += BallDetectEvent;
        }
    }
}
