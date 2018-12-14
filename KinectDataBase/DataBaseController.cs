using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public class DataBaseController : IController
    {
        private string basePath = @"..\..\..\KinectDataBase\KinectDataOutput\";
        private string[] paths = { "Vitruvius.txt", "BodyIndex.txt", "DepthData.txt", "InfraredData.txt", "LongExposure.txt" };

        public DataBaseAccess DataBaseAccess = null;

        public DataBaseController()
        {
            DataBaseAccess = new DataBaseAccess();
        }

        public async void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            await DataBaseAccess.WriteBodyIndexDataToDataBase(bodyIndexData);
        }

        public async void GetDepthData(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            await DataBaseAccess.WriteDepthDataToDataBase(depthData);
        }

        public async void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            await DataBaseAccess.WriteInfraredDataToDataBase(infraredData);
        }

        public async void GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData)
        {
            await DataBaseAccess.WriteLongExposureDataToDataBase(longExposureData);
        }

        public async void GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper)
        {
            await DataBaseAccess.WriteVitruviusToDataBase(bodyWrapper);
        }
    }
}
