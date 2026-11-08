# CI/CD Setup for Private NuGet Registry with GitHub

## Executive Summary

Yes, GitHub provides a **built-in private NuGet registry** called **GitHub Packages** that integrates seamlessly with GitHub Actions for CI/CD. This is the recommended approach for TimeWarp.Flexbox. Authentication uses the automatic `GITHUB_TOKEN` within workflows, requiring no additional secrets for publishing to your own organization's registry.

## Scope

This report covers:
- GitHub Packages NuGet registry setup
- GitHub Actions workflow for automated NuGet publishing
- Authentication and permissions configuration
- Best practices for versioning and releases

## Options for Private NuGet Registries

| Option | Cost | Integration | Setup Complexity |
|--------|------|-------------|------------------|
| **GitHub Packages** | Free (included) | Native GitHub Actions | Low |
| Azure Artifacts | Free tier available | Good | Medium |
| MyGet | Paid | Good | Medium |
| Self-hosted (BaGet) | Server costs | Manual | High |

**Recommendation**: GitHub Packages - native integration, no additional cost, automatic authentication.

## Implementation Guide

### 1. Project Configuration

Your `.csproj` file needs package metadata. Update `source/timewarp-flexbox/timewarp-flexbox.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <RootNamespace>TimeWarp.Flexbox</RootNamespace>
    <AssemblyName>TimeWarp.Flexbox</AssemblyName>
    <Description>A pure C# implementation of the Flexbox layout algorithm for .NET applications</Description>
    <PackageTags>flexbox;layout;css;yoga;ui</PackageTags>
    
    <!-- Enable packing for this project -->
    <IsPackable>true</IsPackable>
    
    <!-- Package metadata -->
    <PackageId>TimeWarp.Flexbox</PackageId>
    <Authors>TimeWarp Engineering</Authors>
    <Company>TimeWarp Engineering</Company>
    <PackageProjectUrl>https://github.com/TimeWarpEngineering/timewarp-flexbox</PackageProjectUrl>
    <RepositoryUrl>https://github.com/TimeWarpEngineering/timewarp-flexbox</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageReadmeFile>readme.md</PackageReadmeFile>
    
    <!-- Version - consider using MinVer or GitVersion for automated versioning -->
    <Version>1.0.0</Version>
  </PropertyGroup>

  <ItemGroup>
    <None Include="../../readme.md" Pack="true" PackagePath="/"/>
  </ItemGroup>

</Project>
```

### 2. GitHub Actions Workflow

Create `.github/workflows/publish-nuget.yml`:

```yaml
name: Publish NuGet Package

on:
  push:
    tags:
      - 'v*'  # Trigger on version tags like v1.0.0, v1.0.0-beta.1
  workflow_dispatch:  # Allow manual triggering
    inputs:
      version:
        description: 'Package version (e.g., 1.0.0 or 1.0.0-beta.1)'
        required: true
        type: string

env:
  DOTNET_VERSION: '10.0.x'
  NUGET_SOURCE: 'https://nuget.pkg.github.com/TimeWarpEngineering/index.json'

jobs:
  build-and-publish:
    runs-on: ubuntu-latest
    
    permissions:
      contents: read
      packages: write  # Required for publishing to GitHub Packages
    
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          fetch-depth: 0  # Full history for versioning
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}
      
      - name: Determine version
        id: version
        run: |
          if [ "${{ github.event_name }}" == "workflow_dispatch" ]; then
            echo "version=${{ inputs.version }}" >> $GITHUB_OUTPUT
          else
            # Extract version from tag (remove 'v' prefix)
            VERSION="${GITHUB_REF#refs/tags/v}"
            echo "version=$VERSION" >> $GITHUB_OUTPUT
          fi
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --configuration Release --no-restore
      
      - name: Run tests
        run: dotnet test --configuration Release --no-build --verbosity normal
      
      - name: Pack NuGet package
        run: |
          dotnet pack source/timewarp-flexbox/timewarp-flexbox.csproj \
            --configuration Release \
            --no-build \
            --output ./artifacts \
            -p:PackageVersion=${{ steps.version.outputs.version }}
      
      - name: Add GitHub Packages source
        run: |
          dotnet nuget add source \
            --username ${{ github.actor }} \
            --password ${{ secrets.GITHUB_TOKEN }} \
            --store-password-in-clear-text \
            --name github \
            "${{ env.NUGET_SOURCE }}"
      
      - name: Publish to GitHub Packages
        run: |
          dotnet nuget push ./artifacts/*.nupkg \
            --source "github" \
            --skip-duplicate
      
      - name: Upload artifacts
        uses: actions/upload-artifact@v4
        with:
          name: nuget-packages
          path: ./artifacts/*.nupkg
          retention-days: 30
```

### 3. CI Workflow (Build and Test)

Create `.github/workflows/ci.yml` for pull requests and main branch:

```yaml
name: CI

on:
  push:
    branches: [ main, master ]
  pull_request:
    branches: [ main, master ]

env:
  DOTNET_VERSION: '10.0.x'

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --configuration Release --no-restore
      
      - name: Test
        run: dotnet test --configuration Release --no-build --verbosity normal
      
      - name: Pack (validation only)
        run: |
          dotnet pack source/timewarp-flexbox/timewarp-flexbox.csproj \
            --configuration Release \
            --no-build \
            --output ./artifacts
```

### 4. Consuming the Private Package

#### Option A: Using `nuget.config` (Recommended)

Create a `nuget.config` in your consuming project's root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="github" value="https://nuget.pkg.github.com/TimeWarpEngineering/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_PAT_WITH_READ_PACKAGES" />
    </github>
  </packageSourceCredentials>
</configuration>
```

#### Option B: Environment Variable Authentication

```bash
# Set environment variable
export NUGET_AUTH_TOKEN=your_github_pat

# Configure source
dotnet nuget add source \
  --username USERNAME \
  --password $NUGET_AUTH_TOKEN \
  --store-password-in-clear-text \
  --name github \
  "https://nuget.pkg.github.com/TimeWarpEngineering/index.json"
```

#### Option C: GitHub Actions (for CI in consuming repos)

```yaml
- name: Authenticate to GitHub Packages
  run: |
    dotnet nuget add source \
      --username ${{ github.actor }} \
      --password ${{ secrets.GITHUB_TOKEN }} \
      --store-password-in-clear-text \
      --name github \
      "https://nuget.pkg.github.com/TimeWarpEngineering/index.json"
```

### 5. Publishing to NuGet.org (Optional - Public Release)

For public releases, add a step to publish to nuget.org:

```yaml
- name: Publish to NuGet.org
  if: ${{ !contains(steps.version.outputs.version, '-') }}  # Skip pre-release versions
  run: |
    dotnet nuget push ./artifacts/*.nupkg \
      --api-key ${{ secrets.NUGET_API_KEY }} \
      --source https://api.nuget.org/v3/index.json \
      --skip-duplicate
```

**Note**: You'll need to create a `NUGET_API_KEY` secret in your repository settings with an API key from nuget.org.

## Versioning Strategies

### Option 1: Git Tags (Manual)

```bash
# Create and push a tag
git tag v1.0.0
git push origin v1.0.0
```

### Option 2: MinVer (Automated from Git)

Add to your `.csproj`:

```xml
<PackageReference Include="MinVer" Version="5.0.0" PrivateAssets="all" />
```

MinVer automatically derives version from git tags and commit height.

### Option 3: GitHub Release Workflow

Use GitHub's release feature to create releases with tags, which triggers the publish workflow.

## Security Considerations

| Aspect | Recommendation |
|--------|----------------|
| Authentication | Use `GITHUB_TOKEN` in workflows (automatic, scoped) |
| PAT Scope | For manual access, use `read:packages` (read) or `write:packages` (publish) |
| Package Visibility | Default is private; can be made public in package settings |
| Cross-repo Access | Requires explicit permission grants or PAT with appropriate scope |

## Package Visibility Options

After publishing, configure visibility in GitHub:

1. Navigate to: `https://github.com/orgs/TimeWarpEngineering/packages`
2. Select your package
3. Package Settings > Danger Zone > Change visibility

Options:
- **Private**: Only organization members with explicit access
- **Internal**: All organization members (Enterprise only)
- **Public**: Anyone can view and download

## Release Checklist

- [ ] Update version in `.csproj` or use automated versioning
- [ ] Ensure all tests pass
- [ ] Update changelog/release notes
- [ ] Create git tag: `git tag v1.0.0`
- [ ] Push tag: `git push origin v1.0.0`
- [ ] Verify package appears in GitHub Packages
- [ ] Test installation in a consuming project

## Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| 401 Unauthorized | Check PAT has `write:packages` scope |
| Package not found | Verify package visibility and consumer authentication |
| Version conflict | Use `--skip-duplicate` flag or increment version |
| .NET version mismatch | Ensure SDK version matches in workflow and project |

### Useful Commands

```bash
# List configured NuGet sources
dotnet nuget list source

# Remove a source
dotnet nuget remove source github

# Clear NuGet cache
dotnet nuget locals all --clear

# Test package creation locally
dotnet pack --configuration Release --output ./artifacts
```

## References

- [GitHub Packages NuGet Registry](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry)
- [Publishing with GitHub Actions](https://docs.github.com/en/packages/managing-github-packages-using-github-actions-workflows/publishing-and-installing-a-package-with-github-actions)
- [NuGet Pack Command](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-pack)
- [MinVer Versioning](https://github.com/adamralph/minver)

---

*Generated: 2024-12-06*
