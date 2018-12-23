using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Access
{
    public interface IVitruviusSingleAccess
    {
        Task WriteVitruviusSingle(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token, string path);
    }
}
