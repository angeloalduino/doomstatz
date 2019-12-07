using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Webview;

namespace doom_memory_reader
{
    enum DoomAddress
    {
        GameTicCount = 0x005db164,
        TotalTicCount = 0x005db9cc,
        KilledEnemies = 0x005820ac,
        RemainingEnemies = 0x005db184,
        SecretsFound = 0x005820b4,
        TotalSecrets = 0x005db390,
    }

    static class JsonKeys
    {
        public const string GameTimeSeconds = "game_time_seconds";
        public const string GameTimeFormatted = "game_time_formatted";
        public const string KilledEnemies = "killed_enemies";
        public const string RemainingEnemies = "remaining_enemies";
        public const string TotalEnemies = "total_enemies";
        public const string SecretsFound = "secrets_found";
        public const string TotalSecrets = "total_secrets";
    }

    class Program
    {
        static Webview.Webview webview;
        static int pollTime = 100;
        static MemoryReader memoryReader;

        [STAThread]
        static void Main(string[] args)
        {
            var path = Environment.CurrentDirectory + "\\html\\statsview.html";
            var pathUri = new Uri(path);

            webview = new WebviewBuilder("Doom Stats", Content.FromUri(pathUri))
                .WithSize(new Size(300, 180))
                .Resizeable()
                .Build();
            TimeSpan pollInterval = TimeSpan.FromMilliseconds(pollTime);
            DateTime timeout = DateTime.Now;

            memoryReader = new MemoryReader();
            memoryReader.Start("prboom-plus");

            while (webview.Loop() == 0)
            {
                if (DateTime.Now > timeout)
                {
                    timeout = DateTime.Now + pollInterval;
                    if (memoryReader.ProcessRunning)
                    {
                        var gameSeconds = readAddress(DoomAddress.GameTicCount) / 35;
                        var timeSpan = new TimeSpan(0, 0, gameSeconds);
                        var timeFormat = gameSeconds >= 60 * 60 ? "h\\:mm\\:ss" : "mm\\:ss";
                        var timeString = timeSpan.ToString(timeFormat);
                        var killed = readAddress(DoomAddress.KilledEnemies);
                        var remaining = readAddress(DoomAddress.RemainingEnemies);
                        var totalEnemies = killed + remaining;
                        var secretsFound = readAddress(DoomAddress.SecretsFound);
                        var totalSecrets = readAddress(DoomAddress.TotalSecrets);

                        var json = string.Format("{{\"{0}\":{1},\"{2}\":\"{3}\",\"{4}\":{5},\"{6}\":{7},\"{8}\":{9},\"{10}\":{11},\"{12}\":{13}}}",
                        JsonKeys.GameTimeSeconds, gameSeconds,
                        JsonKeys.GameTimeFormatted, timeString,
                        JsonKeys.KilledEnemies, killed,
                        JsonKeys.RemainingEnemies, remaining,
                        JsonKeys.TotalEnemies, totalEnemies,
                        JsonKeys.SecretsFound, secretsFound,
                        JsonKeys.TotalSecrets, totalSecrets);

                        webview.Eval("handleStats('" + json + "');");
                    }
                    else
                    {
                        webview.Eval("handleWaiting();");
                    }
                }
                GC.Collect();
            }
        }

        private static int readAddress(DoomAddress address)
        {
            return memoryReader.ReadInt32((int)address);
        }
    }
}
