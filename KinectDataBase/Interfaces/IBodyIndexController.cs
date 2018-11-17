using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IBodyIndexController
    {
        void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData);
    }
}
