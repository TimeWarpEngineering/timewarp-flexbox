# Task 074-add-config-context

## Summary
Add Context property to FlexConfig to match Yoga's config context. This allows storing arbitrary user data on the config that can be accessed during callbacks.

## Todo List
- [ ] Add `Context` property (object?) to FlexConfig
- [ ] Add XML documentation for Context property
- [ ] Document usage pattern for context in callbacks
- [ ] Add unit test for config context usage
- [ ] Update Clone() to copy Context reference

## Notes
Yoga reference (YGConfig.h lines 119-125):
```cpp
/**
 * Sets an arbitrary context pointer on the config which may be read from during
 * callbacks.
 */
YG_EXPORT void YGConfigSetContext(YGConfigRef config, void* context);
YG_EXPORT void* YGConfigGetContext(YGConfigConstRef config);
```

Context is useful for passing application-specific data to callbacks without closures.
