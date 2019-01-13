using Scout;

namespace PingPongScoutConsole
{
    class ScoutRunner
    {
        static void Main(string[] args)
        {
            ScoutRecorder IvanovoDetstvo = new ScoutRecorder(CameraType.Infrared);
            IvanovoDetstvo.OpenKinect();

            /* Done in the constructor */
            //scoutRunner.WindowLoaded();
            //scoutRunner.AssignConstructors();
            //scoutRunner.AssignEndOperations();


            /* Both are event driven from the Kinect. */

            //scoutRunner.MultiSourceFrameArrived();
            //scoutRunner.Window_Closed();

        }
    }
}
