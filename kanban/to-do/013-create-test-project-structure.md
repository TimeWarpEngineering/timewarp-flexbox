# Task 013-create-test-project-structure

## Summary
Create the unit test project structure for TimeWarp.Flexbox with xUnit and initial test organization.

## Todo List
- [ ] Create test/TimeWarp.Flexbox.Tests/TimeWarp.Flexbox.Tests.csproj
- [ ] Add xUnit, FluentAssertions, and coverlet packages
- [ ] Reference source/TimeWarp.Flexbox project
- [ ] Create test folder structure mirroring source: Tests/{Enums,Values,Nodes,Layout}
- [ ] Add test/TimeWarp.Flexbox.Tests/GlobalUsings.cs with test usings
- [ ] Create sample test file to verify setup works
- [ ] Add test project to solution file
- [ ] Verify `dotnet test` runs successfully

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
    <PackageReference Include="xunit" Version="..." />
    <PackageReference Include="xunit.runner.visualstudio" Version="..." />
    <PackageReference Include="FluentAssertions" Version="..." />
    <PackageReference Include="coverlet.collector" Version="..." />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\..\source\TimeWarp.Flexbox\TimeWarp.Flexbox.csproj" />
  </ItemGroup>
</Project>
```

GlobalUsings.cs:
```csharp
global using Xunit;
global using FluentAssertions;
global using TimeWarp.Flexbox;
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
