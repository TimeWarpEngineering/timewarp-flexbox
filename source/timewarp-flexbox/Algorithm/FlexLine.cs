/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/FlexLine.h, yoga/algorithm/FlexLine.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Running layout state for a flex line during layout calculation.
/// </summary>
public struct FlexLineRunningLayout : IEquatable<FlexLineRunningLayout>
{
    /// <summary>
    /// Total flex grow factors of flex items which are to be laid in the current
    /// line. This is decremented as free space is distributed.
    /// </summary>
    public float TotalFlexGrowFactors { get; set; }

    /// <summary>
    /// Total flex shrink factors of flex items which are to be laid in the current
    /// line. This is decremented as free space is distributed.
    /// </summary>
    public float TotalFlexShrinkScaledFactors { get; set; }

    /// <summary>
    /// The amount of available space within inner dimensions of the line which may
    /// still be distributed.
    /// </summary>
    public float RemainingFreeSpace { get; set; }

    /// <summary>
    /// The size of the mainDim for the row after considering size, padding, margin
    /// and border of flex items. This is used to calculate maxLineDim after going
    /// through all the rows to decide on the main axis size of owner.
    /// </summary>
    public float MainDim { get; set; }

    /// <summary>
    /// The size of the crossDim for the row after considering size, padding,
    /// margin and border of flex items. Used for calculating containers crossSize.
    /// </summary>
    public float CrossDim { get; set; }

    /// <inheritdoc />
    public readonly bool Equals(FlexLineRunningLayout other)
    {
        return TotalFlexGrowFactors.Equals(other.TotalFlexGrowFactors) &&
               TotalFlexShrinkScaledFactors.Equals(other.TotalFlexShrinkScaledFactors) &&
               RemainingFreeSpace.Equals(other.RemainingFreeSpace) &&
               MainDim.Equals(other.MainDim) &&
               CrossDim.Equals(other.CrossDim);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => obj is FlexLineRunningLayout other && Equals(other);

    /// <inheritdoc />
    public override readonly int GetHashCode() =>
        HashCode.Combine(TotalFlexGrowFactors, TotalFlexShrinkScaledFactors, RemainingFreeSpace, MainDim, CrossDim);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(FlexLineRunningLayout left, FlexLineRunningLayout right) => left.Equals(right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(FlexLineRunningLayout left, FlexLineRunningLayout right) => !left.Equals(right);
}

/// <summary>
/// Represents a flex line containing items that flow together.
/// </summary>
public sealed class FlexLine
{
    private readonly List<Node> _itemsInFlow = [];

    /// <summary>
    /// List of children which are part of the line flow. This means they are not
    /// positioned absolutely, or with `display: "none"`, and do not overflow the
    /// available dimensions.
    /// </summary>
    public ReadOnlyCollection<Node> ItemsInFlow => _itemsInFlow.AsReadOnly();

    /// <summary>
    /// Accumulation of the dimensions and margin of all the children on the
    /// current line. This will be used in order to either set the dimensions of
    /// the node if none already exist or to compute the remaining space left for
    /// the flexible children.
    /// </summary>
    public float SizeConsumed { get; init; }

    /// <summary>
    /// Number of edges along the line flow with an auto margin.
    /// </summary>
    public int NumberOfAutoMargins { get; init; }

    /// <summary>
    /// Layout information about the line computed in steps after line-breaking.
    /// </summary>
    public FlexLineRunningLayout Layout { get; set; }

    /// <summary>
    /// Adds an item to the items in flow.
    /// </summary>
    internal void AddItemInFlow(Node item)
    {
        _itemsInFlow.Add(item);
    }

    /// <summary>
    /// Calculates where a line starting at a given index should break, returning
    /// information about the collective children on the line.
    /// </summary>
    /// <remarks>
    /// This function assumes that all the children of node have their
    /// computedFlexBasis properly computed (To do this use
    /// computeFlexBasisForChildren function).
    /// </remarks>
    /// <param name="node">The parent node.</param>
    /// <param name="ownerDirection">The owner's direction.</param>
    /// <param name="ownerWidth">The owner's width.</param>
    /// <param name="mainAxisOwnerSize">The main axis size of the owner.</param>
    /// <param name="availableInnerWidth">Available inner width.</param>
    /// <param name="availableInnerMainDim">Available inner main dimension.</param>
    /// <param name="iterator">Iterator over layoutable children.</param>
    /// <param name="lineCount">The current line count.</param>
    /// <returns>A new FlexLine with the calculated line break.</returns>
    public static FlexLine CalculateFlexLine(
        Node node,
        Direction ownerDirection,
        float ownerWidth,
        float mainAxisOwnerSize,
        float availableInnerWidth,
        float availableInnerMainDim,
        ref LayoutableChildren<Node>.Enumerator iterator,
        int lineCount)
    {
        ArgumentNullException.ThrowIfNull(node);

        FlexLine flexLine = new();
        float sizeConsumed = 0.0f;
        float totalFlexGrowFactors = 0.0f;
        float totalFlexShrinkScaledFactors = 0.0f;
        int numberOfAutoMargins = 0;
        Node? firstElementInLine = null;

        float sizeConsumedIncludingMinConstraint = 0;
        Direction direction = node.ResolveDirection(ownerDirection);
        FlexDirection mainAxis = node.Style.FlexDirection.ResolveDirection(direction);
        bool isNodeFlexWrap = node.Style.FlexWrap != Wrap.NoWrap;
        float gap = node.Style.ComputeGapForAxis(mainAxis, availableInnerMainDim);

        // Add items to the current line until it's full or we run out of items.
        while (iterator.MoveNext())
        {
            Node child = iterator.Current;
            if (child.Style.Display == Display.None ||
                child.Style.PositionType == PositionType.Absolute)
            {
                continue;
            }

            if (firstElementInLine is null)
            {
                firstElementInLine = child;
            }

            if (child.Style.FlexStartMarginIsAuto(mainAxis, ownerDirection))
            {
                numberOfAutoMargins++;
            }

            if (child.Style.FlexEndMarginIsAuto(mainAxis, ownerDirection))
            {
                numberOfAutoMargins++;
            }

            child.LineIndex = lineCount;
            float childMarginMainAxis = child.Style.ComputeMarginForAxis(mainAxis, availableInnerWidth);
            float childLeadingGapMainAxis = child == firstElementInLine ? 0.0f : gap;
            float flexBasisWithMinAndMaxConstraints =
                BoundAxis.BoundAxisWithinMinAndMax(
                    child,
                    direction,
                    mainAxis,
                    child.Layout.ComputedFlexBasis,
                    mainAxisOwnerSize,
                    ownerWidth).Unwrap();

            // If this is a multi-line flow and this item pushes us over the available
            // size, we've hit the end of the current line. Break out of the loop and
            // lay out the current line.
            if (sizeConsumedIncludingMinConstraint + flexBasisWithMinAndMaxConstraints +
                    childMarginMainAxis + childLeadingGapMainAxis >
                    availableInnerMainDim &&
                isNodeFlexWrap && flexLine._itemsInFlow.Count > 0)
            {
                // Move iterator back so this child is processed in the next line
                // Note: In C#, we can't easily "put back" the item, so we'll need to handle this differently
                // For now, we'll use a different approach in the calling code
                break;
            }

            sizeConsumedIncludingMinConstraint += flexBasisWithMinAndMaxConstraints +
                childMarginMainAxis + childLeadingGapMainAxis;
            sizeConsumed += flexBasisWithMinAndMaxConstraints + childMarginMainAxis +
                childLeadingGapMainAxis;

            if (child.IsNodeFlexible())
            {
                totalFlexGrowFactors += child.ResolveFlexGrow();

                // Unlike the grow factor, the shrink factor is scaled relative to the
                // child dimension.
                totalFlexShrinkScaledFactors += -child.ResolveFlexShrink() *
                    child.Layout.ComputedFlexBasis.Unwrap();
            }

            flexLine._itemsInFlow.Add(child);
        }

        // The total flex factor needs to be floored to 1.
        if (totalFlexGrowFactors > 0 && totalFlexGrowFactors < 1)
        {
            totalFlexGrowFactors = 1;
        }

        // The total flex shrink factor needs to be floored to 1.
        if (totalFlexShrinkScaledFactors > 0 && totalFlexShrinkScaledFactors < 1)
        {
            totalFlexShrinkScaledFactors = 1;
        }

        return new FlexLine
        {
            SizeConsumed = sizeConsumed,
            NumberOfAutoMargins = numberOfAutoMargins,
            Layout = new FlexLineRunningLayout
            {
                TotalFlexGrowFactors = totalFlexGrowFactors,
                TotalFlexShrinkScaledFactors = totalFlexShrinkScaledFactors
            }
        };
    }
}
