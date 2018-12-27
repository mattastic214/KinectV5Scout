using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Access
{
    public interface IVitruviusEnumerAccess
    {
        Task WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IEnumerable<Body>> bodyWrapper, CancellationToken token, string path);
    }
}
