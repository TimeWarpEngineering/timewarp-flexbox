/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ test: tests/YGValueTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests;

/// <summary>
/// Tests for YGValue ported from C++ YGValueTest.cpp
/// </summary>
public class YGValueTests
{
    // Port of TEST(YGValue, supports_equality)
    // Tests equality comparison for various YGValue combinations.

    public void SupportsEqualitySamePercentValuesAreEqual()
    {
        new YGValue(12.5f, Unit.Percent).ShouldBe(new YGValue(12.5f, Unit.Percent));
    }

    public void SupportsEqualityDifferentPercentValuesAreNotEqual()
    {
        new YGValue(12.5f, Unit.Percent).ShouldNotBe(new YGValue(56.7f, Unit.Percent));
    }

    public void SupportsEqualitySameValueDifferentUnitsAreNotEqual()
    {
        new YGValue(12.5f, Unit.Percent).ShouldNotBe(new YGValue(12.5f, Unit.Point));
    }

    public void SupportsEqualityPercentVsAutoNotEqual()
    {
        new YGValue(12.5f, Unit.Percent).ShouldNotBe(new YGValue(12.5f, Unit.Auto));
    }

    public void SupportsEqualityPercentVsUndefinedNotEqual()
    {
        new YGValue(12.5f, Unit.Percent).ShouldNotBe(new YGValue(12.5f, Unit.Undefined));
    }

    public void SupportsEqualityUndefinedValuesEqualRegardlessOfFloat()
    {
        // Undefined values are equal regardless of the float value
        // (because Undefined is a unit-only type)
        new YGValue(12.5f, Unit.Undefined).ShouldBe(new YGValue(float.NaN, Unit.Undefined));
    }

    public void SupportsEqualityAutoValuesEqualRegardlessOfFloat()
    {
        // Auto values are equal regardless of the float value
        // (because Auto is a unit-only type)
        new YGValue(0, Unit.Auto).ShouldBe(new YGValue(-1, Unit.Auto));
    }

    #region Additional Tests (beyond C++ test file)

    // Tests the static constant properties.
    public void StaticConstantZeroHasCorrectValues()
    {
        YGValue.Zero.Value.ShouldBe(0f);
        YGValue.Zero.Unit.ShouldBe(Unit.Point);
    }

    public void StaticConstantUndefinedHasCorrectValues()
    {
        float.IsNaN(YGValue.Undefined.Value).ShouldBeTrue();
        YGValue.Undefined.Unit.ShouldBe(Unit.Undefined);
    }

    public void StaticConstantAutoHasCorrectValues()
    {
        float.IsNaN(YGValue.Auto.Value).ShouldBeTrue();
        YGValue.Auto.Unit.ShouldBe(Unit.Auto);
    }

    // Tests the factory methods.
    public void FactoryMethodPointCreatesCorrectValue()
    {
        YGValue point = YGValue.Point(100f);
        point.Value.ShouldBe(100f);
        point.Unit.ShouldBe(Unit.Point);
    }

    public void FactoryMethodPercentCreatesCorrectValue()
    {
        YGValue percent = YGValue.Percent(50f);
        percent.Value.ShouldBe(50f);
        percent.Unit.ShouldBe(Unit.Percent);
    }

    // Tests the negation operator.
    public void NegationOperatorNegatesPositiveValue()
    {
        YGValue positive = YGValue.Point(10f);
        YGValue negated = -positive;

        negated.Value.ShouldBe(-10f);
        negated.Unit.ShouldBe(Unit.Point);
    }

    public void NegationOperatorNegatesNegativeValue()
    {
        YGValue negative = YGValue.Percent(-25f);
        YGValue doubleNegated = -negative;

        doubleNegated.Value.ShouldBe(25f);
        doubleNegated.Unit.ShouldBe(Unit.Percent);
    }

    public void NegateMethodWorks()
    {
        YGValue value = YGValue.Point(10f);
        YGValue negated = value.Negate();

        negated.Value.ShouldBe(-10f);
        negated.Unit.ShouldBe(Unit.Point);
    }

    // Tests equality for all unit-only types (where value is ignored).
    public void UnitOnlyTypeFitContentIgnoresValueInEquality()
    {
        new YGValue(0f, Unit.FitContent).ShouldBe(new YGValue(100f, Unit.FitContent));
    }

    public void UnitOnlyTypeMaxContentIgnoresValueInEquality()
    {
        new YGValue(-5f, Unit.MaxContent).ShouldBe(new YGValue(float.PositiveInfinity, Unit.MaxContent));
    }

    public void UnitOnlyTypeStretchIgnoresValueInEquality()
    {
        new YGValue(float.NaN, Unit.Stretch).ShouldBe(new YGValue(42f, Unit.Stretch));
    }

    // Tests inequality operator.
    public void InequalityOperatorDifferentValuesAreNotEqual()
    {
        (YGValue.Point(10f) != YGValue.Point(20f)).ShouldBeTrue();
    }

    public void InequalityOperatorDifferentUnitsAreNotEqual()
    {
        (YGValue.Point(10f) != YGValue.Percent(10f)).ShouldBeTrue();
    }

    public void InequalityOperatorSameAutoValuesAreEqual()
    {
        (YGValue.Auto != YGValue.Auto).ShouldBeFalse();
    }

    // Tests ToString for various value types.
    public void ToStringUndefinedReturnsUndefined()
    {
        YGValue.Undefined.ToString().ShouldBe("undefined");
    }

    public void ToStringAutoReturnsAuto()
    {
        YGValue.Auto.ToString().ShouldBe("auto");
    }

    public void ToStringPointReturnsValueWithPt()
    {
        YGValue.Point(100f).ToString().ShouldBe("100pt");
    }

    public void ToStringPercentReturnsValueWithPercent()
    {
        YGValue.Percent(50f).ToString().ShouldBe("50%");
    }

    public void ToStringMaxContentReturnsMaxContent()
    {
        new YGValue(0, Unit.MaxContent).ToString().ShouldBe("max-content");
    }

    public void ToStringFitContentReturnsFitContent()
    {
        new YGValue(0, Unit.FitContent).ToString().ShouldBe("fit-content");
    }

    public void ToStringStretchReturnsStretch()
    {
        new YGValue(0, Unit.Stretch).ToString().ShouldBe("stretch");
    }

    // Tests YGFloatIsUndefined utility function.
    public void YGFloatIsUndefinedReturnsTrueForNaN()
    {
        YGValueUtilities.YGFloatIsUndefined(float.NaN).ShouldBeTrue();
    }

    public void YGFloatIsUndefinedReturnsTrueForYGUndefinedConstant()
    {
        YGValueUtilities.YGFloatIsUndefined(YGValueUtilities.YGUndefined).ShouldBeTrue();
    }

    public void YGFloatIsUndefinedReturnsFalseForZero()
    {
        YGValueUtilities.YGFloatIsUndefined(0f).ShouldBeFalse();
    }

    public void YGFloatIsUndefinedReturnsFalseForPositiveValue()
    {
        YGValueUtilities.YGFloatIsUndefined(100f).ShouldBeFalse();
    }

    public void YGFloatIsUndefinedReturnsFalseForPositiveInfinity()
    {
        YGValueUtilities.YGFloatIsUndefined(float.PositiveInfinity).ShouldBeFalse();
    }

    public void YGFloatIsUndefinedReturnsFalseForNegativeInfinity()
    {
        YGValueUtilities.YGFloatIsUndefined(float.NegativeInfinity).ShouldBeFalse();
    }

    // Tests that YGUndefined constant matches float.NaN.
    public void YGUndefinedConstantIsNaN()
    {
        float.IsNaN(YGValueUtilities.YGUndefined).ShouldBeTrue();
    }

    // Tests GetHashCode consistency with Equals.
    public void GetHashCodeIsConsistentWithEqualsForPointValues()
    {
        YGValue a = YGValue.Point(100f);
        YGValue b = YGValue.Point(100f);
        a.ShouldBe(b);
        a.GetHashCode().ShouldBe(b.GetHashCode());
    }

    public void GetHashCodeIsConsistentWithEqualsForUnitOnlyTypes()
    {
        // Unit-only types with different values should still have equal hash codes
        YGValue auto1 = new(0, Unit.Auto);
        YGValue auto2 = new(-1, Unit.Auto);
        auto1.ShouldBe(auto2);
        auto1.GetHashCode().ShouldBe(auto2.GetHashCode());
    }

    #endregion
}
