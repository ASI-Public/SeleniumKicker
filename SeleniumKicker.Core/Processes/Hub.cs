namespace SeleniumKicker.Core.Processes
{
  public class Hub : AbstractProcess
  {
    public Hub()
    {
      NodeName = "Hub";
      var arguments = "-jar " +
                      $"{FileHelpers.GetLatestVersion(HubFileName, false)} " +
                      "-role hub";
      CurrentArgs = arguments;
    }
  }
}
