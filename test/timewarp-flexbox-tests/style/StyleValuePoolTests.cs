/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ test: tests/StyleValuePoolTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests;

public class StyleValuePoolTests
{
  public void UndefinedAtInit()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    handle.IsUndefined.ShouldBeTrue();
    handle.IsDefined.ShouldBeFalse();
    pool.GetLength(handle).ShouldBe(StyleLength.Undefined);
    pool.GetNumber(handle).ShouldBe(FloatOptional.Undefined);
  }

  public void AutoAtInit()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = StyleValueHandle.Auto;

    handle.IsAuto.ShouldBeTrue();
    pool.GetLength(handle).ShouldBe(StyleLength.Auto);
  }

  public void StoreSmallIntPoints()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Points(10));

    pool.GetLength(handle).ShouldBe(StyleLength.Points(10));
  }

  public void StoreSmallNegativeIntPoints()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Points(-10));

    pool.GetLength(handle).ShouldBe(StyleLength.Points(-10));
  }

  public void StoreSmallIntPercent()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Percent(10));

    pool.GetLength(handle).ShouldBe(StyleLength.Percent(10));
  }

  public void StoreLargeIntPercent()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Percent(262144));

    pool.GetLength(handle).ShouldBe(StyleLength.Percent(262144));
  }

  public void StoreLargeIntAfterSmallInt()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Percent(10));
    pool.Store(ref handle, StyleLength.Percent(262144));

    pool.GetLength(handle).ShouldBe(StyleLength.Percent(262144));
  }

  public void StoreSmallIntAfterLargeInt()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Percent(262144));
    pool.Store(ref handle, StyleLength.Percent(10));

    pool.GetLength(handle).ShouldBe(StyleLength.Percent(10));
  }

  public void StoreSmallIntNumber()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, new FloatOptional(10.0f));

    pool.GetNumber(handle).ShouldBe(new FloatOptional(10.0f));
  }

  public void StoreUndefined()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Undefined);

    handle.IsUndefined.ShouldBeTrue();
    handle.IsDefined.ShouldBeFalse();
    pool.GetLength(handle).ShouldBe(StyleLength.Undefined);
  }

  public void StoreUndefinedAfterSmallInt()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Points(10));
    pool.Store(ref handle, StyleLength.Undefined);

    handle.IsUndefined.ShouldBeTrue();
    handle.IsDefined.ShouldBeFalse();
    pool.GetLength(handle).ShouldBe(StyleLength.Undefined);
  }

  public void StoreUndefinedAfterLargeInt()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleLength.Points(262144));
    pool.Store(ref handle, StyleLength.Undefined);

    handle.IsUndefined.ShouldBeTrue();
    handle.IsDefined.ShouldBeFalse();
    pool.GetLength(handle).ShouldBe(StyleLength.Undefined);
  }

  public void StoreKeywords()
  {
    StyleValuePool pool = new();
    StyleValueHandle handleMaxContent = new();
    StyleValueHandle handleFitContent = new();
    StyleValueHandle handleStretch = new();

    pool.Store(ref handleMaxContent, StyleSizeLength.MaxContent);
    pool.Store(ref handleFitContent, StyleSizeLength.FitContent);
    pool.Store(ref handleStretch, StyleSizeLength.Stretch);

    pool.GetSize(handleMaxContent).ShouldBe(StyleSizeLength.MaxContent);
    pool.GetSize(handleFitContent).ShouldBe(StyleSizeLength.FitContent);
    pool.GetSize(handleStretch).ShouldBe(StyleSizeLength.Stretch);
  }

  public void StoreAutoSize()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleSizeLength.Auto);

    pool.GetSize(handle).ShouldBe(StyleSizeLength.Auto);
  }

  public void StorePointsSize()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleSizeLength.Points(100));

    pool.GetSize(handle).ShouldBe(StyleSizeLength.Points(100));
  }

  public void StorePercentSize()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, StyleSizeLength.Percent(50));

    pool.GetSize(handle).ShouldBe(StyleSizeLength.Percent(50));
  }

  // Additional edge cases

  public void StoreMaxInlineValue()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    // Max inline value is 2047
    pool.Store(ref handle, StyleLength.Points(2047));

    pool.GetLength(handle).ShouldBe(StyleLength.Points(2047));
  }

  public void StoreMinInlineValue()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    // Min inline value is -2047
    pool.Store(ref handle, StyleLength.Points(-2047));

    pool.GetLength(handle).ShouldBe(StyleLength.Points(-2047));
  }

  public void StoreJustOverMaxInline()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    // 2048 is just over max inline (2047), so it goes to buffer
    pool.Store(ref handle, StyleLength.Points(2048));

    pool.GetLength(handle).ShouldBe(StyleLength.Points(2048));
  }

  public void StoreFloatWithDecimal()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    // Float with decimal - cannot be packed inline
    pool.Store(ref handle, StyleLength.Points(10.5f));

    pool.GetLength(handle).ShouldBe(StyleLength.Points(10.5f));
  }

  public void StoreUndefinedNumber()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle = new();

    pool.Store(ref handle, FloatOptional.Undefined);

    handle.IsUndefined.ShouldBeTrue();
    pool.GetNumber(handle).ShouldBe(FloatOptional.Undefined);
  }

  public void StoreMultipleValuesIndependent()
  {
    StyleValuePool pool = new();
    StyleValueHandle handle1 = new();
    StyleValueHandle handle2 = new();
    StyleValueHandle handle3 = new();

    pool.Store(ref handle1, StyleLength.Points(10));
    pool.Store(ref handle2, StyleLength.Percent(50));
    pool.Store(ref handle3, new FloatOptional(1.5f));

    pool.GetLength(handle1).ShouldBe(StyleLength.Points(10));
    pool.GetLength(handle2).ShouldBe(StyleLength.Percent(50));
    pool.GetNumber(handle3).ShouldBe(new FloatOptional(1.5f));
  }
}
