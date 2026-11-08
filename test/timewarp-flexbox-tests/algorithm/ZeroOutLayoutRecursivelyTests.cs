/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGZeroOutLayoutRecursivelyTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for zeroing out layout of display:none nodes, ported from YGZeroOutLayoutRecursivelyTest.cpp.
/// </summary>
public class ZeroOutLayoutRecursivelyTests
{
  public void zero_out_layout()
  {
    FlexNode root = new();
    root.Style.FlexDirection = FlexDirection.Row;
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(200f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(200f));

    FlexNode child = new();
    root.InsertChild(child, 0);
    child.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    child.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    child.Style.SetMargin(Edge.Top, StyleLength.Points(10f));
    child.Style.SetPadding(Edge.Top, StyleLength.Points(10f));

    CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

    child.Layout.GetMargin(PhysicalEdge.Top).ShouldBe(10f);
    child.Layout.GetPadding(PhysicalEdge.Top).ShouldBe(10f);

    child.Style.Display = Display.None;

    CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

    child.Layout.GetMargin(PhysicalEdge.Top).ShouldBe(0f);
    child.Layout.GetPadding(PhysicalEdge.Top).ShouldBe(0f);
  }
}
