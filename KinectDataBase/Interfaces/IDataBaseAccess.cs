using System;
using System.Collections.Generic;
using LightBuzz.Vitruvius;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace KinectDataBase
{
    public interface IDataBaseAccess
    {
        Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, ushort[]> depthData, CancellationToken token);

        Task WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token);

        Task WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token);

        Task WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token);

        Task WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper, CancellationToken token);
    }
}
