using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IInfraredController
    {
        void GetFrameData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData);
    }
}
