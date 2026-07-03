/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/PixelGrid.h, yoga/algorithm/PixelGrid.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides utilities for rounding layout values to the pixel grid.
/// </summary>
public static class PixelGrid
{
  /// <summary>
  /// Rounds a point value to the nearest physical pixel based on DPI (pointScaleFactor).
  /// </summary>
  /// <param name="value">The value to round.</param>
  /// <param name="pointScaleFactor">The scale factor (pixels per point).</param>
  /// <param name="forceCeil">Force rounding up.</param>
  /// <param name="forceFloor">Force rounding down.</param>
  /// <returns>The rounded value, or NaN if inputs are invalid.</returns>
  public static float RoundValueToPixelGrid(
      double value,
      double pointScaleFactor,
      bool forceCeil,
      bool forceFloor)
  {
    double scaledValue = value * pointScaleFactor;

    // We want to calculate `fractional` such that `floor(scaledValue) = scaledValue - fractional`.
    double fractional = scaledValue % 1.0;

    if (fractional < 0)
    {
      // This branch is for handling negative numbers for `value`.
      //
      // Regarding `floor` and `ceil`. Note that for a number x, `floor(x) <= x <= ceil(x)`
      // even for negative numbers. Here are a couple of examples:
      //   - x =  2.2: floor( 2.2) =  2, ceil( 2.2) =  3
      //   - x = -2.2: floor(-2.2) = -3, ceil(-2.2) = -2
      //
      // Regarding modulo for negative numbers, in C# the % operator returns a negative number
      // for negative operands. For example, -2.2 % 1.0 = -0.2. However, we want `fractional`
      // to be the number such that subtracting it from `value` will give us `floor(value)`.
      // In the case of negative numbers, adding 1 to the modulo result gives us this:
      //   - fractional = -2.2 % 1.0 = -0.2
      //   - Add 1 to the fraction: fractional2 = fractional + 1 = -0.2 + 1 = 0.8
      //   - Finding the `floor`: -2.2 - fractional2 = -2.2 - 0.8 = -3
      ++fractional;
    }

    if (Comparison.InexactEquals((float)fractional, 0))
    {
      // First we check if the value is already rounded
      scaledValue -= fractional;
    }
    else if (Comparison.InexactEquals((float)fractional, 1.0f))
    {
      scaledValue = scaledValue - fractional + 1.0;
    }
    else if (forceCeil)
    {
      // Next we check if we need to use forced rounding
      scaledValue = scaledValue - fractional + 1.0;
    }
    else if (forceFloor)
    {
      scaledValue -= fractional;
    }
    else
    {
      // Finally we just round the value
      scaledValue = scaledValue - fractional +
          (!double.IsNaN(fractional) &&
           (fractional > 0.5 || Comparison.InexactEquals((float)fractional, 0.5f))
              ? 1.0
              : 0.0);
    }

    return (double.IsNaN(scaledValue) || double.IsNaN(pointScaleFactor))
        ? float.NaN
        : (float)(scaledValue / pointScaleFactor);
  }

  /// <summary>
  /// Rounds all layout results in the node tree to the pixel grid.
  /// </summary>
  /// <param name="node">The root node to process.</param>
  /// <param name="absoluteLeft">The absolute left position of the node.</param>
  /// <param name="absoluteTop">The absolute top position of the node.</param>
  /// <remarks>
  /// This method recursively processes all children, rounding positions and dimensions
  /// to align with the physical pixel grid. Text nodes receive special treatment to
  /// avoid unwanted truncation - fractional widths/heights are rounded up (ceil).
  /// </remarks>
  public static void RoundLayoutResultsToPixelGrid(
      Node node,
      double absoluteLeft,
      double absoluteTop)
  {
    ArgumentNullException.ThrowIfNull(node);

    double pointScaleFactor = node.Config.PointScaleFactor;

    double nodeLeft = node.Layout.GetPosition(PhysicalEdge.Left);
    double nodeTop = node.Layout.GetPosition(PhysicalEdge.Top);

    double nodeWidth = node.Layout.GetDimension(Dimension.Width);
    double nodeHeight = node.Layout.GetDimension(Dimension.Height);

    double absoluteNodeLeft = absoluteLeft + nodeLeft;
    double absoluteNodeTop = absoluteTop + nodeTop;

    double absoluteNodeRight = absoluteNodeLeft + nodeWidth;
    double absoluteNodeBottom = absoluteNodeTop + nodeHeight;

    if (pointScaleFactor != 0.0)
    {
      // If a node has a custom measure function we never want to round down its
      // size as this could lead to unwanted text truncation.
      bool textRounding = node.NodeType == NodeType.Text;

      node.SetLayoutPosition(
          RoundValueToPixelGrid(nodeLeft, pointScaleFactor, false, textRounding),
          PhysicalEdge.Left);

      node.SetLayoutPosition(
          RoundValueToPixelGrid(nodeTop, pointScaleFactor, false, textRounding),
          PhysicalEdge.Top);

      // We multiply dimension by scale factor and if the result is close to the
      // whole number, we don't have any fraction. To verify if the result is close
      // to whole number we want to check both floor and ceil numbers.
      double scaledNodeWidth = nodeWidth * pointScaleFactor;
      bool hasFractionalWidth = !Comparison.InexactEquals(Math.Round(scaledNodeWidth), scaledNodeWidth);

      double scaledNodeHeight = nodeHeight * pointScaleFactor;
      bool hasFractionalHeight = !Comparison.InexactEquals(Math.Round(scaledNodeHeight), scaledNodeHeight);

      node.Layout.SetDimension(
          Dimension.Width,
          RoundValueToPixelGrid(
              absoluteNodeRight,
              pointScaleFactor,
              textRounding && hasFractionalWidth,
              textRounding && !hasFractionalWidth) -
          RoundValueToPixelGrid(
              absoluteNodeLeft,
              pointScaleFactor,
              false,
              textRounding));

      node.Layout.SetDimension(
          Dimension.Height,
          RoundValueToPixelGrid(
              absoluteNodeBottom,
              pointScaleFactor,
              textRounding && hasFractionalHeight,
              textRounding && !hasFractionalHeight) -
          RoundValueToPixelGrid(
              absoluteNodeTop,
              pointScaleFactor,
              false,
              textRounding));
    }

    foreach (Node child in node.Children)
    {
      RoundLayoutResultsToPixelGrid(child, absoluteNodeLeft, absoluteNodeTop);
    }
  }
}
