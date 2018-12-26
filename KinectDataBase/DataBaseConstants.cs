using System.Collections.Generic;

namespace KinectDataBase
{
    public class DataBaseConstants
    {
        #region Constants

        public string BasePath { get; } = @"..\..\..\KinectDataBase\KinectDataOutput\";
        public string bodyIndexPath { get; } = @"BodyIndex.txt";
        public string depthDataPath { get; } = @"DepthData.txt";
        public string infraredDataPath { get; } = @"InfraredData.txt";
        public string longExposureDataPath { get; } = @"LongExposureData.txt";
        public string vitruviusPath { get; } = @"Vitruvius.json";

        public readonly List<string> dbConstants = new List<string>();

        public DataBaseConstants()
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
