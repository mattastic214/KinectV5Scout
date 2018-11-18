using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace KinectDataBase.Controllers
{
    public class VitruviusController : Controller, IVitruviusBodyController
    {

        public VitruviusController(string basePath, string specPath) : base(basePath, specPath)
        {
            this.basePath = basePath;
            this.specPath = specPath;
        }

        public void GetVitruviusData(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper)
        {
            WriteJsonToDataBase(bodyWrapper);
        }

        private void WriteJsonToDataBase(KeyValuePair<TimeSpan, IList<BodyWrapper>> bodyWrapper)
        {
            using (StreamWriter str = File.AppendText(basePath + specPath))
            {
                var vitBody = bodyWrapper.Value.FirstOrDefault();
                str.WriteLine("Depth data (ushort) Time stamp: " + bodyWrapper.Key + ", Values:");
                // Check if null before writing.
                //str.WriteLine("Tracking ID: " + vitBody.TrackingId);
                //str.WriteLine("Upper Height: " + vitBody.UpperHeight());
                //str.WriteLine("BodyWrapper Infrared JSON: " + vitBody.ToJSON());
                //str.WriteLine("BodyWrapper: " + vitBody);
                //str.WriteLine("JSON Length: " + vitBody.ToJSON().Length + "\n\n");
            }
        }

    }
}
