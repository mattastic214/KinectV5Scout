using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IJsonSingleController
    {
        Task GetVitruviusSingleData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token, string rootPath);
    }
}
