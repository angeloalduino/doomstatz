# doomstatz

doomstatz is a .NET application that hosts a [webview](https://github.com/webview-cs/webview-cs) that can be configured to display realtime stats in an active game of Doom running in [prboom-plus](https://sourceforge.net/projects/prboom-plus/).

# Installation

1. Download and install the [.NET Core SDK](https://dotnet.microsoft.com/download).
2. Run `dotnet run` within the root of this project.

# Configuration

Since the UI is based on a web browser, it can be infinitely configured by editing the html file in [`html/statsview.html`](./html/statsview.html). The javascript function `handleStats()` is called with a JSON string containing the various game stats that are polled. If the game is not running, `handleWaiting()` is called.
