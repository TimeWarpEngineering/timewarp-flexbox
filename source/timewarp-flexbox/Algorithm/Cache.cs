/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/Cache.h, yoga/algorithm/Cache.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Provides utilities for determining if cached layout measurements can be reused.
/// </summary>
public static class Cache
{
    /// <summary>
    /// Determines if a cached measurement can be reused based on the current sizing constraints.
    /// </summary>
    /// <param name="widthMode">The current width sizing mode.</param>
    /// <param name="availableWidth">The current available width.</param>
    /// <param name="heightMode">The current height sizing mode.</param>
    /// <param name="availableHeight">The current available height.</param>
    /// <param name="lastWidthMode">The width sizing mode from the cached measurement.</param>
    /// <param name="lastAvailableWidth">The available width from the cached measurement.</param>
    /// <param name="lastHeightMode">The height sizing mode from the cached measurement.</param>
    /// <param name="lastAvailableHeight">The available height from the cached measurement.</param>
    /// <param name="lastComputedWidth">The computed width from the cached measurement.</param>
    /// <param name="lastComputedHeight">The computed height from the cached measurement.</param>
    /// <param name="marginRow">The horizontal margin (start + end).</param>
    /// <param name="marginColumn">The vertical margin (top + bottom).</param>
    /// <param name="config">The configuration to use for pixel grid rounding.</param>
    /// <returns>True if the cached measurement can be reused.</returns>
    public static bool CanUseCachedMeasurement(
        SizingMode widthMode,
        float availableWidth,
        SizingMode heightMode,
        float availableHeight,
        SizingMode lastWidthMode,
        float lastAvailableWidth,
        SizingMode lastHeightMode,
        float lastAvailableHeight,
        float lastComputedWidth,
        float lastComputedHeight,
        float marginRow,
        float marginColumn,
        Config? config)
    {
        // Return false if either computed dimension is negative
        if ((Comparison.IsDefined(lastComputedHeight) && lastComputedHeight < 0) ||
            (Comparison.IsDefined(lastComputedWidth) && lastComputedWidth < 0))
        {
            return false;
        }

        float pointScaleFactor = config?.PointScaleFactor ?? 0.0f;

        bool useRoundedComparison = config is not null && pointScaleFactor != 0;
        float effectiveWidth = useRoundedComparison
            ? PixelGrid.RoundValueToPixelGrid(availableWidth, pointScaleFactor, false, false)
            : availableWidth;
        float effectiveHeight = useRoundedComparison
            ? PixelGrid.RoundValueToPixelGrid(availableHeight, pointScaleFactor, false, false)
            : availableHeight;
        float effectiveLastWidth = useRoundedComparison
            ? PixelGrid.RoundValueToPixelGrid(lastAvailableWidth, pointScaleFactor, false, false)
            : lastAvailableWidth;
        float effectiveLastHeight = useRoundedComparison
            ? PixelGrid.RoundValueToPixelGrid(lastAvailableHeight, pointScaleFactor, false, false)
            : lastAvailableHeight;

        bool hasSameWidthSpec = lastWidthMode == widthMode &&
            Comparison.InexactEquals(effectiveLastWidth, effectiveWidth);
        bool hasSameHeightSpec = lastHeightMode == heightMode &&
            Comparison.InexactEquals(effectiveLastHeight, effectiveHeight);

        bool widthIsCompatible =
            hasSameWidthSpec ||
            SizeIsExactAndMatchesOldMeasuredSize(
                widthMode, availableWidth - marginRow, lastComputedWidth) ||
            OldSizeIsMaxContentAndStillFits(
                widthMode,
                availableWidth - marginRow,
                lastWidthMode,
                lastComputedWidth) ||
            NewSizeIsStricterAndStillValid(
                widthMode,
                availableWidth - marginRow,
                lastWidthMode,
                lastAvailableWidth,
                lastComputedWidth);

        bool heightIsCompatible = hasSameHeightSpec ||
            SizeIsExactAndMatchesOldMeasuredSize(
                heightMode,
                availableHeight - marginColumn,
                lastComputedHeight) ||
            OldSizeIsMaxContentAndStillFits(
                heightMode,
                availableHeight - marginColumn,
                lastHeightMode,
                lastComputedHeight) ||
            NewSizeIsStricterAndStillValid(
                heightMode,
                availableHeight - marginColumn,
                lastHeightMode,
                lastAvailableHeight,
                lastComputedHeight);

        return widthIsCompatible && heightIsCompatible;
    }

    /// <summary>
    /// Checks if the current size is exact (StretchFit) and matches the old measured size.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SizeIsExactAndMatchesOldMeasuredSize(
        SizingMode sizeMode,
        float size,
        float lastComputedSize)
    {
        return sizeMode == SizingMode.StretchFit &&
               Comparison.InexactEquals(size, lastComputedSize);
    }

    /// <summary>
    /// Checks if the old measurement was MaxContent and the new size still fits.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool OldSizeIsMaxContentAndStillFits(
        SizingMode sizeMode,
        float size,
        SizingMode lastSizeMode,
        float lastComputedSize)
    {
        return sizeMode == SizingMode.FitContent &&
               lastSizeMode == SizingMode.MaxContent &&
               (size >= lastComputedSize || Comparison.InexactEquals(size, lastComputedSize));
    }

    /// <summary>
    /// Checks if the new size is stricter but still valid for the cached measurement.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool NewSizeIsStricterAndStillValid(
        SizingMode sizeMode,
        float size,
        SizingMode lastSizeMode,
        float lastSize,
        float lastComputedSize)
    {
        return lastSizeMode == SizingMode.FitContent &&
               sizeMode == SizingMode.FitContent &&
               Comparison.IsDefined(lastSize) &&
               Comparison.IsDefined(size) &&
               Comparison.IsDefined(lastComputedSize) &&
               lastSize > size &&
               (lastComputedSize <= size || Comparison.InexactEquals(size, lastComputedSize));
    }
}
