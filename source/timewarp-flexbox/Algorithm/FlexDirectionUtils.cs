/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/FlexDirection.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Extension methods and utilities for FlexDirection.
/// </summary>
public static class FlexDirectionUtils
{
    /// <summary>
    /// Checks if the flex direction is row-based.
    /// </summary>
    public static bool IsRow(this FlexDirection flexDirection)
    {
        return flexDirection is FlexDirection.Row or FlexDirection.RowReverse;
    }

    /// <summary>
    /// Checks if the flex direction is column-based.
    /// </summary>
    public static bool IsColumn(this FlexDirection flexDirection)
    {
        return flexDirection is FlexDirection.Column or FlexDirection.ColumnReverse;
    }

    /// <summary>
    /// Resolves the flex direction based on text direction (RTL/LTR).
    /// </summary>
    public static FlexDirection ResolveDirection(this FlexDirection flexDirection, Direction direction)
    {
        if (direction == Direction.RTL)
        {
            if (flexDirection == FlexDirection.Row)
            {
                return FlexDirection.RowReverse;
            }
            else if (flexDirection == FlexDirection.RowReverse)
            {
                return FlexDirection.Row;
            }
        }

        return flexDirection;
    }

    /// <summary>
    /// Gets the cross direction perpendicular to the main axis.
    /// </summary>
    public static FlexDirection ResolveCrossDirection(this FlexDirection flexDirection, Direction direction)
    {
        return flexDirection.IsColumn()
            ? FlexDirection.Row.ResolveDirection(direction)
            : FlexDirection.Column;
    }

    /// <summary>
    /// Gets the physical edge at the start of the flex direction.
    /// </summary>
    public static PhysicalEdge FlexStartEdge(this FlexDirection flexDirection) => flexDirection switch
    {
        FlexDirection.Column => PhysicalEdge.Top,
        FlexDirection.ColumnReverse => PhysicalEdge.Bottom,
        FlexDirection.Row => PhysicalEdge.Left,
        FlexDirection.RowReverse => PhysicalEdge.Right,
        _ => throw new ArgumentOutOfRangeException(nameof(flexDirection), flexDirection, "Invalid FlexDirection")
    };

    /// <summary>
    /// Gets the physical edge at the end of the flex direction.
    /// </summary>
    public static PhysicalEdge FlexEndEdge(this FlexDirection flexDirection) => flexDirection switch
    {
        FlexDirection.Column => PhysicalEdge.Bottom,
        FlexDirection.ColumnReverse => PhysicalEdge.Top,
        FlexDirection.Row => PhysicalEdge.Right,
        FlexDirection.RowReverse => PhysicalEdge.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(flexDirection), flexDirection, "Invalid FlexDirection")
    };

    /// <summary>
    /// Gets the physical edge at the inline start (considering text direction).
    /// </summary>
    public static PhysicalEdge InlineStartEdge(this FlexDirection flexDirection, Direction direction)
    {
        if (flexDirection.IsRow())
        {
            return direction == Direction.RTL ? PhysicalEdge.Right : PhysicalEdge.Left;
        }

        return PhysicalEdge.Top;
    }

    /// <summary>
    /// Gets the physical edge at the inline end (considering text direction).
    /// </summary>
    public static PhysicalEdge InlineEndEdge(this FlexDirection flexDirection, Direction direction)
    {
        if (flexDirection.IsRow())
        {
            return direction == Direction.RTL ? PhysicalEdge.Left : PhysicalEdge.Right;
        }

        return PhysicalEdge.Bottom;
    }

    /// <summary>
    /// Gets the dimension (Width or Height) for this flex direction.
    /// </summary>
    public static Dimension GetDimension(this FlexDirection flexDirection) => flexDirection switch
    {
        FlexDirection.Column or FlexDirection.ColumnReverse => Dimension.Height,
        FlexDirection.Row or FlexDirection.RowReverse => Dimension.Width,
        _ => throw new ArgumentOutOfRangeException(nameof(flexDirection), flexDirection, "Invalid FlexDirection")
    };
}
