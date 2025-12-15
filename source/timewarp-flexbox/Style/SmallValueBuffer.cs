/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/style/SmallValueBuffer.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

using System.Collections;

/// <summary>
/// Container for storing 32 or 64 bit integer values, whose index may never change.
/// Values are first stored in a fixed buffer of <typeparamref name="TBufferSize"/> 32-bit chunks,
/// before falling back to heap allocation.
/// </summary>
/// <remarks>
/// Design Decision: C++ uses a template size parameter. In C#, we use a generic struct
/// with a buffer size constant defined at the type level. The default size of 4 matches
/// the typical C++ usage. For different sizes, create a derived type or use the parameterized constructor.
///
/// Implementation Note: This is a class (not struct) because:
/// 1. It has mutable state with overflow allocation
/// 2. Copy semantics need to be explicit (like C++)
/// 3. It can grow beyond the inline buffer
///
/// Question: Should we pool the Overflow allocations? For now, following C++ which uses unique_ptr.
/// </remarks>
public class SmallValueBuffer<TBufferSize> where TBufferSize : IBufferSize
{
    private ushort _count;
    private readonly uint[] _buffer;
    private readonly BitArray _wideElements;
    private Overflow? _overflow;

    /// <summary>
    /// Initializes a new empty SmallValueBuffer.
    /// </summary>
    public SmallValueBuffer()
    {
        int bufferSize = TBufferSize.Size;
        _buffer = new uint[bufferSize];
        _wideElements = new BitArray(bufferSize);
        _count = 0;
        _overflow = null;
    }

    /// <summary>
    /// Copy constructor - creates a deep copy of another buffer.
    /// </summary>
    public SmallValueBuffer(SmallValueBuffer<TBufferSize> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        int bufferSize = TBufferSize.Size;
        _count = other._count;
        _buffer = new uint[bufferSize];
        Array.Copy(other._buffer, _buffer, bufferSize);
        _wideElements = new BitArray(other._wideElements);
        _overflow = other._overflow is not null ? new Overflow(other._overflow) : null;
    }

    /// <summary>
    /// Add a new 32-bit element to the buffer, returning the index of the element.
    /// </summary>
    /// <param name="value">The 32-bit value to store.</param>
    /// <returns>The index where the value was stored.</returns>
    /// <exception cref="InvalidOperationException">If the buffer exceeds 4096 chunks.</exception>
    public ushort Push(uint value)
    {
        ushort index = _count++;

        if (index >= 4096)
        {
            throw new InvalidOperationException("SmallValueBuffer can only hold up to 4096 chunks");
        }

        if (index < _buffer.Length)
        {
            _buffer[index] = value;
            return index;
        }

        _overflow ??= new Overflow();
        _overflow.Buffer.Add(value);
        _overflow.WideElements.Add(false);
        return index;
    }

    /// <summary>
    /// Add a new 64-bit element to the buffer, returning the index of the element.
    /// The 64-bit value is stored as two consecutive 32-bit chunks.
    /// </summary>
    /// <param name="value">The 64-bit value to store.</param>
    /// <returns>The index where the value was stored (the LSB index).</returns>
    public ushort Push(ulong value)
    {
        uint lsb = (uint)(value & 0xFFFFFFFF);
        uint msb = (uint)(value >> 32);

        ushort lsbIndex = Push(lsb);
        ushort msbIndex = Push(msb);

        if (msbIndex >= 4096)
        {
            throw new InvalidOperationException("SmallValueBuffer can only hold up to 4096 chunks");
        }

        if (lsbIndex < _buffer.Length)
        {
            _wideElements[lsbIndex] = true;
        }
        else
        {
            _overflow!.WideElements[lsbIndex - _buffer.Length] = true;
        }

        return lsbIndex;
    }

    /// <summary>
    /// Replace an existing 32-bit element in the buffer with a new value.
    /// </summary>
    /// <param name="index">The index to replace.</param>
    /// <param name="value">The new 32-bit value.</param>
    /// <returns>The index (unchanged for 32-bit replacement).</returns>
    public ushort Replace(ushort index, uint value)
    {
        if (index < _buffer.Length)
        {
            _buffer[index] = value;
        }
        else
        {
            int overflowIndex = index - _buffer.Length;
            if (_overflow is null || overflowIndex >= _overflow.Buffer.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _overflow.Buffer[overflowIndex] = value;
        }

        return index;
    }

    /// <summary>
    /// Replace an existing element with a 64-bit value.
    /// If the element was originally 64-bit, it's replaced in-place.
    /// If it was 32-bit, a new 64-bit slot is allocated.
    /// </summary>
    /// <param name="index">The index to replace.</param>
    /// <param name="value">The new 64-bit value.</param>
    /// <returns>The index where the value is stored (may be different if widening).</returns>
    public ushort Replace(ushort index, ulong value)
    {
        bool isWide = index < _wideElements.Length
            ? _wideElements[index]
            : _overflow!.WideElements[index - _buffer.Length];

        if (isWide)
        {
            uint lsb = (uint)(value & 0xFFFFFFFF);
            uint msb = (uint)(value >> 32);

            Replace(index, lsb);
            Replace((ushort)(index + 1), msb);
            return index;
        }
        else
        {
            return Push(value);
        }
    }

    /// <summary>
    /// Get a 32-bit value from the buffer.
    /// </summary>
    /// <param name="index">The index to retrieve.</param>
    /// <returns>The 32-bit value at the index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the index is out of range.</exception>
    public uint Get32(ushort index)
    {
        if (index < _buffer.Length)
        {
            return _buffer[index];
        }

        int overflowIndex = index - _buffer.Length;
        if (_overflow is null || overflowIndex >= _overflow.Buffer.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _overflow.Buffer[overflowIndex];
    }

    /// <summary>
    /// Get a 64-bit value from the buffer.
    /// </summary>
    /// <param name="index">The index of the LSB (as returned by Push(ulong)).</param>
    /// <returns>The 64-bit value.</returns>
    public ulong Get64(ushort index)
    {
        uint lsb = Get32(index);
        uint msb = Get32((ushort)(index + 1));
        return ((ulong)msb << 32) | lsb;
    }

    /// <summary>
    /// Internal overflow storage for values beyond the inline buffer.
    /// </summary>
    private sealed class Overflow
    {
        public List<uint> Buffer { get; } = [];
        public List<bool> WideElements { get; } = [];

        public Overflow() { }

        public Overflow(Overflow other)
        {
            Buffer = [.. other.Buffer];
            WideElements = [.. other.WideElements];
        }
    }
}

/// <summary>
/// Interface for defining buffer sizes at compile time.
/// </summary>
public interface IBufferSize
{
    /// <summary>
    /// The number of 32-bit chunks in the inline buffer.
    /// </summary>
    static abstract int Size { get; }
}

/// <summary>
/// Default buffer size of 4 (matches C++ typical usage).
/// Note: This is a marker type used only as a generic argument, not for instantiation.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1815:Override equals and operator equals on value types", Justification = "Marker type only used as generic argument")]
public readonly struct BufferSize4 : IBufferSize
{
    /// <inheritdoc />
    public static int Size => 4;
}

/// <summary>
/// Buffer size of 8.
/// Note: This is a marker type used only as a generic argument, not for instantiation.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1815:Override equals and operator equals on value types", Justification = "Marker type only used as generic argument")]
public readonly struct BufferSize8 : IBufferSize
{
    /// <inheritdoc />
    public static int Size => 8;
}

/// <summary>
/// Buffer size of 16.
/// Note: This is a marker type used only as a generic argument, not for instantiation.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1815:Override equals and operator equals on value types", Justification = "Marker type only used as generic argument")]
public readonly struct BufferSize16 : IBufferSize
{
    /// <inheritdoc />
    public static int Size => 16;
}

/// <summary>
/// Convenience alias for SmallValueBuffer with default size of 4.
/// </summary>
public class SmallValueBuffer : SmallValueBuffer<BufferSize4>
{
    /// <summary>
    /// Initializes a new empty SmallValueBuffer with default size.
    /// </summary>
    public SmallValueBuffer() : base() { }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    public SmallValueBuffer(SmallValueBuffer other) : base(other) { }
}
