using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    public class DataBaseController : IBodyIndexController, IDepthController, IInfraredController, IVitruviusBodyController
    {
        private Random random = new Random();

        public void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {    
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("BodyFrameIndex Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i));
        }

        public void GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {            
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.DepthData.GetValue(i));
        }

        public void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {            
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);
            Console.WriteLine("\n" + i);
            Console.WriteLine("Infrared data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.GetValue(i));
        }

        public void GetVitruviusData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper)
        {
            if (!bodyWrapper.Key.Equals(null) && !bodyWrapper.Value.Equals(null))
            {
                // It's a JSON, so it's easy to work with.
                Console.WriteLine("\nInfrared body available:");
                Console.WriteLine("Tracking ID: " + bodyWrapper.Value.TrackingId);
                Console.WriteLine("Upper Height: " + bodyWrapper.Value.UpperHeight());
                Console.WriteLine("Time stamp:" + bodyWrapper.Key);
                Console.WriteLine("BodyWrapper Infrared JSON: " + bodyWrapper.Value.ToJSON());
                Console.WriteLine("BodyWrapper: " + bodyWrapper.Value.ToJSON().GetBytes());
                Console.WriteLine("JSON Length: " + bodyWrapper.Value.ToJSON().GetBytes().Length);
                // BodyLean, HandConfidence, etc.
            }
        }
    }
}
