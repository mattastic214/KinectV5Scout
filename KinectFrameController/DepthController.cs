using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinectFrameService;

namespace KinectFrameController
{
    public class DepthController
    {
        private ServiceInfraredData service = null;

        public void GetFrameData(ushort[] depthData)
        {
            Console.WriteLine("\nDepth data (ushort): " + depthData + ", Length " + depthData.Length + ", var 2: " + depthData[2]);
        }
    }
}
