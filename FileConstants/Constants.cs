﻿using System;
using System.Collections.Generic;

namespace FileConstants
{
    public class Constants
    {
        #region Constants

        public string BasePath { get; } = @"..\..\..\KinectDataBase\KinectDataOutput\";
        public string bodyIndexPath { get; } = @"BodyIndex.txt";
        public string depthDataPath { get; } = @"DepthData.txt";
        public string infraredDataPath { get; } = @"InfraredData.txt";
        public string longExposureDataPath { get; } = @"LongExposureData.txt";
        public string vitruviusPath { get; } = @"Vitruvius.txt";

        public readonly List<string> dbConstants = new List<string>();

        public Constants()
        {
            dbConstants.Add(bodyIndexPath);
            dbConstants.Add(depthDataPath);
            dbConstants.Add(infraredDataPath);
            dbConstants.Add(longExposureDataPath);
            dbConstants.Add(vitruviusPath);
        }

        #endregion
    }
}