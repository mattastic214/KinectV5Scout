using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IController
    {
        void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData);

        void GetDepthData(KeyValuePair<TimeSpan, ushort[]> depthData);

        void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData);

        void GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData);

        void GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper);
    }
}
