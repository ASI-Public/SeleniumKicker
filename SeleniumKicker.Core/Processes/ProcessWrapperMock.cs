using System.Diagnostics;

namespace SeleniumKicker.Core.Processes
{
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class ProcessWrapperMock : IProcessWrapper
  {
    public ProcessStartInfo StartInfo { get; set; }
    public bool Start()
    {
      return true;
    }

    public void BeginOutputReadLine()
    { }

    public void BeginErrorReadLine()
    { }

    public void Kill()
    { }

    public event DataReceivedEventHandler OutputDataReceived;
    public event DataReceivedEventHandler ErrorDataReceived;
    public Process[] GetProcessesByNameWrapper(string processName)
    {
      return new Process[0];
    }

    public bool EnableRaisingEvents { get; set; }
    public string GetProcessUser()
    {
      // ReSharper disable once StringLiteralTypo
      return "testuser";
    }
  }
}
