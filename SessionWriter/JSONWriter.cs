using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;

namespace SessionWriter
{
    public class JSONWriter
    {
        public static void writeJSONSession()
        {
            ScriptEngine scriptEngine = Python.CreateEngine();
            ScriptSource source = scriptEngine.CreateScriptSourceFromFile(Constants.FilePathJsonParse);
            ScriptScope scope = scriptEngine.CreateScope();
            List<String> argv = new List<String>();
            argv.Add(Constants.FilePathVitruvius);

            string[] libs = {
                "../../../SessionWriter/Lib",
                "../../../SessionWriter/Lib/json",
                "../../../SessionWriter/data"
            };
            scriptEngine.GetSysModule().SetVariable("argv", argv);
            scriptEngine.SetSearchPaths(libs);

            source.Execute(scope);
        }
    }
}
