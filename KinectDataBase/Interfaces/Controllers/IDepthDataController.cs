using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IDepthDataController
    {
        Task GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string rootPath);

        Task GetDepthData(ushort[] depthData, CancellationToken token, string rootPath);

        Task GetDepthPixels(byte[] depthPixels, CancellationToken token, string rootPath);
    }
}
