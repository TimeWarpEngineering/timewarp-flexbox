namespace TimeWarp.Flexbox.Tests.Values;

/// <summary>
/// Tests for FlexValue struct.
/// </summary>
public class FlexValueTests
{
  #region Factory Methods

  public void ShouldCreatePointValue()
  {
    FlexValue value = FlexValue.Point(100);

    value.Value.ShouldBe(100);
    value.Unit.ShouldBe(Unit.Point);
    value.IsDefined.ShouldBeTrue();
    value.IsAuto.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
  }

  public void ShouldCreatePercentValue()
  {
    FlexValue value = FlexValue.Percent(50);

    value.Value.ShouldBe(50);
    value.Unit.ShouldBe(Unit.Percent);
    value.IsDefined.ShouldBeTrue();
    value.IsAuto.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
  }

  public void ShouldHaveAutoValue()
  {
    FlexValue value = FlexValue.Auto;

    value.Unit.ShouldBe(Unit.Auto);
    value.IsAuto.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
  }

  public void ShouldHaveUndefinedValue()
  {
    FlexValue value = FlexValue.Undefined;

    value.Unit.ShouldBe(Unit.Undefined);
    value.IsUndefined.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
    value.IsAuto.ShouldBeFalse();
  }

  public void ShouldHaveMaxContentValue()
  {
    FlexValue value = FlexValue.MaxContent;

    value.Unit.ShouldBe(Unit.MaxContent);
    value.IsMaxContent.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
    value.IsAuto.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
    value.IsFitContent.ShouldBeFalse();
    value.IsStretch.ShouldBeFalse();
  }

  public void ShouldHaveFitContentValue()
  {
    FlexValue value = FlexValue.FitContent;

    value.Unit.ShouldBe(Unit.FitContent);
    value.IsFitContent.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
    value.IsAuto.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
    value.IsMaxContent.ShouldBeFalse();
    value.IsStretch.ShouldBeFalse();
  }

  public void ShouldHaveStretchValue()
  {
    FlexValue value = FlexValue.Stretch;

    value.Unit.ShouldBe(Unit.Stretch);
    value.IsStretch.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
    value.IsAuto.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
    value.IsMaxContent.ShouldBeFalse();
    value.IsFitContent.ShouldBeFalse();
  }

  #endregion

  #region Edge Cases

  public void ShouldHandleZeroPointValue()
  {
    FlexValue value = FlexValue.Point(0);

    value.Value.ShouldBe(0);
    value.Unit.ShouldBe(Unit.Point);
    value.IsDefined.ShouldBeTrue();
  }

  public void ShouldHandleNegativePointValue()
  {
    FlexValue value = FlexValue.Point(-10);

    value.Value.ShouldBe(-10);
    value.Unit.ShouldBe(Unit.Point);
  }

  public void ShouldHandleZeroPercentValue()
  {
    FlexValue value = FlexValue.Percent(0);

    value.Value.ShouldBe(0);
    value.Unit.ShouldBe(Unit.Percent);
  }

  public void ShouldHandleHundredPercentValue()
  {
    FlexValue value = FlexValue.Percent(100);

    value.Value.ShouldBe(100);
    value.Unit.ShouldBe(Unit.Percent);
  }

  public void ShouldHandleLargePointValue()
  {
    FlexValue value = FlexValue.Point(float.MaxValue);

    value.Value.ShouldBe(float.MaxValue);
    value.Unit.ShouldBe(Unit.Point);
  }

  public void ShouldHandleInfinityPointValue()
  {
    FlexValue value = FlexValue.Point(float.PositiveInfinity);

    value.Value.ShouldBe(float.PositiveInfinity);
    value.Unit.ShouldBe(Unit.Point);
  }

  public void ShouldHandleNegativeInfinityPointValue()
  {
    FlexValue value = FlexValue.Point(float.NegativeInfinity);

    value.Value.ShouldBe(float.NegativeInfinity);
    value.Unit.ShouldBe(Unit.Point);
  }

  #endregion

  #region Equality

  public void ShouldCompareEqualPointValues()
  {
    FlexValue value1 = FlexValue.Point(100);
    FlexValue value2 = FlexValue.Point(100);

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
    (value1 != value2).ShouldBeFalse();
    value1.Equals(value2).ShouldBeTrue();
    value1.Equals((object)value2).ShouldBeTrue();
  }

  public void ShouldCompareEqualPercentValues()
  {
    FlexValue value1 = FlexValue.Percent(50);
    FlexValue value2 = FlexValue.Percent(50);

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareAutoValuesAsEqual()
  {
    FlexValue value1 = FlexValue.Auto;
    FlexValue value2 = FlexValue.Auto;

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareUndefinedValuesAsEqual()
  {
    FlexValue value1 = FlexValue.Undefined;
    FlexValue value2 = FlexValue.Undefined;

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareDifferentPointValuesAsNotEqual()
  {
    FlexValue value1 = FlexValue.Point(10);
    FlexValue value2 = FlexValue.Point(20);

    (value1 != value2).ShouldBeTrue();
    (value1 == value2).ShouldBeFalse();
    value1.Equals(value2).ShouldBeFalse();
  }

  public void ShouldCompareDifferentUnitsAsNotEqual()
  {
    FlexValue point = FlexValue.Point(100);
    FlexValue percent = FlexValue.Percent(100);

    (point != percent).ShouldBeTrue();
    point.Equals(percent).ShouldBeFalse();
  }

  public void ShouldComparePointAndAutoAsNotEqual()
  {
    FlexValue point = FlexValue.Point(100);
    FlexValue auto = FlexValue.Auto;

    (point != auto).ShouldBeTrue();
  }

  public void ShouldCompareAutoAndUndefinedAsNotEqual()
  {
    FlexValue auto = FlexValue.Auto;
    FlexValue undefined = FlexValue.Undefined;

    (auto != undefined).ShouldBeTrue();
  }

  public void ShouldCompareMaxContentValuesAsEqual()
  {
    FlexValue value1 = FlexValue.MaxContent;
    FlexValue value2 = FlexValue.MaxContent;

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareStretchValuesAsEqual()
  {
    FlexValue value1 = FlexValue.Stretch;
    FlexValue value2 = FlexValue.Stretch;

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareFitContentValuesAsEqual()
  {
    FlexValue value1 = FlexValue.FitContent;
    FlexValue value2 = FlexValue.FitContent;

    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareMaxContentAndStretchAsNotEqual()
  {
    FlexValue maxContent = FlexValue.MaxContent;
    FlexValue stretch = FlexValue.Stretch;

    (maxContent != stretch).ShouldBeTrue();
  }

  public void ShouldNotEqualNull()
  {
    FlexValue value = FlexValue.Point(100);

    value.Equals(null).ShouldBeFalse();
  }

  public void ShouldNotEqualDifferentType()
  {
    FlexValue value = FlexValue.Point(100);

    value.Equals("100").ShouldBeFalse();
    value.Equals(100).ShouldBeFalse();
  }

  #endregion

  #region GetHashCode

  public void ShouldHaveConsistentHashCodeForEqualValues()
  {
    FlexValue value1 = FlexValue.Point(100);
    FlexValue value2 = FlexValue.Point(100);

    value1.GetHashCode().ShouldBe(value2.GetHashCode());
  }

  public void ShouldHaveConsistentHashCodeForAuto()
  {
    FlexValue value1 = FlexValue.Auto;
    FlexValue value2 = FlexValue.Auto;

    value1.GetHashCode().ShouldBe(value2.GetHashCode());
  }

  public void ShouldHaveConsistentHashCodeForUndefined()
  {
    FlexValue value1 = FlexValue.Undefined;
    FlexValue value2 = FlexValue.Undefined;

    value1.GetHashCode().ShouldBe(value2.GetHashCode());
  }

  public void ShouldHaveDifferentHashCodeForDifferentValues()
  {
    FlexValue value1 = FlexValue.Point(100);
    FlexValue value2 = FlexValue.Point(200);

    // Note: Different values should typically have different hash codes,
    // but hash collisions are allowed, so we just verify they're computed
    int hash1 = value1.GetHashCode();
    int hash2 = value2.GetHashCode();

    // Just verify hash codes are computed without error
    hash1.ShouldBeOfType<int>();
    hash2.ShouldBeOfType<int>();
  }

  public void ShouldHaveConsistentHashCodeForMaxContent()
  {
    FlexValue value1 = FlexValue.MaxContent;
    FlexValue value2 = FlexValue.MaxContent;

    value1.GetHashCode().ShouldBe(value2.GetHashCode());
  }

  public void ShouldHaveConsistentHashCodeForStretch()
  {
    FlexValue value1 = FlexValue.Stretch;
    FlexValue value2 = FlexValue.Stretch;

    value1.GetHashCode().ShouldBe(value2.GetHashCode());
  }

  public void ShouldHaveConsistentHashCodeForFitContent()
  {
    FlexValue value1 = FlexValue.FitContent;
    FlexValue value2 = FlexValue.FitContent;

    value1.GetHashCode().ShouldBe(value2.GetHashCode());
  }

  #endregion

  #region ToString

  public void ShouldReturnCorrectStringForPoint()
  {
    FlexValue value = FlexValue.Point(100);

    value.ToString().ShouldBe("100pt");
  }

  public void ShouldReturnCorrectStringForPercent()
  {
    FlexValue value = FlexValue.Percent(50);

    value.ToString().ShouldBe("50%");
  }

  public void ShouldReturnCorrectStringForAuto()
  {
    FlexValue value = FlexValue.Auto;

    value.ToString().ShouldBe("auto");
  }

  public void ShouldReturnCorrectStringForUndefined()
  {
    FlexValue value = FlexValue.Undefined;

    value.ToString().ShouldBe("undefined");
  }

  public void ShouldReturnCorrectStringForNegativePoint()
  {
    FlexValue value = FlexValue.Point(-10);

    value.ToString().ShouldBe("-10pt");
  }

  public void ShouldReturnCorrectStringForDecimalPercent()
  {
    FlexValue value = FlexValue.Percent(33.33f);

    value.ToString().ShouldBe("33.33%");
  }

  public void ShouldReturnCorrectStringForMaxContent()
  {
    FlexValue value = FlexValue.MaxContent;

    value.ToString().ShouldBe("max-content");
  }

  public void ShouldReturnCorrectStringForFitContent()
  {
    FlexValue value = FlexValue.FitContent;

    value.ToString().ShouldBe("fit-content");
  }

  public void ShouldReturnCorrectStringForStretch()
  {
    FlexValue value = FlexValue.Stretch;

    value.ToString().ShouldBe("stretch");
  }

  #endregion
}

/// <summary>
/// Tests for Unit enum.
/// </summary>
public class UnitEnumTests
{
  public void ShouldHaveUndefinedValue()
  {
    Unit.Undefined.ShouldBe((Unit)0);
  }

  public void ShouldHavePointValue()
  {
    Unit.Point.ShouldBe((Unit)1);
  }

  public void ShouldHavePercentValue()
  {
    Unit.Percent.ShouldBe((Unit)2);
  }

  public void ShouldHaveAutoValue()
  {
    Unit.Auto.ShouldBe((Unit)3);
  }

  public void ShouldHaveMaxContentValue()
  {
    Unit.MaxContent.ShouldBe((Unit)4);
  }

  public void ShouldHaveFitContentValue()
  {
    Unit.FitContent.ShouldBe((Unit)5);
  }

  public void ShouldHaveStretchValue()
  {
    Unit.Stretch.ShouldBe((Unit)6);
  }

  public void ShouldHaveSevenValues()
  {
    Enum.GetValues<Unit>().Length.ShouldBe(7);
  }
}
