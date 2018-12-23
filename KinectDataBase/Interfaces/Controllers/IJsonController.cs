using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IJsonController
    {
        Task GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper, CancellationToken token);
    }
}
