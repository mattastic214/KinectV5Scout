using IronPython.Hosting;
using Microsoft.Scripting.Hosting;


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
