using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IBodyIndexController
    {
        Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token);

        Task GetBodyIndexData(byte[] bodyIndexData, CancellationToken token);

        Task GetBodyIndexPixels(byte[] bodyIndexPixels, CancellationToken token);
    }
}
