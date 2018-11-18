using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;
using KinectDataBase.Controllers;

namespace KinectDataBase
{
    public class DataBaseController
    {

        public BodyIndexController BodyIndexController { get; private set; }
        public DepthController DepthController { get; private set; }
        public InfraredController InfraredController { get; private set; }
        public VitruviusController VitruviusController { get; private set; }

        private string basePath =           @"..\..\..\KinectDataBase\KinectDataOutput\";
        private string bodyIndexPath =      @"BodyIndex.txt";
        private string depthDataPath =      @"DepthData.txt";
        private string infraredDataPath =   @"InfraredData.txt";
        private string vitruviusPath =      @"Vitruvius.txt";

        
        public DataBaseController()
        {
            BodyIndexController = new BodyIndexController(basePath, bodyIndexPath);
            DepthController = new DepthController(basePath, depthDataPath);
            InfraredController = new InfraredController(basePath, infraredDataPath);
            VitruviusController = new VitruviusController(basePath, vitruviusPath);
        }
    }
}
