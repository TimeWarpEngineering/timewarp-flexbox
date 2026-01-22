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

| Type      | Path                                                 |
| --------- | ---------------------------------------------------- |
| C# Source | `source/timewarp-flexbox/Event/Event.cs`             |
| C# Test   | `test/timewarp-flexbox-tests/Event/EventTests.cs`    |

## Dependencies

- Task 105: YGEnums
- Task 112: Debug utilities

## Todo List

- [x] Port `event.h` event types to C#
- [x] Port `event.cpp` publisher to C#
- [x] Port `EventsTest.cpp` to xUnit tests
- [x] Ensure all tests pass

## Acceptance Criteria

- [x] All 330 lines of test logic ported
- [x] Event subscription working
- [x] Event publishing working
- [x] All tests pass with identical behavior to C++

## Implementation Notes

### C++ to C# Mapping

| C++ | C# |
|-----|-----|
| `Event::Type` enum | `EventType` enum |
| `LayoutType` enum | `LayoutType` enum |
| `LayoutPassReason` enum | `LayoutPassReason` enum |
| `LayoutData` struct | `LayoutData` class |
| `Event::TypedData<E>` templates | `IEventData` interface + record structs |
| `Event::Subscriber` function type | `EventSubscriber` delegate |
| `Event::subscribe()` | `YogaEvent.Subscribe()` |
| `Event::reset()` | `YogaEvent.Reset()` |
| `Event::publish()` | `YogaEvent.Publish()` + typed convenience methods |

### Key Design Decisions

1. **Class name**: Changed from `Event` to `YogaEvent` to avoid conflict with C# reserved keyword
2. **Event data**: Used record structs implementing `IEventData` interface instead of C++ template specializations
3. **Thread safety**: Used lock-based linked list (matching C++ atomic behavior)
4. **Publisher methods**: Added typed convenience methods like `PublishNodeAllocation()`, `PublishNodeLayout()`, etc.
5. **LayoutData array property**: Changed to methods `GetMeasureCallbackReasonCount()` and `IncrementMeasureCallbackReasonCount()` to comply with CA1819

### Test Coverage

The test file includes 30+ test methods covering:
- Subscribe/Reset functionality
- All event types (NodeAllocation, NodeDeallocation, NodeLayout, LayoutPassStart/End, MeasureCallback, NodeBaseline)
- LayoutData copying and independence
- LayoutPassReason string conversion
- Thread safety (basic concurrent publish test)
