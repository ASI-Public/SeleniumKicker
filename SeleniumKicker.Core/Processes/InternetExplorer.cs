namespace SeleniumKicker.Core.Processes
{
  public class InternetExplorer : AbstractProcess
  {
    private const string IeFileName = "IE\\IEDriverServer-x32-";
    public InternetExplorer()
    {
      NodeName = "IE";
      var arguments = "-jar " +
                      // ReSharper disable once StringLiteralTypo
                      $"-Dwebdriver.ie.driver={FileHelpers.GetLatestVersion(IeFileName)} " +
                      $"{FileHelpers.GetLatestVersion(HubFileName, false)} " +
                      "-role node " + 
                      "-port 5555 " +
                      "-browser \"browserName=internet explorer,version=ANY,platform=WINDOWS\"";
      CurrentArgs = arguments;
    }
  }
}
