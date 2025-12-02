# Task 013-create-test-project-structure

## Summary
Create the unit test project structure for TimeWarp.Flexbox with TimeWarp.Fixie, Shouldly, and initial test organization.

## Todo List
- [ ] Create test/TimeWarp.Flexbox.Tests/TimeWarp.Flexbox.Tests.csproj
- [ ] Add TimeWarp.Fixie, Fixie.TestAdapter, Shouldly, and coverlet packages
- [ ] Reference source/TimeWarp.Flexbox project
- [ ] Create dotnet tool manifest with Fixie.Console
- [ ] Create TestingConvention class inheriting from TimeWarp.Fixie.TestingConvention
- [ ] Create test folder structure mirroring source: Tests/{Enums,Values,Nodes,Layout}
- [ ] Add test/TimeWarp.Flexbox.Tests/GlobalUsings.cs with test usings
- [ ] Create sample test file to verify setup works
- [ ] Add test project to solution file
- [ ] Verify `dotnet fixie` runs successfully

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
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
