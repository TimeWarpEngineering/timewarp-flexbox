# Task 043-implement-config-tests

## Summary
Implement tests for FlexConfig class behavior including point scale factor, use web defaults option, experimental features, and default value verification. Config tests ensure global settings are correctly applied to all nodes using that configuration.

## Todo List
- [ ] Test default FlexConfig values
- [ ] Test PointScaleFactor affects rounding
- [ ] Test UseWebDefaults changes default values
- [ ] Test experimental features toggle
- [ ] Test config shared across multiple nodes
- [ ] Test config changes don't affect existing layouts
- [ ] Test config cloning
- [ ] Test default values match CSS/Yoga spec
- [ ] Test errata mode options
- [ ] Test logger callback configuration

## Notes
Test file: test/TimeWarp.Flexbox.Tests/Config/FlexConfig_/

Reference: 
- yoga/tests/YGConfigTest.cpp
- yoga/tests/YGDefaultValuesTest.cpp

Uses TimeWarp.Fixie conventions with Shouldly assertions.

Example tests:
```csharp
namespace TimeWarp.Flexbox.Tests.Config.FlexConfig_;

using Shouldly;
using TimeWarp.Fixie;

[TestTag(TestTags.Fast)]
public class DefaultValues_Should_
{
  public static void HaveExpectedDefaults()
  {
    FlexConfig config = new();
    
    config.PointScaleFactor.ShouldBe(1.0f);
    config.UseWebDefaults.ShouldBeFalse();
  }
}

[TestTag(TestTags.Fast)]
public class PointScaleFactor_Should_
{
  public static void AffectRoundingBehavior()
  {
    FlexConfig config1x = new() { PointScaleFactor = 1.0f };
    FlexConfig config2x = new() { PointScaleFactor = 2.0f };
    
    FlexNode root1 = new(config1x)
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    FlexNode root2 = new(config2x)
    {
      Width = FlexValue.Point(100),
      FlexDirection = FlexDirection.Row
    };
    
    // Add 3 children to each
    for (int i = 0; i < 3; i++)
    {
      root1.AddChild(new FlexNode(config1x) { FlexGrow = 1 });
      root2.AddChild(new FlexNode(config2x) { FlexGrow = 1 });
    }
    
    root1.CalculateLayout(100, float.NaN);
    root2.CalculateLayout(100, float.NaN);
    
    // 100/3 = 33.333... 
    // At 1x: rounds to whole pixels
    // At 2x: can use 0.5 pixel increments
    // Both should total 100
    float total1 = root1.GetChild(0).Layout.Width + 
                   root1.GetChild(1).Layout.Width + 
                   root1.GetChild(2).Layout.Width;
    float total2 = root2.GetChild(0).Layout.Width + 
                   root2.GetChild(1).Layout.Width + 
                   root2.GetChild(2).Layout.Width;
    
    total1.ShouldBe(100);
    total2.ShouldBe(100);
  }
  
  [Input(0.5f)]
  [Input(1.0f)]
  [Input(2.0f)]
  [Input(3.0f)]
  public static void AcceptValidScaleFactors(float scaleFactor)
  {
    FlexConfig config = new() { PointScaleFactor = scaleFactor };
    config.PointScaleFactor.ShouldBe(scaleFactor);
  }
}

[TestTag(TestTags.Fast)]
public class UseWebDefaults_Should_
{
  public static void ChangeFlexDirectionDefault()
  {
    FlexConfig webConfig = new() { UseWebDefaults = true };
    FlexConfig yogaConfig = new() { UseWebDefaults = false };
    
    FlexNode webNode = new(webConfig);
    FlexNode yogaNode = new(yogaConfig);
    
    // Web default is row, Yoga default is column
    webNode.FlexDirection.ShouldBe(FlexDirection.Row);
    yogaNode.FlexDirection.ShouldBe(FlexDirection.Column);
  }
}

[TestTag(TestTags.Fast)]
public class ConfigSharing_Should_
{
  public static void ApplyToAllNodesUsingConfig()
  {
    FlexConfig config = new() { PointScaleFactor = 2.0f };
    
    FlexNode root = new(config);
    FlexNode child1 = new(config);
    FlexNode child2 = new(config);
    
    root.AddChild(child1);
    root.AddChild(child2);
    
    // All nodes should use same config
    root.Config.ShouldBeSameAs(config);
    child1.Config.ShouldBeSameAs(config);
    child2.Config.ShouldBeSameAs(config);
  }
}

[TestTag(TestTags.Fast)]
public class FlexNodeDefaults_Should_
{
  public static void MatchCssSpecDefaults()
  {
    FlexNode node = new();
    
    // CSS Flexbox spec defaults
    node.FlexDirection.ShouldBe(FlexDirection.Column);  // Yoga default
    node.FlexWrap.ShouldBe(FlexWrap.NoWrap);
    node.JustifyContent.ShouldBe(JustifyContent.FlexStart);
    node.AlignItems.ShouldBe(AlignItems.Stretch);
    node.AlignContent.ShouldBe(AlignContent.Stretch);
    node.AlignSelf.ShouldBe(AlignSelf.Auto);
    
    node.FlexGrow.ShouldBe(0f);
    node.FlexShrink.ShouldBe(0f);  // Yoga uses 0, CSS uses 1
    node.FlexBasis.ShouldBe(FlexValue.Auto);
    
    node.PositionType.ShouldBe(PositionType.Relative);
    node.Display.ShouldBe(Display.Flex);
    
    node.Width.ShouldBe(FlexValue.Auto);
    node.Height.ShouldBe(FlexValue.Auto);
  }
}
```

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
