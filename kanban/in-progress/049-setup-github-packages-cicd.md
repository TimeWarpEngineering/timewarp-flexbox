# Task 049-setup-github-packages-cicd

## Summary
Set up CI/CD pipeline to publish TimeWarp.Flexbox as a **private** NuGet package to GitHub Packages, and document how consuming projects authenticate and restore the package.

## Todo List
- [x] Create `.github/workflows/ci-cd.yml`
  - Build and test on push/PR
  - Publish to GitHub Packages on release
- [x] Add consumer documentation to `readme.md` or `documentation/`
  - Example `nuget.config` for consuming repos
  - How to create a PAT with `read:packages` scope
  - GitHub Actions authentication for consuming repos
- [ ] Test the full flow: build, publish, consume from another project

## Existing Configuration
Package metadata already configured in `source/Directory.Build.props`:
- `IsPackable` = true
- `RepositoryUrl` = https://github.com/TimeWarpEngineering/timewarp-flexbox
- `Version` = 1.0.0-beta.1
- `GeneratePackageOnBuild` = true
- Package icon, readme, license, Source Link all configured

## Notes

### nuget.config for Consumers

Projects that consume this private package need a `nuget.config`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="github-timewarp" value="https://nuget.pkg.github.com/TimeWarpEngineering/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github-timewarp>
      <add key="Username" value="USERNAME" />
      <add key="ClearTextPassword" value="PAT_WITH_READ_PACKAGES" />
    </github-timewarp>
  </packageSourceCredentials>
</configuration>
```

**Authentication Requirements**:
- GitHub account
- Personal Access Token (PAT) with `read:packages` scope
- DO NOT commit credentials to git - use environment variables or user-level nuget.config

### GitHub Actions Workflow

```yaml
name: CI/CD

on:
  push:
    branches: [master, main]
    paths:
      - 'source/**'
      - 'test/**'
      - '.github/workflows/**'
      - 'Directory.Build.props'
      - 'Directory.Packages.props'
  pull_request:
    branches: [master, main]
    paths:
      - 'source/**'
      - 'test/**'
      - '.github/workflows/**'
      - 'Directory.Build.props'
      - 'Directory.Packages.props'
  release:
    types: [published]
  workflow_dispatch:

jobs:
  build-and-publish:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
      
      - name: Restore
        run: dotnet restore
      
      - name: Build
        run: dotnet build --configuration Release --no-restore
      
      - name: Test
        run: dotnet test --configuration Release --no-build
      
      - name: Publish to GitHub Packages
        if: github.event_name == 'release' || github.event_name == 'workflow_dispatch'
        run: |
          dotnet nuget add source \
            --username ${{ github.actor }} \
            --password ${{ secrets.GITHUB_TOKEN }} \
            --store-password-in-clear-text \
            --name github \
            "https://nuget.pkg.github.com/TimeWarpEngineering/index.json"
          
          dotnet nuget push artifacts/packages/*.nupkg --source "github" --skip-duplicate
```

### Key Points
- `GITHUB_TOKEN` is automatic - no secrets to configure for publishing
- `permissions: packages: write` is required
- Consumers need a PAT with `read:packages` scope
- Package visibility defaults to private (can be changed in GitHub package settings)

## Results
(Add after completion)
