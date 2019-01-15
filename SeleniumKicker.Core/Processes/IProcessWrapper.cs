using System;
using System.Diagnostics;

namespace SeleniumKicker.Core.Processes
{
  public interface IProcessWrapper
  {
    ProcessStartInfo StartInfo { get; set; }
    bool Start();
    void BeginOutputReadLine();
    void BeginErrorReadLine();
    event DataReceivedEventHandler OutputDataReceived;
    event DataReceivedEventHandler ErrorDataReceived;
    Process[] GetProcessesByNameWrapper(string processName);
    bool EnableRaisingEvents { get; set; }
    string GetProcessUser();
    void KillParentAndChildren();
  }
}
