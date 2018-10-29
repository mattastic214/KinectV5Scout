using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KinectDataBase
{
    public class DepthDataBase
    {
        public static void WriteToDataBase(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            string path = @"d:\\OneDrive\\DbText\\DepthData.txt";

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(depthData.Value.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

    }
}
