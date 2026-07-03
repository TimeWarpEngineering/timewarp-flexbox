/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/style/StyleValueHandle.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// StyleValueHandle is a small (16-bit) handle to a length or number in a style.
/// The value may be embedded directly in the handle if simple, or the handle may
/// instead point to an index within a StyleValuePool.
/// </summary>
/// <remarks>
/// To read or write a value from a StyleValueHandle, use
/// StyleValuePool.Store(), and StyleValuePool.GetLength()/
/// StyleValuePool.GetNumber().
/// </remarks>
public struct StyleValueHandle : IEquatable<StyleValueHandle>
{
  private const ushort HandleTypeMask = 0b0000_0000_0000_0111;
  private const ushort HandleIndexedMask = 0b0000_0000_0000_1000;
  private const ushort HandleValueMask = 0b1111_1111_1111_0000;

  private ushort _repr;

  /// <summary>
  /// The type of value stored in this handle.
  /// </summary>
  internal enum HandleType : byte
  {
    Undefined = 0,
    Point = 1,
    Percent = 2,
    Number = 3,
    Auto = 4,
    Keyword = 5
  }

  /// <summary>
  /// Special keyword values (not auto, which has its own type).
  /// </summary>
  internal enum HandleKeyword : byte
  {
    MaxContent = 0,
    FitContent = 1,
    Stretch = 2
  }

  #region Factory Methods

  /// <summary>
  /// Creates an auto handle.
  /// </summary>
  public static StyleValueHandle Auto
  {
    get
    {
      StyleValueHandle handle = new();
      handle.SetType(HandleType.Auto);
      return handle;
    }
  }

  /// <summary>
  /// Creates an undefined handle.
  /// </summary>
  public static StyleValueHandle Undefined => new();

  #endregion

  #region Properties

  /// <summary>
  /// Gets whether this handle represents an undefined value.
  /// </summary>
  public bool IsUndefined => Type == HandleType.Undefined;

  /// <summary>
  /// Gets whether this handle represents a defined value.
  /// </summary>
  public bool IsDefined => !IsUndefined;

  /// <summary>
  /// Gets whether this handle represents an auto value.
  /// </summary>
  public bool IsAuto => Type == HandleType.Auto;

  /// <summary>
  /// Gets whether this handle's value is indexed in a pool.
  /// </summary>
  internal readonly bool IsValueIndexed => (_repr & HandleIndexedMask) != 0;

  /// <summary>
  /// Gets the type of this handle.
  /// </summary>
  internal readonly HandleType Type => (HandleType)(_repr & HandleTypeMask);

  /// <summary>
  /// Gets the raw value portion (top 12 bits).
  /// </summary>
  internal readonly ushort Value => (ushort)(_repr >> 4);

  #endregion

  #region Internal Methods

  /// <summary>
  /// Sets the type of this handle.
  /// </summary>
  internal void SetType(HandleType type)
  {
    _repr = (ushort)((_repr & ~HandleTypeMask) | (byte)type);
  }

  /// <summary>
  /// Sets the value portion.
  /// </summary>
  internal void SetValue(ushort value)
  {
    _repr = (ushort)((_repr & ~HandleValueMask) | (value << 4));
  }

  /// <summary>
  /// Marks this handle's value as indexed in a pool.
  /// </summary>
  internal void SetValueIsIndexed()
  {
    _repr |= HandleIndexedMask;
  }

  /// <summary>
  /// Checks if this handle is a specific keyword.
  /// </summary>
  internal bool IsKeyword(HandleKeyword keyword)
  {
    return Type == HandleType.Keyword && Value == (ushort)keyword;
  }

  #endregion

  #region Equality

  /// <inheritdoc />
  public readonly bool Equals(StyleValueHandle other) => _repr == other._repr;

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is StyleValueHandle other && Equals(other);

  /// <inheritdoc />
  public override readonly int GetHashCode() => _repr.GetHashCode();

  /// <summary>
  /// Equality operator.
  /// </summary>
  public static bool operator ==(StyleValueHandle left, StyleValueHandle right) => left.Equals(right);

  /// <summary>
  /// Inequality operator.
  /// </summary>
  public static bool operator !=(StyleValueHandle left, StyleValueHandle right) => !left.Equals(right);

  #endregion
}
