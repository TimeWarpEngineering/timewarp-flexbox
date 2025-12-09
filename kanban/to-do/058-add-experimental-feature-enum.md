# Task 058-add-experimental-feature-enum

## Summary
Add ExperimentalFeature enum to match Yoga's YGExperimentalFeature. This enables toggling experimental features that may change behavior in future versions.

## Todo List
- [ ] Create ExperimentalFeature enum with WebFlexBasis value
- [ ] Add XML documentation for each ExperimentalFeature value
- [ ] Add `SetExperimentalFeatureEnabled(ExperimentalFeature, bool)` to FlexConfig
- [ ] Add `IsExperimentalFeatureEnabled(ExperimentalFeature)` to FlexConfig
- [ ] Store experimental features as flags in FlexConfig
- [ ] Implement WebFlexBasis experimental behavior in layout algorithm
- [ ] Add unit tests for experimental feature toggling
- [ ] Document what each experimental feature does

## Notes
Yoga reference (YGEnums.h lines 71-73):
```cpp
YG_ENUM_DECL(
    YGExperimentalFeature,
    YGExperimentalFeatureWebFlexBasis)
```

Yoga YGConfig.cpp:
```cpp
void YGConfigSetExperimentalFeatureEnabled(
    const YGConfigRef config,
    const YGExperimentalFeature feature,
    const bool enabled) {
  resolveRef(config)->setExperimentalFeatureEnabled(scopedEnum(feature), enabled);
}
```

WebFlexBasis changes how flex-basis is resolved to match web browser behavior.
