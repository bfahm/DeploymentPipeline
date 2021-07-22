using System;
using System.Collections.Generic;

namespace SharpDeploy.Core.Utils
{
    public class InternalConsole
    {
        public InternalConsole()
        {
            Logs = new List<string>();
        }

        public List<string> Logs { get; }

        public void WriteLine(string text)
        {
            Logs.Add(text);
            Console.WriteLine(text);
        }

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
