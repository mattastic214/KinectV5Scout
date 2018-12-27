﻿using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using KinectDataBase.Interfaces.Controllers;
using Microsoft.Kinect;

namespace KinectDataBase
{
    public class DataBaseController : IDepthDataController, IBodyIndexController, IInfraredDataController, ILongExposureController, IJsonSingleController
    {
        private DataBaseAccess DataBaseAccess { get; } = null;
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

        public Task GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token)
        {
            return DataBaseAccess.WriteDepthDataToDataBase(depthData, token, DataBaseConstants.BasePath + DataBaseConstants.depthDataPath);
        }

        public Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token)
        {
            return DataBaseAccess.WriteBodyIndexDataToDataBase(bodyIndexData, token, DataBaseConstants.BasePath + DataBaseConstants.bodyIndexPath);
        }

        public Task GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token)
        {
            return DataBaseAccess.WriteInfraredDataToDataBase(infraredData, token, DataBaseConstants.BasePath + DataBaseConstants.infraredDataPath);
        }

        public Task GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token)
        {
            return DataBaseAccess.WriteLongExposureDataToDataBase(longExposureData, token, DataBaseConstants.BasePath + DataBaseConstants.longExposureDataPath);
        }

        public Task GetVitruviusSingleData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token)
        {
            return DataBaseAccess.WriteVitruviusSingle(bodyWrapper, token, DataBaseConstants.BasePath + DataBaseConstants.vitruviusPath);
        }
    }
}
