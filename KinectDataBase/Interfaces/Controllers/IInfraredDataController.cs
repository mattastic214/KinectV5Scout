using LightBuzz.Vitruvius;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IInfraredDataController
    {
        Task GetInfraredData(TimeSpan time, InfraredBitmapGenerator infraredData, CancellationToken token);
    }
}
