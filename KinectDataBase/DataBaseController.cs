using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public class DataBaseController : IController
    {

        public DataBaseAccess DataBaseAccess = null;

        public DataBaseController()
        {
            DataBaseAccess = new DataBaseAccess();
        }

        public void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            DataBaseAccess.WriteBodyIndexDataToDataBase(bodyIndexData);
        }

        public void GetDepthData(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            DataBaseAccess.WriteDepthDataToDataBase(depthData);
        }

        public void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            DataBaseAccess.WriteInfraredDataToDataBase(infraredData);
        }

        public void GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData)
        {
            DataBaseAccess.WriteLongExposureDataToDataBase(longExposureData);
        }

        public void GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper)
        {
            DataBaseAccess.WriteVitruviusToDataBase(bodyWrapper);
        }
    }
}
