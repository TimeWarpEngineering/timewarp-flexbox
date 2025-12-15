/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/PixelGrid.h, yoga/algorithm/PixelGrid.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides utilities for rounding layout values to the pixel grid.
/// </summary>
public static class PixelGrid
{
    /// <summary>
    /// Rounds a point value to the nearest physical pixel based on DPI (pointScaleFactor).
    /// </summary>
    /// <param name="value">The value to round.</param>
    /// <param name="pointScaleFactor">The scale factor (pixels per point).</param>
    /// <param name="forceCeil">Force rounding up.</param>
    /// <param name="forceFloor">Force rounding down.</param>
    /// <returns>The rounded value, or NaN if inputs are invalid.</returns>
    public static float RoundValueToPixelGrid(
        double value,
        double pointScaleFactor,
        bool forceCeil,
        bool forceFloor)
    {
        double scaledValue = value * pointScaleFactor;

        // We want to calculate `fractional` such that `floor(scaledValue) = scaledValue - fractional`.
        double fractional = scaledValue % 1.0;

        if (fractional < 0)
        {
            // This branch is for handling negative numbers for `value`.
            //
            // Regarding `floor` and `ceil`. Note that for a number x, `floor(x) <= x <= ceil(x)`
            // even for negative numbers. Here are a couple of examples:
            //   - x =  2.2: floor( 2.2) =  2, ceil( 2.2) =  3
            //   - x = -2.2: floor(-2.2) = -3, ceil(-2.2) = -2
            //
            // Regarding modulo for negative numbers, in C# the % operator returns a negative number
            // for negative operands. For example, -2.2 % 1.0 = -0.2. However, we want `fractional`
            // to be the number such that subtracting it from `value` will give us `floor(value)`.
            // In the case of negative numbers, adding 1 to the modulo result gives us this:
            //   - fractional = -2.2 % 1.0 = -0.2
            //   - Add 1 to the fraction: fractional2 = fractional + 1 = -0.2 + 1 = 0.8
            //   - Finding the `floor`: -2.2 - fractional2 = -2.2 - 0.8 = -3
            ++fractional;
        }

        if (Comparison.InexactEquals((float)fractional, 0))
        {
            // First we check if the value is already rounded
            scaledValue -= fractional;
        }
        else if (Comparison.InexactEquals((float)fractional, 1.0f))
        {
            scaledValue = scaledValue - fractional + 1.0;
        }
        else if (forceCeil)
        {
            // Next we check if we need to use forced rounding
            scaledValue = scaledValue - fractional + 1.0;
        }
        else if (forceFloor)
        {
            scaledValue -= fractional;
        }
        else
        {
            // Finally we just round the value
            scaledValue = scaledValue - fractional +
                (!double.IsNaN(fractional) &&
                 (fractional > 0.5 || Comparison.InexactEquals((float)fractional, 0.5f))
                    ? 1.0
                    : 0.0);
        }

        return (double.IsNaN(scaledValue) || double.IsNaN(pointScaleFactor))
            ? float.NaN
            : (float)(scaledValue / pointScaleFactor);
    }
}
