using System;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace KinectDataBase.Interfaces.Service
{
    public interface IApplyKey
    {
        KeyValuePair<TimeSpan, DepthBitmapGenerator> ApplyKeyDepth(TimeSpan key, DepthBitmapGenerator depthBitmapGenerator, CancellationToken token);

        KeyValuePair<TimeSpan, InfraredBitmapGenerator> ApplyKeyInfrared(TimeSpan key, InfraredBitmapGenerator infraredBitmapGenerator, CancellationToken token);

        KeyValuePair<TimeSpan, DepthBitmapGenerator> ApplyKeyBodyIndex(TimeSpan key, DepthBitmapGenerator depthBitmapGenerator, CancellationToken token);

        KeyValuePair<TimeSpan, IList<BodyWrapper>> ApplyKeyVitruvius(TimeSpan key, IList<BodyWrapper> vitruviusCollection, CancellationToken token);

        KeyValuePair<TimeSpan, InfraredBitmapGenerator> ApplyKeyLongExposure(TimeSpan key, InfraredBitmapGenerator infraredBitmapGenerator, CancellationToken token);
    }
}
