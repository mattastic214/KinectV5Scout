using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IDepthController
    {
        void GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData);
    }
}
