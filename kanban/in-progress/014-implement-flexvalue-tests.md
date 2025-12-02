# Task 014-implement-flexvalue-tests

## Summary
Create comprehensive unit tests for FlexValue struct and Unit enum covering all factory methods, equality, and edge cases.

## Todo List
- [ ] Test Unit enum has correct values (Point, Percent, Auto, Undefined)
- [ ] Test FlexValue.Point() creates correct value and unit
- [ ] Test FlexValue.Percent() creates correct value and unit
- [ ] Test FlexValue.Auto has correct unit and zero value
- [ ] Test FlexValue.Undefined has correct unit
- [ ] Test equality operators (==, !=)
- [ ] Test IEquatable<FlexValue> implementation
- [ ] Test GetHashCode consistency with Equals
- [ ] Test ToString() returns useful debugging output
- [ ] Test edge cases: negative values, zero, NaN, infinity

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
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
