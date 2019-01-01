using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using KinectDataBase.Interfaces.Controllers;

namespace KinectDataBase
{
    public class DataBaseController : IDepthDataController, IBodyIndexController, IInfraredDataController, ILongExposureController
    {
        private DataBaseAccess DataBaseAccess { get; } = null;
        private Constants DataBaseConstants { get; } = null;

        public DataBaseController(string pathRoot)
        {
            DataBaseAccess = new DataBaseAccess();
            DataBaseConstants = new Constants();

            foreach (string file in DataBaseConstants.dbConstants)
            {
                if (File.Exists(pathRoot + file))
                {
                    File.Delete(pathRoot + file);
                    File.Create(pathRoot + file);
                }
            }
        }

        public DataBaseController(ICollection<string> paths, DataBaseAccess dataBaseAccess)
        {
            DataBaseAccess = dataBaseAccess;

            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    File.Create(path);
                }
            }
        }

        public Task GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string path)
        {
            return DataBaseAccess.WriteDepthDataToDataBase(depthData, token, path);
        }

        public Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string path)
        {
            return DataBaseAccess.WriteBodyIndexDataToDataBase(bodyIndexData, token, path);
        }

        public Task GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token, string path)
        {
            return DataBaseAccess.WriteInfraredDataToDataBase(infraredData, token, path);
        }

        public Task GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token, string path)
        {
            return DataBaseAccess.WriteLongExposureDataToDataBase(longExposureData, token, path);
        }

        public Task GetDepthData(ushort[] depthData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task GetDepthPixels(byte[] depthPixels, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task GetBodyIndexData(byte[] bodyIndexData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task GetBodyIndexPixels(byte[] bodyIndexPixels, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task GetInfrardData(ushort[] infraredData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task GetInfraredPixels(byte[] infraredPixels, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task GetLongExposureData(ushort[] longExposureData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }
    }
}
