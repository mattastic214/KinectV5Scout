using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;

namespace KinectDataBase
{
    public interface IController
    {
        void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData);

        void GetDepthData(KeyValuePair<TimeSpan, ushort[]> depthData);

        void GetInfraredData(KeyValuePair<TimeSpan, ushort[]> infraredData);

        void GetLongExposureData(KeyValuePair<TimeSpan, ushort[]> longExposureData);

        void GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper);
    }
}
