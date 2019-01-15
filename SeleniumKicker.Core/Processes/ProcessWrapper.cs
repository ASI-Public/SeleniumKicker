using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace SeleniumKicker.Core.Processes
{
  public class ProcessWrapper : Process, IProcessWrapper
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
     public void KillParentAndChildren()
    {
      var children = new List<Process>();
      using (var mos = new ManagementObjectSearcher(
        $"Select * From Win32_Process Where ParentProcessID={Id}"))
      {
        foreach (var mo in mos.Get())
        {
          children.Add(GetProcessById(Convert.ToInt32(mo["ProcessID"])));
        }
      }

      foreach (var child in children)
      {
        child.Kill();
      }

      Kill();
    }

    // ReSharper disable once StringLiteralTypo
    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr hObject);

  }
}
