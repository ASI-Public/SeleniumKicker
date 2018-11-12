namespace SeleniumKicker.Core.Processes
{
  public class Chrome : AbstractProcess
  {
    private const string ChromeFileName = "Chrome\\chromedriver-x32-";
    public Chrome()
    {
      NodeName = "Chrome";
      var arguments = "-jar " +
                      // ReSharper disable once StringLiteralTypo
                      $"-Dwebdriver.chrome.driver={FileHelpers.GetLatestVersion(ChromeFileName)} " +
                      $"{FileHelpers.GetLatestVersion(HubFileName, false)} " +
                      "-role node " + 
                      "-port 5551 " +
                      "-browser browserName=chrome,version=ANY,platform=WINDOWS";
      CurrentArgs =  arguments;
    }
  }
}
