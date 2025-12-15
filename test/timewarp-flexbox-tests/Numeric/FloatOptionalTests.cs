/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: tests/FloatOptionalTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Numeric;

/// <summary>
/// Tests for FloatOptional.
/// Ported from C++ tests/FloatOptionalTest.cpp
/// </summary>
#pragma warning disable CS1718 // Comparison made to same variable - intentional for testing operator overloads
#pragma warning disable CA2242 // Test for NaN correctly - testing our custom NaN handling
public class FloatOptionalTests
{
    private static readonly FloatOptional Empty = FloatOptional.Undefined;
    private static readonly FloatOptional Zero = new(0.0f);
    private static readonly FloatOptional One = new(1.0f);
    private static readonly FloatOptional Positive = new(1234.5f);
    private static readonly FloatOptional Negative = new(-9876.5f);

    // Value tests
    public void ValueEmptyIsUndefined()
    {
        Comparison.IsUndefined(Empty.Unwrap()).ShouldBeTrue();
    }

    public void ValueZeroIsZero()
    {
        Zero.Unwrap().ShouldBe(0.0f);
    }

    public void ValueOneIsOne()
    {
        One.Unwrap().ShouldBe(1.0f);
    }

    public void ValuePositiveIsPositive()
    {
        Positive.Unwrap().ShouldBe(1234.5f);
    }

    public void ValueNegativeIsNegative()
    {
        Negative.Unwrap().ShouldBe(-9876.5f);
    }

    public void IsUndefinedForEmpty()
    {
        Empty.IsUndefined.ShouldBeTrue();
    }

    public void IsUndefinedFalseForDefined()
    {
        Zero.IsUndefined.ShouldBeFalse();
        One.IsUndefined.ShouldBeFalse();
        Positive.IsUndefined.ShouldBeFalse();
        Negative.IsUndefined.ShouldBeFalse();
    }

    // Equality tests
    public void EqualityEmptyEqualsEmpty()
    {
        (Empty == Empty).ShouldBeTrue();
    }

    public void EqualityEmptyEqualsNaN()
    {
        (Empty == float.NaN).ShouldBeTrue();
    }

    public void EqualityEmptyNotEqualsZero()
    {
        (Empty == Zero).ShouldBeFalse();
    }

    public void EqualityEmptyNotEqualsNegative()
    {
        (Empty == Negative).ShouldBeFalse();
    }

    public void EqualityEmptyNotEqualsFloat()
    {
        (Empty == 12.3f).ShouldBeFalse();
    }

    public void EqualityZeroEqualsZero()
    {
        (Zero == Zero).ShouldBeTrue();
    }

    public void EqualityZeroEqualsFloat()
    {
        (Zero == 0.0f).ShouldBeTrue();
    }

    public void EqualityZeroNotEqualsPositive()
    {
        (Zero == Positive).ShouldBeFalse();
    }

    public void EqualityOneEqualsOne()
    {
        (One == One).ShouldBeTrue();
    }

    public void EqualityOneEqualsFloat()
    {
        (One == 1.0f).ShouldBeTrue();
    }

    public void EqualityPositiveEqualsPositive()
    {
        (Positive == Positive).ShouldBeTrue();
    }

    public void EqualityPositiveEqualsUnwrap()
    {
        (Positive == Positive.Unwrap()).ShouldBeTrue();
    }

    // Inequality tests
    public void InequalityEmptyNotNotEqualsEmpty()
    {
        (Empty != Empty).ShouldBeFalse();
    }

    public void InequalityEmptyNotNotEqualsNaN()
    {
        (Empty != float.NaN).ShouldBeFalse();
    }

    public void InequalityEmptyNotEqualsZero()
    {
        (Empty != Zero).ShouldBeTrue();
    }

    // Greater than with undefined
    public void GreaterThanEmptyNotGreaterThanEmpty()
    {
        (Empty > Empty).ShouldBeFalse();
    }

    public void GreaterThanEmptyNotGreaterThanZero()
    {
        (Empty > Zero).ShouldBeFalse();
    }

    public void GreaterThanZeroNotGreaterThanEmpty()
    {
        (Zero > Empty).ShouldBeFalse();
    }

    // Greater than
    public void GreaterThanZeroGreaterThanNegative()
    {
        (Zero > Negative).ShouldBeTrue();
    }

    public void GreaterThanZeroNotGreaterThanZero()
    {
        (Zero > Zero).ShouldBeFalse();
    }

    public void GreaterThanOneGreaterThanNegative()
    {
        (One > Negative).ShouldBeTrue();
    }

    public void GreaterThanOneGreaterThanZero()
    {
        (One > Zero).ShouldBeTrue();
    }

    public void GreaterThanNegativeGreaterThanNegativeInfinity()
    {
        (Negative > new FloatOptional(float.NegativeInfinity)).ShouldBeTrue();
    }

    // Less than with undefined
    public void LessThanEmptyNotLessThanEmpty()
    {
        (Empty < Empty).ShouldBeFalse();
    }

    public void LessThanZeroNotLessThanEmpty()
    {
        (Zero < Empty).ShouldBeFalse();
    }

    // Less than
    public void LessThanNegativeLessThanZero()
    {
        (Negative < Zero).ShouldBeTrue();
    }

    public void LessThanZeroNotLessThanZero()
    {
        (Zero < Zero).ShouldBeFalse();
    }

    public void LessThanNegativeInfinityLessThanNegative()
    {
        (new FloatOptional(float.NegativeInfinity) < Negative).ShouldBeTrue();
    }

    // Greater than or equals with undefined
    public void GreaterThanOrEqualsEmptyGreaterThanOrEqualsEmpty()
    {
        (Empty >= Empty).ShouldBeTrue();
    }

    public void GreaterThanOrEqualsEmptyNotGreaterThanOrEqualsZero()
    {
        (Empty >= Zero).ShouldBeFalse();
    }

    public void GreaterThanOrEqualsZeroNotGreaterThanOrEqualsEmpty()
    {
        (Zero >= Empty).ShouldBeFalse();
    }

    // Greater than or equals
    public void GreaterThanOrEqualsZeroGreaterThanOrEqualsNegative()
    {
        (Zero >= Negative).ShouldBeTrue();
    }

    public void GreaterThanOrEqualsZeroGreaterThanOrEqualsZero()
    {
        (Zero >= Zero).ShouldBeTrue();
    }

    // Less than or equals with undefined
    public void LessThanOrEqualsEmptyLessThanOrEqualsEmpty()
    {
        (Empty <= Empty).ShouldBeTrue();
    }

    public void LessThanOrEqualsZeroNotLessThanOrEqualsEmpty()
    {
        (Zero <= Empty).ShouldBeFalse();
    }

    // Less than or equals
    public void LessThanOrEqualsNegativeLessThanOrEqualsZero()
    {
        (Negative <= Zero).ShouldBeTrue();
    }

    public void LessThanOrEqualsZeroLessThanOrEqualsZero()
    {
        (Zero <= Zero).ShouldBeTrue();
    }

    // Addition
    public void AdditionZeroPlusOne()
    {
        (Zero + One).ShouldBe(One);
    }

    public void AdditionNegativePlusPositive()
    {
        float n = Negative.Unwrap();
        float p = Positive.Unwrap();
        (Negative + Positive).ShouldBe(new FloatOptional(n + p));
    }

    public void AdditionEmptyPlusZero()
    {
        (Empty + Zero).ShouldBe(Empty);
    }

    public void AdditionEmptyPlusEmpty()
    {
        (Empty + Empty).ShouldBe(Empty);
    }

    public void AdditionNegativePlusEmpty()
    {
        (Negative + Empty).ShouldBe(Empty);
    }

    // MaxOrDefined
    public void MaxOrDefinedEmptyAndEmpty()
    {
        FloatOptionalExtensions.MaxOrDefined(Empty, Empty).ShouldBe(Empty);
    }

    public void MaxOrDefinedEmptyAndPositive()
    {
        FloatOptionalExtensions.MaxOrDefined(Empty, Positive).ShouldBe(Positive);
    }

    public void MaxOrDefinedNegativeAndEmpty()
    {
        FloatOptionalExtensions.MaxOrDefined(Negative, Empty).ShouldBe(Negative);
    }

    public void MaxOrDefinedComparesValues()
    {
        FloatOptionalExtensions.MaxOrDefined(new FloatOptional(1.0f), new FloatOptional(1.125f))
            .ShouldBe(new FloatOptional(1.125f));
    }

    // Unwrap
    public void UnwrapEmpty()
    {
        Comparison.IsUndefined(Empty.Unwrap()).ShouldBeTrue();
    }

    public void UnwrapZero()
    {
        Zero.Unwrap().ShouldBe(0.0f);
    }

    public void UnwrapValue()
    {
        new FloatOptional(123456.78f).Unwrap().ShouldBe(123456.78f);
    }
}
#pragma warning restore CA2242
#pragma warning restore CS1718
