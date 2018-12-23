using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Access
{
    public interface IVitruviusListAccess
    {
        Task WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper, CancellationToken token, string path);
    }
}
