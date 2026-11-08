/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGComputedMarginTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for computed layout margins, ported from YGComputedMarginTest.cpp.
/// </summary>
public class ComputedMarginTests
{
  // Equivalent of C++ YGNodeLayoutGetMargin, which resolves Start/End to
  // physical edges based on the computed layout direction.
  private static float LayoutGetMargin(FlexNode node, Edge edge)
  {
    bool isRtl = node.Layout.Direction == Direction.RTL;
    return edge switch
    {
      Edge.Start => node.Layout.GetMargin(isRtl ? PhysicalEdge.Right : PhysicalEdge.Left),
      Edge.End => node.Layout.GetMargin(isRtl ? PhysicalEdge.Left : PhysicalEdge.Right),
      Edge.Left => node.Layout.GetMargin(PhysicalEdge.Left),
      Edge.Top => node.Layout.GetMargin(PhysicalEdge.Top),
      Edge.Right => node.Layout.GetMargin(PhysicalEdge.Right),
      Edge.Bottom => node.Layout.GetMargin(PhysicalEdge.Bottom),
      Edge.Horizontal or Edge.Vertical or Edge.All or _ => throw new ArgumentOutOfRangeException(nameof(edge)),
    };
  }

  public void computed_layout_margin()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.Style.SetMargin(Edge.Start, StyleLength.Percent(10f));

    CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

    root.Layout.GetMargin(PhysicalEdge.Left).ShouldBe(10f);
    root.Layout.GetMargin(PhysicalEdge.Right).ShouldBe(0f);

    CalculateLayout.Calculate(root, 100, 100, Direction.RTL);

    root.Layout.GetMargin(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetMargin(PhysicalEdge.Right).ShouldBe(10f);
  }

  public void margin_side_overrides_horizontal_and_vertical()
  {
    Edge[] edges = [Edge.Top, Edge.Bottom, Edge.Start, Edge.End, Edge.Left, Edge.Right];

    for (float edgeValue = 0; edgeValue < 2; ++edgeValue)
    {
      foreach (Edge edge in edges)
      {
        Edge horizontalOrVertical = edge is Edge.Top or Edge.Bottom
            ? Edge.Vertical
            : Edge.Horizontal;

        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetMargin(horizontalOrVertical, StyleLength.Points(10f));
        root.Style.SetMargin(edge, StyleLength.Points(edgeValue));

        CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

        LayoutGetMargin(root, edge).ShouldBe(edgeValue);
      }
    }
  }

  public void margin_side_overrides_all()
  {
    Edge[] edges = [Edge.Top, Edge.Bottom, Edge.Start, Edge.End, Edge.Left, Edge.Right];

    for (float edgeValue = 0; edgeValue < 2; ++edgeValue)
    {
      foreach (Edge edge in edges)
      {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetMargin(Edge.All, StyleLength.Points(10f));
        root.Style.SetMargin(edge, StyleLength.Points(edgeValue));

        CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

        LayoutGetMargin(root, edge).ShouldBe(edgeValue);
      }
    }
  }

  public void margin_horizontal_and_vertical_overrides_all()
  {
    Edge[] directions = [Edge.Horizontal, Edge.Vertical];

    for (float directionValue = 0; directionValue < 2; ++directionValue)
    {
      foreach (Edge direction in directions)
      {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetMargin(Edge.All, StyleLength.Points(10f));
        root.Style.SetMargin(direction, StyleLength.Points(directionValue));

        CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

        if (direction == Edge.Vertical)
        {
          LayoutGetMargin(root, Edge.Top).ShouldBe(directionValue);
          LayoutGetMargin(root, Edge.Bottom).ShouldBe(directionValue);
        }
        else
        {
          LayoutGetMargin(root, Edge.Start).ShouldBe(directionValue);
          LayoutGetMargin(root, Edge.End).ShouldBe(directionValue);
          LayoutGetMargin(root, Edge.Left).ShouldBe(directionValue);
          LayoutGetMargin(root, Edge.Right).ShouldBe(directionValue);
        }
      }
    }
  }
}
