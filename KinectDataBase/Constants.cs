using System.Collections.Generic;

namespace KinectDataBase
{
    class Constants
    {
        #region Constants

        // Dictionary? A key value pair of names and associated file paths...


        public static string bodyIndexPath { get; } = @"\bodyIndex\BodyIndex.txt";
        public static string depthDataPath { get; } = @"\depth\Depth.txt";
        public static string infraredDataPath { get; } = @"\infra\Infrared.txt";
        public static string longExposureDataPath { get; } = @"\longExp\LongExposure.txt";

        public readonly Dictionary<string, string> fileNames = null;
        public readonly List<string> dbConstants = new List<string>();

        public Constants()
        {
            dbConstants.Add(bodyIndexPath);
            dbConstants.Add(depthDataPath);
            dbConstants.Add(infraredDataPath);
            dbConstants.Add(longExposureDataPath);
        }

        public Constants(ICollection<string> paths)
        {
            foreach(string path in paths)
            {
                dbConstants.Add(path);
            }
        }

        #endregion
    }
}
