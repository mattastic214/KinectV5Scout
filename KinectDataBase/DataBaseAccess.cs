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
        private string longExposureDataPath = @"LongExposureData.txt";
        private string vitruviusPath = @"Vitruvius.txt";

        private int i;
        private Random random = new Random();

        public bool WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + bodyIndexPath))
            {
                str.WriteLine("Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
                return bodyIndexData.Value.Bitmap.Save(basePath);
            }
        }

        public bool WriteDepthDataToDataBase(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + depthDataPath))
            {
                str.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value[i] + "\n");
                return depthData.Value != null;
            }
        }

        public bool WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, ushort[]> infraredData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + infraredDataPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value[i] + "\n");
                return infraredData.Value != null;
            }
        }

        public bool WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, ushort[]> longExposureData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + longExposureDataPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + longExposureData.Key + ", Value: " + longExposureData.Value[i] + "\n");
                return longExposureData.Value != null;
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
