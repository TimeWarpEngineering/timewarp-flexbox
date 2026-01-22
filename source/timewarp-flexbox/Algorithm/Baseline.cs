/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/Baseline.h, yoga/algorithm/Baseline.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides baseline calculation utilities for the layout algorithm.
/// </summary>
public static class Baseline
{
    /// <summary>
    /// Calculates the baseline represented as an offset from the top edge of the node.
    /// </summary>
    /// <param name="node">The node to calculate the baseline for.</param>
    /// <returns>The baseline offset from the top of the node.</returns>
    public static float CalculateBaseline(Node node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node.HasBaselineFunc)
        {
            YogaEvent.PublishNodeBaselineStart(node);

            float baseline = node.Baseline(
                node.Layout.GetMeasuredDimension(Dimension.Width),
                node.Layout.GetMeasuredDimension(Dimension.Height));

            YogaEvent.PublishNodeBaselineEnd(node);

            YogaAssert.Assert(
                node,
                !float.IsNaN(baseline),
                "Expect custom baseline function to not return NaN");

            return baseline;
        }

        Node? baselineChild = null;
        foreach (Node child in node.LayoutChildren)
        {
            if (child.LineIndex > 0)
            {
                break;
            }

            if (child.Style.PositionType == PositionType.Absolute)
            {
                continue;
            }

            if (AlignUtils.ResolveChildAlignment(node, child) == Align.Baseline ||
                child.IsReferenceBaseline)
            {
                baselineChild = child;
                break;
            }

            if (baselineChild is null)
            {
                baselineChild = child;
            }
        }

        if (baselineChild is null)
        {
            return node.Layout.GetMeasuredDimension(Dimension.Height);
        }

        float childBaseline = CalculateBaseline(baselineChild);
        return childBaseline + baselineChild.Layout.GetPosition(PhysicalEdge.Top);
    }

    /// <summary>
    /// Determines whether any of the children of this node participate in baseline alignment.
    /// </summary>
    /// <param name="node">The node to check.</param>
    /// <returns>True if any children participate in baseline alignment.</returns>
    public static bool IsBaselineLayout(Node node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node.Style.FlexDirection.IsColumn())
        {
            return false;
        }

        if (node.Style.AlignItems == Align.Baseline)
        {
            return true;
        }

        foreach (Node child in node.LayoutChildren)
        {
            if (child.Style.PositionType != PositionType.Absolute &&
                child.Style.AlignSelf == Align.Baseline)
            {
                return true;
            }
        }

        return false;
    }
}
