using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

public class MemoryReader
{
    private const int PROCESS_WM_READ = 0x0010;
    private string processName;
    private bool running = false;
    private IntPtr? processHandle;

    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    public delegate void OnProcessChangedDelegate(bool running);
    public event OnProcessChangedDelegate OnProcessChanged;
    public bool ProcessRunning
    {
        get
        {
            return processHandle != null;
        }
    }

    public void Start(string processName)
    {
        if (running) return;
        running = true;
        this.processName = processName;
        setupScanningThread();
    }

    public void Stop()
    {
        if (!running) return;
        running = false;
    }

    public int ReadInt32(int baseAddress)
    {
        if (!ProcessRunning) return 0;
        int bytesRead = 0;
        byte[] buffer = new byte[sizeof(Int32)];
        ReadProcessMemory((int)processHandle, baseAddress, buffer, buffer.Length, ref bytesRead);
        return BitConverter.ToInt32(buffer, 0);
    }

    private void setupScanningThread()
    {
        Thread thread = new Thread(new ThreadStart(tryOpenProcess));
        thread.Start();
    }

    private void tryOpenProcess()
    {
        while (running && processHandle == null)
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
            {
                Process process = processes[0];
                process.EnableRaisingEvents = true;
                process.Exited += (sender, e) =>
                {
                    processHandle = null;
                    if (OnProcessChanged != null)
                    {
                        OnProcessChanged(false);
                    }
                    setupScanningThread();
                };
                processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
                if (OnProcessChanged != null)
                {
                    OnProcessChanged(true);
                }
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }
}
