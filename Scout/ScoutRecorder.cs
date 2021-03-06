﻿using System;
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

namespace Scout
{
    public enum CameraType
    {
        Depth = 1,
        Infrared = 2,
        None = 3
    };

    public class ScoutRecorder
    {
        

        #region CameraSettings

        readonly string FRAME_DATA_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "../../..", "KinectDataBase/KinectDataOutput");
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
            @"../../../KinectDataBase/KinectDataOutput/bodyIndex/BodyIndex.txt",
            @"../../../KinectDataBase/KinectDataOutput/infra/Infrared.txt",
            @"../../../KinectDataBase/KinectDataOutput/longExp/LongExposure.txt",
        };

        private DataBaseController DataBaseController = null;
        private SessionController SessionController = null;
        private TimeSpan timeStamp;
        private KinectSensor _kinectSensor = null;
        private MultiSourceFrameReader _multiSourceFrameReader = null;
        private CoordinateMapper _coordinateMapper = null;

        private InfraredBitmapGenerator _infraredBitmapGenerator = null;
        private DepthBitmapGenerator _depthBitmapGenerator = null;

        private ushort[] _depthData = null;
        private ushort[] _infraredData = null;
        private byte[] _bodyIndexData = null;
        private ushort[] _longExposureData = null;

        private byte[] _depthPixels = null;
        private byte[] _bodyIndexPixels = null;
        private byte[] _infraredPixels = null;

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

        public ScoutRecorder(CameraType cameraType)
        {
            Visualization = cameraType == CameraType.Infrared ? Visualization.Infrared : Visualization.Depth;

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

        public void OpenKinect() // Open Kinect then listen for events and record until asked to close.
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
            int pixelSizeInBytes = depthHeight * depthWidth * 32;

            _infraredBitmapGenerator = new InfraredBitmapGenerator();
            _depthBitmapGenerator = new DepthBitmapGenerator();

            _infraredPixels = new byte[pixelSizeInBytes];
            _depthPixels = new byte[pixelSizeInBytes];
            _bodyIndexPixels = new byte[pixelSizeInBytes];

        }

        private void InitializeFrameData(int depthWidth, int depthHeight)
        {
            int depthArea = depthWidth * depthHeight;

            _bodyData = new Body[_kinectSensor.BodyFrameSource.BodyCount];
            _depthData = new ushort[depthArea];
            _infraredData = new ushort[depthArea];
            _bodyIndexData = new byte[depthArea];
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

        #region EventHandlers

        private void Window_Closed(object sender, ConsoleCancelEventArgs e) // Notify when the Camera is done recording.
        {
            endOperation();
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
                        _depthBitmapGenerator.Update(depthFrame, bodyIndexFrame);

                        _depthData = _depthBitmapGenerator.DepthData;
                        _bodyIndexData = _depthBitmapGenerator.BodyData;


                        depthTask = DataBaseController.GetDepthData(new KeyValuePair<TimeSpan, DepthBitmapGenerator>(timeStamp, _depthBitmapGenerator), token, FRAME_DATA_PATH + filePaths[0]);

                        if (cameraView == CameraType.Depth)
                        {
                            //camera.Source = DepthExtensions.ToLayeredBitmap(depthFrame, bodyIndexFrame);

                            _depthPixels = _depthBitmapGenerator.Pixels;
                            _bodyIndexPixels = _depthBitmapGenerator.HighlightedPixels;
                        }
                    }

                }


                using (var infraredFrame = reference.InfraredFrameReference.AcquireFrame())
                {
                    if (infraredFrame != null)
                    {
                        timeStamp = infraredFrame.RelativeTime;

                        _infraredBitmapGenerator.Update(infraredFrame);

                        _infraredData = _infraredBitmapGenerator.InfraredData;

                        infraredTask = DataBaseController.GetInfraredData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator), token, FRAME_DATA_PATH + filePaths[2]);

                        using (var longExposureFrame = reference.LongExposureInfraredFrameReference.AcquireFrame())
                        {
                            if (longExposureFrame != null)
                            {
                                _longExposureData = _infraredBitmapGenerator.InfraredData;
                                longExpTask = DataBaseController.GetLongExposureData(new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(timeStamp, _infraredBitmapGenerator), token, FRAME_DATA_PATH + filePaths[3]);
                            }
                        }

                        if (cameraView == CameraType.Infrared)
                        {
                            //camera.Source = InfraredExtensions.ToBitmap(infraredFrame);

                            _infraredPixels = _infraredBitmapGenerator.Pixels;
                        }
                    }
                }

                using (var bodyFrame = reference.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        bodyFrame.GetAndRefreshBodyData(_bodyData);
                        timeStamp = bodyFrame.RelativeTime;

                        var bodyData = BodyWrapper.Create(_bodyData
                                                        .Where(b => b.IsTracked)
                                                        .Closest(), _coordinateMapper, this.Visualization);

                        vitruviusTask = SessionController.GetVitruviusSingleData(new KeyValuePair<TimeSpan, BodyWrapper>(timeStamp, bodyData), token);
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
        #endregion
    }
}
