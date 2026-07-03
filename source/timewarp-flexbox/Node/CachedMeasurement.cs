/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/node/CachedMeasurement.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Stores a cached measurement result to avoid redundant calculations.
/// </summary>
public struct CachedMeasurement : IEquatable<CachedMeasurement>
{
  /// <summary>
  /// The available width used for this measurement.
  /// </summary>
  public float AvailableWidth { get; set; }

  /// <summary>
  /// The available height used for this measurement.
  /// </summary>
  public float AvailableHeight { get; set; }

  /// <summary>
  /// The sizing mode for width.
  /// </summary>
  public SizingMode WidthSizingMode { get; set; }

  /// <summary>
  /// The sizing mode for height.
  /// </summary>
  public SizingMode HeightSizingMode { get; set; }

  /// <summary>
  /// The computed width result.
  /// </summary>
  public float ComputedWidth { get; set; }

  /// <summary>
  /// The computed height result.
  /// </summary>
  public float ComputedHeight { get; set; }

  /// <summary>
  /// Creates a default cached measurement with invalid values.
  /// </summary>
  public static CachedMeasurement Default => new()
  {
    AvailableWidth = -1,
    AvailableHeight = -1,
    WidthSizingMode = SizingMode.MaxContent,
    HeightSizingMode = SizingMode.MaxContent,
    ComputedWidth = -1,
    ComputedHeight = -1
  };

  /// <inheritdoc />
  public readonly bool Equals(CachedMeasurement other)
  {
    bool isEqual = WidthSizingMode == other.WidthSizingMode &&
                   HeightSizingMode == other.HeightSizingMode;

    if (!Comparison.IsUndefined(AvailableWidth) ||
        !Comparison.IsUndefined(other.AvailableWidth))
    {
      isEqual = isEqual && AvailableWidth == other.AvailableWidth;
    }

    if (!Comparison.IsUndefined(AvailableHeight) ||
        !Comparison.IsUndefined(other.AvailableHeight))
    {
      isEqual = isEqual && AvailableHeight == other.AvailableHeight;
    }

    if (!Comparison.IsUndefined(ComputedWidth) ||
        !Comparison.IsUndefined(other.ComputedWidth))
    {
      isEqual = isEqual && ComputedWidth == other.ComputedWidth;
    }

    if (!Comparison.IsUndefined(ComputedHeight) ||
        !Comparison.IsUndefined(other.ComputedHeight))
    {
      isEqual = isEqual && ComputedHeight == other.ComputedHeight;
    }

    return isEqual;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is CachedMeasurement other && Equals(other);

  /// <inheritdoc />
  public override readonly int GetHashCode() =>
      HashCode.Combine(AvailableWidth, AvailableHeight, WidthSizingMode, HeightSizingMode, ComputedWidth, ComputedHeight);

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(CachedMeasurement left, CachedMeasurement right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(CachedMeasurement left, CachedMeasurement right) => !left.Equals(right);
}
