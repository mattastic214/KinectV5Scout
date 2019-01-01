using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace SessionWriter.Interfaces.API
{
    public interface IBodyWrapperController
    {
        Task GetVitruviusSingleData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token);
    }
}
