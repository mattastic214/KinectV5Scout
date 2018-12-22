using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using KinectDataBase.Interfaces.Controllers;

namespace KinectDataBase
{
    public class DataBaseController : IJsonController, IDepthDataController, IBodyIndexController, IInfraredDataController, ILongExposureController
    {
        private DataBaseAccess DataBaseAccess = null;
        private DataBaseService Service = null;
        private DataBaseConstants DataBaseConstants { get; } = null;

        public DataBaseController()
        {
            DataBaseAccess = new DataBaseAccess();
            DataBaseConstants = new DataBaseConstants();

            foreach (string file in DataBaseConstants.dbConstants)
            {
                if (File.Exists(DataBaseConstants.BasePath + file))
                {
                    File.Delete(DataBaseConstants.BasePath + file);
                    File.Create(DataBaseConstants.BasePath + file);
                }
            }
        }

        public Task GetVitruviusData(TimeSpan time, IList<BodyWrapper> bodyWrapper, CancellationToken token)
        {
            var cacheData = Service.ApplyKeyVitruvius(time, bodyWrapper, token);
            return DataBaseAccess.WriteVitruviusToDataBase(cacheData, token, DataBaseConstants.BasePath + DataBaseConstants.vitruviusPath);
        }

        public Task GetDepthData(TimeSpan time, DepthBitmapGenerator depthData, CancellationToken token)
        {
            var cacheData = Service.ApplyKeyDepth(time, depthData, token);
            return DataBaseAccess.WriteDepthDataToDataBase(cacheData, token, DataBaseConstants.BasePath + DataBaseConstants.depthDataPath);
        }

        public Task GetBodyIndexData(TimeSpan time, DepthBitmapGenerator bodyIndexData, CancellationToken token)
        {
            var cacheData = Service.ApplyKeyBodyIndex(time, bodyIndexData, token);
            return DataBaseAccess.WriteBodyIndexDataToDataBase(cacheData, token, DataBaseConstants.BasePath + DataBaseConstants.bodyIndexPath);
        }

        public Task GetInfraredData(TimeSpan time, InfraredBitmapGenerator infraredData, CancellationToken token)
        {
            var cacheData = Service.ApplyKeyInfrared(time, infraredData, token);
            return DataBaseAccess.WriteInfraredDataToDataBase(cacheData, token, DataBaseConstants.BasePath + DataBaseConstants.infraredDataPath);
        }

        public Task GetLongExposureData(TimeSpan time, InfraredBitmapGenerator longExposureData, CancellationToken token)
        {
            var cacheData = Service.ApplyKeyInfrared(time, longExposureData, token);
            return DataBaseAccess.WriteLongExposureDataToDataBase(cacheData, token, DataBaseConstants.BasePath + DataBaseConstants.longExposureDataPath);
        }
    }
}
