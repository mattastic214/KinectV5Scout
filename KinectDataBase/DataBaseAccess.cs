using LightBuzz.Vitruvius;
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
    public class DataBaseAccess : IDataBaseAccess
    {        
        private readonly object fileLock = new object();
        StringBuilder sb = new StringBuilder();
        BitmapEncoder encoder = new PngBitmapEncoder();

        public Task WriteBodyIndexDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    encoder.Frames.Add(BitmapFrame.Create(bodyIndexData.Value.Bitmap));
                    string time = System.DateTime.Now.ToString("d MMM yyyy hh-mm-ss");
                    string fullPath = Path.Combine(path, "ScreenShot" + time + ".png");
                    try
                    {
                        //bodyIndexData.Value.HighlightedBitmap.Save(path);
                        //using (StreamWriter str = File.AppendText(path))
                        //{
                        //    str.WriteLine("BodyIndex data TimeStamp: " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.Bitmap + "\n");
                        //}
                        using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                        {
                            encoder.Save(fs);
                            Console.WriteLine("Saving Pics");
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message + " in WriteBodyIndexData.");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteBodyIndexDataToDataBase(byte[] bodyIndexData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task WriteDepthDataToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    try
                    {
                        //depthData.Value.Bitmap.Save(path);
                        using (StreamWriter str = File.AppendText(path))
                        {
                            str.WriteLine("Depth data TimeStamp: " + depthData.Key + ", Value: " + depthData.Value.Bitmap + "\n");
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message + " in write DepthData.");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteDepthDataToDataBase(ushort[] depthData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task WriteInfraredDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    try
                    {
                        //infraredData.Value.Bitmap.Save(path);
                        using (StreamWriter str = File.AppendText(path))
                        {
                            str.WriteLine("Infrared data TimeStamp: " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData[34] + "\n");
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message + " in WriteInfraredData.");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteInfraredDataToDataBase(ushort[] infraredData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }

        public Task WriteLongExposureDataToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> longExposureData, CancellationToken token, string path)
        {
            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    try
                    {
                        longExposureData.Value.Bitmap.Save(path);
                        using (StreamWriter str = File.AppendText(path))
                        {
                            str.WriteLine("LongExposure data TimeStamp: " + longExposureData.Key + ", Value: " + longExposureData.Value.InfraredData.GetValue(8) + "\n");
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message + " in WriteLongExposureData.");
                    }
                }
            }, token);

            return t;
        }

        public Task WriteLongExposureDataToDataBase(ushort[] longExposureData, CancellationToken token, string path)
        {
            throw new NotImplementedException();
        }
    }
}
