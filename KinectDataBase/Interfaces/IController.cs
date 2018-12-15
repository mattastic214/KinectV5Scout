using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase
{
    public interface IController
    {
        Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token);

        Task GetDepthData(KeyValuePair<TimeSpan, ushort[]> depthData, CancellationToken token);

        Task GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token);

        Task GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token);

        Task GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper, CancellationToken token);
    }
}
