﻿using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Threading.Tasks;
using LightBuzz.Vitruvius.Controls; // Use for KinectViewer. What does it do?
using System.Linq;
using KinectFrameController;


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


        private InfraredBitmapGenerator _infraredBitmapGenerator = null;
        private DepthBitmapGenerator _depthBitmapGenerator = null;

        private IList<Body> _bodyData = null;
        private ushort[] _infraredData = null;

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

            // Edit: Switch to routing with controllers (that have hidden implementation details).
            // COORDINATE MAPPING
            if (reference != null )
            {
                TimeSpan timeStamp;
                // Some kind of tracking 1.
                using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    
                    // Does it cancel out the Table Tennis table? Yes!
                    // Generates highlited bodyIndex frame and displays to camera.Source;
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                        timeStamp = depthFrame.RelativeTime;

                        _depthData = _depthBitmapGenerator.DepthData;
                        _bodyIndexData = _depthBitmapGenerator.BodyData;

                        DepthController.GetFrameData(new KeyValuePair<TimeSpan, ushort[]>(timeStamp, _depthData));
                        BodyIndexController.GetFrameData(new KeyValuePair<TimeSpan, byte[]>(timeStamp, _bodyIndexData));                        
                    }
                }

                // Some kind of tracking 2.
                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    
                    if (infraredFrame != null)
                    {
                        timeStamp = infraredFrame.RelativeTime;
                        
                        _infraredBitmapGenerator.Update(infraredFrame);
                        _infraredData = _infraredBitmapGenerator.InfraredData;

                        InfraredController.GetFrameData(new KeyValuePair<TimeSpan, ushort[]>(timeStamp, _infraredData));
                    }

                    if (depthFrame != null)
                    {
                        timeStamp = depthFrame.RelativeTime;
                        _depthBitmapGenerator.Update(depthFrame);
                        _depthData = _depthBitmapGenerator.DepthData;

                        DepthController.GetFrameData(new KeyValuePair<TimeSpan, ushort[]>(timeStamp, _depthData));
                    }
                }

                // Some kind of tracking 3.
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        timeStamp = bodyFrame.RelativeTime;

                        var trackedBodies = _bodyData.Where(b => b.IsTracked)
                                                    .Select(b => new KeyValuePair<TimeSpan, BodyWrapper>(timeStamp, BodyWrapper
                                                    .Create(b, _coordinateMapper, Visualization.Infrared)) );

                        foreach (KeyValuePair<TimeSpan, BodyWrapper> kvp in trackedBodies)
                        {
                            VitruviusBodyController.GetFrameData(kvp);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
