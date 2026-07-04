/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGBaselineFuncTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for custom baseline functions, ported from YGBaselineFuncTest.cpp.
/// </summary>
public class BaselineFuncTests
{
  public void align_baseline_customer_func()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.AlignItems = Align.Baseline;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    FlexNode root_child0 = new();
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(50f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new();
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root.InsertChild(root_child1, 1);

    float baselineValue = 10;
    FlexNode root_child1_child0 = new();
    root_child1_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(50f));
    root_child1_child0.BaselineFunc = (node, width, height) => baselineValue;
    root_child1_child0.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(20f));
    root_child1.InsertChild(root_child1_child0, 0);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(100f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(100f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(50f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(50f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(40f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(20f);

    root_child1_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child1_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child1_child0.Layout.GetDimension(Dimension.Width).ShouldBe(50f);
    root_child1_child0.Layout.GetDimension(Dimension.Height).ShouldBe(20f);
  }
}
