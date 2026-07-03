/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp (lines 982-1156)
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides justify-content positioning along the main axis.
/// </summary>
public static class JustifyContent
{
  /// <summary>
  /// Positions items along the main axis according to justify-content,
  /// handles auto margins, and calculates cross dimension based on item sizes.
  /// </summary>
  /// <param name="node">The parent flex container node.</param>
  /// <param name="flexLine">The flex line being justified.</param>
  /// <param name="mainAxis">The main axis direction.</param>
  /// <param name="crossAxis">The cross axis direction.</param>
  /// <param name="direction">The resolved direction (LTR/RTL).</param>
  /// <param name="sizingModeMainDim">The sizing mode for the main dimension.</param>
  /// <param name="sizingModeCrossDim">The sizing mode for the cross dimension.</param>
  /// <param name="mainAxisOwnerSize">The owner's size along the main axis.</param>
  /// <param name="ownerWidth">The owner's width (used for percentage resolution).</param>
  /// <param name="availableInnerMainDim">Available inner space along main axis.</param>
  /// <param name="availableInnerCrossDim">Available inner space along cross axis.</param>
  /// <param name="availableInnerWidth">Available inner width.</param>
  /// <param name="performLayout">Whether to actually set positions (true) or just measure (false).</param>
  public static void JustifyMainAxis(
      Node node,
      FlexLine flexLine,
      FlexDirection mainAxis,
      FlexDirection crossAxis,
      Direction direction,
      SizingMode sizingModeMainDim,
      SizingMode sizingModeCrossDim,
      float mainAxisOwnerSize,
      float ownerWidth,
      float availableInnerMainDim,
      float availableInnerCrossDim,
      float availableInnerWidth,
      bool performLayout)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(flexLine);

    Style style = node.Style;

    float leadingPaddingAndBorderMain = style.ComputeFlexStartPaddingAndBorder(
        mainAxis, direction, ownerWidth);
    float trailingPaddingAndBorderMain = style.ComputeFlexEndPaddingAndBorder(
        mainAxis, direction, ownerWidth);

    float gap = style.ComputeGapForAxis(mainAxis, availableInnerMainDim);

    // If we are using "at most" rules in the main axis, make sure that
    // remainingFreeSpace is 0 when min main dimension is not given
    if (sizingModeMainDim == SizingMode.FitContent &&
        flexLine.Layout.RemainingFreeSpace > 0)
    {
      Dimension mainDimension = mainAxis.GetDimension();
      StyleSizeLength minDimension = style.GetMinDimension(mainDimension);

      if (minDimension.IsDefined)
      {
        FloatOptional resolvedMinDimension = style.ResolvedMinDimension(
            direction, mainDimension, mainAxisOwnerSize, ownerWidth);

        if (resolvedMinDimension.IsDefined)
        {
          // This condition makes sure that if the size of main dimension(after
          // considering child nodes main dim, leading and trailing padding etc)
          // falls below min dimension, then the remainingFreeSpace is reassigned
          // considering the min dimension

          // `minAvailableMainDim` denotes minimum available space in which child
          // can be laid out, it will exclude space consumed by padding and border.
          float minAvailableMainDim = resolvedMinDimension.Unwrap() -
              leadingPaddingAndBorderMain - trailingPaddingAndBorderMain;
          float occupiedSpaceByChildNodes =
              availableInnerMainDim - flexLine.Layout.RemainingFreeSpace;

          FlexLineRunningLayout layout = flexLine.Layout;
          layout.RemainingFreeSpace = Comparison.MaxOrDefined(
              0.0f, minAvailableMainDim - occupiedSpaceByChildNodes);
          flexLine.Layout = layout;
        }
        else
        {
          FlexLineRunningLayout layout = flexLine.Layout;
          layout.RemainingFreeSpace = 0;
          flexLine.Layout = layout;
        }
      }
      else
      {
        FlexLineRunningLayout layout = flexLine.Layout;
        layout.RemainingFreeSpace = 0;
        flexLine.Layout = layout;
      }
    }

    // In order to position the elements in the main axis, we have two controls.
    // The space between the beginning and the first element and the space between
    // each two elements.
    float leadingMainDim = 0;
    float betweenMainDim = gap;

    // Use fallback alignment when there's negative free space (overflow)
    Justify justifyContent = flexLine.Layout.RemainingFreeSpace >= 0
        ? style.JustifyContent
        : AlignUtils.FallbackAlignment(style.JustifyContent);

    if (flexLine.NumberOfAutoMargins == 0)
    {
      switch (justifyContent)
      {
        case Justify.Center:
          leadingMainDim = flexLine.Layout.RemainingFreeSpace / 2;
          break;
        case Justify.FlexEnd:
          leadingMainDim = flexLine.Layout.RemainingFreeSpace;
          break;
        case Justify.SpaceBetween:
          if (flexLine.ItemsInFlow.Count > 1)
          {
            betweenMainDim += flexLine.Layout.RemainingFreeSpace /
                (flexLine.ItemsInFlow.Count - 1);
          }

          break;
        case Justify.SpaceEvenly:
          // Space is distributed evenly across all elements
          leadingMainDim = flexLine.Layout.RemainingFreeSpace /
              (flexLine.ItemsInFlow.Count + 1);
          betweenMainDim += leadingMainDim;
          break;
        case Justify.SpaceAround:
          // Space on the edges is half of the space between elements
          leadingMainDim = 0.5f * flexLine.Layout.RemainingFreeSpace /
              flexLine.ItemsInFlow.Count;
          betweenMainDim += leadingMainDim * 2;
          break;
        case Justify.FlexStart:
        default:
          break;
      }
    }

    // Update the layout struct - must copy, modify, reassign since it's a struct
    FlexLineRunningLayout runningLayout = flexLine.Layout;
    runningLayout.MainDim = leadingPaddingAndBorderMain + leadingMainDim;
    runningLayout.CrossDim = 0;
    flexLine.Layout = runningLayout;

    float maxAscentForCurrentLine = 0;
    float maxDescentForCurrentLine = 0;
    bool isNodeBaselineLayout = Baseline.IsBaselineLayout(node);

    for (int i = 0; i < flexLine.ItemsInFlow.Count; i++)
    {
      Node child = flexLine.ItemsInFlow[i];
      LayoutResults childLayout = child.Layout;

      if (child.Style.FlexStartMarginIsAuto(mainAxis, direction) &&
          flexLine.Layout.RemainingFreeSpace > 0.0f)
      {
        runningLayout = flexLine.Layout;
        runningLayout.MainDim += flexLine.Layout.RemainingFreeSpace /
            flexLine.NumberOfAutoMargins;
        flexLine.Layout = runningLayout;
      }

      if (performLayout)
      {
        child.Layout.SetPosition(
            mainAxis.FlexStartEdge(),
            childLayout.GetPosition(mainAxis.FlexStartEdge()) +
                flexLine.Layout.MainDim);
      }

      if (i != flexLine.ItemsInFlow.Count - 1)
      {
        runningLayout = flexLine.Layout;
        runningLayout.MainDim += betweenMainDim;
        flexLine.Layout = runningLayout;
      }

      if (child.Style.FlexEndMarginIsAuto(mainAxis, direction) &&
          flexLine.Layout.RemainingFreeSpace > 0.0f)
      {
        runningLayout = flexLine.Layout;
        runningLayout.MainDim += flexLine.Layout.RemainingFreeSpace /
            flexLine.NumberOfAutoMargins;
        flexLine.Layout = runningLayout;
      }

      bool canSkipFlex = !performLayout && sizingModeCrossDim == SizingMode.StretchFit;
      if (canSkipFlex)
      {
        // If we skipped the flex step, then we can't rely on the measuredDims
        // because they weren't computed. This means we can't call
        // dimensionWithMargin.
        runningLayout = flexLine.Layout;
        runningLayout.MainDim +=
            child.Style.ComputeMarginForAxis(mainAxis, availableInnerWidth) +
            BoundAxis.BoundAxisWithinMinAndMax(
                child,
                direction,
                mainAxis,
                childLayout.ComputedFlexBasis,
                mainAxisOwnerSize,
                ownerWidth).Unwrap();
        runningLayout.CrossDim = availableInnerCrossDim;
        flexLine.Layout = runningLayout;
      }
      else
      {
        // The main dimension is the sum of all the elements dimension plus
        // the spacing.
        runningLayout = flexLine.Layout;
        runningLayout.MainDim +=
            child.DimensionWithMargin(mainAxis, availableInnerWidth);

        if (isNodeBaselineLayout)
        {
          // If the child is baseline aligned then the cross dimension is
          // calculated by adding maxAscent and maxDescent from the baseline.
          float ascent = Baseline.CalculateBaseline(child) +
              child.Style.ComputeFlexStartMargin(
                  FlexDirection.Column, direction, availableInnerWidth);
          float descent =
              child.Layout.GetMeasuredDimension(Dimension.Height) +
              child.Style.ComputeMarginForAxis(
                  FlexDirection.Column, availableInnerWidth) -
              ascent;

          maxAscentForCurrentLine = Comparison.MaxOrDefined(maxAscentForCurrentLine, ascent);
          maxDescentForCurrentLine = Comparison.MaxOrDefined(maxDescentForCurrentLine, descent);
        }
        else
        {
          // The cross dimension is the max of the elements dimension since
          // there can only be one element in that cross dimension in the case
          // when the items are not baseline aligned
          runningLayout.CrossDim = Comparison.MaxOrDefined(
              runningLayout.CrossDim,
              child.DimensionWithMargin(crossAxis, availableInnerWidth));
        }

        flexLine.Layout = runningLayout;
      }
    }

    runningLayout = flexLine.Layout;
    runningLayout.MainDim += trailingPaddingAndBorderMain;

    if (isNodeBaselineLayout)
    {
      runningLayout.CrossDim = maxAscentForCurrentLine + maxDescentForCurrentLine;
    }

    flexLine.Layout = runningLayout;
  }
}
