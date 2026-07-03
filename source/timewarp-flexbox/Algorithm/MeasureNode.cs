/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp (lines 266-469)
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides node measurement functions for the layout algorithm.
/// These handle special cases: nodes with measure functions, nodes without children,
/// and nodes with fixed sizes.
/// </summary>
public static class MeasureNode
{
  /// <summary>
  /// Measures a node that has a custom measure function (e.g., text nodes).
  /// </summary>
  /// <param name="node">The node to measure.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="widthSizingMode">The width sizing mode.</param>
  /// <param name="heightSizingMode">The height sizing mode.</param>
  /// <param name="ownerWidth">The owner's width for percentage calculations.</param>
  /// <param name="ownerHeight">The owner's height for percentage calculations.</param>
  /// <param name="layoutMarkerData">Layout statistics tracker.</param>
  /// <param name="reason">The reason for this layout pass.</param>
  /// <remarks>
  /// This corresponds to measureNodeWithMeasureFunc in CalculateLayout.cpp (lines 266-376).
  /// The measure function is called with inner dimensions (after subtracting padding/border),
  /// and the result is bounded by min/max constraints.
  /// </remarks>
  public static void MeasureNodeWithMeasureFunc(
      Node node,
      Direction direction,
      float availableWidth,
      float availableHeight,
      SizingMode widthSizingMode,
      SizingMode heightSizingMode,
      float ownerWidth,
      float ownerHeight,
      LayoutData layoutMarkerData,
      LayoutPassReason reason)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(layoutMarkerData);

    YogaAssert.Assert(
        node.HasMeasureFunc,
        "Expected node to have custom measure function");

    if (widthSizingMode == SizingMode.MaxContent)
    {
      availableWidth = float.NaN;
    }

    if (heightSizingMode == SizingMode.MaxContent)
    {
      availableHeight = float.NaN;
    }

    LayoutResults layout = node.Layout;
    float paddingAndBorderAxisRow =
        layout.GetPadding(PhysicalEdge.Left) + layout.GetPadding(PhysicalEdge.Right) +
        layout.GetBorder(PhysicalEdge.Left) + layout.GetBorder(PhysicalEdge.Right);
    float paddingAndBorderAxisColumn =
        layout.GetPadding(PhysicalEdge.Top) + layout.GetPadding(PhysicalEdge.Bottom) +
        layout.GetBorder(PhysicalEdge.Top) + layout.GetBorder(PhysicalEdge.Bottom);

    // We want to make sure we don't call measure with negative size
    float innerWidth = Comparison.IsUndefined(availableWidth)
        ? availableWidth
        : Comparison.MaxOrDefined(0.0f, availableWidth - paddingAndBorderAxisRow);
    float innerHeight = Comparison.IsUndefined(availableHeight)
        ? availableHeight
        : Comparison.MaxOrDefined(0.0f, availableHeight - paddingAndBorderAxisColumn);

    if (widthSizingMode == SizingMode.StretchFit && heightSizingMode == SizingMode.StretchFit)
    {
      // Don't bother sizing the text if both dimensions are already defined.
      node.SetLayoutMeasuredDimension(
          BoundAxis.BoundAxisValue(
              node,
              FlexDirection.Row,
              direction,
              availableWidth,
              ownerWidth,
              ownerWidth),
          Dimension.Width);
      node.SetLayoutMeasuredDimension(
          BoundAxis.BoundAxisValue(
              node,
              FlexDirection.Column,
              direction,
              availableHeight,
              ownerHeight,
              ownerWidth),
          Dimension.Height);
    }
    else
    {
      YogaEvent.PublishMeasureCallbackStart(node);

      // Measure the text under the current constraints.
      YGSize measuredSize = node.Measure(
          innerWidth,
          widthSizingMode.ToMeasureMode(),
          innerHeight,
          heightSizingMode.ToMeasureMode());

      layoutMarkerData.MeasureCallbacks++;
      layoutMarkerData.IncrementMeasureCallbackReasonCount(reason);

      YogaEvent.PublishMeasureCallbackEnd(
          node,
          innerWidth,
          widthSizingMode.ToMeasureMode(),
          innerHeight,
          heightSizingMode.ToMeasureMode(),
          measuredSize.Width,
          measuredSize.Height,
          reason);

      node.SetLayoutMeasuredDimension(
          BoundAxis.BoundAxisValue(
              node,
              FlexDirection.Row,
              direction,
              widthSizingMode == SizingMode.MaxContent || widthSizingMode == SizingMode.FitContent
                  ? measuredSize.Width + paddingAndBorderAxisRow
                  : availableWidth,
              ownerWidth,
              ownerWidth),
          Dimension.Width);

      node.SetLayoutMeasuredDimension(
          BoundAxis.BoundAxisValue(
              node,
              FlexDirection.Column,
              direction,
              heightSizingMode == SizingMode.MaxContent || heightSizingMode == SizingMode.FitContent
                  ? measuredSize.Height + paddingAndBorderAxisColumn
                  : availableHeight,
              ownerHeight,
              ownerWidth),
          Dimension.Height);
    }
  }

  /// <summary>
  /// Measures a node that has no children (leaf node without measure function).
  /// Uses padding and border as the minimum size.
  /// </summary>
  /// <param name="node">The node to measure.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="widthSizingMode">The width sizing mode.</param>
  /// <param name="heightSizingMode">The height sizing mode.</param>
  /// <param name="ownerWidth">The owner's width for percentage calculations.</param>
  /// <param name="ownerHeight">The owner's height for percentage calculations.</param>
  /// <remarks>
  /// This corresponds to measureNodeWithoutChildren in CalculateLayout.cpp (lines 380-419).
  /// For leaf nodes without a measure function, the size is determined by:
  /// - StretchFit: use the available dimension
  /// - MaxContent/FitContent: use padding + border as minimum
  /// </remarks>
  public static void MeasureNodeWithoutChildren(
      Node node,
      Direction direction,
      float availableWidth,
      float availableHeight,
      SizingMode widthSizingMode,
      SizingMode heightSizingMode,
      float ownerWidth,
      float ownerHeight)
  {
    ArgumentNullException.ThrowIfNull(node);

    // Calculate padding and border for both axes
    float paddingAndBorderAxisRow = BoundAxis.PaddingAndBorderForAxis(
        node, FlexDirection.Row, direction, ownerWidth);
    float paddingAndBorderAxisColumn = BoundAxis.PaddingAndBorderForAxis(
        node, FlexDirection.Column, direction, ownerWidth);

    // Determine width based on sizing mode
    float width;
    if (widthSizingMode == SizingMode.StretchFit)
    {
      // In StretchFit mode, use the available width
      width = availableWidth;
    }
    else
    {
      // In MaxContent or FitContent mode, use padding+border as minimum
      width = paddingAndBorderAxisRow;
    }

    // Apply min/max bounds to width
    float boundedWidth = BoundAxis.BoundAxisValue(
        node,
        FlexDirection.Row,
        direction,
        width,
        ownerWidth,
        ownerWidth);

    // Determine height based on sizing mode
    float height;
    if (heightSizingMode == SizingMode.StretchFit)
    {
      // In StretchFit mode, use the available height
      height = availableHeight;
    }
    else
    {
      // In MaxContent or FitContent mode, use padding+border as minimum
      height = paddingAndBorderAxisColumn;
    }

    // Apply min/max bounds to height
    float boundedHeight = BoundAxis.BoundAxisValue(
        node,
        FlexDirection.Column,
        direction,
        height,
        ownerHeight,
        ownerWidth);

    // Set the measured dimensions on the node
    node.SetLayoutMeasuredDimension(boundedWidth, Dimension.Width);
    node.SetLayoutMeasuredDimension(boundedHeight, Dimension.Height);
  }

  /// <summary>
  /// Fast path optimization when both dimensions are already determined (fixed size).
  /// Returns true if measurement was handled, false if regular measurement is needed.
  /// </summary>
  /// <param name="node">The node to measure.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="availableWidth">The available width.</param>
  /// <param name="availableHeight">The available height.</param>
  /// <param name="widthSizingMode">The width sizing mode.</param>
  /// <param name="heightSizingMode">The height sizing mode.</param>
  /// <param name="ownerWidth">The owner's width for percentage calculations.</param>
  /// <param name="ownerHeight">The owner's height for percentage calculations.</param>
  /// <returns>True if both dimensions were fixed and measurement is complete, false otherwise.</returns>
  /// <remarks>
  /// This corresponds to measureNodeWithFixedSize in CalculateLayout.cpp (lines 427-469).
  /// A dimension is considered fixed when:
  /// 1. The sizing mode is StretchFit (exact size is known), OR
  /// 2. The dimension is defined, the mode is FitContent, and the dimension is zero or negative
  /// </remarks>
  public static bool MeasureNodeWithFixedSize(
      Node node,
      Direction direction,
      float availableWidth,
      float availableHeight,
      SizingMode widthSizingMode,
      SizingMode heightSizingMode,
      float ownerWidth,
      float ownerHeight)
  {
    ArgumentNullException.ThrowIfNull(node);

    // Check if both dimensions are fixed
    bool widthIsFixed = LayoutHelpers.IsFixedSize(availableWidth, widthSizingMode);
    bool heightIsFixed = LayoutHelpers.IsFixedSize(availableHeight, heightSizingMode);

    if (!widthIsFixed || !heightIsFixed)
    {
      // Not both dimensions are fixed, need regular measurement
      return false;
    }

    // Both dimensions are fixed - apply bounds and set measured dimensions

    // For width: use available width, bounded by min/max
    float boundedWidth = BoundAxis.BoundAxisValue(
        node,
        FlexDirection.Row,
        direction,
        availableWidth,
        ownerWidth,
        ownerWidth);

    // For height: use available height, bounded by min/max
    float boundedHeight = BoundAxis.BoundAxisValue(
        node,
        FlexDirection.Column,
        direction,
        availableHeight,
        ownerHeight,
        ownerWidth);

    // Set the measured dimensions on the node
    node.SetLayoutMeasuredDimension(boundedWidth, Dimension.Width);
    node.SetLayoutMeasuredDimension(boundedHeight, Dimension.Height);

    return true;
  }
}
