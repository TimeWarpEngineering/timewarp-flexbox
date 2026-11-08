/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/numeric/FloatOptional.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

using System.Globalization;

/// <summary>
/// A wrapper around float that uses NaN to represent undefined values.
/// This is the core numeric type used throughout Yoga for optional dimension values.
/// </summary>
/// <remarks>
/// Design Decision: Using readonly struct for value semantics and zero allocation.
/// NaN is used as the sentinel value (same as C++) to represent "undefined".
/// This allows the struct to be exactly 4 bytes (same as float).
///
/// All operators pass by value since this is a 32-bit value type.
/// </remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2231:Overload operator equals on overriding value type Equals", Justification = "Operators are implemented below")]
public readonly struct FloatOptional : IEquatable<FloatOptional>, IComparable<FloatOptional>
{
  /// <summary>
  /// A FloatOptional representing an undefined value.
  /// </summary>
  public static FloatOptional Undefined => new(float.NaN);

  /// <summary>
  /// A FloatOptional representing zero.
  /// </summary>
  public static readonly FloatOptional Zero = new(0f);

  // Note: C++ initializes to NaN by default. In C#, default(float) is 0.
  // Therefore, we explicitly initialize with NaN to match C++ behavior when
  // the default constructor is called. However, since this is a readonly struct,
  // we can't have a parameterless constructor. Users should use FloatOptional.Undefined
  // or new FloatOptional(float.NaN) to get an undefined value.
  private readonly float Value;

  /// <summary>
  /// Creates a FloatOptional with the specified value.
  /// </summary>
  /// <param name="value">The float value to wrap.</param>
  public FloatOptional(float value)
  {
    Value = value;
  }

  /// <summary>
  /// Creates a FloatOptional from a float value.
  /// </summary>
  public static FloatOptional FromSingle(float value) => new(value);

  /// <summary>
  /// Gets the wrapped value, or NaN if undefined.
  /// </summary>
  /// <returns>The wrapped float value.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float Unwrap() => Value;

  /// <summary>
  /// Gets the wrapped value if defined, otherwise returns the default value.
  /// </summary>
  /// <param name="defaultValue">The value to return if undefined.</param>
  /// <returns>The wrapped value or the default.</returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float UnwrapOrDefault(float defaultValue) => IsUndefined ? defaultValue : Value;

  /// <summary>
  /// Gets whether this value is undefined (NaN).
  /// </summary>
  public bool IsUndefined
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => Comparison.IsUndefined(Value);
  }

  /// <summary>
  /// Gets whether this value is defined (not NaN).
  /// </summary>
  public bool IsDefined
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get => Comparison.IsDefined(Value);
  }

  /// <summary>
  /// Adds two FloatOptional values.
  /// </summary>
  public static FloatOptional Add(FloatOptional left, FloatOptional right) => new(left.Value + right.Value);

  /// <summary>
  /// Compares this FloatOptional with another.
  /// </summary>
  public int CompareTo(FloatOptional other)
  {
    if (IsUndefined && other.IsUndefined)
    {
      return 0;
    }

    if (IsUndefined)
    {
      return -1;
    }

    if (other.IsUndefined)
    {
      return 1;
    }

    return Value.CompareTo(other.Value);
  }

  /// <inheritdoc />
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(FloatOptional other) =>
      // Equal if both values are equal OR both are undefined (NaN)
      Value == other.Value || (IsUndefined && other.IsUndefined);

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is FloatOptional other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode() =>
      // NaN values should all hash to the same value
      IsUndefined ? 0 : Value.GetHashCode();

  /// <inheritdoc />
  public override string ToString() => IsUndefined ? "undefined" : Value.ToString(CultureInfo.InvariantCulture);

  // Equality operators
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator ==(FloatOptional left, FloatOptional right) => left.Equals(right);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator !=(FloatOptional left, FloatOptional right) => !left.Equals(right);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator ==(FloatOptional left, float right) => left.Equals(new FloatOptional(right));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator !=(FloatOptional left, float right) => !left.Equals(new FloatOptional(right));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator ==(float left, FloatOptional right) => right.Equals(new FloatOptional(left));

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator !=(float left, FloatOptional right) => !right.Equals(new FloatOptional(left));

  // Comparison operators
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator >(FloatOptional left, FloatOptional right) => left.Value > right.Value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator <(FloatOptional left, FloatOptional right) => left.Value < right.Value;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator >=(FloatOptional left, FloatOptional right) => left > right || left == right;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool operator <=(FloatOptional left, FloatOptional right) => left < right || left == right;

  // Arithmetic operators
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static FloatOptional operator +(FloatOptional left, FloatOptional right) => new(left.Value + right.Value);

  // Implicit conversion from float
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static implicit operator FloatOptional(float value) => new(value);
}

/// <summary>
/// Extension methods for FloatOptional.
/// </summary>
internal static class FloatOptionalExtensions
{
  /// <summary>
  /// Returns the maximum of two FloatOptional values, treating undefined values specially.
  /// If one value is undefined, returns the other. If both are undefined, returns undefined.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static FloatOptional MaxOrDefined(FloatOptional left, FloatOptional right) =>
      new(Comparison.MaxOrDefined(left.Unwrap(), right.Unwrap()));

  /// <summary>
  /// Compares two FloatOptional values for approximate equality using a tolerance.
  /// Returns true if both values are undefined.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool InexactEquals(FloatOptional left, FloatOptional right) =>
      Comparison.InexactEquals(left.Unwrap(), right.Unwrap());
}
