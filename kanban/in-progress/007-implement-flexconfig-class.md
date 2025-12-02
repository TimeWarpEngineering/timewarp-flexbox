# Task 007-implement-flexconfig-class

## Summary
Implement the FlexConfig class for global configuration settings and feature flags that affect layout behavior.

## Todo List
- [ ] Create Layout/FlexConfig.cs class
- [ ] Add PointScaleFactor property (float, default 1.0f) for pixel grid rounding
- [ ] Add UseWebDefaults property (bool) to toggle between Yoga and W3C defaults
- [ ] Add ExperimentalFeatures flags if needed for future features
- [ ] Add static Default property returning a default configuration
- [ ] Implement Clone() method for creating configuration copies
- [ ] Verify code follows csharp-coding.md standards

## Notes
FlexConfig provides global settings that affect layout calculations:
- PointScaleFactor: Used for pixel grid rounding (for high DPI displays)
- UseWebDefaults: Yoga has some defaults that differ from W3C spec

```csharp
public class FlexConfig
{
  public float PointScaleFactor { get; set; } = 1.0f;
  public bool UseWebDefaults { get; set; } = false;
  
  public static FlexConfig Default { get; } = new FlexConfig();
  
  public FlexConfig Clone();
}
```

In Yoga, config affects:
- How values are rounded to pixel grid
- Default values for flex-shrink (1 vs 0)
- Errata handling for spec compliance

## Results
(Add after completion)
- Document outcomes
- Include metrics, observations, decisions
- Note any deviations from plan
