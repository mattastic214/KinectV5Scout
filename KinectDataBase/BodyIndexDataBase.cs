using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    public class BodyIndexDataBase : IBodyIndexController
    {
        public void GetFrameData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            Random random = new Random();
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("BodyFrameIndex Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i));
        }
    }
}
