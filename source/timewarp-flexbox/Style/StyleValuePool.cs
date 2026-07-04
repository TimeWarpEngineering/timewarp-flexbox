/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/style/StyleValuePool.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

/// <summary>
/// StyleValuePool allows compact storage for a sparse collection of assigned
/// lengths and numbers. Values are referred to using StyleValueHandle. In most
/// cases StyleValueHandle can embed the value directly, but if not, the value is
/// stored within a buffer provided by the pool. The pool contains a fixed number
/// of inline slots before falling back to heap allocating additional slots.
/// </summary>
internal sealed class StyleValuePool
{
  private readonly SmallValueBuffer<BufferSize4> Buffer = new();

  // Constants for inline integer packing
  private const ushort MaxInlineAbsValue = (1 << 11) - 1; // 2047

  #region Store Methods

  /// <summary>
  /// Stores a StyleLength value into the given handle.
  /// </summary>
  public void Store(ref StyleValueHandle handle, StyleLength length)
  {
    if (length.IsUndefined)
    {
      handle.SetType(StyleValueHandle.HandleType.Undefined);
    }
    else if (length.IsAuto)
    {
      handle.SetType(StyleValueHandle.HandleType.Auto);
    }
    else
    {
      StyleValueHandle.HandleType type = length.IsPoints
        ? StyleValueHandle.HandleType.Point
        : StyleValueHandle.HandleType.Percent;
      StoreValue(ref handle, length.Value.Unwrap(), type);
    }
  }

  /// <summary>
  /// Stores a StyleSizeLength value into the given handle.
  /// </summary>
  public void Store(ref StyleValueHandle handle, StyleSizeLength sizeValue)
  {
    if (sizeValue.IsUndefined)
    {
      handle.SetType(StyleValueHandle.HandleType.Undefined);
    }
    else if (sizeValue.IsAuto)
    {
      handle.SetType(StyleValueHandle.HandleType.Auto);
    }
    else if (sizeValue.IsMaxContent)
    {
      StoreKeyword(ref handle, StyleValueHandle.HandleKeyword.MaxContent);
    }
    else if (sizeValue.IsStretch)
    {
      StoreKeyword(ref handle, StyleValueHandle.HandleKeyword.Stretch);
    }
    else if (sizeValue.IsFitContent)
    {
      StoreKeyword(ref handle, StyleValueHandle.HandleKeyword.FitContent);
    }
    else
    {
      StyleValueHandle.HandleType type = sizeValue.IsPoints
        ? StyleValueHandle.HandleType.Point
        : StyleValueHandle.HandleType.Percent;
      StoreValue(ref handle, sizeValue.Value.Unwrap(), type);
    }
  }

  /// <summary>
  /// Stores a FloatOptional number value into the given handle.
  /// </summary>
  public void Store(ref StyleValueHandle handle, FloatOptional number)
  {
    if (number.IsUndefined)
    {
      handle.SetType(StyleValueHandle.HandleType.Undefined);
    }
    else
    {
      StoreValue(ref handle, number.Unwrap(), StyleValueHandle.HandleType.Number);
    }
  }

  #endregion

  #region Get Methods

  /// <summary>
  /// Gets a StyleLength value from the handle.
  /// </summary>
  public StyleLength GetLength(StyleValueHandle handle)
  {
    if (handle.IsUndefined)
    {
      return StyleLength.Undefined;
    }
    else if (handle.IsAuto)
    {
      return StyleLength.Auto;
    }
    else
    {
      Debug.Assert(
        handle.Type is StyleValueHandle.HandleType.Point or StyleValueHandle.HandleType.Percent,
        "Invalid handle type for GetLength");

      float value = handle.IsValueIndexed
        ? BitCastToFloat(Buffer.Get32(handle.Value))
        : UnpackInlineInteger(handle.Value);

      return handle.Type == StyleValueHandle.HandleType.Point
        ? StyleLength.Points(value)
        : StyleLength.Percent(value);
    }
  }

  /// <summary>
  /// Gets a StyleSizeLength value from the handle.
  /// </summary>
  public StyleSizeLength GetSize(StyleValueHandle handle)
  {
    if (handle.IsUndefined)
    {
      return StyleSizeLength.Undefined;
    }
    else if (handle.IsAuto)
    {
      return StyleSizeLength.Auto;
    }
    else if (handle.IsKeyword(StyleValueHandle.HandleKeyword.MaxContent))
    {
      return StyleSizeLength.MaxContent;
    }
    else if (handle.IsKeyword(StyleValueHandle.HandleKeyword.FitContent))
    {
      return StyleSizeLength.FitContent;
    }
    else if (handle.IsKeyword(StyleValueHandle.HandleKeyword.Stretch))
    {
      return StyleSizeLength.Stretch;
    }
    else
    {
      Debug.Assert(
        handle.Type is StyleValueHandle.HandleType.Point or StyleValueHandle.HandleType.Percent,
        "Invalid handle type for GetSize");

      float value = handle.IsValueIndexed
        ? BitCastToFloat(Buffer.Get32(handle.Value))
        : UnpackInlineInteger(handle.Value);

      return handle.Type == StyleValueHandle.HandleType.Point
        ? StyleSizeLength.Points(value)
        : StyleSizeLength.Percent(value);
    }
  }

  /// <summary>
  /// Gets a FloatOptional number from the handle.
  /// </summary>
  public FloatOptional GetNumber(StyleValueHandle handle)
  {
    if (handle.IsUndefined)
    {
      return FloatOptional.Undefined;
    }
    else
    {
      Debug.Assert(
        handle.Type == StyleValueHandle.HandleType.Number,
        "Invalid handle type for GetNumber");

      float value = handle.IsValueIndexed
        ? BitCastToFloat(Buffer.Get32(handle.Value))
        : UnpackInlineInteger(handle.Value);

      return new FloatOptional(value);
    }
  }

  #endregion

  #region Private Implementation

  private void StoreValue(ref StyleValueHandle handle, float value, StyleValueHandle.HandleType type)
  {
    handle.SetType(type);

    if (handle.IsValueIndexed)
    {
      // Already indexed - replace in buffer
      ushort newIndex = Buffer.Replace(handle.Value, BitCastToUInt(value));
      handle.SetValue(newIndex);
    }
    else if (IsIntegerPackable(value))
    {
      // Can pack inline
      handle.SetValue(PackInlineInteger(value));
    }
    else
    {
      // Need to store in buffer
      ushort newIndex = Buffer.Push(BitCastToUInt(value));
      handle.SetValue(newIndex);
      handle.SetValueIsIndexed();
    }
  }

  private void StoreKeyword(ref StyleValueHandle handle, StyleValueHandle.HandleKeyword keyword)
  {
    handle.SetType(StyleValueHandle.HandleType.Keyword);

    if (handle.IsValueIndexed)
    {
      // Already indexed - replace in buffer
      ushort newIndex = Buffer.Replace(handle.Value, (uint)keyword);
      handle.SetValue(newIndex);
    }
    else
    {
      // Store keyword value directly
      handle.SetValue((ushort)keyword);
    }
  }

  /// <summary>
  /// Checks if a float value can be packed as an inline integer.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static bool IsIntegerPackable(float f)
  {
    int i = (int)f;
    return i == f && i >= -MaxInlineAbsValue && i <= MaxInlineAbsValue;
  }

  /// <summary>
  /// Packs a float integer value into a 12-bit inline representation.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static ushort PackInlineInteger(float value)
  {
    ushort isNegative = value < 0 ? (ushort)1 : (ushort)0;
    int magnitude = (int)value * (isNegative != 0 ? -1 : 1);
    return (ushort)((isNegative << 11) | magnitude);
  }

  /// <summary>
  /// Unpacks a 12-bit inline integer representation to a float.
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float UnpackInlineInteger(ushort value)
  {
    const ushort ValueSignMask = 0b0000_1000_0000_0000;
    const ushort ValueMagnitudeMask = 0b0000_0111_1111_1111;
    bool isNegative = (value & ValueSignMask) != 0;
    int magnitude = value & ValueMagnitudeMask;
    return magnitude * (isNegative ? -1 : 1);
  }

  /// <summary>
  /// Bit-casts a float to uint (equivalent to C++ std::bit_cast).
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static uint BitCastToUInt(float value)
  {
    return BitConverter.SingleToUInt32Bits(value);
  }

  /// <summary>
  /// Bit-casts a uint to float (equivalent to C++ std::bit_cast).
  /// </summary>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static float BitCastToFloat(uint value)
  {
    return BitConverter.UInt32BitsToSingle(value);
  }

  #endregion
}
