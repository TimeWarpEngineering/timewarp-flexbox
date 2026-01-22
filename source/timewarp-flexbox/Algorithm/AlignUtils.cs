/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/Align.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides alignment utility functions for the layout algorithm.
/// </summary>
public static class AlignUtils
{
    /// <summary>
    /// Resolves the alignment for a child based on parent's alignItems and child's alignSelf.
    /// </summary>
    /// <param name="node">The parent node.</param>
    /// <param name="child">The child node.</param>
    /// <returns>The resolved alignment for the child.</returns>
    public static Align ResolveChildAlignment(Node node, Node child)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(child);

        Align align = child.Style.AlignSelf == Align.Auto
            ? node.Style.AlignItems
            : child.Style.AlignSelf;

        // Baseline alignment in column flex direction defaults to flex-start
        if (align == Align.Baseline && node.Style.FlexDirection.IsColumn())
        {
            return Align.FlexStart;
        }

        return align;
    }

    /// <summary>
    /// Gets the fallback alignment to use on overflow.
    /// </summary>
    /// <remarks>
    /// See: https://www.w3.org/TR/css-align-3/#distribution-values
    /// </remarks>
    /// <param name="align">The original alignment.</param>
    /// <returns>The fallback alignment.</returns>
    public static Align FallbackAlignment(Align align)
    {
        return align switch
        {
            // Fallback to flex-start
            Align.SpaceBetween or Align.Stretch => Align.FlexStart,

            // Fallback to safe center. TODO (T208209388): This should be aligned to
            // Start instead of FlexStart (for row-reverse containers)
            Align.SpaceAround or Align.SpaceEvenly => Align.FlexStart,

            _ => align
        };
    }

    /// <summary>
    /// Gets the fallback justify alignment to use on overflow.
    /// </summary>
    /// <remarks>
    /// See: https://www.w3.org/TR/css-align-3/#distribution-values
    /// </remarks>
    /// <param name="justify">The original justify alignment.</param>
    /// <returns>The fallback justify alignment.</returns>
    public static Justify FallbackAlignment(Justify justify)
    {
        return justify switch
        {
            // Fallback to flex-start
            Justify.SpaceBetween => Justify.FlexStart,
            // TODO: Support `justify-content: stretch`
            // Justify.Stretch => Justify.FlexStart,

            // Fallback to safe center. TODO (T208209388): This should be aligned to
            // Start instead of FlexStart (for row-reverse containers)
            Justify.SpaceAround or Justify.SpaceEvenly => Justify.FlexStart,

            _ => justify
        };
    }
}
