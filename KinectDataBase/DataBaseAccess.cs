using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase
{
    public class DataBaseAccess : IDataBaseAccess
    {        
        private readonly object fileLock = new object();
        private readonly object bodyLock = new object();
        StringBuilder sb = new StringBuilder();             // Make StringBuilder an IDisposable

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
                        // var path = "BodyIndex.txt";
                        str.WriteLine("Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
                        //bodyIndexData.Value.Bitmap.Save(basePath);
                    }
                }
            }, token);

            return t;
        }

        public Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, ushort[]> depthData, CancellationToken token, string path)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    {
                        str.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value[i] + "\n");
                        // return depthData.Value != null;
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
                        // return infraredData.Value != null;
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

        // Need to work through major performance issues when 2 people present.

        // Use Node.js Asynchronous concept:
        // 1. Send the task to the computer's file system.
        // 2. Ready to handle the next request
        // 3. When the file system has opened and read the file, the server returns the content to the client.
        // Node.js eliminates the waiting, and simply continues with the next request.
        // Node.js runs single-threaded, non-blocking, asynchronously programming, which is very memory efficient.
        public Task WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapperList, CancellationToken token, string path)
        {
            // IList<BodyWrapper> bodyList = bodyWrapperList.Value;
            BodyWrapper body = bodyWrapperList.Value.Closest();
            TimeSpan time = bodyWrapperList.Key;

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
                        str.Write(sb.ToString());
                        sb.Clear();

                        // Async could work if the sb.Clear(); were made to be Async as well with a group of the write functions.
                        //writer.WriteRawAsync(body.ToJSON());
                        //writer.WriteEndObjectAsync();
                        //str.WriteLineAsync(sb.ToString());
                        //sb.Clear();

                        /* Code to write to same location on disk. Lock Thread. */
                        //foreach (BodyWrapper body in bodyList)
                        //{
                        //    writer.WriteRaw(body.ToJSON());
                        //    writer.WriteEndObject();
                        //    str.WriteLine(sb.ToString());
                        //    sb.Clear();
                        //}
                    }
                }
            }, token);

            return t;
        }

    }
}
