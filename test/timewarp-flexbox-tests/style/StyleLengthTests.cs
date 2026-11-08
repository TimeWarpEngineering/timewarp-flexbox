/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for StyleLength and StyleSizeLength types.
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Style;

/// <summary>
/// Tests for StyleLength.
/// </summary>
public class StyleLengthTests
{
  #region Factory Methods

  public void PointsCreatesPointValue()
  {
    StyleLength length = StyleLength.Points(100f);
    length.IsPoints.ShouldBeTrue();
    length.Value.Unwrap().ShouldBe(100f);
  }

  public void PointsWithNaNReturnsUndefined()
  {
    StyleLength length = StyleLength.Points(float.NaN);
    length.IsUndefined.ShouldBeTrue();
  }

  public void PointsWithInfinityReturnsUndefined()
  {
    StyleLength length = StyleLength.Points(float.PositiveInfinity);
    length.IsUndefined.ShouldBeTrue();
  }

  public void PercentCreatesPercentValue()
  {
    StyleLength length = StyleLength.Percent(50f);
    length.IsPercent.ShouldBeTrue();
    length.Value.Unwrap().ShouldBe(50f);
  }

  public void PercentWithNaNReturnsUndefined()
  {
    StyleLength length = StyleLength.Percent(float.NaN);
    length.IsUndefined.ShouldBeTrue();
  }

  public void AutoCreatesAutoValue()
  {
    StyleLength length = StyleLength.Auto;
    length.IsAuto.ShouldBeTrue();
  }

  public void UndefinedCreatesUndefinedValue()
  {
    StyleLength length = StyleLength.Undefined;
    length.IsUndefined.ShouldBeTrue();
  }

  #endregion

  #region Properties

  public void IsDefinedReturnsTrueForDefinedValues()
  {
    StyleLength.Points(10f).IsDefined.ShouldBeTrue();
    StyleLength.Percent(50f).IsDefined.ShouldBeTrue();
    StyleLength.Auto.IsDefined.ShouldBeTrue();
  }

  public void IsDefinedReturnsFalseForUndefined()
  {
    StyleLength.Undefined.IsDefined.ShouldBeFalse();
  }

  #endregion

  #region Resolve

  public void ResolvePointsReturnsValue()
  {
    StyleLength length = StyleLength.Points(100f);
    FloatOptional result = length.Resolve(200f);
    result.Unwrap().ShouldBe(100f);
  }

  public void ResolvePercentReturnsCalculatedValue()
  {
    StyleLength length = StyleLength.Percent(50f);
    FloatOptional result = length.Resolve(200f);
    result.Unwrap().ShouldBe(100f); // 50% of 200 = 100
  }

  public void ResolveAutoReturnsUndefined()
  {
    StyleLength length = StyleLength.Auto;
    FloatOptional result = length.Resolve(200f);
    result.IsUndefined.ShouldBeTrue();
  }

  public void ResolveUndefinedReturnsUndefined()
  {
    StyleLength length = StyleLength.Undefined;
    FloatOptional result = length.Resolve(200f);
    result.IsUndefined.ShouldBeTrue();
  }

  #endregion

  #region Equality

  public void EqualPointValuesAreEqual()
  {
    StyleLength a = StyleLength.Points(100f);
    StyleLength b = StyleLength.Points(100f);
    (a == b).ShouldBeTrue();
  }

  public void DifferentPointValuesAreNotEqual()
  {
    StyleLength a = StyleLength.Points(100f);
    StyleLength b = StyleLength.Points(200f);
    (a != b).ShouldBeTrue();
  }

  public void SameValueDifferentUnitsAreNotEqual()
  {
    StyleLength a = StyleLength.Points(100f);
    StyleLength b = StyleLength.Percent(100f);
    (a != b).ShouldBeTrue();
  }

  public void InexactEqualsWithinTolerance()
  {
    StyleLength a = StyleLength.Points(100f);
    StyleLength b = StyleLength.Points(100.00005f);
    a.InexactEquals(b).ShouldBeTrue();
  }

  public void InexactEqualsOutsideTolerance()
  {
    StyleLength a = StyleLength.Points(100f);
    StyleLength b = StyleLength.Points(100.001f);
    a.InexactEquals(b).ShouldBeFalse();
  }

  #endregion

  #region Conversion

  public void ToYGValueConvertsCorrectly()
  {
    StyleLength length = StyleLength.Points(100f);
    YGValue value = length.ToYGValue();
    value.Value.ShouldBe(100f);
    value.Unit.ShouldBe(Unit.Point);
  }

  public void ExplicitCastToYGValueWorks()
  {
    StyleLength length = StyleLength.Percent(50f);
    YGValue value = (YGValue)length;
    value.Value.ShouldBe(50f);
    value.Unit.ShouldBe(Unit.Percent);
  }

  #endregion

  #region ToString

  public void ToStringPointsReturnsCorrectFormat()
  {
    StyleLength.Points(100f).ToString().ShouldBe("100pt");
  }

  public void ToStringPercentReturnsCorrectFormat()
  {
    StyleLength.Percent(50f).ToString().ShouldBe("50%");
  }

  public void ToStringAutoReturnsAuto()
  {
    StyleLength.Auto.ToString().ShouldBe("auto");
  }

  public void ToStringUndefinedReturnsUndefined()
  {
    StyleLength.Undefined.ToString().ShouldBe("undefined");
  }

  #endregion
}

/// <summary>
/// Tests for StyleSizeLength.
/// </summary>
public class StyleSizeLengthTests
{
  #region Factory Methods

  public void PointsCreatesPointValue()
  {
    StyleSizeLength length = StyleSizeLength.Points(100f);
    length.IsPoints.ShouldBeTrue();
    length.Value.Unwrap().ShouldBe(100f);
  }

  public void PointsWithNaNReturnsUndefined()
  {
    StyleSizeLength length = StyleSizeLength.Points(float.NaN);
    length.IsUndefined.ShouldBeTrue();
  }

  public void PercentCreatesPercentValue()
  {
    StyleSizeLength length = StyleSizeLength.Percent(50f);
    length.IsPercent.ShouldBeTrue();
    length.Value.Unwrap().ShouldBe(50f);
  }

  public void AutoCreatesAutoValue()
  {
    StyleSizeLength length = StyleSizeLength.Auto;
    length.IsAuto.ShouldBeTrue();
  }

  public void MaxContentCreatesMaxContentValue()
  {
    StyleSizeLength length = StyleSizeLength.MaxContent;
    length.IsMaxContent.ShouldBeTrue();
  }

  public void FitContentCreatesFitContentValue()
  {
    StyleSizeLength length = StyleSizeLength.FitContent;
    length.IsFitContent.ShouldBeTrue();
  }

  public void StretchCreatesStretchValue()
  {
    StyleSizeLength length = StyleSizeLength.Stretch;
    length.IsStretch.ShouldBeTrue();
  }

  public void UndefinedCreatesUndefinedValue()
  {
    StyleSizeLength length = StyleSizeLength.Undefined;
    length.IsUndefined.ShouldBeTrue();
  }

  #endregion

  #region Resolve

  public void ResolvePointsReturnsValue()
  {
    StyleSizeLength length = StyleSizeLength.Points(100f);
    FloatOptional result = length.Resolve(200f);
    result.Unwrap().ShouldBe(100f);
  }

  public void ResolvePercentReturnsCalculatedValue()
  {
    StyleSizeLength length = StyleSizeLength.Percent(25f);
    FloatOptional result = length.Resolve(400f);
    result.Unwrap().ShouldBe(100f); // 25% of 400 = 100
  }

  public void ResolveMaxContentReturnsUndefined()
  {
    StyleSizeLength length = StyleSizeLength.MaxContent;
    FloatOptional result = length.Resolve(200f);
    result.IsUndefined.ShouldBeTrue();
  }

  public void ResolveFitContentReturnsUndefined()
  {
    StyleSizeLength length = StyleSizeLength.FitContent;
    FloatOptional result = length.Resolve(200f);
    result.IsUndefined.ShouldBeTrue();
  }

  public void ResolveStretchReturnsUndefined()
  {
    StyleSizeLength length = StyleSizeLength.Stretch;
    FloatOptional result = length.Resolve(200f);
    result.IsUndefined.ShouldBeTrue();
  }

  #endregion

  #region Equality

  public void EqualPointValuesAreEqual()
  {
    StyleSizeLength a = StyleSizeLength.Points(100f);
    StyleSizeLength b = StyleSizeLength.Points(100f);
    (a == b).ShouldBeTrue();
  }

  public void InexactEqualsWithinTolerance()
  {
    StyleSizeLength a = StyleSizeLength.Points(100f);
    StyleSizeLength b = StyleSizeLength.Points(100.00005f);
    a.InexactEquals(b).ShouldBeTrue();
  }

  #endregion

  #region ToString

  public void ToStringMaxContentReturnsCorrectFormat()
  {
    StyleSizeLength.MaxContent.ToString().ShouldBe("max-content");
  }

  public void ToStringFitContentReturnsCorrectFormat()
  {
    StyleSizeLength.FitContent.ToString().ShouldBe("fit-content");
  }

  public void ToStringStretchReturnsCorrectFormat()
  {
    StyleSizeLength.Stretch.ToString().ShouldBe("stretch");
  }

  #endregion
}
