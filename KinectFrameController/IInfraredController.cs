using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectFrameController
{
    public interface IInfraredController
    {
        void GetFrameData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData);
    }
}
