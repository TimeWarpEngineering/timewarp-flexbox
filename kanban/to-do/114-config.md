# Task 114 - Config

## Summary

Port the Config class from C++ to C#. This holds configuration options for layout (errata, experimental features, point scale factor). This is a Level 4 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                     | Lines |
| ---------- | ------------------------ | ----- |
| C++ Header | `yoga/config/Config.h`   | ~150  |
| C++ Source | `yoga/config/Config.cpp` | ~100  |
| C++ Header | `yoga/YGConfig.h`        | ~80   |
| C++ Source | `yoga/YGConfig.cpp`      | ~50   |
| C++ Test   | `tests/YGConfigTest.cpp` | ~150  |

## Target Files

| Type      | Path                                              |
| --------- | ------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/config/config.cs`        |
| C# Source | `source/timewarp-flexbox/yg-config.cs`            |
| C# Test   | `tests/timewarp-flexbox-tests/yg-config-tests.cs` |

## Dependencies

- Task 109: Enums (Errata, ExperimentalFeature, LogLevel)
- Task 112: Debug utilities

## Todo List

- [ ] Port `Config.h/.cpp` to C#
- [ ] Port `YGConfig.h/.cpp` public API to C#
- [ ] Port `YGConfigTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All ~150 lines of test logic ported
- [ ] Errata flags working
- [ ] Experimental features working
- [ ] Point scale factor working
- [ ] Log callback working
- [ ] All tests pass with identical behavior to C++

## Notes

```csharp
public sealed class Config
{
    public static Config Default { get; } = new();

    public Errata Errata { get; set; } = Errata.None;
    public float PointScaleFactor { get; set; } = 1.0f;
    public bool UseWebDefaults { get; set; }

    public void SetExperimentalFeature(ExperimentalFeature feature, bool enabled);
    public bool IsExperimentalFeatureEnabled(ExperimentalFeature feature);
}
```
