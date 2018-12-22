using LightBuzz.Vitruvius;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IDepthDataController
    {
        Task GetDepthData(TimeSpan time, DepthBitmapGenerator depthData, CancellationToken token);
    }
}
