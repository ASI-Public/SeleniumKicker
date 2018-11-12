using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SeleniumKicker.Core.Processes
{
  public abstract class ProcessWrapper : Process, IProcessWrapper
  {
    public Process[] GetProcessesByNameWrapper(string processName)
    {
      return GetProcessesByName(processName);
    }

    public string GetProcessUser()
    {
      var processHandle = IntPtr.Zero;
      try
      {
        OpenProcessToken(Handle, 8, out processHandle);
        var wi = new WindowsIdentity(processHandle);
        return wi.Name;
      }
      catch
      {
        return null;
      }
      finally
      {
        if (processHandle != IntPtr.Zero)
        {
          CloseHandle(processHandle);
        }
      }
    }

    // ReSharper disable once StringLiteralTypo
    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr hObject);

  }
}
