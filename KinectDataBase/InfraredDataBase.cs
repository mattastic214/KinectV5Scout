using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    public class InfraredDataBase : IInfraredController
    {
        public void GetFrameData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            Random random = new Random();
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("Infrared data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.GetValue(i));
        }
    }
}
