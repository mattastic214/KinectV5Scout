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
    public class BodyIndexController
    {
        private ServiceInfraredData service = null;

        public void GetFrameData(byte[] bodyIndexData)
        {
            Console.WriteLine("BodyFrameIndex data (byte): " + bodyIndexData + ", Length " + bodyIndexData.Length + ", var 5: " + bodyIndexData[5] + "\n");
        }
    }
}
