# doomstatz

doomstatz is a .NET application that hosts a [web view](https://github.com/webview-cs/webview-cs) that can be configured to display continuously updated memory values in a running process. An example use case would be using the web view as an [OBS](https://obsproject.com/) overlay to display game information that is unavailable in the standard game UI.

It was originally written to be used with [prboom-plus](https://sourceforge.net/projects/prboom-plus/) and the included config/html are set up for that game specifically. However, it can be used to read int32 values from static memory offsets of any application.

## Installation

1. Download and install the [.NET Core SDK](https://dotnet.microsoft.com/download).
2. Navigate to the project root directory in the command line.
3. Install the Webview dependency by running `dotnet add package Webview.Core`.
4. Install the JSON dependency by running `dotnet add package Newtonsoft.Json`.
5. Run `dotnet run`.

If using the compiled binary without using `dotnet run`, please copy the `html` directory and `config.json` to the same directory as the exe file.

## Configuration

##### UI

Since the UI is based on a web browser, it can be infinitely configured by editing the html file in [`html/statsview.html`](./html/statsview.html). The javascript function `handleStats()` is called with a JSON string containing the various memory values that are polled. If the process is not running, `handleWaiting()` is called.

##### Process

An executable name can be defined in [`config.json`](./config.json) along with a list of memory offsets and string identifiers to use for them. doomstatz will continuously read int32 values from these addresses and pass them to the webview via the `handleStats()` call.

## Resources

Seems to be pretty light on resources, only using a fraction of a percent of CPU time and 16MB of ram in testing on my 7700K machine.

## Ideas

The biggest necessity is to support all other common data types instead of just ints.

It would be cool to have maybe a color/pulse effect on the kill count depending on rate. Also a progress bar for kills?

It'd also be nice if the javascript could define/register the process name and the memory offsets and call into the native code, but the javascript callback API in the webview library is not working in my testing, so there's no way to receive messages from the js world right now.
