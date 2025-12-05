# Task 007-implement-flexconfig-class

## Summary
Implement the FlexConfig class for global configuration settings and feature flags that affect layout behavior.

## Todo List
- [x] Create layout/flex-config.cs class
- [x] Add PointScaleFactor property (float, default 1.0f) for pixel grid rounding
- [x] Add UseWebDefaults property (bool) to toggle between Yoga and W3C defaults
- [x] Add UseErrata property (bool) for strict W3C spec compliance
- [x] Add static Default property returning a default configuration
- [x] Implement Clone() method for creating configuration copies
- [x] Verify code follows csharp-coding.md standards

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
- Created layout/flex-config.cs with global configuration settings
- PointScaleFactor: float, default 1.0f for pixel grid rounding
- UseWebDefaults: bool, toggles W3C vs Yoga defaults
- UseErrata: bool, enables strict W3C spec compliance (added instead of ExperimentalFeatures)
- Static Default property provides singleton default instance
- Clone() method creates independent copies
- Build verified: 0 warnings, 0 errors
