/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGEdgeTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for edge precedence (start/end/horizontal/vertical/all overrides), ported from YGEdgeTest.cpp.
/// </summary>
public class EdgeTests
{
  public void start_overrides()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.Start, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(20f));
    root_child0.Style.SetMargin(Edge.Right, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(20f);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(10f);
  }

  public void end_overrides()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.End, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(20f));
    root_child0.Style.SetMargin(Edge.Right, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(10f);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(20f);
  }

  public void horizontal_overridden()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.Horizontal, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(10f);
  }

  public void vertical_overridden()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Column;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.Vertical, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Top, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(10f);
  }

  public void horizontal_overrides_all()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Column;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.Horizontal, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.All, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(20f);
  }

  public void vertical_overrides_all()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Column;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.Vertical, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.All, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(20f);
    root_child0.Layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(10f);
  }

  public void all_overridden()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Column;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.FlexGrow = 1f;
    root_child0.Style.SetMargin(Edge.Left, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Right, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.Bottom, StyleLength.Points(10f));
    root_child0.Style.SetMargin(Edge.All, StyleLength.Points(20f));
    root.InsertChild(root_child0, 0);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);
    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Right).ShouldBe(10f);
    root_child0.Layout.GetPosition(PhysicalEdge.Bottom).ShouldBe(10f);
  }
}
