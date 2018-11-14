using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    public class DepthDataBase : IDepthController
    {
        public void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {
            Random random = new Random();
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.DepthData.GetValue(i));
        }
    }
}
