using System;
using System.Collections.Generic;
using LightBuzz.Vitruvius;
using System.Threading.Tasks;
using System.Threading;

namespace KinectDataBase.Interfaces.Access
{
    public interface IDataBaseAccess
    {
        Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string path);

        Task WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token, string path);

        Task WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token, string path);

        Task WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string path);

    }
}
