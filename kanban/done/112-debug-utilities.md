# Task 112 - Debug Utilities

## Summary

Port the debug utilities (AssertFatal, Log) from C++ to C#. This is a Level 4 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                         | Lines |
| ---------- | ---------------------------- | ----- |
| C++ Header | `yoga/debug/AssertFatal.h`   | ~30   |
| C++ Source | `yoga/debug/AssertFatal.cpp` | ~30   |
| C++ Header | `yoga/debug/Log.h`           | ~50   |
| C++ Source | `yoga/debug/Log.cpp`         | ~50   |

## Target Files

| Type      | Path                                            |
| --------- | ----------------------------------------------- |
| C# Source | `source/timewarp-flexbox/debug/assert-fatal.cs` |
| C# Source | `source/timewarp-flexbox/debug/log.cs`          |

## Dependencies

- Task 105: YGEnums (LogLevel)

## Todo List

- [ ] Port `AssertFatal.h/.cpp` to C#
- [ ] Port `Log.h/.cpp` to C#
- [ ] Use C# exception handling for fatal assertions
- [ ] Implement configurable logging

## Acceptance Criteria

- [ ] Fatal assertions throw appropriate exceptions
- [ ] Logging system works with configurable callbacks
- [ ] Log levels respected

## Notes

Key C++ constructs to convert:

- `yoga_fatal()` -> `throw new YogaException()` or `Debug.Assert()`
- Log callback system -> C# events or delegates

```csharp
public static class AssertFatal
{
    [Conditional("DEBUG")]
    public static void Assert(bool condition, string message = "")
    {
        if (!condition)
            throw new YogaAssertionException(message);
    }
}

public static class Log
{
    public static Action<LogLevel, string>? Logger { get; set; }

    public static void Write(LogLevel level, string message)
    {
        Logger?.Invoke(level, message);
    }
}
```
