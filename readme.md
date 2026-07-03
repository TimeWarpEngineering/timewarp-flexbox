# TimeWarp.Flexbox

A pure C# implementation of the Flexbox layout algorithm for .NET applications.

TimeWarp.Flexbox is a from-scratch C# port of the algorithm behind Facebook's
[Yoga](https://github.com/facebook/yoga) layout engine. It computes CSS-flexbox
layouts (positions and sizes) for a tree of nodes — no UI framework required —
and its behavior is verified against Yoga's own generated conformance suite
(530 tests, LTR and RTL).

The library has zero runtime dependencies and is fully **Native AOT and
trimming compatible** (`IsAotCompatible`): no reflection, no dynamic code. A
PublishAot smoke test runs in CI on every build.

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
<PackageReference Include="TimeWarp.Flexbox" Version="1.0.0-beta.3" />
```

## Usage

```csharp
using TimeWarp.Flexbox;

// Create a flex container
Node root = new();
root.Style.FlexDirection = FlexDirection.Row;
root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(300));
root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200));

// Add children that share the free space 1:2
Node left = new();
left.Style.FlexGrow = 1;
root.InsertChild(left, 0);

Node right = new();
right.Style.FlexGrow = 2;
root.InsertChild(right, 1);

// Calculate layout
CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

// Read computed positions and sizes
Console.WriteLine($"left:  x={left.Layout.GetPosition(PhysicalEdge.Left)} width={left.Layout.GetDimension(Dimension.Width)}");
Console.WriteLine($"right: x={right.Layout.GetPosition(PhysicalEdge.Left)} width={right.Layout.GetDimension(Dimension.Width)}");
// left:  x=0 width=100
// right: x=100 width=200
```

Styles are set through `Node.Style`:

- Dimensions: `SetDimension`, `SetMinDimension`, `SetMaxDimension` with
  `StyleSizeLength.Points(..)`, `.Percent(..)`, `.Auto`
- Flex: `FlexDirection`, `FlexGrow`, `FlexShrink`, `FlexBasis`, `FlexWrap`
- Alignment: `JustifyContent`, `AlignItems`, `AlignSelf`, `AlignContent`
- Box model: `SetMargin`, `SetPadding`, `SetBorder` per `Edge`, `SetGap` per `Gutter`
- Positioning: `PositionType` (relative/absolute/static) with `SetPosition` insets

Results are read from `Node.Layout` after calling `CalculateLayout.Calculate`:
`GetPosition(PhysicalEdge)` and `GetDimension(Dimension)`.

### Defaults

Defaults match Yoga (not web CSS): `flex-direction: column`, `flex-shrink: 0`,
`align-content: flex-start`, and `box-sizing: border-box`. Construct nodes with
`new Node(new Config { UseWebDefaults = true })` for web-style defaults.

### Demo

Run the visual demo to see computed layouts rendered as ASCII boxes, or as an
HTML page comparing the engine's output against your browser's native flexbox:

```bash
./samples/layout-demo/layout-demo.cs                 # terminal
./samples/layout-demo/layout-demo.cs --html out.html # browser comparison
```

## License

MIT — see [LICENSE](LICENSE) for details. Behavior modeled on Facebook's
[Yoga](https://github.com/facebook/yoga) (MIT, Copyright (c) Meta Platforms, Inc.).
