using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SeleniumKicker.Core.Encryption
{
  internal class DecryptionMethods : IDisposable
  {
    private readonly RSACryptoServiceProvider _rsa;
    internal DecryptionMethods()
    {
      var csp = new CspParameters
      {
        KeyContainerName = GetApplicationId().ToString()
      };
      _rsa = new RSACryptoServiceProvider(csp);
    }

    internal byte[] GetPublicKeyModulus()
    {
      return _rsa.ExportParameters(false).Modulus;
    }

    internal byte[] GetPublicKeyExponent()
    {
      return _rsa.ExportParameters(false).Exponent;
    }

    internal string DecryptString(byte[] encryptedString)
    {
      return Encoding.UTF8.GetString(_rsa.Decrypt(encryptedString, false));
    }

    internal byte[] EncryptString(string stringToEncrypt)
    {
      return _rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), false);
    }

    private Guid GetApplicationId()
    {
      var md5Hasher = MD5.Create();
      var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(
        Assembly.GetCallingAssembly().GetName().Name));
      return new Guid(data);
    }

    public void Dispose()
    {
      _rsa?.Dispose();
    }
  }
}
