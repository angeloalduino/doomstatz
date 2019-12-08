using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Webview;
using Newtonsoft.Json;

namespace doomstatz
{
    class MemoryAddress
    {
        public string name { get; set; }
        public string offsetHex { get; set; }

        public int offset
        {
            get
            {
                return Convert.ToInt32(offsetHex, 16);
            }
        }
    }

    class ProcessConfig
    {
        public string processName { get; set; }
        public MemoryAddress[] memoryAddresses { get; set; }
    }

    class Program
    {
        private static Webview.Webview webview;
        private static MemoryReader memoryReader;
        private static ProcessConfig config;
        private static int pollTime = 100;

        // This is necessary for webview to work in .net core 3 apparently.
        [STAThread]
        static void Main(string[] args)
        {
            var configPath = Environment.CurrentDirectory + "\\config.json";
            config = JsonConvert.DeserializeObject<ProcessConfig>(File.ReadAllText(configPath));
            var path = Environment.CurrentDirectory + "\\html\\statsview.html";
            var pathUri = new Uri(path);

            webview = new WebviewBuilder(config.processName + " stats", Content.FromUri(pathUri))
                .WithSize(new Size(300, 180))
                .Resizeable()
                .Build();
            TimeSpan pollInterval = TimeSpan.FromMilliseconds(pollTime);
            DateTime timeout = DateTime.Now + pollInterval;

            memoryReader = new MemoryReader();
            memoryReader.Start(config.processName);

            while (webview.Loop() == 0)
            {
                if (DateTime.Now > timeout)
                {
                    timeout = DateTime.Now + pollInterval;
                    if (memoryReader.ProcessRunning)
                    {
                        var stats = generateStats();
                        var jsonString = JsonConvert.SerializeObject(stats);
                        webview.Eval("handleStats('" + jsonString + "');");
                    }
                    else
                    {
                        webview.Eval("handleWaiting();");
                    }
                }
                GC.Collect();
            }

            memoryReader.Stop();
        }

        private static Dictionary<string, int> generateStats()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            foreach (var definition in config.memoryAddresses)
            {
                stats[definition.name] = memoryReader.ReadInt32(definition.offset);
            }
            return stats;
        }
    }
}
