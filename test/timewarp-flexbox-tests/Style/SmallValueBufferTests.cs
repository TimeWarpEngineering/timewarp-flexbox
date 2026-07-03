/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: tests/SmallValueBufferTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Style;

/// <summary>
/// Tests for SmallValueBuffer.
/// Ported from C++ tests/SmallValueBufferTest.cpp
/// </summary>
public class SmallValueBufferTests
{
  /// <summary>
  /// Test copy assignment with overflow values.
  /// Equivalent to C++ TEST(SmallValueBuffer, copy_assignment_with_overflow)
  /// </summary>
  public void CopyAssignmentWithOverflow()
  {
    const int bufferSize = 4;
    ushort[] handles = new ushort[bufferSize + 1];

    SmallValueBuffer<BufferSize4> buffer1 = new();
    for (int i = 0; i < bufferSize + 1; i++)
    {
      handles[i] = buffer1.Push((uint)i);
    }

    SmallValueBuffer<BufferSize4> buffer2 = new(buffer1);
    for (int i = 0; i < bufferSize + 1; i++)
    {
      buffer2.Get32(handles[i]).ShouldBe((uint)i);
    }

    ushort handle = buffer1.Push(42u);
    buffer1.Get32(handle).ShouldBe(42u);

    // Buffer2 should not have the new value (it's a copy, not a reference)
    Should.Throw<ArgumentOutOfRangeException>(() => buffer2.Get32(handle));
  }

  /// <summary>
  /// Test pushing a 32-bit value.
  /// Equivalent to C++ TEST(SmallValueBuffer, push_32)
  /// </summary>
  public void Push32()
  {
    uint magic = 88567114u;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle = buffer.Push(magic);
    buffer.Get32(handle).ShouldBe(magic);
  }

  /// <summary>
  /// Test pushing values that overflow the inline buffer.
  /// Equivalent to C++ TEST(SmallValueBuffer, push_overflow)
  /// </summary>
  public void PushOverflow()
  {
    uint magic1 = 88567114u;
    uint magic2 = 351012214u;
    uint magic3 = 146122128u;
    uint magic4 = 2171092154u;
    uint magic5 = 2269016953u;

    SmallValueBuffer<BufferSize4> buffer = new();
    buffer.Get32(buffer.Push(magic1)).ShouldBe(magic1);
    buffer.Get32(buffer.Push(magic2)).ShouldBe(magic2);
    buffer.Get32(buffer.Push(magic3)).ShouldBe(magic3);
    buffer.Get32(buffer.Push(magic4)).ShouldBe(magic4);
    buffer.Get32(buffer.Push(magic5)).ShouldBe(magic5);
  }

  /// <summary>
  /// Test pushing a 64-bit value.
  /// Equivalent to C++ TEST(SmallValueBuffer, push_64)
  /// </summary>
  public void Push64()
  {
    ulong magic = 118138934255546108uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle = buffer.Push(magic);
    buffer.Get64(handle).ShouldBe(magic);
  }

  /// <summary>
  /// Test pushing 64-bit values that overflow the inline buffer.
  /// Equivalent to C++ TEST(SmallValueBuffer, push_64_overflow)
  /// </summary>
  public void Push64Overflow()
  {
    ulong magic1 = 1401612388342512uL;
    ulong magic2 = 118712305386210uL;
    ulong magic3 = 752431801563359011uL;
    ulong magic4 = 118138934255546108uL;
    ulong magic5 = 237115443124116111uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    buffer.Get64(buffer.Push(magic1)).ShouldBe(magic1);
    buffer.Get64(buffer.Push(magic2)).ShouldBe(magic2);
    buffer.Get64(buffer.Push(magic3)).ShouldBe(magic3);
    buffer.Get64(buffer.Push(magic4)).ShouldBe(magic4);
    buffer.Get64(buffer.Push(magic5)).ShouldBe(magic5);
  }

  /// <summary>
  /// Test pushing 64-bit after 32-bit.
  /// Equivalent to C++ TEST(SmallValueBuffer, push_64_after_32)
  /// </summary>
  public void Push64After32()
  {
    uint magic32 = 88567114u;
    ulong magic64 = 118712305386210uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle32 = buffer.Push(magic32);
    buffer.Get32(handle32).ShouldBe(magic32);

    ushort handle64 = buffer.Push(magic64);
    buffer.Get64(handle64).ShouldBe(magic64);
  }

  /// <summary>
  /// Test pushing 32-bit after 64-bit.
  /// Equivalent to C++ TEST(SmallValueBuffer, push_32_after_64)
  /// </summary>
  public void Push32After64()
  {
    uint magic32 = 88567114u;
    ulong magic64 = 118712305386210uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle64 = buffer.Push(magic64);
    buffer.Get64(handle64).ShouldBe(magic64);

    ushort handle32 = buffer.Push(magic32);
    buffer.Get32(handle32).ShouldBe(magic32);
  }

  /// <summary>
  /// Test replacing 32-bit with 32-bit.
  /// Equivalent to C++ TEST(SmallValueBuffer, replace_32_with_32)
  /// </summary>
  public void Replace32With32()
  {
    uint magic1 = 88567114u;
    uint magic2 = 351012214u;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle = buffer.Push(magic1);

    buffer.Get32(buffer.Replace(handle, magic2)).ShouldBe(magic2);
  }

  /// <summary>
  /// Test replacing 32-bit with 64-bit.
  /// Equivalent to C++ TEST(SmallValueBuffer, replace_32_with_64)
  /// </summary>
  public void Replace32With64()
  {
    uint magic32 = 88567114u;
    ulong magic64 = 118712305386210uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle = buffer.Push(magic32);

    buffer.Get64(buffer.Replace(handle, magic64)).ShouldBe(magic64);
  }

  /// <summary>
  /// Test replacing 32-bit with 64-bit when it causes overflow.
  /// Equivalent to C++ TEST(SmallValueBuffer, replace_32_with_64_causes_overflow)
  /// </summary>
  public void Replace32With64CausesOverflow()
  {
    uint magic1 = 88567114u;
    uint magic2 = 351012214u;
    uint magic3 = 146122128u;
    uint magic4 = 2171092154u;

    ulong magic64 = 118712305386210uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle1 = buffer.Push(magic1);
    buffer.Push(magic2);
    buffer.Push(magic3);
    buffer.Push(magic4);

    buffer.Get64(buffer.Replace(handle1, magic64)).ShouldBe(magic64);
  }

  /// <summary>
  /// Test replacing 64-bit with 32-bit.
  /// Equivalent to C++ TEST(SmallValueBuffer, replace_64_with_32)
  /// </summary>
  public void Replace64With32()
  {
    uint magic32 = 88567114u;
    ulong magic64 = 118712305386210uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle = buffer.Push(magic64);

    buffer.Get32(buffer.Replace(handle, magic32)).ShouldBe(magic32);
  }

  /// <summary>
  /// Test replacing 64-bit with 64-bit.
  /// Equivalent to C++ TEST(SmallValueBuffer, replace_64_with_64)
  /// </summary>
  public void Replace64With64()
  {
    ulong magic1 = 1401612388342512uL;
    ulong magic2 = 118712305386210uL;

    SmallValueBuffer<BufferSize4> buffer = new();
    ushort handle = buffer.Push(magic1);

    buffer.Get64(buffer.Replace(handle, magic2)).ShouldBe(magic2);
  }
}
