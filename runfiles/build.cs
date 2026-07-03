#!/usr/bin/env -S dotnet --
#:property TreatWarningsAsErrors=false
#:property CodeAnalysisTreatWarningsAsErrors=false
#:package TimeWarp.Amuru
#:property EnablePreviewFeatures=true
#:property NoWarn=CA1303;CA2007

// Build TimeWarp.Flexbox project
// Usage: ./runfiles/build.cs [configuration]
// Example: ./runfiles/build.cs Release

using TimeWarp.Amuru;

string configuration = args.Length > 0 ? args[0] : "Debug";

Console.WriteLine($"Building TimeWarp.Flexbox ({configuration})...");

await DotNet.Build()
  .WithConfiguration(configuration)
  .RunAsync();

Console.WriteLine($"Build completed successfully ({configuration})");
