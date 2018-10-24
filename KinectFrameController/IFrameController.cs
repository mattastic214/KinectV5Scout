﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectFrameController
{
    interface IFrameController
    {
        void GetFrameData(IDictionary frameDictionary);        
    }
}
