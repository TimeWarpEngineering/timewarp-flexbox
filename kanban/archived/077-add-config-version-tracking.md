# Task 077-add-config-version-tracking

## Summary
Add config version tracking to FlexConfig and LayoutResult to match Yoga's config invalidation. When config changes, cached layouts with old config versions are invalidated.

## Todo List
- [ ] Add `Version` property (uint) to FlexConfig
- [ ] Increment version when config properties change
- [ ] Add `ConfigVersion` to LayoutResult/cache
- [ ] Update cache validation to check config version
- [ ] Add helper method to check if config change invalidates layout
- [ ] Add XML documentation for version tracking
- [ ] Add unit tests for config version invalidation
- [ ] Verify cache is invalidated when config changes

## Notes
Yoga reference (LayoutResults.h line 33):
```cpp
uint32_t configVersion = 0;
```

Yoga Node.cpp setConfig:
```cpp
void Node::setConfig(yoga::Config* config) {
  if (yoga::configUpdateInvalidatesLayout(*config_, *config)) {
    markDirtyAndPropagate();
    layout_.configVersion = 0;
  } else {
    layout_.configVersion = config->getVersion();
  }
  config_ = config;
}
```

Config version tracking allows reusing cached layouts when config changes don't affect layout.
