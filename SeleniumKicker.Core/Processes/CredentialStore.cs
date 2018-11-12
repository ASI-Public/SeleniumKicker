using System;
using System.DirectoryServices.AccountManagement;
using CredentialManagement;
using SeleniumKicker.Core.Encryption;

namespace SeleniumKicker.Core.Processes
{
  public static class CredentialStore
  {
    public static bool SetCredentials(
      string target, string username, string password)
    {
      if (!IsValidUser(username, password)) return false;

      string encryptedPass;
      using (var dm = new DecryptionMethods())
      {
        encryptedPass = Convert.ToBase64String(dm.EncryptString(password));
      }
      return new Credential {Target = target, Username = username, 
        Password = encryptedPass, PersistanceType = PersistanceType.LocalComputer}.Save();
    }

    public static Credential GetCredential(string username)
    {
      var cm = new Credential { Target = username };
      cm.Load();
      string decryptedPass;
      using (var dm = new DecryptionMethods())
      {
        decryptedPass = dm.DecryptString(Convert.FromBase64String(cm.Password));
      }

      cm.Password = decryptedPass;
      return cm;
    }

    public static bool ValidateUser(string username)
    {
      var cm = new Credential {Target = username};
      if (!cm.Load()) return false;
      string decryptedPass;
      using (var dm = new DecryptionMethods())
      {
        decryptedPass = dm.DecryptString(Convert.FromBase64String(cm.Password));
      }
      return IsValidUser(cm.Username, decryptedPass);
    }

    private static bool IsValidUser(string username, string password)
    {
      ContextType ct;
      string user;
      string domain = null;
      if (username.Contains("\\"))
      {
        ct = ContextType.Machine;
        user = username;
      }
      else
      {
        ct = ContextType.Domain;
        domain = username.Split('\\')[0];
        user = username.Split('\\')[1];
      }

      using (var pc = new PrincipalContext(ct, domain))
      {
        return pc.ValidateCredentials(user, password);
      }
    }
  }
}
