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
    public class DepthController
    {

        public static void GetFrameData(KeyValuePair<TimeSpan, ushort[]> depthData)
        {
            if (!depthData.Key.Equals(null) && !depthData.Value.Equals(null))
            {
                Console.WriteLine("\nDepth data (ushort): " + depthData + ", Value: " + depthData.Value);
                DepthDataBase.WriteToDataBase(depthData);
            }
            
        }
    }
}
