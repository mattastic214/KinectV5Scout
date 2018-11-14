using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectFrameController
{
    public interface IDepthController
    {
        void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData);
    }
}
