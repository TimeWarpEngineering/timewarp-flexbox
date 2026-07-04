/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGAlignBaselineTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Hand-written align-baseline tests, ported from YGAlignBaselineTest.cpp.
/// </summary>
public class AlignBaselineTests
{
  private static float HalfHeightBaseline(FlexNode node, float width, float height)
  {
    return height / 2;
  }

  private static YGSize Measure1(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    return new YGSize(42, 50);
  }

  private static YGSize Measure2(FlexNode node, float width, MeasureMode widthMode, float height, MeasureMode heightMode)
  {
    return new YGSize(279, 126);
  }

  private static FlexNode CreateNode(FlexConfig config, FlexDirection direction, int width, int height, bool alignBaseline)
  {
    FlexNode node = new(config);
    node.Style.FlexDirection = direction;
    if (alignBaseline)
    {
      node.Style.AlignItems = Align.Baseline;
    }

    node.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(width));
    node.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(height));
    return node;
  }

  // Test case for bug in T32999822
  public void align_baseline_parent_ht_not_specified()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignContent = Align.Stretch;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(340f));
    root.Style.SetMaxDimension(Dimension.Height, StyleSizeLength.Points(170f));
    root.Style.SetMinDimension(Dimension.Height, StyleSizeLength.Points(0f));

    FlexNode root_child0 = new(config);
    root_child0.Style.FlexGrow = 0f;
    root_child0.Style.FlexShrink = 1f;
    root_child0.SetMeasureFunc(Measure1);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexGrow = 0f;
    root_child1.Style.FlexShrink = 1f;
    root_child1.SetMeasureFunc(Measure2);
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(340f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(126f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(42f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(76f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(42f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(279f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(126f);
  }

  public void align_baseline_with_no_parent_ht()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(150f));

    FlexNode root_child0 = new(config);
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(40f));
    root_child1.BaselineFunc = HalfHeightBaseline;
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(150f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(70f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(50f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(30f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(40f);
  }

  public void align_baseline_with_no_baseline_func_and_no_parent_ht()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(150f));

    FlexNode root_child0 = new(config);
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(80f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(root_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(150f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(80f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(80f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(50f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(30f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(50f);
  }

  public void align_baseline_parent_using_child_in_column_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Column, 500, 800, false);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_with_padding_in_column_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Column, 500, 800, false);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1_child1.Style.SetPadding(Edge.Left, StyleLength.Points(100f));
    root_child1_child1.Style.SetPadding(Edge.Right, StyleLength.Points(100f));
    root_child1_child1.Style.SetPadding(Edge.Top, StyleLength.Points(100f));
    root_child1_child1.Style.SetPadding(Edge.Bottom, StyleLength.Points(100f));
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_with_padding_using_child_in_column_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Column, 500, 800, false);
    root_child1.Style.SetPadding(Edge.Left, StyleLength.Points(100f));
    root_child1.Style.SetPadding(Edge.Right, StyleLength.Points(100f));
    root_child1.Style.SetPadding(Edge.Top, StyleLength.Points(100f));
    root_child1.Style.SetPadding(Edge.Bottom, StyleLength.Points(100f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(100f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(100f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(400f);
  }

  public void align_baseline_parent_with_margin_using_child_in_column_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Column, 500, 800, false);
    root_child1.Style.SetMargin(Edge.Left, StyleLength.Points(100f));
    root_child1.Style.SetMargin(Edge.Right, StyleLength.Points(100f));
    root_child1.Style.SetMargin(Edge.Top, StyleLength.Points(100f));
    root_child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(100f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(600f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_with_margin_in_column_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Column, 500, 800, false);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1_child1.Style.SetMargin(Edge.Left, StyleLength.Points(100f));
    root_child1_child1.Style.SetMargin(Edge.Right, StyleLength.Points(100f));
    root_child1_child1.Style.SetMargin(Edge.Top, StyleLength.Points(100f));
    root_child1_child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(100f));
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(100f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(400f);
  }

  public void align_baseline_parent_using_child_in_row_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Row, 500, 800, true);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 500, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_with_padding_in_row_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Row, 500, 800, true);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 500, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1_child1.Style.SetPadding(Edge.Left, StyleLength.Points(100f));
    root_child1_child1.Style.SetPadding(Edge.Right, StyleLength.Points(100f));
    root_child1_child1.Style.SetPadding(Edge.Top, StyleLength.Points(100f));
    root_child1_child1.Style.SetPadding(Edge.Bottom, StyleLength.Points(100f));
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_with_margin_in_row_as_reference()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Row, 500, 800, true);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 500, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1_child1.Style.SetMargin(Edge.Left, StyleLength.Points(100f));
    root_child1_child1.Style.SetMargin(Edge.Right, StyleLength.Points(100f));
    root_child1_child1.Style.SetMargin(Edge.Top, StyleLength.Points(100f));
    root_child1_child1.Style.SetMargin(Edge.Bottom, StyleLength.Points(100f));
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(600f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_in_column_as_reference_with_no_baseline_func()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Column, 500, 800, false);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_in_row_as_reference_with_no_baseline_func()
  {
    FlexConfig config = new();

    FlexNode root = CreateNode(config, FlexDirection.Row, 1000, 1000, true);

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = CreateNode(config, FlexDirection.Row, 500, 800, true);
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 500, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);
  }

  public void align_baseline_parent_using_child_in_column_as_reference_with_height_not_specified()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(1000f));

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexDirection = FlexDirection.Column;
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Height).ShouldBe(800f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(700f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_in_row_as_reference_with_height_not_specified()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(1000f));

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexDirection = FlexDirection.Row;
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 500, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.BaselineFunc = HalfHeightBaseline;
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Height).ShouldBe(900f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(400f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
  }

  public void align_baseline_parent_using_child_in_column_as_reference_with_no_baseline_func_and_height_not_specified()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(1000f));

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexDirection = FlexDirection.Column;
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 300, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Height).ShouldBe(700f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(100f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(700f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(300f);
  }

  public void align_baseline_parent_using_child_in_row_as_reference_with_no_baseline_func_and_height_not_specified()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(1000f));

    FlexNode root_child0 = CreateNode(config, FlexDirection.Column, 500, 600, false);
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.FlexDirection = FlexDirection.Row;
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(500f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child1_child0 = CreateNode(config, FlexDirection.Column, 500, 500, false);
    root_child1.InsertChild(root_child1_child0, 0);

    FlexNode root_child1_child1 = CreateNode(config, FlexDirection.Column, 500, 400, false);
    root_child1_child1.IsReferenceBaseline = true;
    root_child1.InsertChild(root_child1_child1, 1);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetDimension(Dimension.Height).ShouldBe(700f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(200f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(500f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);

    root_child1_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(500f);
    root_child1_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
  }
}
