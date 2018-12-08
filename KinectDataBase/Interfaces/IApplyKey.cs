using System;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Collections.Generic;

namespace KinectDataBase.Interfaces
{
    public interface IApplyKey
    {
        KeyValuePair<TimeSpan, DepthBitmapGenerator> ApplyKeyDepth(DepthFrame depthFrame);

        KeyValuePair<TimeSpan, InfraredBitmapGenerator> ApplyKeyInfrared(InfraredFrame infraredFrame);

        KeyValuePair<TimeSpan, DepthBitmapGenerator> ApplyKeyBodyIndex(BodyIndexFrame bodyIndexFrame);

        KeyValuePair<TimeSpan, IList<BodyWrapper>> ApplyKeyVitruvius(BodyFrame bodyFrame);

        KeyValuePair<TimeSpan, InfraredBitmapGenerator> ApplyKeyLongExposure(InfraredFrame infraredFrame);
    }
}
