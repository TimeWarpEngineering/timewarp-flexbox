namespace TimeWarp.Flexbox.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmarks for FlexBox layout algorithm performance.
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class LayoutBenchmarks
{
  private FlexNode SimpleRoot = null!;
  private FlexNode DeepRoot = null!;
  private FlexNode WideRoot = null!;
  private FlexNode FlexGrowRoot = null!;
  private FlexNode WrappingRoot = null!;
  private FlexLayoutEngine Engine = null!;

  [GlobalSetup]
  public void Setup()
  {
    Engine = new FlexLayoutEngine();

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
    SimpleRoot.MarkDirty();
    Engine.CalculateLayout(SimpleRoot, 800, 600);
  }

  [Benchmark]
  public void DeepLayout()
  {
    DeepRoot.MarkDirty();
    Engine.CalculateLayout(DeepRoot, 800, 600);
  }

  [Benchmark]
  public void WideLayout()
  {
    WideRoot.MarkDirty();
    Engine.CalculateLayout(WideRoot, 800, 600);
  }

  [Benchmark]
  public void FlexGrowLayout()
  {
    FlexGrowRoot.MarkDirty();
    Engine.CalculateLayout(FlexGrowRoot, 800, 600);
  }

  [Benchmark]
  public void WrappingLayout()
  {
    WrappingRoot.MarkDirty();
    Engine.CalculateLayout(WrappingRoot, 800, 600);
  }

  [Benchmark]
  public void CachedLayout()
  {
    // Layout already calculated, should hit cache (no MarkDirty)
    Engine.CalculateLayout(SimpleRoot, 800, 600);
  }

  private static FlexNode CreateSimpleLayout()
  {
    return new FlexNode()
      .Direction(FlexDirection.Row)
      .Size(800, 600)
      .AddChildren(
        new FlexNode().Size(200, 100),
        new FlexNode().Size(200, 100),
        new FlexNode().Size(200, 100)
      );
  }

  private static FlexNode CreateDeepLayout(int depth)
  {
    FlexNode root = new FlexNode()
      .Direction(FlexDirection.Column)
      .Size(800, 600);

    FlexNode current = root;
    for (int i = 0; i < depth; i++)
    {
      FlexNode child = new FlexNode()
        .Direction(i % 2 == 0 ? FlexDirection.Row : FlexDirection.Column)
        .Grow(1)
        .Padding(5);

      current.AddChild(child);
      current = child;
    }

    // Add leaf nodes at the deepest level
    current.AddChildren(
      new FlexNode().Size(50, 50),
      new FlexNode().Size(50, 50)
    );

    return root;
  }

  private static FlexNode CreateWideLayout(int childCount)
  {
    FlexNode root = new FlexNode()
      .Direction(FlexDirection.Row)
      .Wrap(FlexWrap.Wrap)
      .Size(800, 600);

    for (int i = 0; i < childCount; i++)
    {
      root.AddChild(new FlexNode().Size(75, 50));
    }

    return root;
  }

  private static FlexNode CreateFlexGrowLayout()
  {
    return new FlexNode()
      .Direction(FlexDirection.Row)
      .Size(800, 600)
      .AddChildren(
        new FlexNode().Width(100).Grow(0),
        new FlexNode().Grow(1),
        new FlexNode().Grow(2),
        new FlexNode().Width(100).Grow(0),
        new FlexNode().Grow(1)
      );
  }

  private static FlexNode CreateWrappingLayout()
  {
    FlexNode root = new FlexNode()
      .Direction(FlexDirection.Row)
      .Wrap(FlexWrap.Wrap)
      .Size(800, 600)
      .Gap(10);

    for (int i = 0; i < 50; i++)
    {
      root.AddChild(new FlexNode().Size(150, 100));
    }

    return root;
  }
}
