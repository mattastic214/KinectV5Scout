using Microsoft.Kinect;

namespace BallDetection
{
    public class BallMultiPoint
    {
        public CameraSpacePoint CameraPoint { get; set; }

        public DepthSpacePoint DepthPoint { get; set; }

        public ColorSpacePoint ColorPoint { get; set; }
    }
}
