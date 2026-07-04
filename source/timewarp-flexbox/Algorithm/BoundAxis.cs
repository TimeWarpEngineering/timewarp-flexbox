/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/BoundAxis.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides axis bounding utilities for the layout algorithm.
/// </summary>
internal static class BoundAxis
{
  /// <summary>
  /// Gets the total padding and border for the specified axis.
  /// </summary>
  /// <param name="node">The node.</param>
  /// <param name="axis">The flex direction axis.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="widthSize">The width for percentage calculations.</param>
  /// <returns>The total padding and border for the axis.</returns>
  public static float PaddingAndBorderForAxis(
      Node node,
      FlexDirection axis,
      Direction direction,
      float widthSize)
  {
    ArgumentNullException.ThrowIfNull(node);

    return node.Style.ComputeInlineStartPaddingAndBorder(axis, direction, widthSize) +
           node.Style.ComputeInlineEndPaddingAndBorder(axis, direction, widthSize);
  }

  /// <summary>
  /// Bounds a value within the min and max dimensions for the specified axis.
  /// </summary>
  /// <param name="node">The node.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="axis">The flex direction axis.</param>
  /// <param name="value">The value to bound.</param>
  /// <param name="axisSize">The size of the container along the axis.</param>
  /// <param name="widthSize">The width for percentage calculations.</param>
  /// <returns>The bounded value.</returns>
  public static FloatOptional BoundAxisWithinMinAndMax(
      Node node,
      Direction direction,
      FlexDirection axis,
      FloatOptional value,
      float axisSize,
      float widthSize)
  {
    ArgumentNullException.ThrowIfNull(node);

    FloatOptional min;
    FloatOptional max;

    if (axis.IsColumn())
    {
      min = node.Style.ResolvedMinDimension(direction, Dimension.Height, axisSize, widthSize);
      max = node.Style.ResolvedMaxDimension(direction, Dimension.Height, axisSize, widthSize);
    }
    else // isRow
    {
      min = node.Style.ResolvedMinDimension(direction, Dimension.Width, axisSize, widthSize);
      max = node.Style.ResolvedMaxDimension(direction, Dimension.Width, axisSize, widthSize);
    }

    if (max >= new FloatOptional(0) && value > max)
    {
      return max;
    }

    if (min >= new FloatOptional(0) && value < min)
    {
      return min;
    }

    return value;
  }

  /// <summary>
  /// Like BoundAxisWithinMinAndMax but also ensures that the value doesn't
  /// go below the padding and border amount.
  /// </summary>
  /// <param name="node">The node.</param>
  /// <param name="axis">The flex direction axis.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="value">The value to bound.</param>
  /// <param name="axisSize">The size of the container along the axis.</param>
  /// <param name="widthSize">The width for percentage calculations.</param>
  /// <returns>The bounded value.</returns>
  public static float BoundAxisValue(
      Node node,
      FlexDirection axis,
      Direction direction,
      float value,
      float axisSize,
      float widthSize)
  {
    ArgumentNullException.ThrowIfNull(node);

    return Comparison.MaxOrDefined(
        BoundAxisWithinMinAndMax(
            node,
            direction,
            axis,
            new FloatOptional(value),
            axisSize,
            widthSize).Unwrap(),
        PaddingAndBorderForAxis(node, axis, direction, widthSize));
  }
}
