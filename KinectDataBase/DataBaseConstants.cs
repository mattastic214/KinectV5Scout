using System.Collections.Generic;

namespace KinectDataBase
{
    public class DataBaseConstants
    {
        #region Constants

        // Dictionary? A key value pair of names and associated file paths...


        public string bodyIndexPath { get; } = @"\bodyIndex\BodyIndex.txt";
        public string depthDataPath { get; } = @"\depth\Depth.txt";
        public string infraredDataPath { get; } = @"\infra\Infrared.txt";
        public string longExposureDataPath { get; } = @"\longExp\LongExposure.txt";
        public string vitruviusPath { get; } = @"\Vitruvius.txt";

        public readonly Dictionary<string, string> fileNames = null;
        public readonly List<string> dbConstants = new List<string>();

        public DataBaseConstants()
        {
            dbConstants.Add(bodyIndexPath);
            dbConstants.Add(depthDataPath);
            dbConstants.Add(infraredDataPath);
            dbConstants.Add(longExposureDataPath);
            dbConstants.Add(vitruviusPath);
        }

        public DataBaseConstants(DataBaseConstants dataBaseConstants)
        {
            this.dbConstants.Add(dataBaseConstants.bodyIndexPath);
            this.dbConstants.Add(dataBaseConstants.depthDataPath);
            this.dbConstants.Add(dataBaseConstants.infraredDataPath);
            this.dbConstants.Add(dataBaseConstants.longExposureDataPath);
            this.dbConstants.Add(dataBaseConstants.vitruviusPath);
        }

        #endregion
    }
}
