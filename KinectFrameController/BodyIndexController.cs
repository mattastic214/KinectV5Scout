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
            if(!bodyIndexData.Key.Equals(null) && !bodyIndexData.Value.Equals(null))
            {
                Random random = new Random();
                int i = random.Next(0, 255);
                Console.WriteLine("\n" + i);
                Console.WriteLine("BodyFrameIndex Depth data (byte): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.GetValue(i));
                BodyIndexDataBase.WriteToDataBase(bodyIndexData);
            }            
        }

        public static void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            Random random = new Random();
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("BodyFrameIndex Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i));
        }
    }
}
