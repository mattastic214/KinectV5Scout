using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;

namespace KinectFrameController
{
    public class VitruviusBodyController
    {
        public static void GetFrameData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper)
        {            
            if (!bodyWrapper.Key.Equals(null) && !bodyWrapper.Value.Equals(null))
            {
                // It's a JSON, so it's easy to work with.
                Console.WriteLine("\nInfrared body available:");
                Console.WriteLine("Tracking ID: " + bodyWrapper.Value.TrackingId);
                Console.WriteLine("Upper Height: " + bodyWrapper.Value.UpperHeight());
                Console.WriteLine("Time stamp:" + bodyWrapper.Key);
                Console.WriteLine("BodyWrapper Infrared JSON: " + bodyWrapper.Value.ToJSON());
                Console.WriteLine("BodyWrapper: " + bodyWrapper.Value.ToJSON().GetBytes());
                Console.WriteLine("JSON Length: " + bodyWrapper.Value.ToJSON().GetBytes().Length);
                // BodyLean, HandConfidence, etc.
            }
        }
    }
}
