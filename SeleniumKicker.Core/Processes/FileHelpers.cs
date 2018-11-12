using System;
using System.IO;
using System.Linq;

namespace SeleniumKicker.Core.Processes
{
  internal static class FileHelpers
  {
    internal static string GetLatestVersion(string fileName, bool isNode = true)
    {
      var subfolder = string.Empty;
      if (isNode)
      {
        var fileNameParts = fileName.Split('\\');
        subfolder = fileNameParts[0];
        fileName = fileNameParts[1];
      }

      var combine = Path.Combine(Properties.Settings.Default.SeleniumPath, isNode ? $"Driver\\{subfolder}" : "Server");
      var extension = isNode ? "exe" : "jar";
      var versions = Directory.GetFiles(combine, $"{fileName}*.{extension}");
      var maxVersion = string.Empty;
      foreach (var version in versions)
      {
        var versionCurrent = ParseVersion(version);
        var versionMax = ParseVersion(maxVersion);
        if (versionCurrent > versionMax)
        {
          maxVersion = version;
        }
      }

      return maxVersion;
    }

    private static Version ParseVersion(string ver)
    {
      var parts = Path.GetFileNameWithoutExtension(ver)?.Replace("-beta",".").Split('-').Last().Split('.');
      var versionParts = new [] {0,0,0,0};
      if (parts == null) return new Version(versionParts[0], versionParts[1], versionParts[2], versionParts[3]);
      for (var i = 0; i < parts.Length; i++)
      {
        if (int.TryParse(parts[i], out var originalPart))
        {
          versionParts[i] = originalPart;
        }
        else
        {
          if (int.TryParse(parts[i].Replace("-beta", ""), out var intPart))
          {
            versionParts[i] = intPart;
          }
        }
      }

      return new Version(versionParts[0], versionParts[1], versionParts[2], versionParts[3]);
    }
  }
}
