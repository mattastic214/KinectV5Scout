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
        bool WriteDepthBitMapToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData);

        bool WriteInfraredBitMapToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData);

        bool WriteBodyIndexDataBitMapToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData);

        void WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper);
    }
}
