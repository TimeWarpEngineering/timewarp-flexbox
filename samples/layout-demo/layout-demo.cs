#!/usr/bin/dotnet --
#:project ../../source/timewarp-flexbox/timewarp-flexbox.csproj
#:property EnablePreviewFeatures=true
#:property NoWarn=CA1303;CA2007;IDE0058

// Visual demo for TimeWarp.Flexbox.
//
// Usage:
//   ./samples/layout-demo/layout-demo.cs                 ASCII-render scenarios to the terminal
//   ./samples/layout-demo/layout-demo.cs --html out.html Also write an HTML page that draws the
//                                                        engine's computed boxes next to the
//                                                        browser's native CSS flexbox for the
//                                                        same styles - the two must match.

using System.Globalization;
using System.Text;
using TimeWarp.Flexbox;

List<Scenario> scenarios =
[
  new("Row, flexGrow 1:2:1", new BoxSpec
  {
    Width = 420, Height = 120, FlexDirection = FlexDirection.Row, PaddingAll = 10, Gap = 10,
    Children =
    [
      new BoxSpec { Grow = 1 },
      new BoxSpec { Grow = 2 },
      new BoxSpec { Grow = 1 },
    ],
  }),
  new("Wrap with gap, align-content flex-start", new BoxSpec
  {
    Width = 420, Height = 200, FlexDirection = FlexDirection.Row, Wrap = Wrap.Wrap, Gap = 10, PaddingAll = 10,
    Children =
    [
      new BoxSpec { Width = 120, Height = 60 },
      new BoxSpec { Width = 120, Height = 80 },
      new BoxSpec { Width = 120, Height = 60 },
      new BoxSpec { Width = 120, Height = 60 },
      new BoxSpec { Width = 120, Height = 60 },
    ],
  }),
  new("justify-content space-between, align-items center", new BoxSpec
  {
    Width = 420, Height = 140, FlexDirection = FlexDirection.Row,
    Justify = Justify.SpaceBetween, AlignItems = Align.Center, PaddingAll = 10,
    Children =
    [
      new BoxSpec { Width = 80, Height = 40 },
      new BoxSpec { Width = 80, Height = 90 },
      new BoxSpec { Width = 80, Height = 60 },
    ],
  }),
  new("Nested: sidebar + content column", new BoxSpec
  {
    Width = 420, Height = 200, FlexDirection = FlexDirection.Row, Gap = 8, PaddingAll = 8,
    Children =
    [
      new BoxSpec { Width = 100 },
      new BoxSpec
      {
        Grow = 1, FlexDirection = FlexDirection.Column, Gap = 8,
        Children =
        [
          new BoxSpec { Height = 48 },
          new BoxSpec { Grow = 1 },
          new BoxSpec { Height = 32 },
        ],
      },
    ],
  }),
  new("Absolute child with insets", new BoxSpec
  {
    Width = 420, Height = 160, FlexDirection = FlexDirection.Row, PaddingAll = 10,
    Children =
    [
      new BoxSpec { Grow = 1 },
      new BoxSpec { Absolute = true, Right = 20, Bottom = 16, Width = 120, Height = 48 },
    ],
  }),
  new("RTL row with margins", new BoxSpec
  {
    Width = 420, Height = 110, FlexDirection = FlexDirection.Row, PaddingAll = 10, Rtl = true,
    Children =
    [
      new BoxSpec { Width = 90, Height = 60, MarginAll = 6 },
      new BoxSpec { Width = 90, Height = 60, MarginAll = 6 },
      new BoxSpec { Grow = 1, Height = 60, MarginAll = 6 },
    ],
  }),
];

string? htmlPath = null;
for (int i = 0; i < args.Length - 1; i++)
{
  if (args[i] == "--html")
  {
    htmlPath = args[i + 1];
  }
}

List<(Scenario Scenario, LaidOutBox Root)> results = [];
foreach (Scenario scenario in scenarios)
{
  Node root = Build(scenario.Root);
  CalculateLayout.Calculate(root, float.NaN, float.NaN, scenario.Root.Rtl ? Direction.RTL : Direction.LTR);
  LaidOutBox laidOut = Capture(root, scenario.Root, 0, 0);
  results.Add((scenario, laidOut));

  Console.WriteLine();
  Console.WriteLine($"=== {scenario.Name} ===");
  Console.WriteLine(AsciiRender.Render(laidOut));
}

if (htmlPath is not null)
{
  File.WriteAllText(htmlPath, HtmlRender.Render(results));
  Console.WriteLine($"\nHTML comparison written to {htmlPath}");
}

return 0;

static Node Build(BoxSpec spec)
{
  Node node = new();
  Style style = node.Style;
  if (spec.Width is float w)
  {
    style.SetDimension(Dimension.Width, StyleSizeLength.Points(w));
  }

  if (spec.Height is float h)
  {
    style.SetDimension(Dimension.Height, StyleSizeLength.Points(h));
  }

  if (spec.FlexDirection is FlexDirection fd)
  {
    style.FlexDirection = fd;
  }

  if (spec.Justify is Justify j)
  {
    style.JustifyContent = j;
  }

  if (spec.AlignItems is Align ai)
  {
    style.AlignItems = ai;
  }

  if (spec.Wrap is Wrap wr)
  {
    style.FlexWrap = wr;
  }

  if (spec.Grow is float g)
  {
    style.FlexGrow = g;
  }

  if (spec.Gap is float gap)
  {
    style.SetGap(Gutter.All, StyleLength.Points(gap));
  }

  if (spec.PaddingAll is float p)
  {
    style.SetPadding(Edge.All, StyleLength.Points(p));
  }

  if (spec.MarginAll is float m)
  {
    style.SetMargin(Edge.All, StyleLength.Points(m));
  }

  if (spec.Absolute)
  {
    style.PositionType = PositionType.Absolute;
    if (spec.Left is float l)
    {
      style.SetPosition(Edge.Left, StyleLength.Points(l));
    }

    if (spec.Top is float t)
    {
      style.SetPosition(Edge.Top, StyleLength.Points(t));
    }

    if (spec.Right is float r)
    {
      style.SetPosition(Edge.Right, StyleLength.Points(r));
    }

    if (spec.Bottom is float b)
    {
      style.SetPosition(Edge.Bottom, StyleLength.Points(b));
    }
  }

  for (int i = 0; i < spec.Children.Count; i++)
  {
    node.InsertChild(Build(spec.Children[i]), i);
  }

  return node;
}

static LaidOutBox Capture(Node node, BoxSpec spec, float absX, float absY)
{
  float x = absX + node.Layout.GetPosition(PhysicalEdge.Left);
  float y = absY + node.Layout.GetPosition(PhysicalEdge.Top);
  LaidOutBox box = new(
      x, y,
      node.Layout.GetDimension(Dimension.Width),
      node.Layout.GetDimension(Dimension.Height),
      []);
  for (int i = 0; i < node.Children.Count; i++)
  {
    box.Children.Add(Capture(node.Children[i], spec.Children[i], x, y));
  }

  return box;
}

internal sealed record Scenario(string Name, BoxSpec Root);

internal sealed class BoxSpec
{
  public float? Width { get; init; }
  public float? Height { get; init; }
  public FlexDirection? FlexDirection { get; init; }
  public Justify? Justify { get; init; }
  public Align? AlignItems { get; init; }
  public Wrap? Wrap { get; init; }
  public float? Grow { get; init; }
  public float? Gap { get; init; }
  public float? PaddingAll { get; init; }
  public float? MarginAll { get; init; }
  public bool Absolute { get; init; }
  public float? Left { get; init; }
  public float? Top { get; init; }
  public float? Right { get; init; }
  public float? Bottom { get; init; }
  public bool Rtl { get; init; }
  public List<BoxSpec> Children { get; init; } = [];

  public string ToCss(bool isRoot)
  {
    StringBuilder css = new();
    css.Append("display:flex;box-sizing:border-box;");
    css.Append("flex-direction:").Append((FlexDirection ?? TimeWarp.Flexbox.FlexDirection.Column) switch
    {
      TimeWarp.Flexbox.FlexDirection.Row => "row",
      TimeWarp.Flexbox.FlexDirection.RowReverse => "row-reverse",
      TimeWarp.Flexbox.FlexDirection.ColumnReverse => "column-reverse",
      _ => "column",
    }).Append(';');
    // Yoga defaults that differ from CSS defaults:
    css.Append("align-content:flex-start;flex-shrink:0;");
    if (isRoot)
    {
      css.Append("position:relative;");
    }

    void Px(string name, float? v)
    {
      if (v is float f)
      {
        css.Append(CultureInfo.InvariantCulture, $"{name}:{f}px;");
      }
    }

    Px("width", Width);
    Px("height", Height);
    Px("gap", Gap);
    Px("padding", PaddingAll);
    Px("margin", MarginAll);
    if (Justify is Justify jv)
    {
      css.Append("justify-content:").Append(jv switch
      {
        TimeWarp.Flexbox.Justify.Center => "center",
        TimeWarp.Flexbox.Justify.FlexEnd => "flex-end",
        TimeWarp.Flexbox.Justify.SpaceBetween => "space-between",
        TimeWarp.Flexbox.Justify.SpaceAround => "space-around",
        TimeWarp.Flexbox.Justify.SpaceEvenly => "space-evenly",
        _ => "flex-start",
      }).Append(';');
    }

    if (AlignItems is Align av)
    {
      css.Append("align-items:").Append(av switch
      {
        Align.Center => "center",
        Align.FlexEnd => "flex-end",
        Align.FlexStart => "flex-start",
        Align.Baseline => "baseline",
        _ => "stretch",
      }).Append(';');
    }

    if (Wrap is TimeWarp.Flexbox.Wrap.Wrap)
    {
      css.Append("flex-wrap:wrap;");
    }

    if (Grow is float gr)
    {
      css.Append(CultureInfo.InvariantCulture, $"flex-grow:{gr};");
    }

    if (Absolute)
    {
      css.Append("position:absolute;");
      Px("left", Left);
      Px("top", Top);
      Px("right", Right);
      Px("bottom", Bottom);
    }

    return css.ToString();
  }
}

internal sealed record LaidOutBox(float X, float Y, float Width, float Height, List<LaidOutBox> Children);

internal static class AsciiRender
{
  private const float ScaleX = 10f; // px per column
  private const float ScaleY = 20f; // px per row

  public static string Render(LaidOutBox root)
  {
    int cols = (int)MathF.Ceiling(root.Width / ScaleX) + 1;
    int rows = (int)MathF.Ceiling(root.Height / ScaleY) + 1;
    char[,] grid = new char[rows, cols];
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        grid[r, c] = ' ';
      }
    }

    int label = 0;
    Draw(grid, root, ref label, depth: 0);

    StringBuilder sb = new();
    for (int r = 0; r < rows; r++)
    {
      for (int c = 0; c < cols; c++)
      {
        sb.Append(grid[r, c]);
      }

      sb.AppendLine();
    }

    return sb.ToString();
  }

  private static void Draw(char[,] grid, LaidOutBox box, ref int label, int depth)
  {
    int x0 = (int)MathF.Round(box.X / ScaleX);
    int y0 = (int)MathF.Round(box.Y / ScaleY);
    int x1 = (int)MathF.Round((box.X + box.Width) / ScaleX);
    int y1 = (int)MathF.Round((box.Y + box.Height) / ScaleY);
    x1 = Math.Max(x1, x0 + 1);
    y1 = Math.Max(y1, y0 + 1);

    for (int c = x0; c <= x1 && c < grid.GetLength(1); c++)
    {
      Put(grid, y0, c, '─');
      Put(grid, y1, c, '─');
    }

    for (int r = y0; r <= y1 && r < grid.GetLength(0); r++)
    {
      Put(grid, r, x0, '│');
      Put(grid, r, x1, '│');
    }

    Put(grid, y0, x0, '┌');
    Put(grid, y0, x1, '┐');
    Put(grid, y1, x0, '└');
    Put(grid, y1, x1, '┘');

    if (depth > 0)
    {
      string tag = ((char)('A' + (label % 26))).ToString();
      label++;
      if (y0 + 1 <= y1 - 0 && x0 + 2 < x1)
      {
        Put(grid, y0 == y1 ? y0 : y0 + ((y1 - y0) / 2), x0 + ((x1 - x0) / 2), tag[0]);
      }
    }

    foreach (LaidOutBox child in box.Children)
    {
      Draw(grid, child, ref label, depth + 1);
    }
  }

  private static void Put(char[,] grid, int r, int c, char ch)
  {
    if (r >= 0 && r < grid.GetLength(0) && c >= 0 && c < grid.GetLength(1))
    {
      grid[r, c] = ch;
    }
  }
}

internal static class HtmlRender
{
  private static readonly string[] Palette =
  [
    "#5B8DEF", "#E8843C", "#4CAF7D", "#C75D8A", "#8B6FC9", "#D4A72C", "#4FA3A5", "#B85C4F",
  ];

  public static string Render(List<(Scenario Scenario, LaidOutBox Root)> results)
  {
    StringBuilder html = new();
    html.AppendLine("<!doctype html><html><head><meta charset=\"utf-8\">");
    html.AppendLine("<title>TimeWarp.Flexbox vs browser flexbox</title>");
    html.AppendLine("""
      <style>
        body { font-family: system-ui, sans-serif; margin: 2rem; background: #f5f5f4; color: #1c1917; }
        h1 { font-size: 1.3rem; } h2 { font-size: 1rem; margin: 2rem 0 .5rem; }
        .pair { display: flex; gap: 24px; flex-wrap: wrap; }
        .panel { background: #fff; border: 1px solid #d6d3d1; border-radius: 8px; padding: 12px; }
        .panel h3 { margin: 0 0 8px; font-size: .8rem; font-weight: 600; color: #57534e;
                    text-transform: uppercase; letter-spacing: .05em; }
        .cell { border: 1px dashed #a8a29e; }
        .engine-root { position: relative; }
        .engine-box { position: absolute; box-sizing: border-box; box-shadow: inset 0 0 0 1px rgba(0,0,0,.35); }
        .css-box { box-sizing: border-box; box-shadow: inset 0 0 0 1px rgba(0,0,0,.35); }
        p.note { color: #57534e; max-width: 60ch; }
      </style></head><body>
      <h1>TimeWarp.Flexbox vs the browser's native flexbox</h1>
      <p class="note">Each scenario is rendered twice from the same styles: the left panel is the
      browser's own CSS flexbox; the right panel draws absolutely-positioned boxes at the exact
      coordinates computed by TimeWarp.Flexbox. If the engine is correct, the panels are identical.</p>
      """);

    foreach ((Scenario scenario, LaidOutBox root) in results)
    {
      html.AppendLine(CultureInfo.InvariantCulture, $"<h2>{scenario.Name}</h2><div class=\"pair\">");

      html.AppendLine("<div class=\"panel\"><h3>Browser CSS flexbox</h3><div class=\"cell\">");
      if (scenario.Root.Rtl)
      {
        html.AppendLine("<div dir=\"rtl\">");
      }

      AppendCssBox(html, scenario.Root, isRoot: true, colorIndex: 0, counter: new int[1]);
      if (scenario.Root.Rtl)
      {
        html.AppendLine("</div>");
      }

      html.AppendLine("</div></div>");

      html.AppendLine("<div class=\"panel\"><h3>TimeWarp.Flexbox computed</h3><div class=\"cell\">");
      html.AppendLine(CultureInfo.InvariantCulture,
          $"<div class=\"engine-root\" style=\"width:{root.Width}px;height:{root.Height}px\">");
      int[] engineCounter = [0];
      foreach (LaidOutBox child in root.Children)
      {
        AppendEngineBox(html, child, engineCounter);
      }

      html.AppendLine("</div></div></div>");
      html.AppendLine("</div>");
    }

    html.AppendLine("</body></html>");
    return html.ToString();
  }

  private static void AppendCssBox(StringBuilder html, BoxSpec spec, bool isRoot, int colorIndex, int[] counter)
  {
    string background = isRoot ? "background:#fafaf9;" : $"background:{Palette[colorIndex % Palette.Length]};";
    html.AppendLine(CultureInfo.InvariantCulture,
        $"<div class=\"css-box\" style=\"{spec.ToCss(isRoot)}{background}\">");
    foreach (BoxSpec child in spec.Children)
    {
      counter[0]++;
      AppendCssBox(html, child, isRoot: false, colorIndex: counter[0], counter);
    }

    html.AppendLine("</div>");
  }

  private static void AppendEngineBox(StringBuilder html, LaidOutBox box, int[] counter)
  {
    counter[0]++;
    string color = Palette[counter[0] % Palette.Length];
    html.AppendLine(CultureInfo.InvariantCulture,
        $"<div class=\"engine-box\" style=\"left:{box.X}px;top:{box.Y}px;width:{box.Width}px;height:{box.Height}px;background:{color}\">");
    foreach (LaidOutBox child in box.Children)
    {
      AppendEngineChildBox(html, child, box, counter);
    }

    html.AppendLine("</div>");
  }

  private static void AppendEngineChildBox(StringBuilder html, LaidOutBox box, LaidOutBox parent, int[] counter)
  {
    counter[0]++;
    string color = Palette[counter[0] % Palette.Length];
    html.AppendLine(CultureInfo.InvariantCulture,
        $"<div class=\"engine-box\" style=\"left:{box.X - parent.X}px;top:{box.Y - parent.Y}px;width:{box.Width}px;height:{box.Height}px;background:{color}\">");
    foreach (LaidOutBox child in box.Children)
    {
      AppendEngineChildBox(html, child, box, counter);
    }

    html.AppendLine("</div>");
  }
}
