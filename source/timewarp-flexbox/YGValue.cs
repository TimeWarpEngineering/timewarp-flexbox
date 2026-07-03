/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/YGValue.h, yoga/YGValue.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// Represents a dimension value with a unit type used in Yoga layout styles.
/// </summary>
/// <remarks>
/// Design Decision: This is a readonly struct matching C++ YGValue.
/// The equality comparison follows C++ semantics where unit-only types
/// (Auto, Undefined, FitContent, MaxContent, Stretch) compare equal regardless of value,
/// while Point and Percent compare both unit and value.
/// </remarks>
public readonly struct YGValue : IEquatable<YGValue>
{
  /// <summary>
  /// The numeric value (meaningful only for Point and Percent units).
  /// </summary>
  public float Value { get; }

  /// <summary>
  /// The unit type for this value.
  /// </summary>
  public Unit Unit { get; }

  /// <summary>
  /// Creates a new YGValue with the specified value and unit.
  /// </summary>
  /// <param name="value">The numeric value.</param>
  /// <param name="unit">The unit type.</param>
  public YGValue(float value, Unit unit)
  {
    Value = value;
    Unit = unit;
  }

  #region Static Constants

  /// <summary>
  /// Constant for a dimension that is zero-length (0 points).
  /// </summary>
  public static YGValue Zero => new(0, Unit.Point);

  /// <summary>
  /// Constant for a dimension which is not defined.
  /// </summary>
  public static YGValue Undefined => new(float.NaN, Unit.Undefined);

  /// <summary>
  /// Constant for a dimension of "auto".
  /// </summary>
  public static YGValue Auto => new(float.NaN, Unit.Auto);

  #endregion

  #region Factory Methods

  /// <summary>
  /// Creates a point (absolute) value.
  /// </summary>
  /// <param name="value">The value in points.</param>
  /// <returns>A YGValue with Point unit.</returns>
  public static YGValue Point(float value) => new(value, Unit.Point);

  /// <summary>
  /// Creates a percentage value.
  /// </summary>
  /// <param name="value">The percentage value.</param>
  /// <returns>A YGValue with Percent unit.</returns>
  public static YGValue Percent(float value) => new(value, Unit.Percent);

  #endregion

  #region Equality

  /// <summary>
  /// Determines whether two YGValue instances are equal.
  /// </summary>
  /// <remarks>
  /// For unit-only types (Undefined, Auto, FitContent, MaxContent, Stretch),
  /// only the unit is compared. For Point and Percent, both unit and value are compared.
  /// </remarks>
  public bool Equals(YGValue other)
  {
    if (Unit != other.Unit)
    {
      return false;
    }

    return Unit switch
    {
      Unit.Undefined or
      Unit.Auto or
      Unit.FitContent or
      Unit.MaxContent or
      Unit.Stretch => true,
      Unit.Point or
      Unit.Percent => Value == other.Value,
      _ => false
    };
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is YGValue other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
  {
    // For unit-only types, only hash the unit
    return Unit switch
    {
      Unit.Undefined or
      Unit.Auto or
      Unit.FitContent or
      Unit.MaxContent or
      Unit.Stretch => Unit.GetHashCode(),
      _ => HashCode.Combine(Value, Unit)
    };
  }

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(YGValue left, YGValue right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(YGValue left, YGValue right) => !left.Equals(right);

  #endregion

  #region Operators

  /// <summary>
  /// Negation operator - returns a new YGValue with negated value.
  /// </summary>
  /// <param name="value">The value to negate.</param>
  /// <returns>A new YGValue with the negated value and same unit.</returns>
  public static YGValue operator -(YGValue value) => value.Negate();

  /// <summary>
  /// Returns a new YGValue with negated value (friendly alternate for unary minus operator).
  /// </summary>
  /// <returns>A new YGValue with the negated value and same unit.</returns>
  public YGValue Negate() => new(-Value, Unit);

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
      Unit.Point => $"{Value}pt",
      Unit.Percent => $"{Value}%",
      _ => $"{Value} ({Unit})"
    };
  }

  #endregion
}

/// <summary>
/// Provides utility methods related to YGValue and undefined float values.
/// </summary>
public static class YGValueUtilities
{
  /// <summary>
  /// Float value to represent "undefined" in style values.
  /// Matches C++ YGUndefined constant.
  /// </summary>
  public const float YGUndefined = float.NaN;

  /// <summary>
  /// Whether a dimension represented as a float is defined.
  /// </summary>
  /// <param name="value">The value to check.</param>
  /// <returns>True if the value is undefined (NaN).</returns>
  /// <remarks>
  /// This is the C API function YGFloatIsUndefined.
  /// Internally delegates to Comparison.IsUndefined.
  /// </remarks>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool YGFloatIsUndefined(float value) => Comparison.IsUndefined(value);
}
