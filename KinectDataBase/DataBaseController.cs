using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public class DataBaseController : IController
    {

        public DataBaseAccess DataBaseAccess = null;

        public void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            DataBaseAccess.WriteDepthBitMapToDataBase(bodyIndexData);
        }

        public void GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {
            DataBaseAccess.WriteDepthBitMapToDataBase(depthData);
        }

        public void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            DataBaseAccess.WriteInfraredBitMapToDataBase(infraredData);
        }

        public void GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper)
        {
            DataBaseAccess.WriteVitruviusToDataBase(bodyWrapper);
        }
    }
}
