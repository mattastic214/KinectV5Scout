using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KinectDataBase.Interfaces.Access;

namespace KinectDataBase
{
    public class DataBaseAccess : IDataBaseAccess, IVitruviusSingleAccess
    {        
        private readonly object fileLock = new object();
        StringBuilder sb = new StringBuilder();

        private int i;
        private Random random = new Random();

        public Task WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string path)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    {
                        str.WriteLine("Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string path)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    {
                        str.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value.DepthData.GetValue(i) + "\n");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    {
                        str.WriteLine("BodyFrameIndex Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.FirstOrDefault() + "\n");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    {
                        str.WriteLine("BodyFrameIndex Depth data (ushort): " + longExposureData.Key + ", Value: " + longExposureData.Value + "\n");
                        return longExposureData.Value != null;
                    }
                }
            }, token);

            return t;
        }

        public Task WriteVitruviusSingle(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token, string path)
        {
            TimeSpan time = bodyWrapper.Key;
            BodyWrapper body = bodyWrapper.Value;

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    using (StringWriter strwtr = new StringWriter(sb))
                    using (JsonTextWriter writer = new JsonTextWriter(strwtr))
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("RelativeTime");
                        writer.WriteValue(time);

                        writer.WritePropertyName("TrackedPlayer");


                        if (body != null)
                        {
                            writer.WriteRaw(body.ToJSON());
                        }

                        writer.WriteEndObject();
                        str.WriteLine(sb.ToString());
                        sb.Clear();
                    }
                }
            }, token);

            return t;
        }
    }
}
