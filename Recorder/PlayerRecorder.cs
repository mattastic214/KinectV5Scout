using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;

namespace Recorder
{
    public class PlayerRecorder
    {

        public void ToggleRecord()
        {
            if (VideoRecorder.IsRecording)
            {
                VideoRecorder.Stop();
            }
            else if (!VideoRecorder.IsRecording)
            {
                VideoRecorder.Start();
            }
        }
    }
}
