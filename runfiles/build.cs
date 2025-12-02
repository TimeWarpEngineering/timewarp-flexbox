#!/usr/bin/dotnet --
#:package TimeWarp.Amuru
#:property EnablePreviewFeatures=true
#:property NoWarn=CA1303;CA2007

// Build TimeWarp.Flexbox project
// Usage: ./runfiles/build.cs [configuration]
// Example: ./runfiles/build.cs Release

using TimeWarp.Amuru;

string configuration = args.Length > 0 ? args[0] : "Debug";

Console.WriteLine($"Building TimeWarp.Flexbox ({configuration})...");

CommandOutput result = await Shell.Builder("dotnet")
  .WithArguments("build", "--configuration", configuration)
  .CaptureAsync();

if (result.Success)
{
  Console.WriteLine($"Build completed successfully ({configuration})");
  Environment.Exit(0);
}
else
{
  Console.WriteLine("Build failed!");
  Console.WriteLine(result.Stderr);
  Environment.Exit(1);
}
