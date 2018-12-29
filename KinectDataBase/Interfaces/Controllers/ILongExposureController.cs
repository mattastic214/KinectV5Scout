using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface ILongExposureController
    {
        Task GetLongExposureData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token);

        Task GetLongExposureData(ushort[] longExposureData, CancellationToken token);
    }
}
