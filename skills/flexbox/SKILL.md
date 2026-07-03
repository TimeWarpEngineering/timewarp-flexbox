---
name: flexbox
description: TimeWarp.Flexbox library - compute CSS flexbox layouts (positions and sizes) in pure C# with no UI framework; a Native AOT-safe port of Facebook's Yoga engine. Use for laying out boxes/nodes in canvases, terminals, games, PDF/image generation, or custom renderers - anywhere you need flex-direction, flex-grow, wrap, gap, align/justify, percentages, or absolute positioning computed for you.
---

# TimeWarp.Flexbox

Pure C# flexbox layout engine — build a tree of nodes, set CSS-flexbox styles,
and read back computed pixel positions and sizes. No UI framework involved.

**Repository:** https://github.com/TimeWarpEngineering/timewarp-flexbox
**Package:** `TimeWarp.Flexbox`

Behavior is a from-scratch port of Facebook's Yoga engine, verified against
Yoga's own conformance suite (530 generated tests, LTR and RTL). Zero runtime
dependencies; fully Native AOT and trimming compatible.

## When to Use What

| Need | Use |
|------|-----|
| A layout container | `new Node()` + `node.Style.FlexDirection = ...` |
| Fixed size | `Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100))` |
| Percentage of parent | `StyleSizeLength.Percent(50)` |
| Share leftover space | `Style.FlexGrow = 1` |
| Spacing between items | `Style.SetGap(Gutter.All, StyleLength.Points(10))` |
| Margins / padding / borders | `Style.SetMargin/SetPadding/SetBorder(Edge.All, StyleLength.Points(8))` |
| Pin to a corner/edge | `Style.PositionType = PositionType.Absolute` + `Style.SetPosition(Edge.Right, ...)` |
| Text or other content-sized leaf | `node.SetMeasureFunc(...)` |
| Compute the layout | `CalculateLayout.Calculate(root, availW, availH, Direction.LTR)` |
| Read results | `node.Layout.GetPosition(PhysicalEdge.Left)` / `GetDimension(Dimension.Width)` |
| Web-CSS defaults instead of Yoga defaults | `new Node(new Config { UseWebDefaults = true })` |

## Core Model

Five steps, always the same shape:

```csharp
using TimeWarp.Flexbox;

// 1. Build a node tree
Node root = new();
root.Style.FlexDirection = FlexDirection.Row;
root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(300));
root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200));

Node left = new();
left.Style.FlexGrow = 1;
root.InsertChild(left, 0);          // 2. children attach via InsertChild(child, index)

Node right = new();
right.Style.FlexGrow = 2;
root.InsertChild(right, 1);

// 3. Calculate (float.NaN = unconstrained available space)
CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

// 4. Read computed values from node.Layout
Console.WriteLine($"left:  x={left.Layout.GetPosition(PhysicalEdge.Left)} width={left.Layout.GetDimension(Dimension.Width)}");
Console.WriteLine($"right: x={right.Layout.GetPosition(PhysicalEdge.Left)} width={right.Layout.GetDimension(Dimension.Width)}");
// left:  x=0 width=100
// right: x=100 width=200

// 5. Change styles and re-Calculate any time — style writes dirty the tree automatically
```

Positions are **relative to the parent**. For absolute page coordinates,
accumulate ancestors' positions (see Reading Results below).

## CSS-to-C# Mapping

| CSS | TimeWarp.Flexbox |
|-----|------------------|
| `width: 100px` | `Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100))` |
| `height: 50%` | `Style.SetDimension(Dimension.Height, StyleSizeLength.Percent(50))` |
| `width: auto` | `StyleSizeLength.Auto` (also `.MaxContent`, `.FitContent`, `.Stretch`) |
| `min-width` / `max-width` | `Style.SetMinDimension` / `Style.SetMaxDimension` |
| `flex-direction: row` | `Style.FlexDirection = FlexDirection.Row` (`Column`, `RowReverse`, `ColumnReverse`) |
| `flex-wrap: wrap` | `Style.FlexWrap = Wrap.Wrap` (`NoWrap`, `WrapReverse`) |
| `flex-grow: 1` | `Style.FlexGrow = 1` |
| `flex-shrink: 1` | `Style.FlexShrink = 1` |
| `flex-basis: 100px` | `Style.FlexBasis = StyleSizeLength.Points(100)` |
| `justify-content: space-between` | `Style.JustifyContent = Justify.SpaceBetween` (`FlexStart`, `Center`, `FlexEnd`, `SpaceAround`, `SpaceEvenly`) |
| `align-items: center` | `Style.AlignItems = Align.Center` (`FlexStart`, `FlexEnd`, `Stretch`, `Baseline`) |
| `align-self` / `align-content` | `Style.AlignSelf` / `Style.AlignContent` (same `Align` enum) |
| `gap: 10px` | `Style.SetGap(Gutter.All, StyleLength.Points(10))` (`Gutter.Row`, `Gutter.Column`) |
| `margin: 8px` | `Style.SetMargin(Edge.All, StyleLength.Points(8))` (per-edge: `Edge.Left/Top/Right/Bottom/Start/End/Horizontal/Vertical`) |
| `margin-left: auto` | `Style.SetMargin(Edge.Left, StyleLength.Auto)` |
| `padding` / `border-width` | `Style.SetPadding` / `Style.SetBorder` (same Edge pattern) |
| `position: absolute; right: 20px` | `Style.PositionType = PositionType.Absolute; Style.SetPosition(Edge.Right, StyleLength.Points(20))` |
| `display: none` | `Style.Display = Display.None` |
| `overflow: hidden` | `Style.Overflow = Overflow.Hidden` |
| `box-sizing` | `Style.BoxSizing = BoxSizing.BorderBox` (default) or `ContentBox` |
| `aspect-ratio: 16/9` | `Style.AspectRatio = 16f / 9f` |
| `direction: rtl` | pass `Direction.RTL` to `Calculate` (or `Style.Direction` per subtree) |

Two length types — do not mix them up:
- **`StyleSizeLength`** for dimensions and flex-basis (supports `Auto`, `MaxContent`, `FitContent`, `Stretch`).
- **`StyleLength`** for edges and gaps (margin, padding, border, position, gap; supports `Auto` for margins/positions).

## Defaults Are Yoga's, Not Web CSS

This is the #1 source of surprises:

| Property | TimeWarp.Flexbox / Yoga default | Web CSS default |
|----------|--------------------------------|-----------------|
| `flex-direction` | **Column** | row |
| `flex-shrink` | **0** | 1 |
| `align-content` | **flex-start** | stretch |
| `position` | relative | static |
| `box-sizing` | border-box | content-box |

For web-style behavior, construct nodes with a web-defaults config:

```csharp
Config config = new() { UseWebDefaults = true };
Node root = new(config);   // pass the same config to every node in the tree
```

## Reading Results

```csharp
float x = node.Layout.GetPosition(PhysicalEdge.Left);   // relative to parent
float y = node.Layout.GetPosition(PhysicalEdge.Top);
float w = node.Layout.GetDimension(Dimension.Width);
float h = node.Layout.GetDimension(Dimension.Height);
```

Positions are parent-relative. Accumulate for absolute coordinates:

```csharp
static (float X, float Y) AbsolutePosition(Node node)
{
  float x = 0, y = 0;
  for (Node? n = node; n is not null; n = n.Owner)
  {
    x += n.Layout.GetPosition(PhysicalEdge.Left);
    y += n.Layout.GetPosition(PhysicalEdge.Top);
  }

  return (x, y);
}
```

## Recipes

All outputs below are the actual values produced by the engine.

### App shell: sidebar + header/main/footer

```csharp
Node shell = new();
shell.Style.FlexDirection = FlexDirection.Row;
shell.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(420));
shell.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200));
shell.Style.SetPadding(Edge.All, StyleLength.Points(8));
shell.Style.SetGap(Gutter.All, StyleLength.Points(8));

Node sidebar = new();
sidebar.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100));
shell.InsertChild(sidebar, 0);

Node content = new();
content.Style.FlexGrow = 1;                       // fill remaining width
content.Style.FlexDirection = FlexDirection.Column;
content.Style.SetGap(Gutter.All, StyleLength.Points(8));
shell.InsertChild(content, 1);

Node header = new();
header.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(48));
content.InsertChild(header, 0);

Node main = new();
main.Style.FlexGrow = 1;                          // fill remaining height
content.InsertChild(main, 1);

Node footer = new();
footer.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(32));
content.InsertChild(footer, 2);

CalculateLayout.Calculate(shell, float.NaN, float.NaN, Direction.LTR);
// sidebar: x=8  w=100 h=184   (stretched to fill cross axis inside padding)
// content: x=116 w=296
// main:    y=56 h=88          (relative to content; absolute = (116, 64))
```

### Wrapping card grid with gap

```csharp
Node grid = new();
grid.Style.FlexDirection = FlexDirection.Row;
grid.Style.FlexWrap = Wrap.Wrap;
grid.Style.SetGap(Gutter.All, StyleLength.Points(10));
grid.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(250));
grid.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200));

for (int i = 0; i < 3; i++)
{
  Node card = new();
  card.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(120));
  card.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(60));
  grid.InsertChild(card, i);
}

CalculateLayout.Calculate(grid, float.NaN, float.NaN, Direction.LTR);
// card0: (0, 0)   card1: (130, 0)   card2: (0, 70)  <- wrapped, gap applied both axes
```

### Corner overlay via absolute insets

```csharp
Node overlay = new();
overlay.Style.PositionType = PositionType.Absolute;
overlay.Style.SetPosition(Edge.Right, StyleLength.Points(20));
overlay.Style.SetPosition(Edge.Bottom, StyleLength.Points(16));
overlay.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(120));
overlay.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(48));
root.InsertChild(overlay, root.Children.Count);
// inside a 420x160 root: overlay at (280, 96)
```

### RTL

```csharp
CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);
// A 200-wide row of two 30x30 children flows right-to-left:
// child0 x=170, child1 x=140
```

### Content-measured leaf (text, images)

For leaves whose size depends on content, attach a measure function. It is
called with the available space and a `MeasureMode` per axis:

```csharp
Node text = new();
text.SetMeasureFunc((node, width, widthMode, height, heightMode) =>
{
  // widthMode/heightMode: Undefined (size to content), AtMost (fit within),
  // Exactly (value is fixed - your result for that axis is ignored)
  float measuredWidth = Math.Min(80, width);
  return new YGSize(measuredWidth, 20);
});
container.InsertChild(text, 0);
// In a 100x100 container: measure is called once with (100, Exactly, 100, AtMost)
// -> text ends up w=100 (Exactly wins over the measured 80), h=20 (AtMost honors it)
```

Nodes with a measure function cannot have children (asserted).

### Incremental re-layout

Style writes mark the tree dirty automatically; just call `Calculate` again.
Unchanged subtrees hit the internal measurement cache.

```csharp
// two flexGrow:1 children in a 200-wide row -> a=100, b=100
b.Style.FlexGrow = 3;
CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
// now a=50, b=150
```

## Pitfalls

- **A child belongs to one parent.** `InsertChild` takes ownership; inserting a
  node that already has an owner throws `YogaAssertException` ("Child already
  has a owner, it must be removed first."). Call `parent.RemoveChild(child)`
  first — removal resets the child's layout and clears its owner.
- **`float.NaN` means "undefined/unconstrained"** for available space and any
  optional float. Check with `Comparison.IsUndefined(value)`, not `== NaN`.
- **`StyleSizeLength` vs `StyleLength`**: dimensions/flex-basis take the former,
  edges/gaps the latter. The compiler catches it, but pick the right factory.
- **Positions are parent-relative** — accumulate up the `Owner` chain for
  absolute coordinates.
- **One `Config` per tree**: nodes constructed with different `UseWebDefaults`
  values cannot be mixed after construction (asserted on `SetConfig`).
- The whole API is `float`-based; use `f` literals or implicit int conversions.

## Installation

```xml
<PackageReference Include="TimeWarp.Flexbox" Version="1.0.0-beta.3" />
```

The package is on the TimeWarpEngineering GitHub Packages feed (see the
repository readme for feed authentication) and ships XML docs. Targets
`net10.0`, zero dependencies, `IsAotCompatible`.

## Further Reference

- `samples/layout-demo/layout-demo.cs` in the repo renders layouts as ASCII
  boxes in the terminal, or (with `--html`) as a page comparing engine output
  against the browser's native flexbox side by side.
- `test/timewarp-flexbox-tests/Generated/` contains Yoga's conformance suite
  ported to C# — a searchable catalog of expected values for any flexbox
  scenario.
- Defaults, enums, and the full style surface: `source/timewarp-flexbox/Style/Style.cs`
  and `Enums/YGEnums.cs`.
