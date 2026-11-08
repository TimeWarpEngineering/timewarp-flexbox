/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/style/StyleSizeLength.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a CSS Value for sizes (e.g. width, height, min-width, etc.).
/// It may be one of:
/// 1. Undefined
/// 2. A keyword (e.g. auto, max-content, stretch, etc.)
/// 3. A CSS length-percentage value:
///    a. length value (e.g. 10px)
///    b. percentage value of a reference length
/// </summary>
/// <remarks>
/// References:
/// 1. https://www.w3.org/TR/css-values-4/#lengths
/// 2. https://www.w3.org/TR/css-values-4/#percentage-value
/// 3. https://www.w3.org/TR/css-values-4/#mixed-percentages
/// </remarks>
public readonly struct StyleSizeLength : IEquatable<StyleSizeLength>
{
  private readonly Unit Unit;

  /// <summary>
  /// Private constructor to prevent invalid combinations.
  /// Use factory methods instead.
  /// </summary>
  private StyleSizeLength(FloatOptional value, Unit unit)
  {
    Value = value;
    Unit = unit;
  }

  #region Factory Methods

  /// <summary>
  /// Creates a point (absolute) size value.
  /// </summary>
  /// <param name="value">The value in points.</param>
  /// <returns>A StyleSizeLength with Point unit, or Undefined if value is NaN or infinite.</returns>
  public static StyleSizeLength Points(float value)
  {
    return Comparison.IsUndefined(value) || Comparison.IsInfinity(value)
        ? Undefined
        : new StyleSizeLength(new FloatOptional(value), Unit.Point);
  }

  /// <summary>
  /// Creates a percentage size value.
  /// </summary>
  /// <param name="value">The percentage value.</param>
  /// <returns>A StyleSizeLength with Percent unit, or Undefined if value is NaN or infinite.</returns>
  public static StyleSizeLength Percent(float value)
  {
    return Comparison.IsUndefined(value) || Comparison.IsInfinity(value)
        ? Undefined
        : new StyleSizeLength(new FloatOptional(value), Unit.Percent);
  }

  /// <summary>
  /// Creates an auto size value.
  /// </summary>
  public static StyleSizeLength Auto => new(FloatOptional.Undefined, Unit.Auto);

  /// <summary>
  /// Creates a max-content size value.
  /// </summary>
  public static StyleSizeLength MaxContent => new(FloatOptional.Undefined, Unit.MaxContent);

  /// <summary>
  /// Creates a fit-content size value.
  /// </summary>
  public static StyleSizeLength FitContent => new(FloatOptional.Undefined, Unit.FitContent);

  /// <summary>
  /// Creates a stretch size value.
  /// </summary>
  public static StyleSizeLength Stretch => new(FloatOptional.Undefined, Unit.Stretch);

  /// <summary>
  /// Creates an undefined size value.
  /// </summary>
  public static StyleSizeLength Undefined => new(FloatOptional.Undefined, Unit.Undefined);

  #endregion

  #region Properties

  /// <summary>
  /// Gets the numeric value (meaningful only for Point and Percent units).
  /// </summary>
  public FloatOptional Value { get; }

  /// <summary>
  /// Gets whether this is an auto value.
  /// </summary>
  public bool IsAuto => Unit == Unit.Auto;

  /// <summary>
  /// Gets whether this is a max-content value.
  /// </summary>
  public bool IsMaxContent => Unit == Unit.MaxContent;

  /// <summary>
  /// Gets whether this is a fit-content value.
  /// </summary>
  public bool IsFitContent => Unit == Unit.FitContent;

  /// <summary>
  /// Gets whether this is a stretch value.
  /// </summary>
  public bool IsStretch => Unit == Unit.Stretch;

  /// <summary>
  /// Gets whether this is undefined.
  /// </summary>
  public bool IsUndefined => Unit == Unit.Undefined;

  /// <summary>
  /// Gets whether this is defined (not undefined).
  /// </summary>
  public bool IsDefined => !IsUndefined;

  /// <summary>
  /// Gets whether this is a point value.
  /// </summary>
  public bool IsPoints => Unit == Unit.Point;

  /// <summary>
  /// Gets whether this is a percentage value.
  /// </summary>
  public bool IsPercent => Unit == Unit.Percent;

  #endregion

  #region Methods

  /// <summary>
  /// Resolves this size length against a reference length.
  /// </summary>
  /// <param name="referenceLength">The reference length for percentage resolution.</param>
  /// <returns>
  /// For Point: returns the value as-is.
  /// For Percent: returns value * referenceLength * 0.01.
  /// For others: returns undefined.
  /// </returns>
  public FloatOptional Resolve(float referenceLength)
  {
    return Unit switch
    {
      Unit.Point => Value,
      Unit.Percent => new FloatOptional(Value.Unwrap() * referenceLength * 0.01f),
      Unit.Undefined or Unit.Auto or Unit.MaxContent or Unit.FitContent or Unit.Stretch => FloatOptional.Undefined,
      _ => FloatOptional.Undefined
    };
  }

  /// <summary>
  /// Converts to a YGValue.
  /// </summary>
  public YGValue ToYGValue() => new(Value.Unwrap(), Unit);

  /// <summary>
  /// Explicit conversion to YGValue.
  /// </summary>
  public static explicit operator YGValue(StyleSizeLength length) => length.ToYGValue();

  #endregion

  #region Equality

  /// <inheritdoc />
  public bool Equals(StyleSizeLength other)
  {
    return Value == other.Value && Unit == other.Unit;
  }

  /// <summary>
  /// Compares for approximate equality using tolerance.
  /// </summary>
  public bool InexactEquals(StyleSizeLength other)
  {
    return Unit == other.Unit && FloatOptionalExtensions.InexactEquals(Value, other.Value);
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is StyleSizeLength other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() => HashCode.Combine(Value, Unit);

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(StyleSizeLength left, StyleSizeLength right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(StyleSizeLength left, StyleSizeLength right) => !left.Equals(right);

  #endregion

  #region String Representation

  /// <inheritdoc />
  public override string ToString()
  {
    return Unit switch
    {
      Unit.Undefined => "undefined",
      Unit.Auto => "auto",
      Unit.MaxContent => "max-content",
      Unit.FitContent => "fit-content",
      Unit.Stretch => "stretch",
      Unit.Point => $"{Value.Unwrap()}pt",
      Unit.Percent => $"{Value.Unwrap()}%",
      _ => $"{Value} ({Unit})"
    };
  }

  #endregion
}

/// <summary>
/// Extension methods for StyleSizeLength.
/// </summary>
public static class StyleSizeLengthExtensions
{
  /// <summary>
  /// Compares two StyleSizeLength values for approximate equality.
  /// </summary>
  public static bool InexactEquals(StyleSizeLength a, StyleSizeLength b) => a.InexactEquals(b);
}
