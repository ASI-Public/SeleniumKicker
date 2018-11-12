using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using SeleniumKicker.Core.Encryption;

namespace SeleniumKicker.Core.WebServer
{
  public static class CreateWebServer
  {
    private static readonly List<WebServer> WebServers = new List<WebServer>();
    private static readonly DecryptionMethods Dm = new DecryptionMethods();
    private static IHubCommands _commands;
    private static IUpdateUser _updateUser;

    public static void SetWebServer(IHubCommands commands, IUpdateUser updateUser)
    {
      _commands = commands;
      _updateUser = updateUser;
      WebServers.Add(new WebServer(KickResponse, "http://*:8080/Kick/"));
      WebServers.Add(new WebServer(PublicKeyModulusResponse, "http://*:8080/GetPublicKeyModulus/"));
      WebServers.Add(new WebServer(PublicKeyExponentResponse, "http://*:8080/GetPublicKeyExponent/"));
      WebServers.Add(new WebServer(SetUserResponse, "http://*:8080/SetUser/"));
      WebServers.Add(new WebServer(ResetUserResponse, "http://*:8080/ResetUser/"));
      foreach (var webServer in WebServers)
      {
        webServer.Run();
      }
    }

    private static byte[] KickResponse(HttpListenerRequest request)
    {
      _updateUser.RestartAll();
      
      return null;
    }

    private static byte[] PublicKeyModulusResponse(HttpListenerRequest request)
    {
      return Dm.GetPublicKeyModulus();
    }

    private static byte[] PublicKeyExponentResponse(HttpListenerRequest request)
    {
      return Dm.GetPublicKeyExponent();
    }

    private static byte[] SetUserResponse(HttpListenerRequest request)
    {
      byte[] username;
      using (var body = request.InputStream)
      {

        using (var reader = new BinaryReader(body, request.ContentEncoding))
        {
          username = reader.ReadBytes((int)request.ContentLength64);
        }
      }

      _commands.SetUser(Encoding.UTF8.GetString(username));
      return null;
    }

    private static byte[] ResetUserResponse(HttpListenerRequest request)
    {
      _commands.SetUser(string.Empty);
      return null;
    }

  }
}
