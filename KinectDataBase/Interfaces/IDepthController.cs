using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IDepthController
    {
        void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData);
    }
}
