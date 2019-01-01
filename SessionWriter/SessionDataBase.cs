using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SessionWriter.Interfaces.DataBaseAccess;
using LightBuzz.Vitruvius;
using Microsoft.Kinect;

namespace SessionWriter
{
    class SessionDataBase : IBodyWrapperAccess
    {
        private readonly object fileLock = new object();
        StringBuilder sb = new StringBuilder();

        public Task WriteVitruviusSingle(KeyValuePair<TimeSpan, BodyWrapper> bodyWrapper, CancellationToken token, string path)
        {
            TimeSpan time = bodyWrapper.Key;
            BodyWrapper body = bodyWrapper.Value;

            Task t = Task.Run(() =>
            {
                lock (fileLock)
                {
                    using (StreamWriter str = File.AppendText(path))
                    using (StringWriter strwtr = new StringWriter(sb))
                    using (JsonTextWriter writer = new JsonTextWriter(strwtr))
                    {
                        writer.WriteRaw(",");

                        writer.WriteStartObject();
                        writer.WritePropertyName("RelativeTime");
                        writer.WriteValue(time);
                        writer.WritePropertyName("TrackedPlayer");

                        writer.WriteRawValue(body.ToJSON());
                        writer.WriteEndObject();
                        str.Write(sb.ToString());
                        sb.Clear();
                    }
                }
            }, token);

            return t;
        }
    }
}
