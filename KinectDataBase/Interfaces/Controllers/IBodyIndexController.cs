using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IBodyIndexController
    {
        Task GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string rootPath);

        Task GetBodyIndexData(byte[] bodyIndexData, CancellationToken token, string rootPath);

        Task GetBodyIndexPixels(byte[] bodyIndexPixels, CancellationToken token, string rootPath);
    }
}
