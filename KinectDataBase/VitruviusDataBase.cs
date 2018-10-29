using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LightBuzz.Vitruvius;

namespace KinectDataBase
{
    public class VitruviusDataBase
    {
        public static void WriteToDataBase(KeyValuePair<TimeSpan, BodyWrapper> vitruviusData)
        {
            string path = @"d:\\OneDrive\\DbText\\VitruviusData.txt";

            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(vitruviusData.Value.ToString());
                fs.Write(info, 0, info.Length);
            }
        }

    }
}
