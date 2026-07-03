namespace TimeWarp.Flexbox.AotSmoke;

using TimeWarp.Flexbox;

/// <summary>
/// Native AOT smoke test: exercises the engine end-to-end in a
/// PublishAot-compiled binary and asserts computed layout values.
/// Exit code 0 = all checks pass.
/// </summary>
internal static class Program
{
  private static int Failures;

  /// <summary>
  /// Runs the smoke scenarios and returns a process exit code.
  /// </summary>
  internal static int Main()
  {
    GrowScenario();
    RowPositionsScenario();
    RtlScenario();
    AbsoluteScenario();
    WrapScenario();

    if (Failures == 0)
    {
      Console.WriteLine("AOT smoke: PASS (all layout checks)");
      return 0;
    }

    Console.Error.WriteLine($"AOT smoke: FAIL ({Failures} check(s))");
    return 1;
  }

  private static void GrowScenario()
  {
    Node root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(300));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200));

    Node left = new();
    left.Style.FlexGrow = 1;
    root.InsertChild(left, 0);

    Node right = new();
    right.Style.FlexGrow = 2;
    root.InsertChild(right, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    Check("grow left width", 100, left.Layout.GetDimension(Dimension.Width));
    Check("grow right x", 100, right.Layout.GetPosition(PhysicalEdge.Left));
    Check("grow right width", 200, right.Layout.GetDimension(Dimension.Width));
  }

  private static void RowPositionsScenario()
  {
    Node root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100));

    for (int i = 0; i < 2; i++)
    {
      Node child = new();
      child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30));
      child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30));
      root.InsertChild(child, i);
    }

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    Check("row c1 x", 0, root.Children[0].Layout.GetPosition(PhysicalEdge.Left));
    Check("row c2 x", 30, root.Children[1].Layout.GetPosition(PhysicalEdge.Left));
  }

  private static void RtlScenario()
  {
    Node root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.FlexStart;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100));

    for (int i = 0; i < 2; i++)
    {
      Node child = new();
      child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(30));
      child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(30));
      root.InsertChild(child, i);
    }

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    Check("rtl c1 x", 170, root.Children[0].Layout.GetPosition(PhysicalEdge.Left));
    Check("rtl c2 x", 140, root.Children[1].Layout.GetPosition(PhysicalEdge.Left));
  }

  private static void AbsoluteScenario()
  {
    Node root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(420));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(160));

    Node overlay = new();
    overlay.Style.PositionType = PositionType.Absolute;
    overlay.Style.SetPosition(Edge.Right, StyleLength.Points(20));
    overlay.Style.SetPosition(Edge.Bottom, StyleLength.Points(16));
    overlay.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(120));
    overlay.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(48));
    root.InsertChild(overlay, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    Check("absolute x", 280, overlay.Layout.GetPosition(PhysicalEdge.Left));
    Check("absolute y", 96, overlay.Layout.GetPosition(PhysicalEdge.Top));
  }

  private static void WrapScenario()
  {
    Node root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.FlexWrap = Wrap.Wrap;
    root.Style.SetGap(Gutter.All, StyleLength.Points(10));
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(250));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200));

    for (int i = 0; i < 3; i++)
    {
      Node child = new();
      child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(120));
      child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(60));
      root.InsertChild(child, i);
    }

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    // 250 wide fits 120 + 10 + 120 = 250 on line one; third child wraps.
    Check("wrap c2 x", 130, root.Children[1].Layout.GetPosition(PhysicalEdge.Left));
    Check("wrap c3 x", 0, root.Children[2].Layout.GetPosition(PhysicalEdge.Left));
    Check("wrap c3 y", 70, root.Children[2].Layout.GetPosition(PhysicalEdge.Top));
  }

  private static void Check(string name, float expected, float actual)
  {
    if (Math.Abs(expected - actual) > 0.001f)
    {
      Console.Error.WriteLine($"FAIL {name}: expected {expected}, got {actual}");
      Failures++;
    }
  }
}
