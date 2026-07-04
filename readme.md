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

```bash
dotnet add package TimeWarp.Flexbox --prerelease
```

```xml
<PackageReference Include="TimeWarp.Flexbox" Version="1.0.0-beta.4" />
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
