#region Purpose
// Finds the runfiles and projects under samples/ and reads each sample's TFM and arguments.
#endregion
#region Design
// A runfile is a .cs file whose first code line is a dotnet shebang or a #: directive.
// A .cs file inside a directory that contains a *.csproj is source for that project.
// Paths under bin/ or obj/ are skipped.
// Runfile TFMs come from #:property TargetFramework(s). Project TFMs come from the
// last TargetFramework(s) element in the csproj. OutputType Exe or WinExe is runnable;
// anything else is a library, matching the SDK default OutputType of Library.
// verify-samples.args in the sample directory is the argument template. {outdir} is
// replaced before {out} because the directory token contains the file token.
// A token that still contains {out} after that is a file the sample must create.
#endregion

namespace DevCli.Commands;

internal static class SampleDiscovery
{
  private const string ArgumentFileName = "verify-samples.args";

  internal static List<SampleEntry> Discover(string samplesDirectory)
  {
    List<SampleEntry> samples = [];
    foreach (string project in Directory.EnumerateFiles(samplesDirectory, "*.csproj", SearchOption.AllDirectories))
    {
      if (IsUnderBuildOutput(project))
      {
        continue;
      }

      string? projectDirectory = Path.GetDirectoryName(project);
      samples.Add(new SampleEntry(
        project,
        IsRunfile: false,
        RunAfterBuild: IsExecutableProject(project),
        Frameworks: ReadProjectFrameworks(project),
        ArgumentTemplate: ReadArgumentTemplate(projectDirectory!)));
    }

    foreach (string source in Directory.EnumerateFiles(samplesDirectory, "*.cs", SearchOption.AllDirectories))
    {
      if (IsUnderBuildOutput(source) || IsInsideProject(source, samplesDirectory) || !IsFileBasedApp(source))
      {
        continue;
      }

      string? sourceDirectory = Path.GetDirectoryName(source);
      samples.Add(new SampleEntry(
        source,
        IsRunfile: true,
        RunAfterBuild: true,
        Frameworks: ReadRunfileFrameworks(source),
        ArgumentTemplate: ReadArgumentTemplate(sourceDirectory!)));
    }

    samples.Sort((left, right) => string.CompareOrdinal(left.EntryPath, right.EntryPath));
    return samples;
  }

  internal static string[] BindArguments(
    string[] template,
    string outputFile,
    string outputDirectory,
    out string[] expectedOutputs)
  {
    List<string> bound = [];
    List<string> expected = [];
    foreach (string token in template)
    {
      string replaced = token
        .Replace("{outdir}", outputDirectory, StringComparison.Ordinal)
        .Replace("{out}", outputFile, StringComparison.Ordinal);
      bound.Add(replaced);
      string withoutDirectory = token.Replace("{outdir}", string.Empty, StringComparison.Ordinal);
      if (withoutDirectory.Contains("{out}", StringComparison.Ordinal))
      {
        expected.Add(replaced);
      }
    }

    expectedOutputs = [.. expected];
    return [.. bound];
  }

  private static bool IsFileBasedApp(string path)
  {
    foreach (string raw in File.ReadLines(path).Take(80))
    {
      string line = raw.Trim();
      if (line.Length == 0 || line.StartsWith("//", StringComparison.Ordinal))
      {
        continue;
      }

      if (line.StartsWith("#!", StringComparison.Ordinal) && line.Contains("dotnet", StringComparison.Ordinal))
      {
        return true;
      }

      if (line.StartsWith("#:", StringComparison.Ordinal))
      {
        return true;
      }

      return false;
    }

    return false;
  }

  private static bool IsUnderBuildOutput(string path)
  {
    return path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
      || path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal);
  }

  private static bool IsInsideProject(string filePath, string samplesDirectory)
  {
    string? directory = Path.GetDirectoryName(filePath);
    string samplesFull = Path.GetFullPath(samplesDirectory);
    while (!string.IsNullOrEmpty(directory))
    {
      string full = Path.GetFullPath(directory);
      if (Directory.EnumerateFiles(full, "*.csproj").Any())
      {
        return true;
      }

      if (string.Equals(full, samplesFull, StringComparison.Ordinal))
      {
        return false;
      }

      directory = Path.GetDirectoryName(full);
    }

    return false;
  }

  private static string[] ReadArgumentTemplate(string directory)
  {
    string path = Path.Combine(directory, ArgumentFileName);
    if (!File.Exists(path))
    {
      return [];
    }

    List<string> tokens = [];
    foreach (string raw in File.ReadAllLines(path))
    {
      string line = raw.Trim();
      if (line.Length == 0 || line.StartsWith('#'))
      {
        continue;
      }

      tokens.AddRange(line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    return [.. tokens];
  }

  private static string[] ReadRunfileFrameworks(string path)
  {
    string? single = null;
    string? multiple = null;
    foreach (string raw in File.ReadAllLines(path))
    {
      string line = raw.Trim();
      if (TryReadProperty(line, "TargetFrameworks", out string? frameworks))
      {
        multiple = frameworks;
      }
      else if (TryReadProperty(line, "TargetFramework", out string? framework))
      {
        single = framework;
      }
    }

    if (!string.IsNullOrEmpty(multiple))
    {
      return SplitFrameworks(multiple);
    }

    return string.IsNullOrEmpty(single) ? [] : [single];
  }

  private static bool TryReadProperty(string line, string name, out string? value)
  {
    string prefix = $"#:property {name}=";
    if (!line.StartsWith(prefix, StringComparison.Ordinal))
    {
      value = null;
      return false;
    }

    value = CutPropertyValue(line[prefix.Length..]);
    return true;
  }

  private static string[] ReadProjectFrameworks(string projectPath)
  {
    XDocument document = XDocument.Load(projectPath);
    string? multiple = document.Descendants("TargetFrameworks")
      .Select(element => element.Value.Trim())
      .LastOrDefault(value => value.Length > 0);
    if (!string.IsNullOrEmpty(multiple))
    {
      return SplitFrameworks(multiple);
    }

    string? single = document.Descendants("TargetFramework")
      .Select(element => element.Value.Trim())
      .LastOrDefault(value => value.Length > 0);
    return string.IsNullOrEmpty(single) ? [] : [single];
  }

  private static string[] SplitFrameworks(string value)
  {
    return value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
  }

  private static string CutPropertyValue(string value)
  {
    string trimmed = value.Trim();
    int end = trimmed.IndexOfAny([' ', '\t', '#']);
    return end < 0 ? trimmed : trimmed[..end];
  }

  private static bool IsExecutableProject(string projectPath)
  {
    return XDocument.Load(projectPath)
      .Descendants("OutputType")
      .Any(element =>
        string.Equals(element.Value.Trim(), "Exe", StringComparison.OrdinalIgnoreCase)
        || string.Equals(element.Value.Trim(), "WinExe", StringComparison.OrdinalIgnoreCase));
  }
}

internal sealed record SampleEntry(
  string EntryPath,
  bool IsRunfile,
  bool RunAfterBuild,
  string[] Frameworks,
  string[] ArgumentTemplate);
