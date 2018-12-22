using LightBuzz.Vitruvius;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IBodyIndexController
    {
        Task GetBodyIndexData(TimeSpan time, DepthBitmapGenerator bodyIndexData, CancellationToken token);
    }
}
