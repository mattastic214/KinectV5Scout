using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectFrameController
{
    public interface IBodyIndexController
    {
        void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData);
    }
}
