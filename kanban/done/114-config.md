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

| Type      | Path                                            |
| --------- | ----------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Config/Config.cs`      |
| C# Test   | `test/timewarp-flexbox-tests/Config/ConfigTests.cs` |

## Dependencies

- Task 109: Enums (Errata, ExperimentalFeature, LogLevel)
- Task 112: Debug utilities

## Todo List

- [x] Port `Config.h/.cpp` to C#
- [x] Port `YGConfig.h/.cpp` public API to C#
- [x] Port `YGConfigTest.cpp` to xUnit tests
- [x] Ensure all tests pass

## Acceptance Criteria

- [x] All ~150 lines of test logic ported
- [x] Errata flags working
- [x] Experimental features working
- [x] Point scale factor working
- [x] Log callback working
- [x] All tests pass with identical behavior to C++

## Results

Implementation completed with:
- `Config` class with all features (Errata, ExperimentalFeatureSet, PointScaleFactor, UseWebDefaults, Context)
- `ExperimentalFeatureSet` readonly struct for managing experimental features
- Clone node callback support
- Logger support with `YogaLogHandler` delegate
- Version tracking for config changes
- Static `ConfigUpdateInvalidatesLayout` method
- Comprehensive test coverage (55+ test methods)
