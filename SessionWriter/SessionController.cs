using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LightBuzz.Vitruvius;
using SessionWriter.Interfaces.API;

namespace SessionWriter
{
    public class SessionController : IBodyWrapperController
    {

        private SessionDataBase SessionDataBase = new SessionDataBase();

        public SessionController()
        {
            foreach(string path in Constants.pathsToReset)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    File.Create(path);
                }
            }
        }

        public Task GetVitruviusSingleData(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token)
        {
            return SessionDataBase.WriteVitruviusSingle(bodyWrapper, token, Constants.FilePathVitruvius);
        }
    }
}
