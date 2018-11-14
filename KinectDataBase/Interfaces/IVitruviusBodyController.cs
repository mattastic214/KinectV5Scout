using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IVitruviusBodyController
    {
        void GetFrameData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper);
    }
}
