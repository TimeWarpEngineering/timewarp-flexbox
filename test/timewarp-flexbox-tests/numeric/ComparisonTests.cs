/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/numeric/Comparison.h
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Numeric;

/// <summary>
/// Tests for Comparison utilities.
/// </summary>
public class ComparisonTests
{
  // IsUndefined tests
  public void IsUndefinedReturnsTrueForNaN()
  {
    Comparison.IsUndefined(float.NaN).ShouldBeTrue();
  }

  public void IsUndefinedReturnsFalseForRegularValues()
  {
    Comparison.IsUndefined(0f).ShouldBeFalse();
    Comparison.IsUndefined(1f).ShouldBeFalse();
    Comparison.IsUndefined(-1f).ShouldBeFalse();
    Comparison.IsUndefined(float.PositiveInfinity).ShouldBeFalse();
    Comparison.IsUndefined(float.NegativeInfinity).ShouldBeFalse();
  }

  // IsDefined tests
  public void IsDefinedReturnsFalseForNaN()
  {
    Comparison.IsDefined(float.NaN).ShouldBeFalse();
  }

  public void IsDefinedReturnsTrueForRegularValues()
  {
    Comparison.IsDefined(0f).ShouldBeTrue();
    Comparison.IsDefined(1f).ShouldBeTrue();
    Comparison.IsDefined(-1f).ShouldBeTrue();
    Comparison.IsDefined(float.PositiveInfinity).ShouldBeTrue();
    Comparison.IsDefined(float.NegativeInfinity).ShouldBeTrue();
  }

  // IsInfinity tests
  public void IsInfinityReturnsTrueForPositiveInfinity()
  {
    Comparison.IsInfinity(float.PositiveInfinity).ShouldBeTrue();
  }

  public void IsInfinityReturnsTrueForNegativeInfinity()
  {
    Comparison.IsInfinity(float.NegativeInfinity).ShouldBeTrue();
  }

  public void IsInfinityReturnsFalseForFiniteValues()
  {
    Comparison.IsInfinity(0f).ShouldBeFalse();
    Comparison.IsInfinity(1f).ShouldBeFalse();
    Comparison.IsInfinity(float.MaxValue).ShouldBeFalse();
    Comparison.IsInfinity(float.MinValue).ShouldBeFalse();
  }

  // MaxOrDefined tests
  public void MaxOrDefinedReturnsMaxWhenBothDefined()
  {
    Comparison.MaxOrDefined(1f, 2f).ShouldBe(2f);
    Comparison.MaxOrDefined(5f, 3f).ShouldBe(5f);
    Comparison.MaxOrDefined(-1f, -2f).ShouldBe(-1f);
  }

  public void MaxOrDefinedReturnsDefinedValueWhenOneIsNaN()
  {
    Comparison.MaxOrDefined(float.NaN, 5f).ShouldBe(5f);
    Comparison.MaxOrDefined(3f, float.NaN).ShouldBe(3f);
  }

  public void MaxOrDefinedReturnsNaNWhenBothAreNaN()
  {
    Comparison.IsUndefined(Comparison.MaxOrDefined(float.NaN, float.NaN)).ShouldBeTrue();
  }

  // MinOrDefined tests
  public void MinOrDefinedReturnsMinWhenBothDefined()
  {
    Comparison.MinOrDefined(1f, 2f).ShouldBe(1f);
    Comparison.MinOrDefined(5f, 3f).ShouldBe(3f);
    Comparison.MinOrDefined(-1f, -2f).ShouldBe(-2f);
  }

  public void MinOrDefinedReturnsDefinedValueWhenOneIsNaN()
  {
    Comparison.MinOrDefined(float.NaN, 5f).ShouldBe(5f);
    Comparison.MinOrDefined(3f, float.NaN).ShouldBe(3f);
  }

  public void MinOrDefinedReturnsNaNWhenBothAreNaN()
  {
    Comparison.IsUndefined(Comparison.MinOrDefined(float.NaN, float.NaN)).ShouldBeTrue();
  }

  // InexactEquals tests
  public void InexactEqualsReturnsTrueForIdenticalValues()
  {
    Comparison.InexactEquals(1f, 1f).ShouldBeTrue();
    Comparison.InexactEquals(0f, 0f).ShouldBeTrue();
    Comparison.InexactEquals(-5f, -5f).ShouldBeTrue();
  }

  public void InexactEqualsReturnsTrueForValuesWithinTolerance()
  {
    Comparison.InexactEquals(1f, 1.00005f).ShouldBeTrue();
    Comparison.InexactEquals(1f, 0.99995f).ShouldBeTrue();
  }

  public void InexactEqualsReturnsFalseForValuesOutsideTolerance()
  {
    Comparison.InexactEquals(1f, 1.001f).ShouldBeFalse();
    Comparison.InexactEquals(1f, 0.999f).ShouldBeFalse();
  }

  public void InexactEqualsReturnsTrueForBothNaN()
  {
    Comparison.InexactEquals(float.NaN, float.NaN).ShouldBeTrue();
  }

  public void InexactEqualsReturnsFalseForMixedNaNAndValue()
  {
    Comparison.InexactEquals(float.NaN, 1f).ShouldBeFalse();
    Comparison.InexactEquals(1f, float.NaN).ShouldBeFalse();
  }

  // Array comparison tests
  public void InexactEqualsArraysReturnsTrueForEqualArrays()
  {
    float[] a = [1f, 2f, 3f];
    float[] b = [1f, 2f, 3f];
    Comparison.InexactEquals(a, b).ShouldBeTrue();
  }

  public void InexactEqualsArraysReturnsTrueForApproximatelyEqualArrays()
  {
    float[] a = [1f, 2f, 3f];
    float[] b = [1.00005f, 2.00005f, 3.00005f];
    Comparison.InexactEquals(a, b).ShouldBeTrue();
  }

  public void InexactEqualsArraysReturnsFalseForDifferentLengthArrays()
  {
    float[] a = [1f, 2f];
    float[] b = [1f, 2f, 3f];
    Comparison.InexactEquals(a, b).ShouldBeFalse();
  }

  public void InexactEqualsArraysReturnsFalseForDifferentValues()
  {
    float[] a = [1f, 2f, 3f];
    float[] b = [1f, 5f, 3f];
    Comparison.InexactEquals(a, b).ShouldBeFalse();
  }
}
