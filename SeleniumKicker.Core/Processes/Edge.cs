namespace SeleniumKicker.Core.Processes
{
  public class Edge : AbstractProcess
  {
    private const string EdgeFileName = "Edge\\MicrosoftWebDriver-x32-";
    public Edge()
    {
      NodeName = "Edge";
      var arguments = "-jar " +
                      // ReSharper disable once StringLiteralTypo
                      $"-Dwebdriver.edge.driver={FileHelpers.GetLatestVersion(EdgeFileName)} " +
                      $"{FileHelpers.GetLatestVersion(HubFileName, false)} " +
                      "-role node " + 
                      "-port 5556 " +
                      "-browser browserName=MicrosoftEdge,version=ANY,platform=WINDOWS";
      CurrentArgs = arguments;
    }
  }
}
