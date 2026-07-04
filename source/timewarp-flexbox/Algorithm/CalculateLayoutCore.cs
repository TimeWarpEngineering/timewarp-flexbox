/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp (lines 1214-2139)
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// The core layout algorithm implementation.
/// Implements all 11 steps of the flexbox layout algorithm.
/// </summary>
internal static class CalculateLayoutCore
{
  /// <summary>
  /// The main recursive layout function that implements the 11-step flexbox algorithm.
  /// </summary>
  public static void Calculate(
      Node node,
      float availableWidth,
      float availableHeight,
      Direction ownerDirection,
      SizingMode widthSizingMode,
      SizingMode heightSizingMode,
      float ownerWidth,
      float ownerHeight,
      bool performLayout,
      LayoutPassReason reason,
      LayoutData layoutMarkerData,
      int depth,
      int generationCount)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(layoutMarkerData);

    // Assertions
    YogaAssert.Assert(
        node,
        !float.IsNaN(availableWidth) || widthSizingMode == SizingMode.MaxContent,
        "availableWidth is indefinite so widthSizingMode must be SizingMode.MaxContent");
    YogaAssert.Assert(
        node,
        !float.IsNaN(availableHeight) || heightSizingMode == SizingMode.MaxContent,
        "availableHeight is indefinite so heightSizingMode must be SizingMode.MaxContent");

    if (performLayout)
    {
      layoutMarkerData.Layouts++;
    }
    else
    {
      layoutMarkerData.Measures++;
    }

    // Set the resolved direction in the node's layout.
    Direction direction = node.ResolveDirection(ownerDirection);
    node.Layout.SetDirection(direction);

    FlexDirection flexRowDirection = FlexDirection.Row.ResolveDirection(direction);
    FlexDirection flexColumnDirection = FlexDirection.Column.ResolveDirection(direction);

    PhysicalEdge startEdge = direction == Direction.LTR ? PhysicalEdge.Left : PhysicalEdge.Right;
    PhysicalEdge endEdge = direction == Direction.LTR ? PhysicalEdge.Right : PhysicalEdge.Left;

    // Set margins
    float marginRowLeading = node.Style.ComputeInlineStartMargin(flexRowDirection, direction, ownerWidth);
    node.Layout.SetMargin(startEdge, marginRowLeading);
    float marginRowTrailing = node.Style.ComputeInlineEndMargin(flexRowDirection, direction, ownerWidth);
    node.Layout.SetMargin(endEdge, marginRowTrailing);
    float marginColumnLeading = node.Style.ComputeInlineStartMargin(flexColumnDirection, direction, ownerWidth);
    node.Layout.SetMargin(PhysicalEdge.Top, marginColumnLeading);
    float marginColumnTrailing = node.Style.ComputeInlineEndMargin(flexColumnDirection, direction, ownerWidth);
    node.Layout.SetMargin(PhysicalEdge.Bottom, marginColumnTrailing);

    float marginAxisRow = marginRowLeading + marginRowTrailing;
    float marginAxisColumn = marginColumnLeading + marginColumnTrailing;

    // Set borders
    node.Layout.SetBorder(startEdge, node.Style.ComputeInlineStartBorder(flexRowDirection, direction));
    node.Layout.SetBorder(endEdge, node.Style.ComputeInlineEndBorder(flexRowDirection, direction));
    node.Layout.SetBorder(PhysicalEdge.Top, node.Style.ComputeInlineStartBorder(flexColumnDirection, direction));
    node.Layout.SetBorder(PhysicalEdge.Bottom, node.Style.ComputeInlineEndBorder(flexColumnDirection, direction));

    // Set paddings
    node.Layout.SetPadding(startEdge, node.Style.ComputeInlineStartPadding(flexRowDirection, direction, ownerWidth));
    node.Layout.SetPadding(endEdge, node.Style.ComputeInlineEndPadding(flexRowDirection, direction, ownerWidth));
    node.Layout.SetPadding(PhysicalEdge.Top, node.Style.ComputeInlineStartPadding(flexColumnDirection, direction, ownerWidth));
    node.Layout.SetPadding(PhysicalEdge.Bottom, node.Style.ComputeInlineEndPadding(flexColumnDirection, direction, ownerWidth));

    // Early return: node has measure function
    if (node.HasMeasureFunc)
    {
      MeasureNode.MeasureNodeWithMeasureFunc(
          node,
          direction,
          availableWidth - marginAxisRow,
          availableHeight - marginAxisColumn,
          widthSizingMode,
          heightSizingMode,
          ownerWidth,
          ownerHeight,
          layoutMarkerData,
          reason);

      LayoutHelpers.CleanupContentsNodesRecursively(node, performLayout);
      return;
    }

    int childCount = node.LayoutChildCount;

    // Early return: no children
    if (childCount == 0)
    {
      MeasureNode.MeasureNodeWithoutChildren(
          node,
          direction,
          availableWidth - marginAxisRow,
          availableHeight - marginAxisColumn,
          widthSizingMode,
          heightSizingMode,
          ownerWidth,
          ownerHeight);

      LayoutHelpers.CleanupContentsNodesRecursively(node, performLayout);
      return;
    }

    // Early return: fixed size optimization (measure only)
    if (!performLayout &&
        MeasureNode.MeasureNodeWithFixedSize(
            node,
            direction,
            availableWidth - marginAxisRow,
            availableHeight - marginAxisColumn,
            widthSizingMode,
            heightSizingMode,
            ownerWidth,
            ownerHeight))
    {
      LayoutHelpers.CleanupContentsNodesRecursively(node, didPerformLayout: false);
      return;
    }

    // At this point we know we're going to perform work.
    node.CloneChildrenIfNeeded();
    node.Layout.SetHadOverflow(false);
    LayoutHelpers.CleanupContentsNodesRecursively(node, performLayout);

    // STEP 1: CALCULATE VALUES FOR REMAINDER OF ALGORITHM
    FlexDirection mainAxis = node.Style.FlexDirection.ResolveDirection(direction);
    FlexDirection crossAxis = mainAxis.ResolveCrossDirection(direction);
    bool isMainAxisRow = mainAxis.IsRow();
    bool isNodeFlexWrap = node.Style.FlexWrap != Wrap.NoWrap;

    float mainAxisOwnerSize = isMainAxisRow ? ownerWidth : ownerHeight;
    float crossAxisOwnerSize = isMainAxisRow ? ownerHeight : ownerWidth;

    float paddingAndBorderAxisMain = BoundAxis.PaddingAndBorderForAxis(node, mainAxis, direction, ownerWidth);
    float paddingAndBorderAxisCross = BoundAxis.PaddingAndBorderForAxis(node, crossAxis, direction, ownerWidth);
    float leadingPaddingAndBorderCross = node.Style.ComputeFlexStartPaddingAndBorder(crossAxis, direction, ownerWidth);

    SizingMode sizingModeMainDim = isMainAxisRow ? widthSizingMode : heightSizingMode;
    SizingMode sizingModeCrossDim = isMainAxisRow ? heightSizingMode : widthSizingMode;

    float paddingAndBorderAxisRow = isMainAxisRow ? paddingAndBorderAxisMain : paddingAndBorderAxisCross;
    float paddingAndBorderAxisColumn = isMainAxisRow ? paddingAndBorderAxisCross : paddingAndBorderAxisMain;

    // STEP 2: DETERMINE AVAILABLE SIZE IN MAIN AND CROSS DIRECTIONS
    float availableInnerWidth = LayoutHelpers.CalculateAvailableInnerDimension(
        node, direction, Dimension.Width,
        availableWidth - marginAxisRow, paddingAndBorderAxisRow,
        ownerWidth, ownerWidth);

    float availableInnerHeight = LayoutHelpers.CalculateAvailableInnerDimension(
        node, direction, Dimension.Height,
        availableHeight - marginAxisColumn, paddingAndBorderAxisColumn,
        ownerHeight, ownerWidth);

    float availableInnerMainDim = isMainAxisRow ? availableInnerWidth : availableInnerHeight;
    float availableInnerCrossDim = isMainAxisRow ? availableInnerHeight : availableInnerWidth;

    // STEP 3: DETERMINE FLEX BASIS FOR EACH ITEM
    float totalMainDim = FlexBasis.ComputeFlexBasisForChildren(
        node, availableInnerWidth, availableInnerHeight,
        widthSizingMode, heightSizingMode,
        direction, mainAxis, performLayout,
        layoutMarkerData, depth, (uint)generationCount);

    if (childCount > 1)
    {
      totalMainDim += node.Style.ComputeGapForAxis(mainAxis, availableInnerMainDim) * (childCount - 1);
    }

    bool mainAxisOverflows = sizingModeMainDim != SizingMode.MaxContent && totalMainDim > availableInnerMainDim;

    if (isNodeFlexWrap && mainAxisOverflows && sizingModeMainDim == SizingMode.FitContent)
    {
      sizingModeMainDim = SizingMode.StretchFit;
    }

    // STEP 4: COLLECT FLEX ITEMS INTO FLEX LINES
    LayoutableChildren<Node>.Enumerator iterator = node.LayoutChildren.GetEnumerator();
    int lineCount = 0;
    float totalLineCrossDim = 0;
    float crossAxisGap = node.Style.ComputeGapForAxis(crossAxis, availableInnerCrossDim);
    float maxLineMainDim = 0;
    Node? pendingChild = null;

    // Process all flex lines
    bool hasMoreChildren = true;
    while (hasMoreChildren)
    {
      FlexLine flexLine = FlexLine.CalculateFlexLine(
          node, ownerDirection, ownerWidth, mainAxisOwnerSize,
          availableInnerWidth, availableInnerMainDim,
          ref iterator, lineCount, pendingChild);

      pendingChild = flexLine.PendingChild;

      // If line has no items and no pending child, we're done
      if (flexLine.ItemsInFlow.Count == 0 && pendingChild is null)
      {
        hasMoreChildren = false;
        continue;
      }

      bool canSkipFlex = !performLayout && sizingModeCrossDim == SizingMode.StretchFit;

      // STEP 5: RESOLVING FLEXIBLE LENGTHS ON MAIN AXIS
      bool sizeBasedOnContent = false;

      if (sizingModeMainDim != SizingMode.StretchFit)
      {
        Style style = node.Style;
        float minInnerWidth = style.ResolvedMinDimension(direction, Dimension.Width, ownerWidth, ownerWidth).Unwrap() - paddingAndBorderAxisRow;
        float maxInnerWidth = style.ResolvedMaxDimension(direction, Dimension.Width, ownerWidth, ownerWidth).Unwrap() - paddingAndBorderAxisRow;
        float minInnerHeight = style.ResolvedMinDimension(direction, Dimension.Height, ownerHeight, ownerWidth).Unwrap() - paddingAndBorderAxisColumn;
        float maxInnerHeight = style.ResolvedMaxDimension(direction, Dimension.Height, ownerHeight, ownerWidth).Unwrap() - paddingAndBorderAxisColumn;

        float minInnerMainDim = isMainAxisRow ? minInnerWidth : minInnerHeight;
        float maxInnerMainDim = isMainAxisRow ? maxInnerWidth : maxInnerHeight;

        if (!float.IsNaN(minInnerMainDim) && flexLine.SizeConsumed < minInnerMainDim)
        {
          availableInnerMainDim = minInnerMainDim;
        }
        else if (!float.IsNaN(maxInnerMainDim) && flexLine.SizeConsumed > maxInnerMainDim)
        {
          availableInnerMainDim = maxInnerMainDim;
        }
        else
        {
          bool useLegacyStretchBehaviour = node.HasErrata(Errata.StretchFlexBasis);

          if (!useLegacyStretchBehaviour &&
              ((!float.IsNaN(flexLine.Layout.TotalFlexGrowFactors) && flexLine.Layout.TotalFlexGrowFactors == 0) ||
               (!float.IsNaN(node.ResolveFlexGrow()) && node.ResolveFlexGrow() == 0)))
          {
            availableInnerMainDim = flexLine.SizeConsumed;
          }

          sizeBasedOnContent = !useLegacyStretchBehaviour;
        }
      }

      if (!sizeBasedOnContent && !float.IsNaN(availableInnerMainDim))
      {
        FlexLineRunningLayout layout = flexLine.Layout;
        layout.RemainingFreeSpace = availableInnerMainDim - flexLine.SizeConsumed;
        flexLine.Layout = layout;
      }
      else if (flexLine.SizeConsumed < 0)
      {
        FlexLineRunningLayout layout = flexLine.Layout;
        layout.RemainingFreeSpace = -flexLine.SizeConsumed;
        flexLine.Layout = layout;
      }

      if (!canSkipFlex)
      {
        FlexDistribution.ResolveFlexibleLength(
            node, flexLine, mainAxis, crossAxis, direction,
            ownerWidth, mainAxisOwnerSize, availableInnerMainDim, availableInnerCrossDim,
            availableInnerWidth, availableInnerHeight, mainAxisOverflows, sizingModeCrossDim,
            performLayout, layoutMarkerData, depth, (uint)generationCount);
      }

      node.Layout.SetHadOverflow(node.Layout.HadOverflow || flexLine.Layout.RemainingFreeSpace < 0);

      // STEP 6: MAIN-AXIS JUSTIFICATION & CROSS-AXIS SIZE DETERMINATION
      JustifyContent.JustifyMainAxis(
          node, flexLine, mainAxis, crossAxis, direction,
          sizingModeMainDim, sizingModeCrossDim, mainAxisOwnerSize, ownerWidth,
          availableInnerMainDim, availableInnerCrossDim, availableInnerWidth, performLayout);

      float containerCrossAxis = availableInnerCrossDim;
      if (sizingModeCrossDim is SizingMode.MaxContent or SizingMode.FitContent)
      {
        containerCrossAxis = BoundAxis.BoundAxisValue(node, crossAxis, direction,
            flexLine.Layout.CrossDim + paddingAndBorderAxisCross, crossAxisOwnerSize, ownerWidth) - paddingAndBorderAxisCross;
      }

      if (!isNodeFlexWrap && sizingModeCrossDim == SizingMode.StretchFit)
      {
        FlexLineRunningLayout layout = flexLine.Layout;
        layout.CrossDim = availableInnerCrossDim;
        flexLine.Layout = layout;
      }

      if (!isNodeFlexWrap)
      {
        FlexLineRunningLayout layout = flexLine.Layout;
        layout.CrossDim = BoundAxis.BoundAxisValue(node, crossAxis, direction,
            flexLine.Layout.CrossDim + paddingAndBorderAxisCross, crossAxisOwnerSize, ownerWidth) - paddingAndBorderAxisCross;
        flexLine.Layout = layout;
      }

      // STEP 7: CROSS-AXIS ALIGNMENT
      if (performLayout)
      {
        PerformCrossAxisAlignment(
            node, flexLine, mainAxis, crossAxis, direction, isMainAxisRow, isNodeFlexWrap,
            availableInnerMainDim, availableInnerCrossDim, availableInnerWidth, availableInnerHeight,
            containerCrossAxis, leadingPaddingAndBorderCross, totalLineCrossDim,
            layoutMarkerData, depth, generationCount);
      }

      float appliedCrossGap = lineCount != 0 ? crossAxisGap : 0.0f;
      totalLineCrossDim += flexLine.Layout.CrossDim + appliedCrossGap;
      maxLineMainDim = Comparison.MaxOrDefined(maxLineMainDim, flexLine.Layout.MainDim);

      lineCount++;
      hasMoreChildren = pendingChild is not null;
    }

    // STEP 8: MULTI-LINE CONTENT ALIGNMENT
    if (performLayout && (isNodeFlexWrap || Baseline.IsBaselineLayout(node)))
    {
      PerformMultiLineContentAlignment(
          node, direction, mainAxis, crossAxis, isMainAxisRow,
          sizingModeCrossDim, availableInnerWidth, availableInnerHeight, availableInnerCrossDim,
          paddingAndBorderAxisCross, crossAxisOwnerSize, ownerWidth,
          leadingPaddingAndBorderCross, crossAxisGap, totalLineCrossDim, lineCount,
          layoutMarkerData, depth, generationCount);
    }

    // STEP 9: COMPUTING FINAL DIMENSIONS
    node.Layout.SetMeasuredDimension(Dimension.Width,
        BoundAxis.BoundAxisValue(node, FlexDirection.Row, direction,
            availableWidth - marginAxisRow, ownerWidth, ownerWidth));

    node.Layout.SetMeasuredDimension(Dimension.Height,
        BoundAxis.BoundAxisValue(node, FlexDirection.Column, direction,
            availableHeight - marginAxisColumn, ownerHeight, ownerWidth));

    if (sizingModeMainDim == SizingMode.MaxContent ||
        (node.Style.Overflow != Overflow.Scroll && sizingModeMainDim == SizingMode.FitContent))
    {
      node.Layout.SetMeasuredDimension(mainAxis.GetDimension(),
          BoundAxis.BoundAxisValue(node, mainAxis, direction, maxLineMainDim, mainAxisOwnerSize, ownerWidth));
    }
    else if (sizingModeMainDim == SizingMode.FitContent && node.Style.Overflow == Overflow.Scroll)
    {
      node.Layout.SetMeasuredDimension(mainAxis.GetDimension(),
          Comparison.MaxOrDefined(
              Comparison.MinOrDefined(
                  availableInnerMainDim + paddingAndBorderAxisMain,
                  BoundAxis.BoundAxisWithinMinAndMax(node, direction, mainAxis,
                      new FloatOptional(maxLineMainDim), mainAxisOwnerSize, ownerWidth).Unwrap()),
              paddingAndBorderAxisMain));
    }

    if (sizingModeCrossDim == SizingMode.MaxContent ||
        (node.Style.Overflow != Overflow.Scroll && sizingModeCrossDim == SizingMode.FitContent))
    {
      node.Layout.SetMeasuredDimension(crossAxis.GetDimension(),
          BoundAxis.BoundAxisValue(node, crossAxis, direction,
              totalLineCrossDim + paddingAndBorderAxisCross, crossAxisOwnerSize, ownerWidth));
    }
    else if (sizingModeCrossDim == SizingMode.FitContent && node.Style.Overflow == Overflow.Scroll)
    {
      node.Layout.SetMeasuredDimension(crossAxis.GetDimension(),
          Comparison.MaxOrDefined(
              Comparison.MinOrDefined(
                  availableInnerCrossDim + paddingAndBorderAxisCross,
                  BoundAxis.BoundAxisWithinMinAndMax(node, direction, crossAxis,
                      new FloatOptional(totalLineCrossDim + paddingAndBorderAxisCross),
                      crossAxisOwnerSize, ownerWidth).Unwrap()),
              paddingAndBorderAxisCross));
    }

    // Reverse positions on wrap-reverse
    if (performLayout && node.Style.FlexWrap == Wrap.WrapReverse)
    {
      foreach (Node child in node.LayoutChildren)
      {
        if (child.Style.PositionType != PositionType.Absolute)
        {
          child.Layout.SetPosition(crossAxis.FlexStartEdge(),
              node.Layout.GetMeasuredDimension(crossAxis.GetDimension()) -
              child.Layout.GetPosition(crossAxis.FlexStartEdge()) -
              child.Layout.GetMeasuredDimension(crossAxis.GetDimension()));
        }
      }
    }

    if (performLayout)
    {
      // STEP 10: SETTING TRAILING POSITIONS FOR CHILDREN
      bool needsMainTrailingPos = TrailingPosition.NeedsTrailingPosition(mainAxis);
      bool needsCrossTrailingPos = TrailingPosition.NeedsTrailingPosition(crossAxis);

      if (needsMainTrailingPos || needsCrossTrailingPos)
      {
        foreach (Node child in node.LayoutChildren)
        {
          if (child.Style.Display == Display.None || child.Style.PositionType == PositionType.Absolute)
          {
            continue;
          }

          if (needsMainTrailingPos)
          {
            TrailingPosition.SetChildTrailingPosition(node, child, mainAxis);
          }

          if (needsCrossTrailingPos)
          {
            TrailingPosition.SetChildTrailingPosition(node, child, crossAxis);
          }
        }
      }

      // STEP 11: SIZING AND POSITIONING ABSOLUTE CHILDREN
      if (node.Style.PositionType != PositionType.Static || node.AlwaysFormsContainingBlock || depth == 1)
      {
        AbsoluteLayout.LayoutAbsoluteDescendants(
            node, node,
            isMainAxisRow ? sizingModeMainDim : sizingModeCrossDim,
            direction, layoutMarkerData, depth, generationCount,
            0.0f, 0.0f, availableInnerWidth, availableInnerHeight);
      }
    }
  }

  private static void PerformCrossAxisAlignment(
      Node node, FlexLine flexLine,
      FlexDirection mainAxis, FlexDirection crossAxis, Direction direction,
      bool isMainAxisRow, bool isNodeFlexWrap,
      float availableInnerMainDim, float availableInnerCrossDim,
      float availableInnerWidth, float availableInnerHeight,
      float containerCrossAxis, float leadingPaddingAndBorderCross, float totalLineCrossDim,
      LayoutData layoutMarkerData, int depth, int generationCount)
  {
    for (int i = 0; i < flexLine.ItemsInFlow.Count; i++)
    {
      Node child = flexLine.ItemsInFlow[i];
      float leadingCrossDim = leadingPaddingAndBorderCross;
      Align alignItem = AlignUtils.ResolveChildAlignment(node, child);

      if (alignItem == Align.Stretch &&
          !child.Style.FlexStartMarginIsAuto(crossAxis, direction) &&
          !child.Style.FlexEndMarginIsAuto(crossAxis, direction))
      {
        if (!child.HasDefiniteLength(crossAxis.GetDimension(), availableInnerCrossDim))
        {
          float childMainSize = child.Layout.GetMeasuredDimension(mainAxis.GetDimension());
          Style childStyle = child.Style;
          float childCrossSize = childStyle.AspectRatio.IsDefined
              ? child.Style.ComputeMarginForAxis(crossAxis, availableInnerWidth) +
                (isMainAxisRow
                    ? childMainSize / childStyle.AspectRatio.Unwrap()
                    : childMainSize * childStyle.AspectRatio.Unwrap())
              : flexLine.Layout.CrossDim;

          childMainSize += child.Style.ComputeMarginForAxis(mainAxis, availableInnerWidth);

          SizingMode childMainSizingMode = SizingMode.StretchFit;
          SizingMode childCrossSizingMode = SizingMode.StretchFit;

          LayoutHelpers.ConstrainMaxSizeForMode(child, direction, mainAxis,
              availableInnerMainDim, availableInnerWidth, ref childMainSizingMode, ref childMainSize);
          LayoutHelpers.ConstrainMaxSizeForMode(child, direction, crossAxis,
              availableInnerCrossDim, availableInnerWidth, ref childCrossSizingMode, ref childCrossSize);

          float childWidth = isMainAxisRow ? childMainSize : childCrossSize;
          float childHeight = !isMainAxisRow ? childMainSize : childCrossSize;

          Align alignContent = node.Style.AlignContent;
          bool crossAxisDoesNotGrow = alignContent != Align.Stretch && isNodeFlexWrap;

          SizingMode childWidthSizingMode = float.IsNaN(childWidth) || (!isMainAxisRow && crossAxisDoesNotGrow)
              ? SizingMode.MaxContent : SizingMode.StretchFit;
          SizingMode childHeightSizingMode = float.IsNaN(childHeight) || (isMainAxisRow && crossAxisDoesNotGrow)
              ? SizingMode.MaxContent : SizingMode.StretchFit;

          CalculateLayoutInternal(child, childWidth, childHeight, direction,
              childWidthSizingMode, childHeightSizingMode,
              availableInnerWidth, availableInnerHeight, true, LayoutPassReason.Stretch,
              layoutMarkerData, depth, generationCount);
        }
      }
      else
      {
        float remainingCrossDim = containerCrossAxis - child.DimensionWithMargin(crossAxis, availableInnerWidth);

        if (child.Style.FlexStartMarginIsAuto(crossAxis, direction) &&
            child.Style.FlexEndMarginIsAuto(crossAxis, direction))
        {
          leadingCrossDim += Comparison.MaxOrDefined(0.0f, remainingCrossDim / 2);
        }
        else if (child.Style.FlexEndMarginIsAuto(crossAxis, direction))
        {
          // No-Op
        }
        else if (child.Style.FlexStartMarginIsAuto(crossAxis, direction))
        {
          leadingCrossDim += Comparison.MaxOrDefined(0.0f, remainingCrossDim);
        }
        else if (alignItem == Align.FlexStart)
        {
          // No-Op
        }
        else if (alignItem == Align.Center)
        {
          leadingCrossDim += remainingCrossDim / 2;
        }
        else
        {
          leadingCrossDim += remainingCrossDim;
        }
      }

      child.Layout.SetPosition(crossAxis.FlexStartEdge(),
          child.Layout.GetPosition(crossAxis.FlexStartEdge()) + totalLineCrossDim + leadingCrossDim);
    }
  }

  private static void PerformMultiLineContentAlignment(
      Node node, Direction direction,
      FlexDirection mainAxis, FlexDirection crossAxis, bool isMainAxisRow,
      SizingMode sizingModeCrossDim,
      float availableInnerWidth, float availableInnerHeight, float availableInnerCrossDim,
      float paddingAndBorderAxisCross, float crossAxisOwnerSize, float ownerWidth,
      float leadingPaddingAndBorderCross, float crossAxisGap, float totalLineCrossDim, int lineCount,
      LayoutData layoutMarkerData, int depth, int generationCount)
  {
    float leadPerLine = 0;
    float currentLead = leadingPaddingAndBorderCross;
    float extraSpacePerLine = 0;

    float unclampedCrossDim = sizingModeCrossDim == SizingMode.StretchFit
        ? availableInnerCrossDim + paddingAndBorderAxisCross
        : node.HasDefiniteLength(crossAxis.GetDimension(), crossAxisOwnerSize)
            ? node.GetResolvedDimension(direction, crossAxis.GetDimension(), crossAxisOwnerSize, ownerWidth).Unwrap()
            : totalLineCrossDim + paddingAndBorderAxisCross;

    float innerCrossDim = BoundAxis.BoundAxisValue(node, crossAxis, direction,
        unclampedCrossDim, crossAxisOwnerSize, ownerWidth) - paddingAndBorderAxisCross;

    float remainingAlignContentDim = innerCrossDim - totalLineCrossDim;

    Align alignContent = remainingAlignContentDim >= 0
        ? node.Style.AlignContent
        : AlignUtils.FallbackAlignment(node.Style.AlignContent);

    switch (alignContent)
    {
      case Align.FlexEnd:
        currentLead += remainingAlignContentDim;
        break;
      case Align.Center:
        currentLead += remainingAlignContentDim / 2;
        break;
      case Align.Stretch:
        extraSpacePerLine = remainingAlignContentDim / lineCount;
        break;
      case Align.SpaceAround:
        currentLead += remainingAlignContentDim / (2 * lineCount);
        leadPerLine = remainingAlignContentDim / lineCount;
        break;
      case Align.SpaceEvenly:
        currentLead += remainingAlignContentDim / (lineCount + 1);
        leadPerLine = remainingAlignContentDim / (lineCount + 1);
        break;
      case Align.SpaceBetween:
        if (lineCount > 1)
        {
          leadPerLine = remainingAlignContentDim / (lineCount - 1);
        }

        break;
      case Align.Auto:
      case Align.FlexStart:
      case Align.Baseline:
      default:
        break;
    }

    List<Node> children = [.. node.LayoutChildren];
    int endIndex = 0;

    for (int i = 0; i < lineCount; i++)
    {
      int startIndex = endIndex;
      int childIndex = startIndex;

      // compute the line's height and find the endIndex
      float lineHeight = 0;
      float maxAscentForCurrentLine = 0;
      float maxDescentForCurrentLine = 0;

      for (; childIndex < children.Count; childIndex++)
      {
        Node child = children[childIndex];
        if (child.Style.Display == Display.None)
        {
          continue;
        }

        if (child.Style.PositionType != PositionType.Absolute)
        {
          if (child.LineIndex != i)
          {
            break;
          }

          if (child.IsLayoutDimensionDefined(crossAxis))
          {
            lineHeight = Comparison.MaxOrDefined(lineHeight,
                child.Layout.GetMeasuredDimension(crossAxis.GetDimension()) +
                child.Style.ComputeMarginForAxis(crossAxis, availableInnerWidth));
          }

          if (AlignUtils.ResolveChildAlignment(node, child) == Align.Baseline)
          {
            float ascent = Baseline.CalculateBaseline(child) +
                child.Style.ComputeFlexStartMargin(FlexDirection.Column, direction, availableInnerWidth);
            float descent = child.Layout.GetMeasuredDimension(Dimension.Height) +
                child.Style.ComputeMarginForAxis(FlexDirection.Column, availableInnerWidth) - ascent;
            maxAscentForCurrentLine = Comparison.MaxOrDefined(maxAscentForCurrentLine, ascent);
            maxDescentForCurrentLine = Comparison.MaxOrDefined(maxDescentForCurrentLine, descent);
            lineHeight = Comparison.MaxOrDefined(lineHeight,
                maxAscentForCurrentLine + maxDescentForCurrentLine);
          }
        }
      }

      endIndex = childIndex;
      currentLead += i != 0 ? crossAxisGap : 0;
      lineHeight += extraSpacePerLine;

      for (childIndex = startIndex; childIndex < endIndex; childIndex++)
      {
        Node child = children[childIndex];
        if (child.Style.Display == Display.None)
        {
          continue;
        }

        if (child.Style.PositionType != PositionType.Absolute)
        {
          switch (AlignUtils.ResolveChildAlignment(node, child))
          {
            case Align.FlexStart:
              child.Layout.SetPosition(crossAxis.FlexStartEdge(),
                  currentLead + child.Style.ComputeFlexStartPosition(crossAxis, direction, availableInnerWidth));
              break;

            case Align.FlexEnd:
              child.Layout.SetPosition(crossAxis.FlexStartEdge(),
                  currentLead + lineHeight -
                  child.Style.ComputeFlexEndMargin(crossAxis, direction, availableInnerWidth) -
                  child.Layout.GetMeasuredDimension(crossAxis.GetDimension()));
              break;

            case Align.Center:
              float childHeight = child.Layout.GetMeasuredDimension(crossAxis.GetDimension());
              child.Layout.SetPosition(crossAxis.FlexStartEdge(),
                  currentLead + (lineHeight - childHeight) / 2);
              break;

            case Align.Stretch:
              child.Layout.SetPosition(crossAxis.FlexStartEdge(),
                  currentLead + child.Style.ComputeFlexStartMargin(crossAxis, direction, availableInnerWidth));

              // Remeasure child with the line height as it as been only
              // measured with the owners height yet.
              if (!child.HasDefiniteLength(crossAxis.GetDimension(), availableInnerCrossDim))
              {
                float stretchChildWidth = isMainAxisRow
                    ? child.Layout.GetMeasuredDimension(Dimension.Width) +
                      child.Style.ComputeMarginForAxis(mainAxis, availableInnerWidth)
                    : leadPerLine + lineHeight;

                float stretchChildHeight = !isMainAxisRow
                    ? child.Layout.GetMeasuredDimension(Dimension.Height) +
                      child.Style.ComputeMarginForAxis(crossAxis, availableInnerWidth)
                    : leadPerLine + lineHeight;

                if (!(Comparison.InexactEquals(stretchChildWidth, child.Layout.GetMeasuredDimension(Dimension.Width)) &&
                      Comparison.InexactEquals(stretchChildHeight, child.Layout.GetMeasuredDimension(Dimension.Height))))
                {
                  CalculateLayoutInternal(child, stretchChildWidth, stretchChildHeight, direction,
                      SizingMode.StretchFit, SizingMode.StretchFit,
                      availableInnerWidth, availableInnerHeight, true, LayoutPassReason.MultilineStretch,
                      layoutMarkerData, depth, generationCount);
                }
              }

              break;

            case Align.Baseline:
              child.Layout.SetPosition(PhysicalEdge.Top,
                  currentLead + maxAscentForCurrentLine - Baseline.CalculateBaseline(child) +
                  child.Style.ComputeFlexStartPosition(FlexDirection.Column, direction, availableInnerCrossDim));
              break;

            case Align.Auto:
            case Align.SpaceBetween:
            case Align.SpaceAround:
            case Align.SpaceEvenly:
            default:
              break;
          }
        }
      }

      currentLead += leadPerLine + lineHeight;
    }
  }

  /// <summary>
  /// Wrapper that determines whether the layout request is redundant and can be skipped.
  /// This delegates to CalculateLayout.CalculateLayoutInternal for proper caching.
  /// </summary>
  public static bool CalculateLayoutInternal(
      Node node,
      float availableWidth,
      float availableHeight,
      Direction ownerDirection,
      SizingMode widthSizingMode,
      SizingMode heightSizingMode,
      float ownerWidth,
      float ownerHeight,
      bool performLayout,
      LayoutPassReason reason,
      LayoutData layoutMarkerData,
      int depth,
      int generationCount)
  {
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(layoutMarkerData);

    // Delegate to the main CalculateLayout module for proper caching
    return CalculateLayout.CalculateLayoutInternal(
        node, availableWidth, availableHeight, ownerDirection,
        widthSizingMode, heightSizingMode, ownerWidth, ownerHeight,
        performLayout, reason, layoutMarkerData, depth, generationCount);
  }
}
