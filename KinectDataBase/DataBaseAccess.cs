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
        /**
         * Multithreading problems:
         * The string variables exist in state in the scope of 'this' {KinectDatabase.DatabaseAccess}
         * However, a created thread 'Task' holds access to the variables basePath and 'bodyIndexPath.txt'
         * or whichever file .txt it would need at the time. DataBaseAccess should not control state.
         * State should be passed down from a layer that manages state to lower layers.
         *
        **/
        
        private string basePath = @"..\..\..\KinectDataBase\KinectDataOutput\";
        private string bodyIndexPath = @"BodyIndex.txt";
        private string depthDataPath = @"DepthData.txt";
        private string infraredDataPath = @"InfraredData.txt";
        private string longExposureDataPath = @"LongExposureData.txt";
        private string vitruviusPath = @"Vitruvius.txt";
        private readonly object fileLock = new object();
        StringBuilder sb = new StringBuilder();

        private int i;
        private Random random = new Random();

        public Task WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(basePath + bodyIndexPath))
                    {
                        // var path = "BodyIndex.txt";
                        str.WriteLine("Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
                        bodyIndexData.Value.Bitmap.Save(basePath);
                    }
                }
            });

            return t;
        }

        public Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(basePath + depthDataPath))
                    {
                        str.WriteLine("Depth data (ushort): " + depthData.Key + ", Value: " + depthData.Value[i] + "\n");
                        // return depthData.Value != null;
                    }
                }
            });

            return t;
        }

        public Task WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(basePath + infraredDataPath))
                    {
                        str.WriteLine("BodyFrameIndex Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.FirstOrDefault() + "\n");
                        // return infraredData.Value != null;
                    }
                }
            });

            return t;
        }

        public Task WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(basePath + longExposureDataPath))
                    {
                        str.WriteLine("BodyFrameIndex Depth data (ushort): " + longExposureData.Key + ", Value: " + longExposureData.Value + "\n");
                        return longExposureData.Value != null;
                    }
                }
            });

            return t;
        }

        // Need to work through major performance issues when 2 people present.

        // Use Node.js Asynchronous concept:
        // 1. Send the task to the computer's file system.
        // 2. Ready to handle the next request
        // 3. When the file system has opened and read the file, the server returns the content to the client.
        // Node.js eliminates the waiting, and simply continues with the next request.
        // Node.js runs single-threaded, non-blocking, asynchronously programming, which is very memory efficient.
        public Task WriteVitruviusToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapperList)
        {
            IList<BodyWrapper> bodyList = bodyWrapperList.Value;
            TimeSpan time = bodyWrapperList.Key;

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(basePath + vitruviusPath))
                    using (StringWriter strwtr = new StringWriter(sb))
                    using (JsonTextWriter writer = new JsonTextWriter(strwtr))
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("RelativeTime");
                        writer.WriteValue(time);

                        writer.WritePropertyName("TrackedPlayer");

                        /* Code to write to same location on disk. Lock Thread. */
                        foreach (BodyWrapper body in bodyList)
                        {
                            writer.WriteRaw(body.ToJSON());
                            writer.WriteEndObject();
                            str.WriteLine(sb.ToString());
                            sb.Clear();
                        }
                    }
                }
            });

            return t;
        }

    }
}
