using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace KinectDataBase
{
    public class BodyIndexDataBase
    {
        public static void WriteToDataBase(KeyValuePair<TimeSpan, byte[]> bodyIndexData)
        {
            string path = @"d:\\OneDrive\\DbText\\BodyIndexData.txt";

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(bodyIndexData.Value.ToString());
                fs.Write(info, 0, info.Length);
            }
        }
    }
}
