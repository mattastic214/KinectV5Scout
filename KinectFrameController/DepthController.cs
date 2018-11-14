using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectFrameController
{
    public class DepthController
    {
        public static void GetFrameData(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            if (!depthData.Key.Equals(null) && !depthData.Value.Equals(null))
            {
                Random random = new Random();
                Console.WriteLine("\nDepth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.GetValue(random.Next(0, (int)Math.Pow(2, 16) - 1)));
               
            }            
        }

        public static void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {
            Random random = new Random();
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.DepthData.GetValue(i));
        }

    }
}
