using SeleniumKicker.Core;
using SeleniumKicker.Core.Processes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Controls;

namespace SeleniumKicker.Wpf
{
  internal sealed class HubCommands : IHubCommands
  {
    private readonly List<(AbstractProcess Process, NodeTabItem Tab)> _processes = new List<(AbstractProcess Process, NodeTabItem Tab)>();
    private readonly TabControl _tabControl;
    private string _currentUser;

    public HubCommands(TabControl tc)
    {
      _tabControl = tc;
    }

    public string Start()
    {
      var username = StartTab(new Hub());
      Thread.Sleep(3000);
      StartNodes();
      
      return username;
    }

    private string StartNodes()
    {
      var username = string.Empty;
      if (Properties.Settings.Default.StartChrome)
      {
        username = StartTab(new Chrome());
      }

      if (Properties.Settings.Default.StartOpera)
      {
        username = StartTab(new Opera());
      }

      if (Properties.Settings.Default.StartFirefox)
      {
        username = StartTab(new Firefox());
      }

      if (Properties.Settings.Default.StartIE)
      {
        username = StartTab(new InternetExplorer());
      }

      if (Properties.Settings.Default.StartEdge)
      {
        username = StartTab(new Edge());
      }

      return username;
    }

    private string StartTab(AbstractProcess process)
    {
      var username = process.Start<ProcessWrapper>(_currentUser);
      process.OutputDataReceived += Process_DataReceived;
      process.ErrorDataReceived += Process_DataReceived;
      var tabItem = new NodeTabItem() { Header = process.NodeName };
      _tabControl.Items.Add(tabItem);
      _processes.Add((Process: process, Tab: tabItem));
 
      return username;
    }

    public void Stop(int index = 0)
    {
      if (_processes.Count == 0) return;
      do
      {
        StopTab(_processes[index]);
      } while (_processes.Count > index);
    }

    private void StopTab((AbstractProcess Process, NodeTabItem Tab) tab)
    {
      try
      {
        tab.Process.Stop();
      }
      // ReSharper disable once EmptyGeneralCatchClause
      catch (Exception) { }
      _tabControl.Items.Remove(tab.Tab);
      _processes.Remove(tab);
    }

    public string RestartAll()
    {
      if (_processes.Count <= 0)
        return Start();
      Stop();
      return Start();
    }

    public string RestartNodes()
    {
      if (_processes.Count <= 1)
        return StartNodes();
      Stop(1);
      return StartNodes();
    }

    public bool SetUser(string username)
    {
      _currentUser = username == string.Empty ? null : username;
      if (username == string.Empty)
      {
        _currentUser = null;
      }
      else
      {
        _currentUser = CredentialStore.ValidateUser(username) ? username : null;
      }

      return true;
    }

    public bool AddUser(string username, string password)
    {
      return CredentialStore.SetCredentials(username, username, password);
    }

    private delegate void UpdateTextCallback(TextBlock tabItem, string message);

    private void Process_DataReceived(object sender, DataReceivedEventArgs e)
    {
      var strMessage = e.Data;
      if (sender == null || string.IsNullOrEmpty(strMessage)) return;
      var ti = _processes.First(p => p.Process.Process == (Process)sender).Tab;

      ti.Output.Dispatcher.Invoke(new UpdateTextCallback(UpdateText), ti.Output, strMessage);
    }

    private void UpdateText(TextBlock tabItem, string message)
    {
      tabItem.Text += $"{message}\r";
      tabItem.UpdateLayout();
    }
  }
}
