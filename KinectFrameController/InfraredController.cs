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
    public class InfraredController
    {
        private ServiceInfraredData service = null;

        public void GetFrameData(ushort[] infraredData)
        {
            Console.WriteLine("\nInfrared data (ushort): " + infraredData + ", Length " + infraredData.Length + ", var 25: " + infraredData[25]);
        }
    }
}
