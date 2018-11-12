namespace SeleniumKicker.Core.Processes
{
  public class Opera : AbstractProcess
  {
    private const string OperaFileName = "Opera\\operadriver-x32-";
    
    public Opera()
    {
      NodeName = "Opera";
      var arguments = "-jar " +
                      // ReSharper disable once StringLiteralTypo
                      $"-Dwebdriver.opera.driver={FileHelpers.GetLatestVersion(OperaFileName)} " +
                      $"{FileHelpers.GetLatestVersion(HubFileName, false)} " +
                      "-role node " + 
                      "-port 5552 " +
                      "-browser browserName=opera,version=ANY,platform=WINDOWS";
      CurrentArgs = arguments;
    }

    public override void Stop()
    {
      base.Stop();
      foreach (var proc in Process.GetProcessesByNameWrapper("opera"))
      {
        proc.Kill();
      }
    }
  }
}
