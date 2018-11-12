namespace SeleniumKicker.Core.Processes
{
  public class Firefox :AbstractProcess
  {
    private const string FirefoxFileName = "Gecko\\geckodriver-x64-";
    public Firefox()
    {
      NodeName = "Firefox";
      var arguments = "-jar " +
                      // ReSharper disable once StringLiteralTypo
                      $"-Dwebdriver.gecko.driver={FileHelpers.GetLatestVersion(FirefoxFileName)} " +
                      $"{FileHelpers.GetLatestVersion(HubFileName, false)} " +
                      "-role node " + 
                      "-port 5554 " +
                      "-browser browserName=firefox,version=ANY,platform=WINDOWS";
      CurrentArgs = arguments;
    }
  }
}
