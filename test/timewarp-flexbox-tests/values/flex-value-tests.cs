namespace TimeWarp.Flexbox.Tests.Values;

/// <summary>
/// Tests for FlexValue struct.
/// </summary>
public class FlexValueTests
{
  public void ShouldCreatePointValue()
  {
    // Arrange & Act
    FlexValue value = FlexValue.Point(100);

    // Assert
    value.Value.ShouldBe(100);
    value.Unit.ShouldBe(Unit.Point);
    value.IsDefined.ShouldBeTrue();
    value.IsAuto.ShouldBeFalse();
    value.IsUndefined.ShouldBeFalse();
  }

  public void ShouldCreatePercentValue()
  {
    // Arrange & Act
    FlexValue value = FlexValue.Percent(50);

    // Assert
    value.Value.ShouldBe(50);
    value.Unit.ShouldBe(Unit.Percent);
    value.IsDefined.ShouldBeTrue();
  }

  public void ShouldHaveAutoValue()
  {
    // Arrange & Act
    FlexValue value = FlexValue.Auto;

    // Assert
    value.Unit.ShouldBe(Unit.Auto);
    value.IsAuto.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
  }

  public void ShouldHaveUndefinedValue()
  {
    // Arrange & Act
    FlexValue value = FlexValue.Undefined;

    // Assert
    value.Unit.ShouldBe(Unit.Undefined);
    value.IsUndefined.ShouldBeTrue();
    value.IsDefined.ShouldBeFalse();
  }

  public void ShouldCompareEqualValues()
  {
    // Arrange
    FlexValue value1 = FlexValue.Point(100);
    FlexValue value2 = FlexValue.Point(100);

    // Assert
    value1.ShouldBe(value2);
    (value1 == value2).ShouldBeTrue();
  }

  public void ShouldCompareAutoValuesAsEqual()
  {
    // Arrange
    FlexValue value1 = FlexValue.Auto;
    FlexValue value2 = FlexValue.Auto;

    // Assert
    value1.ShouldBe(value2);
  }
}
