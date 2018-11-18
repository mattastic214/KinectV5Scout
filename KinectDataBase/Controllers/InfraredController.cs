using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LightBuzz.Vitruvius;

namespace KinectDataBase.Controllers
{
    public class InfraredController : Controller, IInfraredController
    {
        
        public InfraredController(string basePath, string specPath) : base(basePath, specPath)
        {
            this.basePath = basePath;
            this.specPath = specPath;             
        }

        public void GetInfraredData(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            WriteInfraredBitMapToDataBase(infraredData);
        }

        private bool WriteInfraredBitMapToDataBase(KeyValuePair<TimeSpan, InfraredBitmapGenerator> infraredData)
        {
            i = random.Next(0, (int)Math.Pow(2, 16) - 1);

            using (StreamWriter str = File.AppendText(basePath + specPath))
            {
                str.WriteLine("BodyFrameIndex Depth data (ushort): " + infraredData.Key + ", Value: " + infraredData.Value.InfraredData.GetValue(i) + "\n");
                return infraredData.Value.Bitmap.Save(basePath);
            }
        }
    }
}
