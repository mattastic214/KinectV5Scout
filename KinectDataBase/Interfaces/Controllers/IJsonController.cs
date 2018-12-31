using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IJsonController
    {
        Task GetVitruviusData(KeyValuePair<TimeSpan, IEnumerable<Body>> bodyWrapper, CancellationToken token, string rootPath);
    }
}
