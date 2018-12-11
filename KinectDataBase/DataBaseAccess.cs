using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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
        StringBuilder sb = new StringBuilder();

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

        public bool WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + infraredDataPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.FirstOrDefault() + "\n");
                return infraredData.Value != null;
            }
        }

        public bool WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + longExposureDataPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + longExposureData.Key + ", Value: " + longExposureData.Value + "\n");
                return longExposureData.Value != null;
            }
        }

        public void WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapperList)
        {
            using (StreamWriter str = File.AppendText(basePath + vitruviusPath))
            using (StringWriter strwtr = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(strwtr))
            {
                IList<BodyWrapper> bodyList = bodyWrapperList.Value;
                TimeSpan time = bodyWrapperList.Key;

                writer.WriteStartObject();
                writer.WritePropertyName("RelativeTime");
                writer.WriteValue(time);

                writer.WritePropertyName("TrackedPlayers");
                writer.WriteStartArray();

                foreach (BodyWrapper body in bodyList)
                {
                    Console.WriteLine(body.ToJSON());
                    writer.WriteRaw(body.ToJSON());
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
                str.WriteLine(sb.ToString());
                sb.Clear();
            }
        }

    }
}
