# Task 078-align-default-values-with-yoga

## Summary
Review and align default property values with Yoga to ensure compatibility. Document intentional deviations where CSS spec compliance is preferred.

## Todo List
- [ ] Change FlexDirection default from Row to Column (Yoga default)
- [ ] OR document that Row is intentional for CSS compliance and add UseWebDefaults support
- [ ] Change FlexShrink default from 1 to 0 (Yoga default)
- [ ] OR document that 1 is intentional for CSS compliance and add UseWebDefaults support
- [ ] Change Width/Height default from Undefined to Auto (Yoga default)
- [ ] Implement UseWebDefaults in FlexConfig to toggle between Yoga/CSS defaults
- [ ] When UseWebDefaults is true: FlexDirection=Row, FlexShrink=1
- [ ] When UseWebDefaults is false: FlexDirection=Column, FlexShrink=0
- [ ] Add unit tests verifying defaults match Yoga (with UseWebDefaults=false)
- [ ] Add unit tests verifying CSS defaults work (with UseWebDefaults=true)
- [ ] Document default value behavior in API documentation

## Notes
Yoga reference defaults (Style.h):
```cpp
FlexDirection flexDirection_ = FlexDirection::Column;
Wrap flexWrap_ = Wrap::NoWrap;
// ...

static constexpr float DefaultFlexGrow = 0.0f;
static constexpr float DefaultFlexShrink = 0.0f;
static constexpr float WebDefaultFlexShrink = 1.0f;
```

Yoga Node.cpp useWebDefaults:
```cpp
void useWebDefaults() {
  style_.setFlexDirection(FlexDirection::Row);
  style_.setAlignContent(Align::Stretch);
}
```

The goal is Yoga compatibility by default, with CSS compliance via UseWebDefaults.
