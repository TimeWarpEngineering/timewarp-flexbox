/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/FlexGapTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Hand-written flex gap tests, ported from FlexGapTest.cpp.
/// </summary>
public class FlexGapTests
{
  public void gap_negative_value()
  {
    FlexConfig config = new();

    FlexNode root = new(config);
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetGap(Gutter.All, StyleLength.Points(-20f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));

    FlexNode root_child0 = new(config);
    root_child0.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    root.InsertChild(root_child0, 0);

    FlexNode root_child1 = new(config);
    root_child1.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    root.InsertChild(root_child1, 1);

    FlexNode root_child2 = new(config);
    root_child2.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    root.InsertChild(root_child2, 2);

    FlexNode root_child3 = new(config);
    root_child3.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(20f));
    root.InsertChild(root_child3, 3);
    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.LTR);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(80f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(20f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(40f);
    root_child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child2.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child2.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child3.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(60f);
    root_child3.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child3.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child3.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    CalculateLayout.Calculate(root, float.NaN, float.NaN, Direction.RTL);

    root.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root.Layout.GetDimension(Dimension.Width).ShouldBe(80f);
    root.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child0.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(60f);
    root_child0.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child0.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child0.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child1.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(40f);
    root_child1.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child1.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child1.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child2.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(20f);
    root_child2.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child2.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child2.Layout.GetDimension(Dimension.Height).ShouldBe(200f);

    root_child3.Layout.GetPosition(PhysicalEdge.Left).ShouldBe(0f);
    root_child3.Layout.GetPosition(PhysicalEdge.Top).ShouldBe(0f);
    root_child3.Layout.GetDimension(Dimension.Width).ShouldBe(20f);
    root_child3.Layout.GetDimension(Dimension.Height).ShouldBe(200f);
  }
}
