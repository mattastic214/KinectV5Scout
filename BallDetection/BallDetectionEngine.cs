using System;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;


namespace BallDetection
{
    public class BallDetectionEngine
    {

        private DepthSpacePoint[] _depthPoints = null;

        public CoordinateMapper CoordinateMapper { get; set; }

        public int DepthWidth { get; set; }

        public int DepthHeight { get; set; }

        public event EventHandler<BallDetectionResult> BallDetected;

        public BallDetectionEngine(CoordinateMapper coordinateMapper, int depthWidth, int depthHeight)
        {
            CoordinateMapper = coordinateMapper;
            DepthWidth = depthWidth;
            DepthHeight = depthHeight;

            _depthPoints = new DepthSpacePoint[depthWidth * depthHeight];
        }

    }
}
