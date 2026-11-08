/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/algorithm/SizingMode.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Corresponds to a CSS auto box sizes. Missing "min-content", as Yoga does not
/// currently support automatic minimum sizes.
/// </summary>
/// <remarks>
/// References:
/// https://www.w3.org/TR/css-sizing-3/#auto-box-sizes
/// https://www.w3.org/TR/css-flexbox-1/#min-size-auto
/// </remarks>
internal enum SizingMode
{
  /// <summary>
  /// The size a box would take if its outer size filled the available space in
  /// the given axis; in other words, the stretch fit into the available space,
  /// if that is definite. Undefined if the available space is indefinite.
  /// </summary>
  StretchFit,

  /// <summary>
  /// A box's "ideal" size in a given axis when given infinite available space.
  /// Usually this is the smallest size the box could take in that axis while
  /// still fitting around its contents, i.e. minimizing unfilled space while
  /// avoiding overflow.
  /// </summary>
  MaxContent,

  /// <summary>
  /// If the available space in a given axis is definite, equal to
  /// clamp(min-content size, stretch-fit size, max-content size) (i.e.
  /// max(min-content size, min(max-content size, stretch-fit size))). When
  /// sizing under a min-content constraint, equal to the min-content size.
  /// Otherwise, equal to the max-content size in that axis.
  /// </summary>
  FitContent
}

/// <summary>
/// Extension methods for SizingMode conversions.
/// </summary>
internal static class SizingModeExtensions
{
  /// <summary>
  /// Converts a SizingMode to the corresponding MeasureMode.
  /// </summary>
  /// <param name="mode">The sizing mode to convert.</param>
  /// <returns>The equivalent MeasureMode.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown for invalid SizingMode values.</exception>
  public static MeasureMode ToMeasureMode(this SizingMode mode) => mode switch
  {
    SizingMode.StretchFit => MeasureMode.Exactly,
    SizingMode.MaxContent => MeasureMode.Undefined,
    SizingMode.FitContent => MeasureMode.AtMost,
    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid SizingMode")
  };

  /// <summary>
  /// Converts a MeasureMode to the corresponding SizingMode.
  /// </summary>
  /// <param name="mode">The measure mode to convert.</param>
  /// <returns>The equivalent SizingMode.</returns>
  /// <exception cref="ArgumentOutOfRangeException">Thrown for invalid MeasureMode values.</exception>
  public static SizingMode ToSizingMode(this MeasureMode mode) => mode switch
  {
    MeasureMode.Exactly => SizingMode.StretchFit,
    MeasureMode.Undefined => SizingMode.MaxContent,
    MeasureMode.AtMost => SizingMode.FitContent,
    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid MeasureMode")
  };
}
