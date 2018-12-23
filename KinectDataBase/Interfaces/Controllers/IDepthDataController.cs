﻿using LightBuzz.Vitruvius;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KinectDataBase.Interfaces.Controllers
{
    public interface IDepthDataController
    {
        Task GetDepthData(KeyValuePair<TimeSpan, DepthBitmapGenerator> depthData, CancellationToken token);
    }
}