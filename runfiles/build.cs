#!/usr/bin/dotnet --
#:package TimeWarp.Amuru@1.0.0-beta.5

// Build TimeWarp.Flexbox project
// Usage: ./runfiles/build.cs [configuration]
// Example: ./runfiles/build.cs Release

using TimeWarp.Amuru;

string configuration = args.Length > 0 ? args[0] : "Debug";

Console.WriteLine($"Building TimeWarp.Flexbox ({configuration})...");

var result = await Shell.Builder("dotnet", "build")
  .WithArguments("--configuration", configuration)
  .RunAsync();

if (result.Success)
{
  Console.WriteLine($"✓ Build completed successfully ({configuration})");
  Environment.Exit(0);
}
else
{
  Console.WriteLine("✗ Build failed!");
  Environment.Exit(1);
}
