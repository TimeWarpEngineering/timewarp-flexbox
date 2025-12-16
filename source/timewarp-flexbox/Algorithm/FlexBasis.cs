/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/CalculateLayout.cpp (lines 66-264, 536-616)
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Delegate for the internal layout calculation function.
/// Used to avoid circular dependencies while computing flex basis.
/// </summary>
/// <param name="node">The node to layout.</param>
/// <param name="availableWidth">Available width for layout.</param>
/// <param name="availableHeight">Available height for layout.</param>
/// <param name="ownerDirection">The owner's direction.</param>
/// <param name="widthSizingMode">Width sizing mode.</param>
/// <param name="heightSizingMode">Height sizing mode.</param>
/// <param name="ownerWidth">Owner width for percentage calculations.</param>
/// <param name="ownerHeight">Owner height for percentage calculations.</param>
/// <param name="performLayout">Whether to perform full layout or just measurement.</param>
/// <param name="reason">The reason for this layout pass.</param>
/// <param name="layoutMarkerData">Layout statistics tracker.</param>
/// <param name="depth">Current recursion depth.</param>
/// <param name="generationCount">Current generation counter for cache invalidation.</param>
/// <returns>True if the layout was performed, false if cached.</returns>
public delegate bool CalculateLayoutInternalFunc(
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
    uint generationCount);

/// <summary>
/// Provides flex basis calculation functions for the layout algorithm.
/// These determine the initial size of flex items before flex grow/shrink is applied.
/// </summary>
public static class FlexBasis
{
    /// <summary>
    /// The delegate used for internal layout calculations.
    /// Must be set before calling ComputeFlexBasisForChild with measurement.
    /// </summary>
    public static CalculateLayoutInternalFunc? CalculateLayoutInternal { get; set; }

    /// <summary>
    /// Computes the flex basis for a single child node.
    /// </summary>
    /// <param name="node">The parent node.</param>
    /// <param name="child">The child node to compute flex basis for.</param>
    /// <param name="width">Available width.</param>
    /// <param name="widthMode">Width sizing mode.</param>
    /// <param name="height">Available height.</param>
    /// <param name="ownerWidth">Owner width for percentage calculations.</param>
    /// <param name="ownerHeight">Owner height for percentage calculations.</param>
    /// <param name="heightMode">Height sizing mode.</param>
    /// <param name="direction">Resolved direction.</param>
    /// <param name="layoutMarkerData">Layout statistics tracker.</param>
    /// <param name="depth">Current recursion depth.</param>
    /// <param name="generationCount">Current generation counter.</param>
    /// <remarks>
    /// This corresponds to computeFlexBasisForChild in CalculateLayout.cpp (lines 66-264).
    /// 
    /// Flex basis resolution priority:
    /// 1. Explicit flex-basis if defined and main axis size is defined
    /// 2. Definite dimension in main axis direction
    /// 3. Measure the content (may recurse into calculateLayoutInternal)
    /// </remarks>
    public static void ComputeFlexBasisForChild(
        Node node,
        Node child,
        float width,
        SizingMode widthMode,
        float height,
        float ownerWidth,
        float ownerHeight,
        SizingMode heightMode,
        Direction direction,
        LayoutData layoutMarkerData,
        int depth,
        uint generationCount)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(child);
        ArgumentNullException.ThrowIfNull(layoutMarkerData);

        FlexDirection mainAxis = node.Style.FlexDirection.ResolveDirection(direction);
        bool isMainAxisRow = mainAxis.IsRow();
        float mainAxisSize = isMainAxisRow ? width : height;
        float mainAxisOwnerSize = isMainAxisRow ? ownerWidth : ownerHeight;

        float childWidth;
        float childHeight;
        SizingMode childWidthSizingMode;
        SizingMode childHeightSizingMode;

        FloatOptional resolvedFlexBasis = child.ResolveFlexBasis(
            direction,
            mainAxis,
            mainAxisOwnerSize,
            ownerWidth);

        bool isRowStyleDimDefined = child.HasDefiniteLength(Dimension.Width, ownerWidth);
        bool isColumnStyleDimDefined = child.HasDefiniteLength(Dimension.Height, ownerHeight);

        // If flex basis is defined and main axis size is defined, use flex basis directly
        if (resolvedFlexBasis.IsDefined && Comparison.IsDefined(mainAxisSize))
        {
            if (!child.Layout.ComputedFlexBasis.IsDefined ||
                (child.Config.HasErrata(Errata.None) &&
                 child.Layout.ComputedFlexBasisGeneration != generationCount))
            {
                FloatOptional paddingAndBorder = new(
                    BoundAxis.PaddingAndBorderForAxis(child, mainAxis, direction, ownerWidth));
                child.SetLayoutComputedFlexBasis(
                    FloatOptionalExtensions.MaxOrDefined(resolvedFlexBasis, paddingAndBorder));
            }
        }
        else if (isMainAxisRow && isRowStyleDimDefined)
        {
            // The width is definite, so use that as the flex basis
            FloatOptional paddingAndBorder = new(
                BoundAxis.PaddingAndBorderForAxis(child, FlexDirection.Row, direction, ownerWidth));
            child.SetLayoutComputedFlexBasis(FloatOptionalExtensions.MaxOrDefined(
                child.GetResolvedDimension(direction, Dimension.Width, ownerWidth, ownerWidth),
                paddingAndBorder));
        }
        else if (!isMainAxisRow && isColumnStyleDimDefined)
        {
            // The height is definite, so use that as the flex basis
            FloatOptional paddingAndBorder = new(
                BoundAxis.PaddingAndBorderForAxis(child, FlexDirection.Column, direction, ownerWidth));
            child.SetLayoutComputedFlexBasis(FloatOptionalExtensions.MaxOrDefined(
                child.GetResolvedDimension(direction, Dimension.Height, ownerHeight, ownerWidth),
                paddingAndBorder));
        }
        else
        {
            // Compute the flex basis and hypothetical main size (i.e. the clamped flex
            // basis)
            childWidth = float.NaN;
            childHeight = float.NaN;
            childWidthSizingMode = SizingMode.MaxContent;
            childHeightSizingMode = SizingMode.MaxContent;

            float marginRow = child.Style.ComputeMarginForAxis(FlexDirection.Row, ownerWidth);
            float marginColumn = child.Style.ComputeMarginForAxis(FlexDirection.Column, ownerWidth);

            if (isRowStyleDimDefined)
            {
                childWidth = child.GetResolvedDimension(direction, Dimension.Width, ownerWidth, ownerWidth).Unwrap() +
                             marginRow;
                childWidthSizingMode = SizingMode.StretchFit;
            }

            if (isColumnStyleDimDefined)
            {
                childHeight = child.GetResolvedDimension(direction, Dimension.Height, ownerHeight, ownerWidth).Unwrap() +
                              marginColumn;
                childHeightSizingMode = SizingMode.StretchFit;
            }

            // The W3C spec doesn't say anything about the 'owner' affecting the flex-basis
            // but tests require this behavior
            // So if the owner size is defined, we use it, otherwise we use undefined
            if (!isMainAxisRow && Comparison.IsDefined(width) && widthMode == SizingMode.StretchFit &&
                !isRowStyleDimDefined)
            {
                childWidth = width;
                childWidthSizingMode = SizingMode.StretchFit;
            }

            if (isMainAxisRow && Comparison.IsDefined(height) && heightMode == SizingMode.StretchFit &&
                !isColumnStyleDimDefined)
            {
                childHeight = height;
                childHeightSizingMode = SizingMode.StretchFit;
            }

            // Handle stretch alignment on the cross axis
            FlexDirection crossAxis = mainAxis.ResolveCrossDirection(direction);
            if (!isMainAxisRow && widthMode == SizingMode.StretchFit &&
                !isRowStyleDimDefined &&
                AlignUtils.ResolveChildAlignment(node, child) == Align.Stretch &&
                !child.Style.FlexStartMarginIsAuto(crossAxis, direction) &&
                !child.Style.FlexEndMarginIsAuto(crossAxis, direction))
            {
                childWidth = width;
                childWidthSizingMode = SizingMode.StretchFit;
            }

            if (isMainAxisRow && heightMode == SizingMode.StretchFit &&
                !isColumnStyleDimDefined &&
                AlignUtils.ResolveChildAlignment(node, child) == Align.Stretch &&
                !child.Style.FlexStartMarginIsAuto(crossAxis, direction) &&
                !child.Style.FlexEndMarginIsAuto(crossAxis, direction))
            {
                childHeight = height;
                childHeightSizingMode = SizingMode.StretchFit;
            }

            // Handle aspect ratio
            FloatOptional childAspectRatio = child.Style.AspectRatio;
            if (childAspectRatio.IsDefined)
            {
                if (!isMainAxisRow && childWidthSizingMode == SizingMode.StretchFit)
                {
                    childHeight = marginColumn +
                                  (childWidth - marginRow) / childAspectRatio.Unwrap();
                    childHeightSizingMode = SizingMode.StretchFit;
                }
                else if (isMainAxisRow && childHeightSizingMode == SizingMode.StretchFit)
                {
                    childWidth = marginRow +
                                 (childHeight - marginColumn) * childAspectRatio.Unwrap();
                    childWidthSizingMode = SizingMode.StretchFit;
                }
            }

            // Apply max size constraints
            LayoutHelpers.ConstrainMaxSizeForMode(
                child,
                direction,
                FlexDirection.Row,
                ownerWidth,
                ownerWidth,
                ref childWidthSizingMode,
                ref childWidth);
            LayoutHelpers.ConstrainMaxSizeForMode(
                child,
                direction,
                FlexDirection.Column,
                ownerHeight,
                ownerWidth,
                ref childHeightSizingMode,
                ref childHeight);

            // Measure the child
            YogaAssert.Assert(
                CalculateLayoutInternal is not null,
                "CalculateLayoutInternal delegate must be set before computing flex basis");

            CalculateLayoutInternal!(
                child,
                childWidth,
                childHeight,
                direction,
                childWidthSizingMode,
                childHeightSizingMode,
                ownerWidth,
                ownerHeight,
                false, // performLayout = false for measurement
                LayoutPassReason.MeasureChild,
                layoutMarkerData,
                depth,
                generationCount);

            child.SetLayoutComputedFlexBasis(new FloatOptional(
                Comparison.MaxOrDefined(
                    child.Layout.GetMeasuredDimension(mainAxis.GetDimension()),
                    BoundAxis.PaddingAndBorderForAxis(child, mainAxis, direction, ownerWidth))));
        }

        child.SetLayoutComputedFlexBasisGeneration(generationCount);
    }

    /// <summary>
    /// Computes the flex basis for all children of a node.
    /// </summary>
    /// <param name="node">The parent node.</param>
    /// <param name="availableInnerWidth">Available inner width.</param>
    /// <param name="availableInnerHeight">Available inner height.</param>
    /// <param name="widthSizingMode">Width sizing mode.</param>
    /// <param name="heightSizingMode">Height sizing mode.</param>
    /// <param name="direction">Resolved direction.</param>
    /// <param name="mainAxis">Main flex direction axis.</param>
    /// <param name="layoutMarkerData">Layout statistics tracker.</param>
    /// <param name="depth">Current recursion depth.</param>
    /// <param name="generationCount">Current generation counter.</param>
    /// <returns>The total outer flex basis (sum of all children's flex basis + margins).</returns>
    /// <remarks>
    /// This corresponds to computeFlexBasisForChildren in CalculateLayout.cpp (lines 536-616).
    /// 
    /// This function:
    /// - Implements single flex child optimization
    /// - Skips display:none and absolute positioned children
    /// - Sets initial positions for children
    /// - Accumulates totalOuterFlexBasis
    /// </remarks>
    public static float ComputeFlexBasisForChildren(
        Node node,
        float availableInnerWidth,
        float availableInnerHeight,
        SizingMode widthSizingMode,
        SizingMode heightSizingMode,
        Direction direction,
        FlexDirection mainAxis,
        LayoutData layoutMarkerData,
        int depth,
        uint generationCount)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(layoutMarkerData);

        float totalOuterFlexBasis = 0.0f;
        Node? singleFlexChild = null;
        bool isMainAxisRow = mainAxis.IsRow();
        float mainAxisAvailableSize = isMainAxisRow ? availableInnerWidth : availableInnerHeight;

        // Determine sizing mode for main dimension
        SizingMode sizingModeMainDim = isMainAxisRow ? widthSizingMode : heightSizingMode;

        // If we're sizing to the content (StretchFit with defined available space),
        // look for a single flex child optimization
        if (sizingModeMainDim == SizingMode.StretchFit)
        {
            foreach (Node child in node.LayoutChildren)
            {
                if (child.Style.Display == Display.None)
                {
                    continue;
                }

                if (child.IsNodeFlexible())
                {
                    if (singleFlexChild is not null ||
                        Comparison.InexactEquals(child.ResolveFlexGrow(), 0.0f) ||
                        Comparison.InexactEquals(child.ResolveFlexShrink(), 0.0f))
                    {
                        // There is already a flexible child, or this flexible child
                        // doesn't have both flexGrow and flexShrink set, so we can't
                        // optimize
                        singleFlexChild = null;
                        break;
                    }
                    else
                    {
                        singleFlexChild = child;
                    }
                }
            }
        }

        float ownerWidth = isMainAxisRow ? availableInnerWidth : availableInnerHeight;
        float ownerHeight = isMainAxisRow ? availableInnerHeight : availableInnerWidth;

        foreach (Node child in node.LayoutChildren)
        {
            child.ProcessDimensions();

            if (child.Style.Display == Display.None)
            {
                LayoutHelpers.ZeroOutLayoutRecursively(child);
                child.HasNewLayout = true;
                child.SetDirty(false);
                continue;
            }

            if (child.Style.PositionType == PositionType.Absolute)
            {
                continue;
            }

            // Single flex child optimization: set flex basis to 0
            if (child == singleFlexChild)
            {
                child.SetLayoutComputedFlexBasis(new FloatOptional(0));
            }
            else
            {
                ComputeFlexBasisForChild(
                    node,
                    child,
                    isMainAxisRow ? availableInnerWidth : availableInnerHeight,
                    isMainAxisRow ? widthSizingMode : heightSizingMode,
                    isMainAxisRow ? availableInnerHeight : availableInnerWidth,
                    availableInnerWidth,
                    availableInnerHeight,
                    isMainAxisRow ? heightSizingMode : widthSizingMode,
                    direction,
                    layoutMarkerData,
                    depth + 1,
                    generationCount);
            }

            totalOuterFlexBasis +=
                BoundAxis.BoundAxisWithinMinAndMax(
                    child,
                    direction,
                    mainAxis,
                    child.Layout.ComputedFlexBasis,
                    mainAxisAvailableSize,
                    availableInnerWidth).Unwrap() +
                child.Style.ComputeMarginForAxis(mainAxis, availableInnerWidth);
        }

        return totalOuterFlexBasis;
    }
}
