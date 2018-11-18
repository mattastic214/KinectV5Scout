using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDataBase
{
    public class DataBaseAccess : IDataBaseAccess
    {
        private string basePath = @"..\..\..\KinectDataBase\KinectDataOutput\";
        private string bodyIndexPath = @"BodyIndex.txt";
        private string depthDataPath = @"DepthData.txt";
        private string infraredDataPath = @"InfraredData.txt";
        private string vitruviusPath = @"Vitruvius.txt";

        private int i;
        private Random random = new Random();

        public bool WriteBodyIndexDataBitMapToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + bodyIndexPath))
            {
                str.WriteLine("Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
                return bodyIndexData.Value.Bitmap.Save(basePath);
            }
        }

        public bool WriteDepthBitMapToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + depthDataPath))
            {
                str.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.DepthData.GetValue(i) + "\n");
                return depthData.Value.Bitmap.Save(basePath);
            }
        }

        public bool WriteInfraredBitMapToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + infraredDataPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.GetValue(i) + "\n");
                return infraredData.Value.Bitmap.Save(basePath);
            }
        }

        public void WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper)
        {
            using (StreamWriter str = File.AppendText(basePath + vitruviusPath))
            {
                var vitBody = bodyWrapper.Value.FirstOrDefault();
                str.WriteLine("Depth data (ushort) Time stamp: " + bodyWrapper.Key + ", Values:");
                // Check if null before writing.
                //str.WriteLine("Tracking ID: " + vitBody.TrackingId);
                //str.WriteLine("Upper Height: " + vitBody.UpperHeight());
                //str.WriteLine("BodyWrapper Infrared JSON: " + vitBody.ToJSON());
                //str.WriteLine("BodyWrapper: " + vitBody);
                //str.WriteLine("JSON Length: " + vitBody.ToJSON().Length + "\n\n");
            }
        }

    }
}
