# Task 059-add-log-level-enum

## Summary
Add LogLevel enum to match Yoga's YGLogLevel. This enables configurable logging for layout debugging and diagnostics.

## Todo List
- [ ] Create LogLevel enum with Error, Warn, Info, Debug, Verbose, Fatal values
- [ ] Add XML documentation for each LogLevel value
- [ ] Define logger delegate type matching Yoga's signature
- [ ] Add Logger property to FlexConfig
- [ ] Add default logger implementation that uses .NET logging
- [ ] Add log calls at appropriate points in layout algorithm
- [ ] Ensure logging can be disabled for performance
- [ ] Add unit tests for logging functionality
- [ ] Document logging integration points

## Notes
Yoga reference (YGEnums.h lines 97-104):
```cpp
YG_ENUM_DECL(
    YGLogLevel,
    YGLogLevelError,
    YGLogLevelWarn,
    YGLogLevelInfo,
    YGLogLevelDebug,
    YGLogLevelVerbose,
    YGLogLevelFatal)
```

Yoga YGConfig.h logger signature:
```cpp
typedef int (*YGLogger)(
    YGConfigConstRef config,
    YGNodeConstRef node,
    YGLogLevel level,
    const char* format,
    va_list args);
```

C# equivalent would be a delegate or interface for logging.
