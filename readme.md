# TimeWarp.Flexbox

A pure C# implementation of the Flexbox layout algorithm for .NET applications.

## Installation

This is a private package hosted on GitHub Packages. To consume it, you need to configure authentication.

### 1. Create a Personal Access Token (PAT)

1. Go to [GitHub Settings > Developer settings > Personal access tokens](https://github.com/settings/tokens)
2. Generate a new token (classic) with `read:packages` scope
3. Copy the token

### 2. Configure nuget.config

Create a `nuget.config` file in your repository root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="github-timewarp" value="https://nuget.pkg.github.com/TimeWarpEngineering/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github-timewarp>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_PAT" />
    </github-timewarp>
  </packageSourceCredentials>
</configuration>
```

**Do not commit credentials to git.** Use one of these approaches:

- Use environment variables in CI/CD
- Use a user-level nuget.config (`~/.nuget/NuGet/NuGet.Config`)
- Use `dotnet nuget add source` command

### 3. GitHub Actions Authentication

For consuming this package in GitHub Actions workflows:

```yaml
- name: Authenticate to GitHub Packages
  run: |
    dotnet nuget add source \
      --username ${{ github.actor }} \
      --password ${{ secrets.GITHUB_TOKEN }} \
      --store-password-in-clear-text \
      --name github-timewarp \
      "https://nuget.pkg.github.com/TimeWarpEngineering/index.json"
```

Note: `GITHUB_TOKEN` works for repositories within the same organization. For external repositories, use a PAT stored as a secret.

### 4. Add Package Reference

```xml
<PackageReference Include="TimeWarp.Flexbox" Version="1.0.0-beta.1" />
```

## Usage

```csharp
using TimeWarp.Flexbox;

// Create a flex container
var root = new FlexNode
{
    Width = 300,
    Height = 200,
    FlexDirection = FlexDirection.Row
};

// Add children
root.AddChild(new FlexNode { FlexGrow = 1 });
root.AddChild(new FlexNode { FlexGrow = 2 });

// Calculate layout
FlexLayoutEngine.CalculateLayout(root);

// Access computed layout
Console.WriteLine($"Child 0: {root[0].LayoutResult}");
Console.WriteLine($"Child 1: {root[1].LayoutResult}");
```

## License

Unlicense - See [LICENSE](LICENSE) for details.
