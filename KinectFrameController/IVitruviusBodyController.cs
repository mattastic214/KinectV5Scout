using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectFrameController
{
    public interface IVitruviusBodyController
    {
        void GetFrameData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper);
    }
}
