using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using KinectDataBase.Interfaces.Controllers;

namespace KinectDataBase
{
    public class DataBaseController : IDepthDataController, IBodyIndexController, IInfraredDataController, ILongExposureController, IJsonSingleController
    {
        private DataBaseAccess DataBaseAccess { get; } = null;
        private DataBaseConstants DataBaseConstants { get; } = null;

        public DataBaseController(string pathRoot)
        {
            DataBaseAccess = new DataBaseAccess();
            DataBaseConstants = new DataBaseConstants();

            foreach (string file in DataBaseConstants.dbConstants)
            {
                if (File.Exists(pathRoot + file))
                {
                    File.Delete(pathRoot + file);
                    File.Create(pathRoot + file);
                }
            }
        }

        public Task GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string rootPath)
        {
            return DataBaseAccess.WriteDepthDataToDataBase(depthData, token, rootPath + DataBaseConstants.depthDataPath);
        }

        public Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string rootPath)
        {
            return DataBaseAccess.WriteBodyIndexDataToDataBase(bodyIndexData, token, rootPath + DataBaseConstants.bodyIndexPath);
        }

        public Task GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token, string rootPath)
        {
            return DataBaseAccess.WriteInfraredDataToDataBase(infraredData, token, rootPath + DataBaseConstants.infraredDataPath);
        }

        public Task GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token, string rootPath)
        {
            return DataBaseAccess.WriteLongExposureDataToDataBase(longExposureData, token, rootPath + DataBaseConstants.longExposureDataPath);
        }

        public Task GetVitruviusSingleData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token, string rootPath)
        {
            return DataBaseAccess.WriteVitruviusSingle(bodyWrapper, token, rootPath + DataBaseConstants.vitruviusPath);
        }

        public Task GetDepthData(ushort[] depthData, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }

        public Task GetDepthPixels(byte[] depthPixels, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }

        public Task GetBodyIndexData(byte[] bodyIndexData, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }

        public Task GetBodyIndexPixels(byte[] bodyIndexPixels, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }

        public Task GetInfrardData(ushort[] infraredData, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }

        public Task GetInfraredPixels(byte[] infraredPixels, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }

        public Task GetLongExposureData(ushort[] longExposureData, CancellationToken token, string rootPath)
        {
            throw new NotImplementedException();
        }
    }
}
