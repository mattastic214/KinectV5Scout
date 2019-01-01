using System;
using System.IO;

namespace SessionWriter
{
    class Constants
    {
        #region File Constants

        public static string FilePathVitruvius = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../SessionWriter/data/Vitruvius.txt");
        public static string FilePathJsonParse = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../SessionWriter/jsonParse.py");

        public static string[] pathsToReset = {
            "../../../SessionWriter/data/Vitruvius.txt",
        };
        
        #endregion
    }
}
