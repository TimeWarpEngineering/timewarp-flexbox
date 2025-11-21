# Runfiles Overview

This folder contains executable C# scripts for project automation and tooling.

## Purpose

Runfiles replace traditional shell scripts (.sh, .ps1) with C# scripts using .NET 10's native file-based app support:
- Cross-platform (Windows, Linux, macOS)
- Type-safe with full C# language features
- No external tools required (native .NET 10 SDK support)
- Better IDE support and debugging

## Common Runfiles

- `build.cs` - Build the project
- `test.cs` - Run tests
- `clean.cs` - Clean build artifacts
- `publish.cs` - Publish releases
- `format.cs` - Format code
- `lint.cs` - Run linters and analyzers

## .NET 10 File-Based Apps

Runfiles use .NET 10's native single-file C# script support with directives:

```csharp
#!/usr/bin/dotnet --
#:package TimeWarp.Amuru@1.0.0-beta.5

using TimeWarp.Amuru;

// Execute commands like shell scripts
await Shell.Builder("dotnet", "build").RunAsync();
```

### Key Directives
- `#:package PackageName@Version` - Add NuGet package
- `#:project path/to/project.csproj` - Reference project
- `#:property PropertyName=Value` - Set MSBuild property
- `#:sdk SdkName@Version` - Add SDK reference

**Note**: Use `@` for version specification, `=` for property assignment.

## Execution

Make scripts executable and run directly:

```bash
# Make executable
chmod +x runfiles/build.cs

# Run directly
./runfiles/build.cs

# Or use dotnet
dotnet run runfiles/build.cs
```

## Example: build.cs

```csharp
#!/usr/bin/dotnet --
#:package TimeWarp.Amuru@1.0.0-beta.5

using TimeWarp.Amuru;

Console.WriteLine("Building TimeWarp.Flexbox...");

var result = await Shell.Builder("dotnet", "build")
  .WithArguments("--configuration", "Release")
  .RunAsync();

if (result.Success)
{
  Console.WriteLine("Build completed successfully!");
}
else
{
  Console.WriteLine("Build failed!");
  Environment.Exit(1);
}
```

## Benefits Over Shell Scripts

### Cross-Platform
- No separate .sh and .ps1 versions needed
- Single C# file works everywhere

### Type-Safe
- Compile-time checking
- IntelliSense support
- Refactoring tools

### Maintainable
- Familiar C# syntax
- Better error handling
- Easier to test

### Powerful
- Full .NET libraries available
- NuGet package support
- Async/await for operations

## Best Practices

### Naming
- Use kebab-case: `build.cs`, `run-tests.cs`
- Descriptive names: what the script does
- Keep focused: one primary task per script

### Structure
```csharp
#!/usr/bin/dotnet --
// 1. Directives
#:package TimeWarp.Amuru@1.0.0-beta.5

// 2. Using statements
using TimeWarp.Amuru;

// 3. Script logic
Console.WriteLine("Starting...");
await DoWork();

// 4. Exit code
Environment.Exit(result.Success ? 0 : 1);
```

### Error Handling
- Always check command results
- Use meaningful exit codes (0 = success, non-zero = failure)
- Provide clear error messages

### Documentation
- Add comment header explaining purpose
- Document required parameters
- Show usage examples

## TimeWarp.Amuru

Runfiles use TimeWarp.Amuru for shell-like command execution:

```csharp
// Simple execution - streams to console
await Shell.Builder("dotnet", "build").RunAsync();

// Capture output
var result = await Shell.Builder("git", "status").CaptureAsync();
if (result.Success)
{
  Console.WriteLine(result.Stdout);
}

// Pipeline commands
await Shell.Builder("find", ".", "-name", "*.cs")
  .Pipe("grep", "async")
  .Pipe("wc", "-l")
  .CaptureAsync();
```

## Related Documentation

- See [Standards](../documentation/developer/standards/) for C# coding conventions
- See TimeWarp.Amuru documentation for shell execution API
- .NET 10 file-based apps: [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/)

## Migration from Shell Scripts

If you have existing shell scripts:

1. Create equivalent `.cs` file in `runfiles/`
2. Convert shell commands to `Shell.Builder()` calls
3. Add error handling and exit codes
4. Test on all target platforms
5. Remove old `.sh`/`.ps1` files
6. Update documentation references
