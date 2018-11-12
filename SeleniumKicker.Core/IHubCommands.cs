namespace SeleniumKicker.Core
{
  public interface IHubCommands
  {
    string Start();
    void Stop(int index = 0);
    string RestartAll();
    string RestartNodes();
    bool SetUser(string username);
    bool AddUser(string username, string password);
  }
}
