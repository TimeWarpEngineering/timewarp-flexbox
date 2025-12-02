# Task 013-create-test-project-structure

## Summary
Create the unit test project structure for TimeWarp.Flexbox with TimeWarp.Fixie, Shouldly, and initial test organization.

## Todo List
- [x] Create test/timewarp-flexbox-tests/timewarp-flexbox-tests.csproj
- [x] Add Fixie.TestAdapter, Shouldly, and coverlet packages
- [x] Reference source/timewarp-flexbox project
- [x] Create test/Directory.Build.props with analyzer suppressions
- [x] Create test folder structure: tests/values/
- [x] Add test/timewarp-flexbox-tests/global-usings.cs with test usings
- [x] Create sample test file (flex-value-tests.cs) to verify setup works
- [x] Add test project to solution file
- [x] Verify `dotnet test` runs successfully (6 tests pass)

## Notes
Test project setup:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="..." />
    <PackageReference Include="TimeWarp.Fixie" Version="..." />
    <PackageReference Include="Fixie.TestAdapter" Version="..." />
    <PackageReference Include="Shouldly" Version="..." />
    <PackageReference Include="coverlet.collector" Version="..." />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\..\source\TimeWarp.Flexbox\TimeWarp.Flexbox.csproj" />
  </ItemGroup>
</Project>
```

Create dotnet tool manifest at `.config/dotnet-tools.json`:
```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "fixie.console": {
      "version": "...",
      "commands": ["fixie"]
    }
  }
}
```

Then run: `dotnet tool restore`

TestingConvention.cs (place in test project root):
```csharp
namespace TimeWarp.Flexbox.Tests;

using TimeWarp.Fixie;

public class TestingConvention : TimeWarp.Fixie.TestingConvention;
```

GlobalUsings.cs:
```csharp
global using Shouldly;
global using TimeWarp.Fixie;
global using TimeWarp.Flexbox;
```

Test conventions:
- Public methods in test classes are tests (no [Test] attribute needed)
- Use [Skip("reason")] for skipped tests
- Use [TestTag(TestTags.Fast)] for tagging tests
- Use [Input(...)] for parameterized tests
- Setup and Cleanup methods are lifecycle methods (no attributes needed)
- Use [NotTest] attribute for public classes that aren't tests

Run tests with: `dotnet fixie` (instead of `dotnet test`)

## Results
- Created test/timewarp-flexbox-tests project with kebab-case naming
- Using Fixie.TestAdapter 4.0.0 with default conventions (no TimeWarp.Fixie due to compatibility issues)
- Created test/Directory.Build.props to suppress CA1515, CA1822, CA2007, CA2252 for test projects
- Updated Directory.Packages.props with test package versions
- Created values/flex-value-tests.cs with 6 FlexValue tests
- All 6 tests pass with `dotnet test`
- Deviation: Skipped TimeWarp.Fixie due to API incompatibility with Fixie 4.0.0
- Deviation: Skipped dotnet tool manifest (not needed for `dotnet test`)
