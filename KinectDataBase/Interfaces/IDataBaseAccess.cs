using System;
using System.Collections.Generic;
using LightBuzz.Vitruvius;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDataBase
{
    public interface IDataBaseAccess
    {
        bool WriteDepthDataToDataBase(KeyValuePair<TimeSpan, ushort[]> depthData);

        bool WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, ushort[]> infraredData);

        bool WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, ushort[]> longExposureData);

        bool WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData);

        void WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper);
    }
}
