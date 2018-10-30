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
    public class InfraredController
    {
        public static void GetFrameData(KeyValuePair<TimeSpan, ushort[]> infraredData)
        {
            Random random = new Random();

            if (!infraredData.Key.Equals(null) && !infraredData.Value.Equals(null))
            {
                int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
                Console.WriteLine("\n" + i);
                Console.WriteLine("Infrared data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.GetValue(i));
                InfraredDataBase.WriteToDataBase(infraredData);
            }            
        }

        public static void GetFrameData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            Random random = new Random();
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("Infrared data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.GetValue(i));
        }

    }
}
