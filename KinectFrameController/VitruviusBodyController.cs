using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using KinectFrameService;
using LightBuzz.Vitruvius;

namespace KinectFrameController
{
    public class VitruviusBodyController
    {
        private ServiceInfraredData service = null;

        public void GetFrameData(BodyWrapper bodyWrapper)
        {
            // It's a JSON, so it's easy to work with.
            Console.WriteLine("\nInfrared body available:");
            Console.WriteLine("Tracking ID: " + bodyWrapper.TrackingId);
            Console.WriteLine("Upper Height: " + bodyWrapper.UpperHeight());
            Console.WriteLine("BodyWrapper Infrared JSON: " + bodyWrapper.ToJSON());
            Console.WriteLine("BodyWrapper: " + bodyWrapper.ToJSON().GetBytes());
            Console.WriteLine("JSON Length: " + bodyWrapper.ToJSON().GetBytes().Length);
            // BodyLean, HandConfidence, etc.
        }
    }
}
