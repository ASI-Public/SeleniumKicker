using NUnit.Framework;

namespace SeleniumKicker.Core.Processes
{
  [TestFixture]
  public class ProcessTest
  {
    [Test]
    public void NewHubTest()
    {
      // Arrange
      var hub = new Hub();

      // Act
      // Assert
      Assert.IsNotNull(hub);
    }

    [Test]
    public void ProcessStartTest()
    {
      // Arrange
      var hub = new Hub();

      // Act
      var user = hub.Start<ProcessWrapperMock>(null);

      // Assert
      Assert.AreEqual("testuser", user);
    }

    [Test]
    public void ProcessStopTest()
    {
      // Arrange
      var hub = new Hub();
      hub.Start<ProcessWrapperMock>(null);

      // Act
      hub.Stop();

      // Assert
      Assert.IsNotNull(hub);
    }
  }
}
