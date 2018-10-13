﻿using System;
using System.Windows;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using LightBuzz.Vitruvius.Controls;
using System.Linq;
using KinectConstantsBGRA;
using System.Numerics;
using System.ComponentModel;

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
    /// Gets the highlighted BodyIndexFrame; cancels out table blocking the view of the body.
    /// </summary>
    public partial class MainWindow : Window
    {
        
        #region Members

        private List<Ellipse> _points = new List<Ellipse>();

        private KinectSensor _kinectSensor = null;
        private KinectViewer _kinectViewer = null;
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

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

        private delegate void generateBodyDataDelegate();
        private generateBodyDataDelegate generateBodyData;

        #endregion

        #region Initialization and OpenKinect Definitions

        private void OpenKinect()
        {
            _kinectSensor.Open();
            _coordinateMapper = _kinectSensor.CoordinateMapper;
            _kinectViewer = new KinectViewer();
            _kinectViewer.InitializeComponent();
            _kinectViewer.Visualization = Visualization.Depth;
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

                // Monitor 1:
                // Async 1
                // Async 2
                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                using (var bodyIndexFrame = reference.BodyIndexFrameReference.AcquireFrame())
                using (var depthFrame = reference.DepthFrameReference.AcquireFrame())
                {
                    
                    // Async 1 The Green Screen. Dose it cancel out the Table Tennis table?
                    // Generates highlited bodyIndex frame and displays to camera.Source
                    if (depthFrame != null && bodyIndexFrame != null)
                    {
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                        _depthData = _depthBitmapGenerator.DepthData;
                        _bodyIndexData = _depthBitmapGenerator.BodyData;
                        _bitmap = _depthBitmapGenerator.HighlightedBitmap;
                        // Add body Data somehow with _bodyData? Does it keep track of 1 - 6 bodies?

                        camera.Source = DepthExtensions.ToBitmap(depthFrame, bodyIndexFrame);       // Looks like the Predator.
                    }
                }
            }
        }

        #endregion
    }
}
