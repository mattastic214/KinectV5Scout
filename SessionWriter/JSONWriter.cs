using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionWriter
{
    public class JSONWriter
    {
        public static void writeJSONSession(string rootPath, string dataPath)
        {
            ScriptEngine scriptEngine = Python.CreateEngine();
            string[] libs = {
                rootPath + @"\Lib",
                rootPath + @"\Lib\json",
                dataPath
            };

            scriptEngine.SetSearchPaths(libs);

            scriptEngine.ExecuteFile(rootPath + @"/jsonParse.py");
        }
    }
}
