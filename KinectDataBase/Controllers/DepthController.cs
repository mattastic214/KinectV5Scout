using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LightBuzz.Vitruvius;

namespace KinectDataBase.Controllers
{
    public class DepthController : Controller, IDepthController
    {

        public DepthController(string basePath, string specPath) : base(basePath, specPath)
        {
            this.basePath = basePath;
            this.specPath = specPath;
        }

        public void GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData)
        {
            WriteDepthBitMapToDataBase(depthData);
        }

        private bool WriteDepthBitMapToDataBase(KeyValuePair<TimeSpan, DepthBitmapGenerator> bodyIndexData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + specPath))
            {
                str.WriteLine("Depth data (ushort): " + bodyIndexData.Key + ", Value: " + bodyIndexData.Value.DepthData.GetValue(i) + "\n");
                return bodyIndexData.Value.Bitmap.Save(basePath);
            }
        }
    }
}
