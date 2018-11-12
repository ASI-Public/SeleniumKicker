using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SeleniumKicker.Core.Processes
{
  public abstract class AbstractProcess
  {
    internal const string HubFileName = "selenium-server-standalone-";
    private const string CurrentFileName = "java";
    internal string CurrentArgs;
    public string NodeName;
    public IProcessWrapper Process { get; private set; }

    public string Start<T>(string username) where T : IProcessWrapper
    {
      Process = Activator.CreateInstance<T>();
      Process.StartInfo = new ProcessStartInfo()
      {
        WorkingDirectory = Properties.Settings.Default.SeleniumPath,
        Arguments = CurrentArgs,
        FileName = CurrentFileName,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
      };
      Process.EnableRaisingEvents = true;

      if (username != null)
      {
        var cm = CredentialStore.GetCredential(username);
        Process.StartInfo.Domain = cm.Username.Split('\\')[0];
        Process.StartInfo.UserName = cm.Username.Split('\\')[1];
        var pass = cm.Password;
        Console.WriteLine(pass);
        Process.StartInfo.Password = cm.SecurePassword;
      }

      Process.Start();
      Process.BeginOutputReadLine();
      Process.BeginErrorReadLine();

      return Process.GetProcessUser();
    }
    

    public virtual void Stop()
    {
      Process.Kill();
    }

    public event DataReceivedEventHandler OutputDataReceived
    {
      add => Process.OutputDataReceived += value;
      remove => Process.OutputDataReceived -= value;
    }

    public event DataReceivedEventHandler ErrorDataReceived
    {
      add => Process.ErrorDataReceived += value;
      remove => Process.ErrorDataReceived -= value;
    }
  }
}
