namespace TimeWarp.Flexbox.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmarks for FlexBox layout algorithm performance.
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class LayoutBenchmarks
{
  private Node SimpleRoot = null!;
  private Node DeepRoot = null!;
  private Node WideRoot = null!;
  private Node FlexGrowRoot = null!;
  private Node WrappingRoot = null!;

  [GlobalSetup]
  public void Setup()
  {
    // Simple: root with 3 children
    SimpleRoot = CreateSimpleLayout();

    // Deep: 10 levels of nesting
    DeepRoot = CreateDeepLayout(10);

    // Wide: root with 100 children
    WideRoot = CreateWideLayout(100);

    // FlexGrow: children competing for space
    FlexGrowRoot = CreateFlexGrowLayout();

    // Wrapping: items that wrap to multiple lines
    WrappingRoot = CreateWrappingLayout();
  }

  [Benchmark(Baseline = true)]
  public void SimpleLayout()
  {
    SimpleRoot.MarkDirtyAndPropagate();
    CalculateLayout.Calculate(SimpleRoot, 800, 600, Direction.LTR);
  }

  [Benchmark]
  public void DeepLayout()
  {
    DeepRoot.MarkDirtyAndPropagate();
    CalculateLayout.Calculate(DeepRoot, 800, 600, Direction.LTR);
  }

  [Benchmark]
  public void WideLayout()
  {
    WideRoot.MarkDirtyAndPropagate();
    CalculateLayout.Calculate(WideRoot, 800, 600, Direction.LTR);
  }

  [Benchmark]
  public void FlexGrowLayout()
  {
    FlexGrowRoot.MarkDirtyAndPropagate();
    CalculateLayout.Calculate(FlexGrowRoot, 800, 600, Direction.LTR);
  }

  [Benchmark]
  public void WrappingLayout()
  {
    WrappingRoot.MarkDirtyAndPropagate();
    CalculateLayout.Calculate(WrappingRoot, 800, 600, Direction.LTR);
  }

  [Benchmark]
  public void CachedLayout()
  {
    // Layout already calculated, should hit cache (no dirtying)
    CalculateLayout.Calculate(SimpleRoot, 800, 600, Direction.LTR);
  }

  private static Node CreateNode(
      float? width = null,
      float? height = null,
      FlexDirection? direction = null,
      float? grow = null,
      Wrap? wrap = null,
      float? padding = null,
      float? gap = null)
  {
    Node node = new();
    Style style = node.Style;
    if (width is float w)
    {
      style.SetDimension(Dimension.Width, StyleSizeLength.Points(w));
    }

    if (height is float h)
    {
      style.SetDimension(Dimension.Height, StyleSizeLength.Points(h));
    }

    if (direction is FlexDirection d)
    {
      style.FlexDirection = d;
    }

    if (grow is float g)
    {
      style.FlexGrow = g;
    }

    if (wrap is Wrap wr)
    {
      style.FlexWrap = wr;
    }

    if (padding is float p)
    {
      style.SetPadding(Edge.All, StyleLength.Points(p));
    }

    if (gap is float gp)
    {
      style.SetGap(Gutter.All, StyleLength.Points(gp));
    }

    return node;
  }

  private static Node CreateSimpleLayout()
  {
    Node root = CreateNode(width: 800, height: 600, direction: FlexDirection.Row);
    root.InsertChild(CreateNode(width: 200, height: 100), 0);
    root.InsertChild(CreateNode(width: 200, height: 100), 1);
    root.InsertChild(CreateNode(width: 200, height: 100), 2);
    return root;
  }

  private static Node CreateDeepLayout(int depth)
  {
    Node root = CreateNode(width: 800, height: 600, direction: FlexDirection.Column);

    Node current = root;
    for (int i = 0; i < depth; i++)
    {
      Node child = CreateNode(
          direction: i % 2 == 0 ? FlexDirection.Row : FlexDirection.Column,
          grow: 1,
          padding: 5);

      current.InsertChild(child, 0);
      current = child;
    }

    // Add leaf nodes at the deepest level
    current.InsertChild(CreateNode(width: 50, height: 50), 0);
    current.InsertChild(CreateNode(width: 50, height: 50), 1);

    return root;
  }

  private static Node CreateWideLayout(int childCount)
  {
    Node root = CreateNode(width: 800, height: 600, direction: FlexDirection.Row, wrap: Wrap.Wrap);

    for (int i = 0; i < childCount; i++)
    {
      root.InsertChild(CreateNode(width: 75, height: 50), i);
    }

    return root;
  }

  private static Node CreateFlexGrowLayout()
  {
    Node root = CreateNode(width: 800, height: 600, direction: FlexDirection.Row);
    root.InsertChild(CreateNode(width: 100, grow: 0), 0);
    root.InsertChild(CreateNode(grow: 1), 1);
    root.InsertChild(CreateNode(grow: 2), 2);
    root.InsertChild(CreateNode(width: 100, grow: 0), 3);
    root.InsertChild(CreateNode(grow: 1), 4);
    return root;
  }

  private static Node CreateWrappingLayout()
  {
    Node root = CreateNode(width: 800, height: 600, direction: FlexDirection.Row, wrap: Wrap.Wrap, gap: 10);

    for (int i = 0; i < 50; i++)
    {
      root.InsertChild(CreateNode(width: 150, height: 100), i);
    }

    return root;
  }
}
