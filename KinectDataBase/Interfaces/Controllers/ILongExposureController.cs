using LightBuzz.Vitruvius;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface ILongExposureController
    {
        Task GetLongExposureData(TimeSpan time, InfraredBitmapGenerator longExposureData, CancellationToken token);
    }
}
