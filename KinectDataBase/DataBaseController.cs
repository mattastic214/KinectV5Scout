using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    public class DataBaseController : IBodyIndexController, IDepthController, IInfraredController, IVitruviusBodyController
    {
        private Random random = new Random();
        private String bodyIndexPath =  @"..\..\..\KinectDataBase\KinectDataOutput\BodyIndex.txt";
        private String depthPath =      @"..\..\..\KinectDataBase\KinectDataOutput\DepthData.txt";
        private String infraredPath =   @"..\..\..\KinectDataBase\KinectDataOutput\InfraredData.txt";
        private String vitruviusPath =  @"..\..\..\KinectDataBase\KinectDataOutput\Vitruvius.txt";

        public void GetBodyIndexData(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);            

            using (StreamWriter str = File.AppendText(bodyIndexPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
            }
        }

        public void GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {            
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(depthPath))
            {
                str.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.DepthData.GetValue(i) + "\n");
            }            
        }

        public void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {            
            int i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(infraredPath))
            {
                str.WriteLine("Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.GetValue(i) + "\n");
            }
        }

        public void GetVitruviusData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper)
        {

            using (StreamWriter str = File.AppendText(vitruviusPath))
            {
                str.WriteLine("Depth data (ushort): " + bodyWrapper.Key + ", Values:");
                str.WriteLine("Tracking ID: " + bodyWrapper.Value.TrackingId);

                str.WriteLine("Time stamp:" + bodyWrapper.Key);

                str.WriteLine("Upper Height: " + bodyWrapper.Value.UpperHeight());
                
                str.WriteLine("BodyWrapper Infrared JSON: " + bodyWrapper.Value.ToJSON());
                str.WriteLine("BodyWrapper: " + bodyWrapper.Value.ToJSON().GetBytes());
                str.WriteLine("JSON Length: " + bodyWrapper.Value.ToJSON().GetBytes().Length + "\n\n");
            }
        }
    }
}
