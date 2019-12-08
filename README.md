# doomstatz

doomstatz is a .NET application that hosts a [webview](https://github.com/webview-cs/webview-cs) that can be configured to display realtime memory values in a running process. It was originally written to be used with [prboom-plus](https://sourceforge.net/projects/prboom-plus/) and the included config/html are set up for that game specifically. However, it can be used to read int32 values from memory offsets of any application.

# Installation

1. Download and install the [.NET Core SDK](https://dotnet.microsoft.com/download).
2. Navigate to the project root directory in the command line.
3. Install the Webview dependency by running `dotnet add package Webview.Core`.
4. Install the JSON dependency by running `dotnet add package Newtonsoft.Json`.
5. Run `dotnet run`.

# Configuration

Since the UI is based on a web browser, it can be infinitely configured by editing the html file in [`html/statsview.html`](./html/statsview.html). The javascript function `handleStats()` is called with a JSON string containing the various memory values that are polled. If the process is not running, `handleWaiting()` is called.

# Ideas

The biggest necessity is to support all other common data types instead of just ints.

It'd also be nice if the javascript could define/register the process name and the memory offsets and call into the native code, but the javascript callback API in the webview library is not working in my testing, so there's no way to receive messages from the js world right now.
