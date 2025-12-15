# Task 113 - Event System

## Summary

Port the event system from C++ to C#. This provides hooks for layout events (node layout, measure calls, etc.). This is a Level 4 task.

## Source Files

**Source Repo:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main`

| Type       | Path                   | Lines |
| ---------- | ---------------------- | ----- |
| C++ Header | `yoga/event/event.h`   | ~100  |
| C++ Source | `yoga/event/event.cpp` | ~80   |
| C++ Test   | `tests/EventsTest.cpp` | 330   |

## Target Files

| Type      | Path                                           |
| --------- | ---------------------------------------------- |
| C# Source | `source/timewarp-flexbox/event/event.cs`       |
| C# Test   | `tests/timewarp-flexbox-tests/events-tests.cs` |

## Dependencies

- Task 105: YGEnums
- Task 112: Debug utilities

## Todo List

- [ ] Port `event.h` event types to C#
- [ ] Port `event.cpp` publisher to C#
- [ ] Port `EventsTest.cpp` to xUnit tests
- [ ] Ensure all tests pass

## Acceptance Criteria

- [ ] All 330 lines of test logic ported
- [ ] Event subscription working
- [ ] Event publishing working
- [ ] All tests pass with identical behavior to C++

## Notes

Key C++ constructs to convert:

- Event callback registration -> C# events/delegates
- Event data structs -> C# record structs

```csharp
public static class Event
{
    public static event EventHandler<NodeLayoutEventArgs>? OnNodeLayout;
    public static event EventHandler<MeasureCallEventArgs>? OnMeasureCall;

    internal static void Publish(NodeLayoutEventArgs args)
    {
        OnNodeLayout?.Invoke(null, args);
    }
}

public record struct NodeLayoutEventArgs(Node Node, LayoutType Type);
public record struct MeasureCallEventArgs(Node Node, float Width, float Height);
```
