using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase
{
    public class DataBaseController : IController
    {
        public DataBaseAccess DataBaseAccess = null;

        public DataBaseController()
        {
            DataBaseAccess = new DataBaseAccess();
        }

        public Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token)
        {
            return DataBaseAccess.WriteBodyIndexDataToDataBase(bodyIndexData, token);
        }

        public Task GetDepthData(KeyValuePair<TimeSpan, ushort[]> depthData, CancellationToken token)
        {
            return DataBaseAccess.WriteDepthDataToDataBase(depthData, token);
        }

        public Task GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token)
        {
            return DataBaseAccess.WriteInfraredDataToDataBase(infraredData, token);
        }

        public Task GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token)
        {
            return DataBaseAccess.WriteLongExposureDataToDataBase(longExposureData, token);
        }

        public Task GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper, CancellationToken token)
        {
            return DataBaseAccess.WriteVitruviusToDataBase(bodyWrapper, token);
        }
    }
}
