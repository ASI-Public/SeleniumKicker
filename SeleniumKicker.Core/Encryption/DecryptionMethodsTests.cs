using NUnit.Framework;

namespace SeleniumKicker.Core.Encryption
{
  [TestFixture]
  public class DecryptionMethodsTests
  {
    [Test]
    public void EncryptString()
    {
      // Arrange
      const string origString = "Original string";
      string decString;
      byte[] encBytes;
      using (var dm = new DecryptionMethods())
      {
        // Act
        encBytes = dm.EncryptString(origString);
        decString = dm.DecryptString(encBytes);
      }

      // Assert
      Assert.IsNotNull(encBytes);
      Assert.AreEqual(origString, decString);
    }

    [Test]
    public void GetPublicKeyModulusTest()
    {
      // Arrange
      byte[] modulus;
      using (var dm = new DecryptionMethods())
      {
        // Act
        modulus = dm.GetPublicKeyModulus();
      }

      // Assert
      Assert.IsNotNull(modulus);
    }

    [Test]
    public void GetPublicKeyExponentTest()
    {
      // Arrange
      byte[] exponent;
      using (var dm = new DecryptionMethods())
      {
        // Act
        exponent = dm.GetPublicKeyExponent();
      }

      // Assert
      Assert.IsNotNull(exponent);
    }
  }
}
