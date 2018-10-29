using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace KinectDataBase
{
    public class InfraredDataBase
    {
        public static void WriteToDataBase(KeyValuePair<TimeSpan, ushort[]> infraredData)
        {
            string path = @"d:\\OneDrive\\DbText\\InfraredData.txt";

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(infraredData.Value.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
