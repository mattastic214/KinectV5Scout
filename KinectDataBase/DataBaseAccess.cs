﻿using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using KinectDataBase.Interfaces.Access;
using Microsoft.Kinect;

namespace KinectDataBase
{
    public class DataBaseAccess : IDataBaseAccess, IVitruviusSingleAccess
    {        
        private readonly object fileLock = new object();
        StringBuilder sb = new StringBuilder();
        DataBaseConstants db = new DataBaseConstants();

        public Task WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    bodyIndexData.Value.HighlightedBitmap.Save(path);
                    
                    //using (StreamWriter str = File.AppendText(path))
                    //{
                    //    //str.WriteLine("BodyIndex data TimeStamp: " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.Bitmap + "\n");
                    //    //bodyIndexData.Value.HighlightedPixels;
                    //}
                }
            }, token);

            return t;
        }

        public Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    depthData.Value.Bitmap.Save(path);
                    //using (StreamWriter str = File.AppendText(path))
                    //{
                    //    //str.WriteLine("Depth data TimeStamp: " + depthData.Key + ", Value: " + depthData.Value.Bitmap + "\n");
                        
                    //}
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
                    infraredData.Value.Bitmap.Save(path);
                    //using (StreamWriter str = File.AppendText(path))
                    //{
                    //    //str.WriteLine("Infrared data TimeStamp: " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData[34] + "\n");
                        
                    //}
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
                    longExposureData.Value.Bitmap.Save(path);
                    //using (StreamWriter str = File.AppendText(path))
                    //{
                    //    //str.WriteLine("LongExposure data TimeStamp: " + longExposureData.Key + ", Value: " + longExposureData.Value.InfraredData.GetValue(8) + "\n");
                        
                    //}
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
                        writer.WriteRaw(",");

                        writer.WriteStartObject();
                        writer.WritePropertyName("RelativeTime");
                        writer.WriteValue(time);
                        writer.WritePropertyName("TrackedPlayer");

                        writer.WriteRawValue(body.ToJSON());
                        writer.WriteEndObject();
                        str.Write(sb.ToString());
                        sb.Clear();
                    }
                }
            }, token);

            return t;
        }
    }
}
