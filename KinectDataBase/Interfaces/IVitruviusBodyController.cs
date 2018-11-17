using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IVitruviusBodyController
    {
        void GetVitruviusData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper);
    }
}
