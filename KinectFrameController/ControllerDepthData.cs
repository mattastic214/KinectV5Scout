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
    public class ControllerDepthData : IFrameController
    {
        private ServiceInfraredData service = null;

        public void GetFrameData(ReadOnlyDictionary<Enum, IStructuralComparable> frameDictionary)
        {
            throw new NotImplementedException();
        }
    }
}
