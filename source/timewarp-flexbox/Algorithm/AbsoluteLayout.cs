/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/AbsoluteLayout.h, yoga/algorithm/AbsoluteLayout.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Handles layout of absolutely positioned children.
/// </summary>
/// <remarks>
/// Absolutely positioned nodes do not participate in flex layout and thus their
/// positions can be determined independently from the rest of their siblings.
/// For each axis there are essentially two cases:
/// <list type="number">
/// <item>The node has insets defined. In this case we can just use these to
/// determine the position of the node.</item>
/// <item>The node does not have insets defined. In this case we look at the style
/// of the parent to position the node. Things like justify content and
/// align content will move absolute children around. If none of these
/// special properties are defined, the child is positioned at the start
/// (defined by flex direction) of the leading flex line.</item>
/// </list>
/// See: https://www.w3.org/TR/css-flexbox-1/#abspos-items
/// </remarks>
public static class AbsoluteLayout
{
  /// <summary>
  /// Delegate for the internal layout calculation function.
  /// This is set by CalculateLayout during initialization.
  /// </summary>
  /// <remarks>
  /// This delegate pattern allows AbsoluteLayout to call back into CalculateLayout
  /// without creating a circular compile-time dependency. The CalculateLayout module
  /// sets this delegate when it initializes.
  /// </remarks>
  public static Func<Node, float, float, Direction, SizingMode, SizingMode, float, float, bool, LayoutPassReason, LayoutData, int, int, bool>?
      CalculateLayoutInternal
  { get; set; }

  /// <summary>
  /// Lays out an absolutely positioned child.
  /// </summary>
  /// <param name="containingNode">The containing block node.</param>
  /// <param name="node">The parent node (may differ from containing block for static positioning).</param>
  /// <param name="child">The child to layout.</param>
  /// <param name="containingBlockWidth">Width of the containing block.</param>
  /// <param name="containingBlockHeight">Height of the containing block.</param>
  /// <param name="widthMode">The width sizing mode.</param>
  /// <param name="direction">The resolved direction.</param>
  /// <param name="layoutMarkerData">Layout tracking data.</param>
  /// <param name="depth">Current recursion depth.</param>
  /// <param name="generationCount">Current generation count for cache invalidation.</param>
  public static void LayoutAbsoluteChild(
      Node containingNode,
      Node node,
      Node child,
      float containingBlockWidth,
      float containingBlockHeight,
      SizingMode widthMode,
      Direction direction,
      LayoutData layoutMarkerData,
      int depth,
      int generationCount)
  {
    ArgumentNullException.ThrowIfNull(containingNode);
    ArgumentNullException.ThrowIfNull(node);
    ArgumentNullException.ThrowIfNull(child);

    if (CalculateLayoutInternal is null)
    {
      throw new InvalidOperationException(
          "AbsoluteLayout.CalculateLayoutInternal must be set before calling LayoutAbsoluteChild. " +
          "This is typically done by the CalculateLayout module during initialization.");
    }

    FlexDirection mainAxis = node.Style.FlexDirection.ResolveDirection(direction);
    FlexDirection crossAxis = mainAxis.ResolveCrossDirection(direction);
    bool isMainAxisRow = mainAxis.IsRow();

    float childWidth = float.NaN;
    float childHeight = float.NaN;
    SizingMode childWidthSizingMode = SizingMode.MaxContent;
    SizingMode childHeightSizingMode = SizingMode.MaxContent;

    float marginRow = child.Style.ComputeMarginForAxis(FlexDirection.Row, containingBlockWidth);
    float marginColumn = child.Style.ComputeMarginForAxis(FlexDirection.Column, containingBlockWidth);

    if (child.HasDefiniteLength(Dimension.Width, containingBlockWidth))
    {
      childWidth = child.GetResolvedDimension(
          direction,
          Dimension.Width,
          containingBlockWidth,
          containingBlockWidth).Unwrap() + marginRow;
    }
    else
    {
      // If the child doesn't have a specified width, compute the width based on
      // the left/right offsets if they're defined.
      if (child.Style.IsFlexStartPositionDefined(FlexDirection.Row, direction) &&
          child.Style.IsFlexEndPositionDefined(FlexDirection.Row, direction) &&
          !child.Style.IsFlexStartPositionAuto(FlexDirection.Row, direction) &&
          !child.Style.IsFlexEndPositionAuto(FlexDirection.Row, direction))
      {
        childWidth =
            containingNode.Layout.GetMeasuredDimension(Dimension.Width) -
            (containingNode.Style.ComputeFlexStartBorder(FlexDirection.Row, direction) +
             containingNode.Style.ComputeFlexEndBorder(FlexDirection.Row, direction)) -
            (child.Style.ComputeFlexStartPosition(FlexDirection.Row, direction, containingBlockWidth) +
             child.Style.ComputeFlexEndPosition(FlexDirection.Row, direction, containingBlockWidth));
        childWidth = BoundAxis.BoundAxisValue(
            child,
            FlexDirection.Row,
            direction,
            childWidth,
            containingBlockWidth,
            containingBlockWidth);
      }
    }

    if (child.HasDefiniteLength(Dimension.Height, containingBlockHeight))
    {
      childHeight = child.GetResolvedDimension(
          direction,
          Dimension.Height,
          containingBlockHeight,
          containingBlockWidth).Unwrap() + marginColumn;
    }
    else
    {
      // If the child doesn't have a specified height, compute the height based
      // on the top/bottom offsets if they're defined.
      if (child.Style.IsFlexStartPositionDefined(FlexDirection.Column, direction) &&
          child.Style.IsFlexEndPositionDefined(FlexDirection.Column, direction) &&
          !child.Style.IsFlexStartPositionAuto(FlexDirection.Column, direction) &&
          !child.Style.IsFlexEndPositionAuto(FlexDirection.Column, direction))
      {
        childHeight =
            containingNode.Layout.GetMeasuredDimension(Dimension.Height) -
            (containingNode.Style.ComputeFlexStartBorder(FlexDirection.Column, direction) +
             containingNode.Style.ComputeFlexEndBorder(FlexDirection.Column, direction)) -
            (child.Style.ComputeFlexStartPosition(FlexDirection.Column, direction, containingBlockHeight) +
             child.Style.ComputeFlexEndPosition(FlexDirection.Column, direction, containingBlockHeight));
        childHeight = BoundAxis.BoundAxisValue(
            child,
            FlexDirection.Column,
            direction,
            childHeight,
            containingBlockHeight,
            containingBlockWidth);
      }
    }

    // Exactly one dimension needs to be defined for us to be able to do aspect
    // ratio calculation. One dimension being the anchor and the other being flexible.
    Style childStyle = child.Style;
    if (float.IsNaN(childWidth) ^ float.IsNaN(childHeight))
    {
      if (childStyle.AspectRatio.IsDefined)
      {
        if (float.IsNaN(childWidth))
        {
          childWidth = marginRow +
              (childHeight - marginColumn) * childStyle.AspectRatio.Unwrap();
        }
        else if (float.IsNaN(childHeight))
        {
          childHeight = marginColumn +
              (childWidth - marginRow) / childStyle.AspectRatio.Unwrap();
        }
      }
    }

    // If we're still missing one or the other dimension, measure the content.
    if (float.IsNaN(childWidth) || float.IsNaN(childHeight))
    {
      childWidthSizingMode = float.IsNaN(childWidth)
          ? SizingMode.MaxContent
          : SizingMode.StretchFit;
      childHeightSizingMode = float.IsNaN(childHeight)
          ? SizingMode.MaxContent
          : SizingMode.StretchFit;

      // If the size of the owner is defined then try to constrain the absolute
      // child to that size as well. This allows text within the absolute child
      // to wrap to the size of its owner. This is the same behavior as many
      // browsers implement.
      if (!isMainAxisRow && float.IsNaN(childWidth) &&
          widthMode != SizingMode.MaxContent &&
          !float.IsNaN(containingBlockWidth) && containingBlockWidth > 0)
      {
        childWidth = containingBlockWidth;
        childWidthSizingMode = SizingMode.FitContent;
      }

      CalculateLayoutInternal(
          child,
          childWidth,
          childHeight,
          direction,
          childWidthSizingMode,
          childHeightSizingMode,
          containingBlockWidth,
          containingBlockHeight,
          false,
          LayoutPassReason.AbsMeasureChild,
          layoutMarkerData,
          depth,
          generationCount);

      childWidth = child.Layout.GetMeasuredDimension(Dimension.Width) +
          child.Style.ComputeMarginForAxis(FlexDirection.Row, containingBlockWidth);
      childHeight = child.Layout.GetMeasuredDimension(Dimension.Height) +
          child.Style.ComputeMarginForAxis(FlexDirection.Column, containingBlockWidth);
    }

    CalculateLayoutInternal(
        child,
        childWidth,
        childHeight,
        direction,
        SizingMode.StretchFit,
        SizingMode.StretchFit,
        containingBlockWidth,
        containingBlockHeight,
        true,
        LayoutPassReason.AbsLayout,
        layoutMarkerData,
        depth,
        generationCount);

    PositionAbsoluteChild(
        containingNode,
        node,
        child,
        direction,
        mainAxis,
        isMainAxis: true,
        containingBlockWidth,
        containingBlockHeight);

    PositionAbsoluteChild(
        containingNode,
        node,
        child,
        direction,
        crossAxis,
        isMainAxis: false,
        containingBlockWidth,
        containingBlockHeight);
  }

  /// <summary>
  /// Recursively lays out all absolute descendants.
  /// </summary>
  /// <param name="containingNode">The containing block node.</param>
  /// <param name="currentNode">The current node being processed.</param>
  /// <param name="widthSizingMode">The width sizing mode.</param>
  /// <param name="currentNodeDirection">Direction of the current node.</param>
  /// <param name="layoutMarkerData">Layout tracking data.</param>
  /// <param name="currentDepth">Current recursion depth.</param>
  /// <param name="generationCount">Current generation count.</param>
  /// <param name="currentNodeLeftOffsetFromContainingBlock">Left offset from containing block.</param>
  /// <param name="currentNodeTopOffsetFromContainingBlock">Top offset from containing block.</param>
  /// <param name="containingNodeAvailableInnerWidth">Available inner width of containing block.</param>
  /// <param name="containingNodeAvailableInnerHeight">Available inner height of containing block.</param>
  /// <returns>True if any absolute descendant has new layout.</returns>
  public static bool LayoutAbsoluteDescendants(
      Node containingNode,
      Node currentNode,
      SizingMode widthSizingMode,
      Direction currentNodeDirection,
      LayoutData layoutMarkerData,
      int currentDepth,
      int generationCount,
      float currentNodeLeftOffsetFromContainingBlock,
      float currentNodeTopOffsetFromContainingBlock,
      float containingNodeAvailableInnerWidth,
      float containingNodeAvailableInnerHeight)
  {
    ArgumentNullException.ThrowIfNull(containingNode);
    ArgumentNullException.ThrowIfNull(currentNode);

    bool hasNewLayout = false;

    foreach (Node child in currentNode.LayoutChildren)
    {
      if (child.Style.Display == Display.None)
      {
        // Skip hidden nodes
      }
      else if (child.Style.PositionType == PositionType.Absolute)
      {
        bool absoluteErrata = currentNode.HasErrata(Errata.AbsolutePercentAgainstInnerSize);
        float containingBlockWidth = absoluteErrata
            ? containingNodeAvailableInnerWidth
            : containingNode.Layout.GetMeasuredDimension(Dimension.Width) -
              containingNode.Style.ComputeBorderForAxis(FlexDirection.Row);
        float containingBlockHeight = absoluteErrata
            ? containingNodeAvailableInnerHeight
            : containingNode.Layout.GetMeasuredDimension(Dimension.Height) -
              containingNode.Style.ComputeBorderForAxis(FlexDirection.Column);

        LayoutAbsoluteChild(
            containingNode,
            currentNode,
            child,
            containingBlockWidth,
            containingBlockHeight,
            widthSizingMode,
            currentNodeDirection,
            layoutMarkerData,
            currentDepth,
            generationCount);

        hasNewLayout = hasNewLayout || child.HasNewLayout;

        // At this point the child has its position set but only on its the
        // parent's flexStart edge. Additionally, this position should be
        // interpreted relative to the containing block of the child if it had
        // insets defined. So we need to adjust the position by subtracting the
        // the parents offset from the containing block. However, getting that
        // offset is complicated since the two nodes can have different main/cross
        // axes.
        FlexDirection parentMainAxis = currentNode.Style.FlexDirection.ResolveDirection(currentNodeDirection);
        FlexDirection parentCrossAxis = parentMainAxis.ResolveCrossDirection(currentNodeDirection);

        if (TrailingPosition.NeedsTrailingPosition(parentMainAxis))
        {
          bool mainInsetsDefined = parentMainAxis.IsRow()
              ? child.Style.HorizontalInsetsDefined()
              : child.Style.VerticalInsetsDefined();
          TrailingPosition.SetChildTrailingPosition(
              mainInsetsDefined ? containingNode : currentNode,
              child,
              parentMainAxis);
        }

        if (TrailingPosition.NeedsTrailingPosition(parentCrossAxis))
        {
          bool crossInsetsDefined = parentCrossAxis.IsRow()
              ? child.Style.HorizontalInsetsDefined()
              : child.Style.VerticalInsetsDefined();
          TrailingPosition.SetChildTrailingPosition(
              crossInsetsDefined ? containingNode : currentNode,
              child,
              parentCrossAxis);
        }

        // At this point we know the left and top physical edges of the child are
        // set with positions that are relative to the containing block if insets
        // are defined
        float childLeftPosition = child.Layout.GetPosition(PhysicalEdge.Left);
        float childTopPosition = child.Layout.GetPosition(PhysicalEdge.Top);

        float childLeftOffsetFromParent = child.Style.HorizontalInsetsDefined()
            ? (childLeftPosition - currentNodeLeftOffsetFromContainingBlock)
            : childLeftPosition;
        float childTopOffsetFromParent = child.Style.VerticalInsetsDefined()
            ? (childTopPosition - currentNodeTopOffsetFromContainingBlock)
            : childTopPosition;

        child.SetLayoutPosition(childLeftOffsetFromParent, PhysicalEdge.Left);
        child.SetLayoutPosition(childTopOffsetFromParent, PhysicalEdge.Top);
      }
      else if (child.Style.PositionType == PositionType.Static &&
               !child.AlwaysFormsContainingBlock)
      {
        // We may write new layout results for absolute descendants of "child"
        // which are positioned relative to the current containing block instead
        // of their parent. "child" may not be dirty, or have new constraints, so
        // absolute positioning may be the first time during this layout pass that
        // we need to mutate these descendents. Make sure the path of
        // nodes to them is mutable before positioning.
        child.CloneChildrenIfNeeded();
        Direction childDirection = child.ResolveDirection(currentNodeDirection);

        // By now all descendants of the containing block that are not absolute
        // will have their positions set for left and top.
        float childLeftOffsetFromContainingBlock =
            currentNodeLeftOffsetFromContainingBlock +
            child.Layout.GetPosition(PhysicalEdge.Left);
        float childTopOffsetFromContainingBlock =
            currentNodeTopOffsetFromContainingBlock +
            child.Layout.GetPosition(PhysicalEdge.Top);

        hasNewLayout = LayoutAbsoluteDescendants(
            containingNode,
            child,
            widthSizingMode,
            childDirection,
            layoutMarkerData,
            currentDepth + 1,
            generationCount,
            childLeftOffsetFromContainingBlock,
            childTopOffsetFromContainingBlock,
            containingNodeAvailableInnerWidth,
            containingNodeAvailableInnerHeight) || hasNewLayout;

        if (hasNewLayout)
        {
          child.HasNewLayout = hasNewLayout;
        }
      }
    }

    return hasNewLayout;
  }

  #region Private Helper Methods

  private static void SetFlexStartLayoutPosition(
      Node parent,
      Node child,
      Direction direction,
      FlexDirection axis,
      float containingBlockWidth)
  {
    float position = child.Style.ComputeFlexStartMargin(axis, direction, containingBlockWidth) +
        parent.Layout.GetBorder(axis.FlexStartEdge());

    if (!child.HasErrata(Errata.AbsolutePositionWithoutInsetsExcludesPadding))
    {
      position += parent.Layout.GetPadding(axis.FlexStartEdge());
    }

    child.SetLayoutPosition(position, axis.FlexStartEdge());
  }

  private static void SetFlexEndLayoutPosition(
      Node parent,
      Node child,
      Direction direction,
      FlexDirection axis,
      float containingBlockWidth)
  {
    float flexEndPosition = parent.Layout.GetBorder(axis.FlexEndEdge()) +
        child.Style.ComputeFlexEndMargin(axis, direction, containingBlockWidth);

    if (!child.HasErrata(Errata.AbsolutePositionWithoutInsetsExcludesPadding))
    {
      flexEndPosition += parent.Layout.GetPadding(axis.FlexEndEdge());
    }

    child.SetLayoutPosition(
        TrailingPosition.GetPositionOfOppositeEdge(flexEndPosition, axis, parent, child),
        axis.FlexStartEdge());
  }

  private static void SetCenterLayoutPosition(
      Node parent,
      Node child,
      Direction direction,
      FlexDirection axis,
      float containingBlockWidth)
  {
    float parentContentBoxSize =
        parent.Layout.GetMeasuredDimension(axis.GetDimension()) -
        parent.Layout.GetBorder(axis.FlexStartEdge()) -
        parent.Layout.GetBorder(axis.FlexEndEdge());

    if (!child.HasErrata(Errata.AbsolutePositionWithoutInsetsExcludesPadding))
    {
      parentContentBoxSize -= parent.Layout.GetPadding(axis.FlexStartEdge());
      parentContentBoxSize -= parent.Layout.GetPadding(axis.FlexEndEdge());
    }

    float childOuterSize =
        child.Layout.GetMeasuredDimension(axis.GetDimension()) +
        child.Style.ComputeMarginForAxis(axis, containingBlockWidth);

    float position = (parentContentBoxSize - childOuterSize) / 2.0f +
        parent.Layout.GetBorder(axis.FlexStartEdge()) +
        child.Style.ComputeFlexStartMargin(axis, direction, containingBlockWidth);

    if (!child.HasErrata(Errata.AbsolutePositionWithoutInsetsExcludesPadding))
    {
      position += parent.Layout.GetPadding(axis.FlexStartEdge());
    }

    child.SetLayoutPosition(position, axis.FlexStartEdge());
  }

  private static void JustifyAbsoluteChild(
      Node parent,
      Node child,
      Direction direction,
      FlexDirection mainAxis,
      float containingBlockWidth)
  {
    Justify parentJustifyContent = parent.Style.JustifyContent;
    switch (parentJustifyContent)
    {
      case Justify.FlexStart:
      case Justify.SpaceBetween:
        SetFlexStartLayoutPosition(parent, child, direction, mainAxis, containingBlockWidth);
        break;
      case Justify.FlexEnd:
        SetFlexEndLayoutPosition(parent, child, direction, mainAxis, containingBlockWidth);
        break;
      case Justify.Center:
      case Justify.SpaceAround:
      case Justify.SpaceEvenly:
        SetCenterLayoutPosition(parent, child, direction, mainAxis, containingBlockWidth);
        break;
      default:
        break;
    }
  }

  private static void AlignAbsoluteChild(
      Node parent,
      Node child,
      Direction direction,
      FlexDirection crossAxis,
      float containingBlockWidth)
  {
    Align itemAlign = AlignUtils.ResolveChildAlignment(parent, child);
    Wrap parentWrap = parent.Style.FlexWrap;

    if (parentWrap == Wrap.WrapReverse)
    {
      if (itemAlign == Align.FlexEnd)
      {
        itemAlign = Align.FlexStart;
      }
      else if (itemAlign != Align.Center)
      {
        itemAlign = Align.FlexEnd;
      }
    }

    switch (itemAlign)
    {
      case Align.Auto:
      case Align.FlexStart:
      case Align.Baseline:
      case Align.SpaceAround:
      case Align.SpaceBetween:
      case Align.Stretch:
      case Align.SpaceEvenly:
        SetFlexStartLayoutPosition(parent, child, direction, crossAxis, containingBlockWidth);
        break;
      case Align.FlexEnd:
        SetFlexEndLayoutPosition(parent, child, direction, crossAxis, containingBlockWidth);
        break;
      case Align.Center:
        SetCenterLayoutPosition(parent, child, direction, crossAxis, containingBlockWidth);
        break;
      default:
        break;
    }
  }

  private static void PositionAbsoluteChild(
      Node containingNode,
      Node parent,
      Node child,
      Direction direction,
      FlexDirection axis,
      bool isMainAxis,
      float containingBlockWidth,
      float containingBlockHeight)
  {
    bool isAxisRow = axis.IsRow();
    float containingBlockSize = isAxisRow ? containingBlockWidth : containingBlockHeight;

    // The inline-start position takes priority over the end position in the case
    // that they are both set and the node has a fixed width. Thus we only have 2
    // cases here: if inline-start is defined and if inline-end is defined.
    //
    // Despite checking inline-start to honor prioritization of insets, we write
    // to the flex-start edge because this algorithm works by positioning on the
    // flex-start edge and then filling in the flex-end direction at the end if
    // necessary.
    if (child.Style.IsInlineStartPositionDefined(axis, direction) &&
        !child.Style.IsInlineStartPositionAuto(axis, direction))
    {
      float positionRelativeToInlineStart =
          child.Style.ComputeInlineStartPosition(axis, direction, containingBlockSize) +
          containingNode.Style.ComputeInlineStartBorder(axis, direction) +
          child.Style.ComputeInlineStartMargin(axis, direction, containingBlockSize);

      float positionRelativeToFlexStart =
          axis.InlineStartEdge(direction) != axis.FlexStartEdge()
              ? TrailingPosition.GetPositionOfOppositeEdge(
                  positionRelativeToInlineStart, axis, containingNode, child)
              : positionRelativeToInlineStart;

      child.SetLayoutPosition(positionRelativeToFlexStart, axis.FlexStartEdge());
    }
    else if (child.Style.IsInlineEndPositionDefined(axis, direction) &&
             !child.Style.IsInlineEndPositionAuto(axis, direction))
    {
      float positionRelativeToInlineStart =
          containingNode.Layout.GetMeasuredDimension(axis.GetDimension()) -
          child.Layout.GetMeasuredDimension(axis.GetDimension()) -
          containingNode.Style.ComputeInlineEndBorder(axis, direction) -
          child.Style.ComputeInlineEndMargin(axis, direction, containingBlockSize) -
          child.Style.ComputeInlineEndPosition(axis, direction, containingBlockSize);

      float positionRelativeToFlexStart =
          axis.InlineStartEdge(direction) != axis.FlexStartEdge()
              ? TrailingPosition.GetPositionOfOppositeEdge(
                  positionRelativeToInlineStart, axis, containingNode, child)
              : positionRelativeToInlineStart;

      child.SetLayoutPosition(positionRelativeToFlexStart, axis.FlexStartEdge());
    }
    else
    {
      if (isMainAxis)
      {
        JustifyAbsoluteChild(parent, child, direction, axis, containingBlockWidth);
      }
      else
      {
        AlignAbsoluteChild(parent, child, direction, axis, containingBlockWidth);
      }
    }
  }

  #endregion
}
