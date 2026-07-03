#!/usr/bin/env -S dotnet --
#:package TimeWarp.Amuru
#:property EnablePreviewFeatures=true
#:property NoWarn=CA1303;CA2007

// Run TimeWarp.Flexbox tests using Fixie
// Usage: ./runfiles/test.cs [filter]
// Example: ./runfiles/test.cs
// Example: ./runfiles/test.cs --tests FlexValueTests.*

using TimeWarp.Amuru;
using static System.Console;

// Use ScriptContext to manage directory - automatically restores on dispose
using ScriptContext context = ScriptContext.FromRelativePath("..");

WriteLine($"Script location: {context.ScriptDirectory}");
WriteLine($"Working from: {Directory.GetCurrentDirectory()}");

// Restore tools
WriteLine("\nRestoring dotnet tools...");
await DotNet.Tool().Restore().RunAsync();

// Build arguments for fixie
List<string> fixieArgs = ["fixie", .. args];

// Run tests from test project directory
WriteLine("\nRunning TimeWarp.Flexbox tests...");
await Shell.Builder("dotnet")
  .WithArguments([.. fixieArgs])
  .WithWorkingDirectory("./test/timewarp-flexbox-tests")
  .RunAsync();

WriteLine("\nAll tests passed!");
