using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KinectDataBase.Interfaces.Service;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    class DataBaseService : IApplyKey
    {
        public KeyValuePair<TimeSpan, DepthBitmapGenerator> ApplyKeyBodyIndex(TimeSpan key, DepthBitmapGenerator depthBitmapGenerator, CancellationToken token)
        {
            var t = Task.Run(() =>
            {
                KeyValuePair<TimeSpan, DepthBitmapGenerator> kvp = new KeyValuePair<TimeSpan, DepthBitmapGenerator>(key, depthBitmapGenerator);

                return kvp;

            }, token);

            return t.Result;
        }

        public KeyValuePair<TimeSpan, DepthBitmapGenerator> ApplyKeyDepth(TimeSpan key, DepthBitmapGenerator depthBitmapGenerator, CancellationToken token)
        {
            var t = Task.Run(() =>
            {
                KeyValuePair<TimeSpan, DepthBitmapGenerator> kvp = new KeyValuePair<TimeSpan, DepthBitmapGenerator>(key, depthBitmapGenerator);

                return kvp;

            }, token);

            return t.Result;
        }

        public KeyValuePair<TimeSpan, InfraredBitmapGenerator> ApplyKeyInfrared(TimeSpan key, InfraredBitmapGenerator infraredBitmapGenerator, CancellationToken token)
        {
            var t = Task.Run(() =>
            {
                KeyValuePair<TimeSpan, InfraredBitmapGenerator> kvp = new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(key, infraredBitmapGenerator);

                return kvp;

            }, token);

            return t.Result;
        }

        public KeyValuePair<TimeSpan, InfraredBitmapGenerator> ApplyKeyLongExposure(TimeSpan key, InfraredBitmapGenerator infraredBitmapGenerator, CancellationToken token)
        {
            var t = Task.Run(() =>
            {
                KeyValuePair<TimeSpan, InfraredBitmapGenerator> kvp = new KeyValuePair<TimeSpan, InfraredBitmapGenerator>(key, infraredBitmapGenerator);

                return kvp;

            }, token);

            return t.Result;
        }

        public KeyValuePair<TimeSpan, IList<BodyWrapper>> ApplyKeyVitruvius(TimeSpan key, IList<BodyWrapper> bodyList, CancellationToken token)
        {
            var t = Task.Run(() =>
            {
                KeyValuePair<TimeSpan, IList<BodyWrapper>> kvp = new KeyValuePair<TimeSpan, IList<BodyWrapper>>(key, bodyList);

                return kvp;

            }, token);

            return t.Result;
        }
    }
}
