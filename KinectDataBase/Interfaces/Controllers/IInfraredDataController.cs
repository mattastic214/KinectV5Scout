using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IInfraredDataController
    {
        Task GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token);
    }
}
