using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace SessionWriter.Interfaces.DataBaseAccess
{
    public interface IBodyWrapperAccess
    {
        Task WriteVitruviusSingle(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token, string path);
    }
}
