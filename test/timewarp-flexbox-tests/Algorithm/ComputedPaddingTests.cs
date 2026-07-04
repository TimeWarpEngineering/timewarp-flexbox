/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-ported from yoga/tests/YGComputedPaddingTest.cpp
 */

namespace TimeWarp.Flexbox.Tests.Algorithm;

using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for computed layout padding, ported from YGComputedPaddingTest.cpp.
/// </summary>
public class ComputedPaddingTests
{
  // Equivalent of C++ YGNodeLayoutGetPadding, which resolves Start/End to
  // physical edges based on the computed layout direction.
  private static float LayoutGetPadding(FlexNode node, Edge edge)
  {
    bool isRtl = node.Layout.Direction == Direction.RTL;
    return edge switch
    {
      Edge.Start => node.Layout.GetPadding(isRtl ? PhysicalEdge.Right : PhysicalEdge.Left),
      Edge.End => node.Layout.GetPadding(isRtl ? PhysicalEdge.Left : PhysicalEdge.Right),
      Edge.Left => node.Layout.GetPadding(PhysicalEdge.Left),
      Edge.Top => node.Layout.GetPadding(PhysicalEdge.Top),
      Edge.Right => node.Layout.GetPadding(PhysicalEdge.Right),
      Edge.Bottom => node.Layout.GetPadding(PhysicalEdge.Bottom),
      Edge.Horizontal or Edge.Vertical or Edge.All or _ => throw new ArgumentOutOfRangeException(nameof(edge)),
    };
  }

  public void computed_layout_padding()
  {
    FlexNode root = new();
    root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
    root.Style.SetPadding(Edge.Start, StyleLength.Percent(10f));

    CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

    root.Layout.GetPadding(PhysicalEdge.Left).ShouldBe(10f);
    root.Layout.GetPadding(PhysicalEdge.Right).ShouldBe(0f);

    CalculateLayout.Calculate(root, 100, 100, Direction.RTL);

    root.Layout.GetPadding(PhysicalEdge.Left).ShouldBe(0f);
    root.Layout.GetPadding(PhysicalEdge.Right).ShouldBe(10f);
  }

  public void padding_side_overrides_horizontal_and_vertical()
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
        root.Style.SetPadding(horizontalOrVertical, StyleLength.Points(10f));
        root.Style.SetPadding(edge, StyleLength.Points(edgeValue));

        CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

        LayoutGetPadding(root, edge).ShouldBe(edgeValue);
      }
    }
  }

  public void padding_side_overrides_all()
  {
    Edge[] edges = [Edge.Top, Edge.Bottom, Edge.Start, Edge.End, Edge.Left, Edge.Right];

    for (float edgeValue = 0; edgeValue < 2; ++edgeValue)
    {
      foreach (Edge edge in edges)
      {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetPadding(Edge.All, StyleLength.Points(10f));
        root.Style.SetPadding(edge, StyleLength.Points(edgeValue));

        CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

        LayoutGetPadding(root, edge).ShouldBe(edgeValue);
      }
    }
  }

  public void padding_horizontal_and_vertical_overrides_all()
  {
    Edge[] directions = [Edge.Horizontal, Edge.Vertical];

    for (float directionValue = 0; directionValue < 2; ++directionValue)
    {
      foreach (Edge direction in directions)
      {
        FlexNode root = new();
        root.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
        root.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));
        root.Style.SetPadding(Edge.All, StyleLength.Points(10f));
        root.Style.SetPadding(direction, StyleLength.Points(directionValue));

        CalculateLayout.Calculate(root, 100, 100, Direction.LTR);

        if (direction == Edge.Vertical)
        {
          LayoutGetPadding(root, Edge.Top).ShouldBe(directionValue);
          LayoutGetPadding(root, Edge.Bottom).ShouldBe(directionValue);
        }
        else
        {
          LayoutGetPadding(root, Edge.Start).ShouldBe(directionValue);
          LayoutGetPadding(root, Edge.End).ShouldBe(directionValue);
          LayoutGetPadding(root, Edge.Left).ShouldBe(directionValue);
          LayoutGetPadding(root, Edge.Right).ShouldBe(directionValue);
        }
      }
    }
  }
}
