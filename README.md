# doomstatz

doomstatz is a .NET application that hosts a [webview](https://github.com/webview-cs/webview-cs) that can be configured to display realtime stats in an active game of Doom running in [prboom-plus](https://sourceforge.net/projects/prboom-plus/).

# Installation

1. Download and install the [.NET Core SDK](https://dotnet.microsoft.com/download).
2. Navigate to the project root directory in the command line.
3. Install the Webview dependency by running `dotnet add package Webview.Core`.
4. Run `dotnet run`.

# Configuration

Since the UI is based on a web browser, it can be infinitely configured by editing the html file in [`html/statsview.html`](./html/statsview.html). The javascript function `handleStats()` is called with a JSON string containing the various game stats that are polled. If the game is not running, `handleWaiting()` is called.

# Ideas

It'd be nice if the javascript could define/register the process name and the memory offsets and their expected types being queried so that this can be much more easily tweaked and used with different games. The issue is that the javascript callback API in the webview library is not working in my testing, so there's no way to receive messages from the js world.
