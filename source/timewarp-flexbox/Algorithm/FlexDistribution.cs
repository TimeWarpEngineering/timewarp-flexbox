/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp (lines 622-980)
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides flex space distribution functions that implement flex-grow and flex-shrink.
/// This is the core of the flexbox sizing algorithm - a two-pass approach that handles min/max constraints.
/// </summary>
/// <remarks>
/// <para>
/// This implementation uses a simplified two-pass algorithm that deviates from the W3C spec.
/// The spec describes a process that repeats until no items violate constraints.
/// Yoga uses exactly two passes for performance:
/// </para>
/// <list type="number">
/// <item>First Pass: Find clamped items, exclude them from flex factor totals</item>
/// <item>Second Pass: Distribute space using adjusted totals</item>
/// </list>
/// </remarks>
internal static class FlexDistribution
{
  /// <summary>
  /// First pass: Detects flex items whose min/max constraints trigger and freezes them at those sizes.
  /// For those flexible items whose min and max constraints are triggered, their clamped size
  /// is removed from the remaining free space.
  /// </summary>
  /// <param name="flexLine">The flex line being processed (modified).</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="mainAxis">The main axis direction.</param>
  /// <param name="ownerWidth">The owner width.</param>
  /// <param name="mainAxisOwnerSize">The main axis size of the owner.</param>
  /// <param name="availableInnerMainDim">The available inner main dimension.</param>
  /// <param name="availableInnerWidth">The available inner width.</param>
  /// <remarks>
  /// This corresponds to distributeFreeSpaceFirstPass in CalculateLayout.cpp (lines 821-906).
  /// </remarks>
  public static void DistributeFreeSpaceFirstPass(
      FlexLine flexLine,
      Direction direction,
      FlexDirection mainAxis,
      float ownerWidth,
      float mainAxisOwnerSize,
      float availableInnerMainDim,
      float availableInnerWidth)
  {
    ArgumentNullException.ThrowIfNull(flexLine);

    float flexShrinkScaledFactor = 0;
    float flexGrowFactor = 0;
    float baseMainSize = 0;
    float boundMainSize = 0;
    float deltaFreeSpace = 0;

    FlexLineRunningLayout layout = flexLine.Layout;

    foreach (Node currentLineChild in flexLine.ItemsInFlow)
    {
      float childFlexBasis = BoundAxis.BoundAxisWithinMinAndMax(
          currentLineChild,
          direction,
          mainAxis,
          currentLineChild.Layout.ComputedFlexBasis,
          mainAxisOwnerSize,
          ownerWidth).Unwrap();

      if (layout.RemainingFreeSpace < 0)
      {
        // Shrink case
        flexShrinkScaledFactor = -currentLineChild.ResolveFlexShrink() * childFlexBasis;

        // Is this child able to shrink?
        if (Comparison.IsDefined(flexShrinkScaledFactor) && flexShrinkScaledFactor != 0)
        {
          baseMainSize = childFlexBasis +
              layout.RemainingFreeSpace /
              layout.TotalFlexShrinkScaledFactors *
              flexShrinkScaledFactor;

          boundMainSize = BoundAxis.BoundAxisValue(
              currentLineChild,
              mainAxis,
              direction,
              baseMainSize,
              availableInnerMainDim,
              availableInnerWidth);

          if (Comparison.IsDefined(baseMainSize) && Comparison.IsDefined(boundMainSize) &&
              !Comparison.InexactEquals(baseMainSize, boundMainSize))
          {
            // By excluding this item's size and flex factor from remaining, this
            // item's min/max constraints should also trigger in the second pass
            // resulting in the item's size calculation being identical in the
            // first and second passes.
            deltaFreeSpace += boundMainSize - childFlexBasis;
            layout.TotalFlexShrinkScaledFactors -=
                (-currentLineChild.ResolveFlexShrink() *
                 currentLineChild.Layout.ComputedFlexBasis.Unwrap());
          }
        }
      }
      else if (Comparison.IsDefined(layout.RemainingFreeSpace) &&
               layout.RemainingFreeSpace > 0)
      {
        // Grow case
        flexGrowFactor = currentLineChild.ResolveFlexGrow();

        // Is this child able to grow?
        if (Comparison.IsDefined(flexGrowFactor) && flexGrowFactor != 0)
        {
          baseMainSize = childFlexBasis +
              layout.RemainingFreeSpace /
              layout.TotalFlexGrowFactors * flexGrowFactor;

          boundMainSize = BoundAxis.BoundAxisValue(
              currentLineChild,
              mainAxis,
              direction,
              baseMainSize,
              availableInnerMainDim,
              availableInnerWidth);

          if (Comparison.IsDefined(baseMainSize) && Comparison.IsDefined(boundMainSize) &&
              !Comparison.InexactEquals(baseMainSize, boundMainSize))
          {
            // By excluding this item's size and flex factor from remaining, this
            // item's min/max constraints should also trigger in the second pass
            // resulting in the item's size calculation being identical in the
            // first and second passes.
            deltaFreeSpace += boundMainSize - childFlexBasis;
            layout.TotalFlexGrowFactors -= flexGrowFactor;
          }
        }
      }
    }

    layout.RemainingFreeSpace -= deltaFreeSpace;
    flexLine.Layout = layout;
  }

  /// <summary>
  /// Second pass: Distributes remaining space to flexible items and recursively layouts each child.
  /// This function should be called after distributeFreeSpaceFirstPass.
  /// </summary>
  /// <param name="flexLine">The flex line being processed (modified).</param>
  /// <param name="node">The parent node.</param>
  /// <param name="mainAxis">The main axis direction.</param>
  /// <param name="crossAxis">The cross axis direction.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="ownerWidth">The owner width.</param>
  /// <param name="mainAxisOwnerSize">The main axis size of the owner.</param>
  /// <param name="availableInnerMainDim">The available inner main dimension.</param>
  /// <param name="availableInnerCrossDim">The available inner cross dimension.</param>
  /// <param name="availableInnerWidth">The available inner width.</param>
  /// <param name="availableInnerHeight">The available inner height.</param>
  /// <param name="mainAxisOverflows">Whether the main axis has overflow.</param>
  /// <param name="sizingModeCrossDim">The sizing mode for the cross dimension.</param>
  /// <param name="performLayout">Whether to perform full layout vs. measure only.</param>
  /// <param name="layoutMarkerData">Layout metrics tracking data.</param>
  /// <param name="depth">The current recursion depth.</param>
  /// <param name="generationCount">The current generation count.</param>
  /// <returns>The delta free space consumed.</returns>
  /// <remarks>
  /// This corresponds to distributeFreeSpaceSecondPass in CalculateLayout.cpp (lines 622-816).
  /// </remarks>
  public static float DistributeFreeSpaceSecondPass(
      FlexLine flexLine,
      Node node,
      FlexDirection mainAxis,
      FlexDirection crossAxis,
      Direction direction,
      float ownerWidth,
      float mainAxisOwnerSize,
      float availableInnerMainDim,
      float availableInnerCrossDim,
      float availableInnerWidth,
      float availableInnerHeight,
      bool mainAxisOverflows,
      SizingMode sizingModeCrossDim,
      bool performLayout,
      LayoutData layoutMarkerData,
      int depth,
      uint generationCount)
  {
    ArgumentNullException.ThrowIfNull(flexLine);
    ArgumentNullException.ThrowIfNull(node);

    float childFlexBasis = 0;
    float flexShrinkScaledFactor = 0;
    float flexGrowFactor = 0;
    float deltaFreeSpace = 0;
    bool isMainAxisRow = mainAxis.IsRow();
    bool isNodeFlexWrap = node.Style.FlexWrap != Wrap.NoWrap;

    FlexLineRunningLayout layout = flexLine.Layout;

    foreach (Node currentLineChild in flexLine.ItemsInFlow)
    {
      childFlexBasis = BoundAxis.BoundAxisWithinMinAndMax(
          currentLineChild,
          direction,
          mainAxis,
          currentLineChild.Layout.ComputedFlexBasis,
          mainAxisOwnerSize,
          ownerWidth).Unwrap();

      float updatedMainSize = childFlexBasis;

      if (Comparison.IsDefined(layout.RemainingFreeSpace) &&
          layout.RemainingFreeSpace < 0)
      {
        // Shrink case
        flexShrinkScaledFactor = -currentLineChild.ResolveFlexShrink() * childFlexBasis;

        // Is this child able to shrink?
        if (flexShrinkScaledFactor != 0)
        {
          float childSize;

          if (Comparison.IsDefined(layout.TotalFlexShrinkScaledFactors) &&
              layout.TotalFlexShrinkScaledFactors == 0)
          {
            childSize = childFlexBasis + flexShrinkScaledFactor;
          }
          else
          {
            childSize = childFlexBasis +
                (layout.RemainingFreeSpace /
                 layout.TotalFlexShrinkScaledFactors) *
                flexShrinkScaledFactor;
          }

          updatedMainSize = BoundAxis.BoundAxisValue(
              currentLineChild,
              mainAxis,
              direction,
              childSize,
              availableInnerMainDim,
              availableInnerWidth);
        }
      }
      else if (Comparison.IsDefined(layout.RemainingFreeSpace) &&
               layout.RemainingFreeSpace > 0)
      {
        // Grow case
        flexGrowFactor = currentLineChild.ResolveFlexGrow();

        // Is this child able to grow?
        if (!float.IsNaN(flexGrowFactor) && flexGrowFactor != 0)
        {
          updatedMainSize = BoundAxis.BoundAxisValue(
              currentLineChild,
              mainAxis,
              direction,
              childFlexBasis +
                  layout.RemainingFreeSpace /
                  layout.TotalFlexGrowFactors * flexGrowFactor,
              availableInnerMainDim,
              availableInnerWidth);
        }
      }

      deltaFreeSpace += updatedMainSize - childFlexBasis;

      float marginMain = currentLineChild.Style.ComputeMarginForAxis(mainAxis, availableInnerWidth);
      float marginCross = currentLineChild.Style.ComputeMarginForAxis(crossAxis, availableInnerWidth);

      float childCrossSize = float.NaN;
      float childMainSize = updatedMainSize + marginMain;
      SizingMode childCrossSizingMode;
      SizingMode childMainSizingMode = SizingMode.StretchFit;

      Style childStyle = currentLineChild.Style;

      if (childStyle.AspectRatio.IsDefined)
      {
        // Aspect ratio defined: derive cross size from main size
        childCrossSize = isMainAxisRow
            ? (childMainSize - marginMain) / childStyle.AspectRatio.Unwrap()
            : (childMainSize - marginMain) * childStyle.AspectRatio.Unwrap();
        childCrossSizingMode = SizingMode.StretchFit;
        childCrossSize += marginCross;
      }
      else if (!float.IsNaN(availableInnerCrossDim) &&
               !currentLineChild.HasDefiniteLength(crossAxis.GetDimension(), availableInnerCrossDim) &&
               sizingModeCrossDim == SizingMode.StretchFit &&
               !(isNodeFlexWrap && mainAxisOverflows) &&
               AlignUtils.ResolveChildAlignment(node, currentLineChild) == Align.Stretch &&
               !currentLineChild.Style.FlexStartMarginIsAuto(crossAxis, direction) &&
               !currentLineChild.Style.FlexEndMarginIsAuto(crossAxis, direction))
      {
        // Stretch alignment: use available cross dim
        childCrossSize = availableInnerCrossDim;
        childCrossSizingMode = SizingMode.StretchFit;
      }
      else if (!currentLineChild.HasDefiniteLength(crossAxis.GetDimension(), availableInnerCrossDim))
      {
        // No definite cross dimension: measure content
        childCrossSize = availableInnerCrossDim;
        childCrossSizingMode = Comparison.IsUndefined(childCrossSize)
            ? SizingMode.MaxContent
            : SizingMode.FitContent;
      }
      else
      {
        // Definite cross dimension
        childCrossSize = currentLineChild
            .GetResolvedDimension(direction, crossAxis.GetDimension(), availableInnerCrossDim, availableInnerWidth)
            .Unwrap() + marginCross;

        bool isLoosePercentageMeasurement =
            currentLineChild.GetProcessedDimension(crossAxis.GetDimension()).IsPercent &&
            sizingModeCrossDim != SizingMode.StretchFit;

        childCrossSizingMode = Comparison.IsUndefined(childCrossSize) || isLoosePercentageMeasurement
            ? SizingMode.MaxContent
            : SizingMode.StretchFit;
      }

      // Apply max size constraints
      LayoutHelpers.ConstrainMaxSizeForMode(
          currentLineChild,
          direction,
          mainAxis,
          availableInnerMainDim,
          availableInnerWidth,
          ref childMainSizingMode,
          ref childMainSize);

      LayoutHelpers.ConstrainMaxSizeForMode(
          currentLineChild,
          direction,
          crossAxis,
          availableInnerCrossDim,
          availableInnerWidth,
          ref childCrossSizingMode,
          ref childCrossSize);

      bool requiresStretchLayout =
          !currentLineChild.HasDefiniteLength(crossAxis.GetDimension(), availableInnerCrossDim) &&
          AlignUtils.ResolveChildAlignment(node, currentLineChild) == Align.Stretch &&
          !currentLineChild.Style.FlexStartMarginIsAuto(crossAxis, direction) &&
          !currentLineChild.Style.FlexEndMarginIsAuto(crossAxis, direction);

      float childWidth = isMainAxisRow ? childMainSize : childCrossSize;
      float childHeight = !isMainAxisRow ? childMainSize : childCrossSize;

      SizingMode childWidthSizingMode = isMainAxisRow ? childMainSizingMode : childCrossSizingMode;
      SizingMode childHeightSizingMode = !isMainAxisRow ? childMainSizingMode : childCrossSizingMode;

      bool isLayoutPass = performLayout && !requiresStretchLayout;

      // Recursively call the layout algorithm for this child with the updated main size.
      if (FlexBasis.CalculateLayoutInternal is not null)
      {
        FlexBasis.CalculateLayoutInternal(
            currentLineChild,
            childWidth,
            childHeight,
            node.Layout.Direction,
            childWidthSizingMode,
            childHeightSizingMode,
            availableInnerWidth,
            availableInnerHeight,
            isLayoutPass,
            isLayoutPass ? LayoutPassReason.FlexLayout : LayoutPassReason.FlexMeasure,
            layoutMarkerData,
            depth,
            generationCount);

        node.SetLayoutHadOverflow(
            node.Layout.HadOverflow ||
            currentLineChild.Layout.HadOverflow);
      }
    }

    return deltaFreeSpace;
  }

  /// <summary>
  /// Orchestrates the two-pass flex resolution algorithm.
  /// At the end of this function the child nodes would have the proper size assigned to them.
  /// </summary>
  /// <param name="node">The parent node.</param>
  /// <param name="flexLine">The flex line being processed (modified).</param>
  /// <param name="mainAxis">The main axis direction.</param>
  /// <param name="crossAxis">The cross axis direction.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="ownerWidth">The owner width.</param>
  /// <param name="mainAxisOwnerSize">The main axis size of the owner.</param>
  /// <param name="availableInnerMainDim">The available inner main dimension.</param>
  /// <param name="availableInnerCrossDim">The available inner cross dimension.</param>
  /// <param name="availableInnerWidth">The available inner width.</param>
  /// <param name="availableInnerHeight">The available inner height.</param>
  /// <param name="mainAxisOverflows">Whether the main axis has overflow.</param>
  /// <param name="sizingModeCrossDim">The sizing mode for the cross dimension.</param>
  /// <param name="performLayout">Whether to perform full layout vs. measure only.</param>
  /// <param name="layoutMarkerData">Layout metrics tracking data.</param>
  /// <param name="depth">The current recursion depth.</param>
  /// <param name="generationCount">The current generation count.</param>
  /// <remarks>
  /// <para>
  /// This corresponds to resolveFlexibleLength in CalculateLayout.cpp (lines 930-980).
  /// </para>
  /// <para>
  /// This two pass approach for resolving min/max constraints deviates from the spec.
  /// The spec (https://www.w3.org/TR/CSS-flexbox-1/#resolve-flexible-lengths) describes a
  /// process that needs to be repeated a variable number of times. The algorithm
  /// implemented here won't handle all cases but it was simpler to implement and
  /// it mitigates performance concerns because we know exactly how many passes it'll do.
  /// </para>
  /// </remarks>
  public static void ResolveFlexibleLength(
      Node node,
      FlexLine flexLine,
      FlexDirection mainAxis,
      FlexDirection crossAxis,
      Direction direction,
      float ownerWidth,
      float mainAxisOwnerSize,
      float availableInnerMainDim,
      float availableInnerCrossDim,
      float availableInnerWidth,
      float availableInnerHeight,
      bool mainAxisOverflows,
      SizingMode sizingModeCrossDim,
      bool performLayout,
      LayoutData layoutMarkerData,
      int depth,
      uint generationCount)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(flexLine);

    float originalFreeSpace = flexLine.Layout.RemainingFreeSpace;

    // First pass: detect the flex items whose min/max constraints trigger
    DistributeFreeSpaceFirstPass(
        flexLine,
        direction,
        mainAxis,
        ownerWidth,
        mainAxisOwnerSize,
        availableInnerMainDim,
        availableInnerWidth);

    // Second pass: resolve the sizes of the flexible items
    float distributedFreeSpace = DistributeFreeSpaceSecondPass(
        flexLine,
        node,
        mainAxis,
        crossAxis,
        direction,
        ownerWidth,
        mainAxisOwnerSize,
        availableInnerMainDim,
        availableInnerCrossDim,
        availableInnerWidth,
        availableInnerHeight,
        mainAxisOverflows,
        sizingModeCrossDim,
        performLayout,
        layoutMarkerData,
        depth,
        generationCount);

    // Update remaining free space
    FlexLineRunningLayout layout = flexLine.Layout;
    layout.RemainingFreeSpace = originalFreeSpace - distributedFreeSpace;
    flexLine.Layout = layout;
  }
}
