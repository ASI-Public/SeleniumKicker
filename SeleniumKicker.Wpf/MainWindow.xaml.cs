using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using SeleniumKicker.Core;
using SeleniumKicker.Core.WebServer;
using SeleniumKicker.Wpf.Annotations;

namespace SeleniumKicker.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public sealed partial class MainWindow : IUpdateUser, INotifyPropertyChanged
  {
    private string _currentUsername;

    public string CurrentUsername
    {
      get => _currentUsername;
      set
      {
        _currentUsername = value;
        OnPropertyChanged();

      } 
    }

    public MainWindow()
    {
      InitializeComponent();
      _commands = new HubCommands(SeleniumTabs);
      CurrentUsername = $"{Environment.UserDomainName}\\{Environment.UserName}";
      CreateWebServer.SetWebServer(_commands, this);
      if (Environment.GetCommandLineArgs().Any(a => a.ToLower().Equals("--start")))
      {
        StartClick(null, null);
      }
    }

    private readonly IHubCommands _commands;

    private void StartClick(object sender, RoutedEventArgs e)
    {
      if (StartButton.Content.ToString() == "Start")
      {
        StartButton.Content = "Stop";
        RestartAllButton.Visibility = Visibility.Visible;
        RestartNodesButton.Visibility = Visibility.Visible;
        UserTextBox.Watermark = _commands.Start();
      }
      else
      {
        StartButton.Content = "Start";
        RestartAllButton.Visibility = Visibility.Hidden;
        RestartNodesButton.Visibility = Visibility.Hidden;
        _commands.Stop();
      }
    }

    private void SetUserClick(object sender, RoutedEventArgs e)
    {
      _commands.SetUser(UserTextBox.Text);
      UserTextBox.Text = string.Empty;
    }

    private void RestartAllClick(object sender, RoutedEventArgs e)
    {
      CurrentUsername = _commands.RestartAll();
    }

    private void RestartNodesClick(object sender, RoutedEventArgs e)
    {
      CurrentUsername = _commands.RestartNodes();
    }

    private void AddUserButton_OnClickUserClick(object sender, RoutedEventArgs e)
    {
      if (UsernameTextBox.Text == string.Empty || PasswordTextBox.Password == string.Empty)
      {
        return;
      }

      if (_commands.AddUser(UsernameTextBox.Text, PasswordTextBox.Password))
      {
        UsernameTextBox.Text = string.Empty;
        PasswordTextBox.Text = string.Empty;
        MessageBox.Show("User Added");
        return;
      }
      MessageBox.Show("Failed");

    }

    private void Expander_Expanded(object sender, RoutedEventArgs e)
    {
      OptionsExpander.Height = 130;
    }

    private void Expander_Collapsed(object sender, RoutedEventArgs e)
    {
      OptionsExpander.Height = 23;
    }

    private void Selenium_Kicker_Closing(object sender, CancelEventArgs e)
    {
      _commands.Stop();
      Thread.Sleep(1000);
    }

    public void RestartAll()
    {
      CurrentUsername = Dispatcher.Invoke(new UpdateCallback(_commands.RestartAll), null) as string;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private delegate string UpdateCallback();


  }
}
