using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinectDataBase;

namespace KinectFrameController
{
    public class InfraredController
    {
        public static void GetFrameData(KeyValuePair<TimeSpan, ushort[]> infraredData)
        {
            if (!infraredData.Key.Equals(null) && !infraredData.Value.Equals(null))
            {
                Console.WriteLine("\nInfrared data (ushort): " + infraredData + ", Value: " + infraredData.Value);
                InfraredDataBase.WriteToDataBase(infraredData);
            }
            
        }
    }
}
