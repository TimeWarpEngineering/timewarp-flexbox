/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/TrailingPosition.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides trailing position calculation utilities for the layout algorithm.
/// </summary>
public static class TrailingPosition
{
  /// <summary>
  /// Given an offset to an edge, returns the offset to the opposite edge on the
  /// same axis. This assumes that the width/height of both nodes is determined at
  /// this point.
  /// </summary>
  /// <param name="position">The position offset from the start edge.</param>
  /// <param name="axis">The flex direction axis.</param>
  /// <param name="containingNode">The containing (parent) node.</param>
  /// <param name="node">The child node.</param>
  /// <returns>The position offset from the end edge.</returns>
  public static float GetPositionOfOppositeEdge(
      float position,
      FlexDirection axis,
      Node containingNode,
      Node node)
  {
    ArgumentNullException.ThrowIfNull(containingNode);
    ArgumentNullException.ThrowIfNull(node);

    return containingNode.Layout.GetMeasuredDimension(axis.GetDimension()) -
           node.Layout.GetMeasuredDimension(axis.GetDimension()) -
           position;
  }

  /// <summary>
  /// Sets the trailing position for a child node along the specified axis.
  /// </summary>
  /// <param name="node">The parent node.</param>
  /// <param name="child">The child node.</param>
  /// <param name="axis">The flex direction axis.</param>
  public static void SetChildTrailingPosition(
      Node node,
      Node child,
      FlexDirection axis)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(child);

    child.SetLayoutPosition(
        GetPositionOfOppositeEdge(
            child.Layout.GetPosition(axis.FlexStartEdge()),
            axis,
            node,
            child),
        axis.FlexEndEdge());
  }

  /// <summary>
  /// Determines whether the axis needs trailing position calculation.
  /// </summary>
  /// <param name="axis">The flex direction axis.</param>
  /// <returns>True if the axis is reversed (RowReverse or ColumnReverse).</returns>
  public static bool NeedsTrailingPosition(FlexDirection axis)
  {
    return axis == FlexDirection.RowReverse ||
           axis == FlexDirection.ColumnReverse;
  }
}
