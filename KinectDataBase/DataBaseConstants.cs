using System.Collections.Generic;

namespace KinectDataBase
{
    public class DataBaseConstants
    {
        #region Constants

        public string BasePath { get; } = @"..\..\..\KinectDataBase\KinectDataOutput\";
        public string bodyIndexPath { get; } = @"bodyIndex\BodyIndex.bmp";
        public string depthDataPath { get; } = @"depth\Depth.bmp";
        public string infraredDataPath { get; } = @"infra\Infrared.bmp";
        public string longExposureDataPath { get; } = @"longExp\LongExposure.bmp";
        public string vitruviusPath { get; } = @"Vitruvius.txt";

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
