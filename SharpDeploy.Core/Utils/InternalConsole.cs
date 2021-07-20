using System.Collections.Generic;

namespace SharpDeploy.Core.Utils
{
    public class InternalConsole
    {
        public InternalConsole(string projectId)
        {
            ProjectId = projectId;
            Logs = new List<string>();
        }

        public string ProjectId { get; }
        public List<string> Logs { get; }

        public void WriteLine(string text) => Logs.Add(text);

        public override string ToString()
        {
            string result = "";
            foreach(var log in Logs)
            {
                var entry = $"\n {log}";
                result += entry;
            }
            return result;
        }
    }
}
