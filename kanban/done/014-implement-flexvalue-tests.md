# Task 014-implement-flexvalue-tests

## Summary
Create comprehensive unit tests for FlexValue struct and Unit enum covering all factory methods, equality, and edge cases.

## Todo List
- [x] Test Unit enum has correct values (Point, Percent, Auto, Undefined)
- [x] Test FlexValue.Point() creates correct value and unit
- [x] Test FlexValue.Percent() creates correct value and unit
- [x] Test FlexValue.Auto has correct unit and zero value
- [x] Test FlexValue.Undefined has correct unit
- [x] Test equality operators (==, !=)
- [x] Test IEquatable<FlexValue> implementation
- [x] Test GetHashCode consistency with Equals
- [x] Test ToString() returns useful debugging output
- [x] Test edge cases: negative values, zero, NaN, infinity

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Values/FlexValue_/Constructor_Should_.cs

Uses TimeWarp.Fixie conventions:
- Public methods are tests (no attributes needed)
- Use [Input(...)] for parameterized tests
- Use Shouldly assertions

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Values.FlexValue_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class Constructor_Should_
{
  public static void CreatePointValueWithCorrectUnit()
  {
    FlexValue value = FlexValue.Point(100);
    
    value.Value.ShouldBe(100f);
    value.Unit.ShouldBe(Unit.Point);
  }
  
  public static void CreatePercentValueWithCorrectUnit()
  {
    FlexValue value = FlexValue.Percent(50);
    
    value.Value.ShouldBe(50f);
    value.Unit.ShouldBe(Unit.Percent);
  }
  
  public static void CreateAutoWithAutoUnit()
  {
    FlexValue.Auto.Unit.ShouldBe(Unit.Auto);
  }
  
  [Input(0f)]
  [Input(-10f)]
  [Input(float.MaxValue)]
  public static void HandleEdgeCasesForPointValues(float input)
  {
    FlexValue value = FlexValue.Point(input);
    value.Value.ShouldBe(input);
  }
}

[TestTag(TestTags.Fast)]
public class Equality_Should_
{
  public static void ReturnTrueForEqualValues()
  {
    FlexValue value1 = FlexValue.Point(10);
    FlexValue value2 = FlexValue.Point(10);
    
    (value1 == value2).ShouldBeTrue();
    value1.Equals(value2).ShouldBeTrue();
  }
  
  public static void ReturnFalseForDifferentValues()
  {
    FlexValue value1 = FlexValue.Point(10);
    FlexValue value2 = FlexValue.Point(20);
    
    (value1 != value2).ShouldBeTrue();
  }
}
```

## Results
- Expanded flex-value-tests.cs with comprehensive test coverage
- FlexValueTests class: 31 tests covering factory methods, edge cases, equality, hash codes, and ToString
- UnitEnumTests class: 5 tests covering enum values and count
- Total: 36 tests, all passing
- Test categories: Factory Methods, Edge Cases, Equality, GetHashCode, ToString
- Edge cases covered: zero, negative, large values, infinity, null, different types
- Deviation: Used single test file instead of separate files per category (simpler structure)
