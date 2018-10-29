using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;
using KinectDataBase;

namespace KinectFrameController
{
    public class BodyIndexController
    {

        public static void GetFrameData(KeyValuePair<TimeSpan, byte[]> bodyIndexData)
        {
            Console.WriteLine("BodyFrameIndex data (byte): " + bodyIndexData + ", Value: " + bodyIndexData.Value);
            BodyIndexDataBase.WriteToDataBase(bodyIndexData);
        }

        public static void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            Console.WriteLine("BodyFrameIndex data (byte): " + bodyIndexData + ", Value: " + bodyIndexData.Value);
        }
    }
}
