# TimeWarp.Flexbox vs Yoga Layout Engine - Comprehensive Comparison Report

**Generated:** December 6, 2025  
**Yoga Version:** Latest (main branch)  
**TimeWarp.Flexbox Version:** Development

---

## Executive Summary

TimeWarp.Flexbox is a C# implementation inspired by Facebook's Yoga layout engine. This report provides a detailed class-by-class, method-by-method, and property-by-property comparison, highlighting deviations and their likely rationales.

---

## 1. Enumerations Comparison

### 1.1 Unit / YGUnit

| Yoga (YGUnit) | TimeWarp.Flexbox (Unit) | Status | Notes |
|---------------|-------------------------|--------|-------|
| `YGUnitUndefined` | `Undefined` | **Match** | |
| `YGUnitPoint` | `Point` | **Match** | |
| `YGUnitPercent` | `Percent` | **Match** | |
| `YGUnitAuto` | `Auto` | **Match** | |
| `YGUnitMaxContent` | *Missing* | **DEVIATION** | CSS intrinsic sizing |
| `YGUnitFitContent` | *Missing* | **DEVIATION** | CSS intrinsic sizing |
| `YGUnitStretch` | *Missing* | **DEVIATION** | CSS intrinsic sizing |

**Deviation Reason:** TimeWarp.Flexbox focuses on core flexbox functionality. MaxContent, FitContent, and Stretch are newer CSS intrinsic sizing keywords that Yoga added for enhanced web compatibility.

---

### 1.2 Direction / YGDirection

| Yoga (YGDirection) | TimeWarp.Flexbox (Direction) | Status |
|--------------------|------------------------------|--------|
| `YGDirectionInherit` | `Inherit` | **Match** |
| `YGDirectionLTR` | `Ltr` | **Match** (casing) |
| `YGDirectionRTL` | `Rtl` | **Match** (casing) |

**No deviations** - Only naming convention differs (C# uses PascalCase).

---

### 1.3 FlexDirection / YGFlexDirection

| Yoga (YGFlexDirection) | TimeWarp.Flexbox (FlexDirection) | Status | Notes |
|------------------------|----------------------------------|--------|-------|
| `YGFlexDirectionColumn` | `Column` | **Match** | |
| `YGFlexDirectionColumnReverse` | `ColumnReverse` | **Match** | |
| `YGFlexDirectionRow` | `Row` | **Match** | |
| `YGFlexDirectionRowReverse` | `RowReverse` | **Match** | |

**No deviations** - Complete match.

---

### 1.4 Alignment Enums

#### YGAlign vs AlignItems/AlignSelf/AlignContent

Yoga uses a single `YGAlign` enum for all alignment properties. TimeWarp.Flexbox splits this into three separate enums for type safety:

| Yoga (YGAlign) | TimeWarp AlignItems | TimeWarp AlignSelf | TimeWarp AlignContent | Notes |
|----------------|--------------------|--------------------|----------------------|-------|
| `YGAlignAuto` | *N/A* | `Auto` | *N/A* | Only valid for align-self |
| `YGAlignFlexStart` | `FlexStart` | `FlexStart` | `FlexStart` | **Match** |
| `YGAlignCenter` | `Center` | `Center` | `Center` | **Match** |
| `YGAlignFlexEnd` | `FlexEnd` | `FlexEnd` | `FlexEnd` | **Match** |
| `YGAlignStretch` | `Stretch` | `Stretch` | `Stretch` | **Match** |
| `YGAlignBaseline` | `Baseline` | `Baseline` | *N/A* | Not valid for align-content |
| `YGAlignSpaceBetween` | *N/A* | *N/A* | `SpaceBetween` | Only for align-content |
| `YGAlignSpaceAround` | *N/A* | *N/A* | `SpaceAround` | Only for align-content |
| `YGAlignSpaceEvenly` | *N/A* | *N/A* | *Missing* | **DEVIATION** |

**Deviation Reason:** 
1. **Type Safety:** TimeWarp uses separate enums to prevent invalid combinations at compile time (e.g., `SpaceBetween` is not valid for `align-items`).
2. **Missing SpaceEvenly in AlignContent:** Oversight - should be added for CSS spec compliance.

---

### 1.5 JustifyContent / YGJustify

| Yoga (YGJustify) | TimeWarp.Flexbox (JustifyContent) | Status |
|------------------|-----------------------------------|--------|
| `YGJustifyFlexStart` | `FlexStart` | **Match** |
| `YGJustifyCenter` | `Center` | **Match** |
| `YGJustifyFlexEnd` | `FlexEnd` | **Match** |
| `YGJustifySpaceBetween` | `SpaceBetween` | **Match** |
| `YGJustifySpaceAround` | `SpaceAround` | **Match** |
| `YGJustifySpaceEvenly` | `SpaceEvenly` | **Match** |

**No deviations** - Complete match.

---

### 1.6 Display / YGDisplay

| Yoga (YGDisplay) | TimeWarp.Flexbox (Display) | Status | Notes |
|------------------|---------------------------|--------|-------|
| `YGDisplayFlex` | `Flex` | **Match** | |
| `YGDisplayNone` | `None` | **Match** | |
| `YGDisplayContents` | *Missing* | **DEVIATION** | CSS `display: contents` |

**Deviation Reason:** `display: contents` is a newer CSS feature that makes an element's children behave as if they were direct children of the element's parent. This is a complex feature that may not be needed for typical flexbox use cases.

---

### 1.7 Edge / YGEdge

| Yoga (YGEdge) | TimeWarp.Flexbox (Edge) | Status |
|---------------|-------------------------|--------|
| `YGEdgeLeft` | `Left` | **Match** |
| `YGEdgeTop` | `Top` | **Match** |
| `YGEdgeRight` | `Right` | **Match** |
| `YGEdgeBottom` | `Bottom` | **Match** |
| `YGEdgeStart` | `Start` | **Match** |
| `YGEdgeEnd` | `End` | **Match** |
| `YGEdgeHorizontal` | `Horizontal` | **Match** |
| `YGEdgeVertical` | `Vertical` | **Match** |
| `YGEdgeAll` | `All` | **Match** |

**No deviations** - Complete match.

---

### 1.8 Overflow / YGOverflow

| Yoga (YGOverflow) | TimeWarp.Flexbox (Overflow) | Status |
|-------------------|---------------------------|--------|
| `YGOverflowVisible` | `Visible` | **Match** |
| `YGOverflowHidden` | `Hidden` | **Match** |
| `YGOverflowScroll` | `Scroll` | **Match** |

**No deviations** - Complete match.

---

### 1.9 PositionType / YGPositionType

| Yoga (YGPositionType) | TimeWarp.Flexbox (PositionType) | Status | Notes |
|-----------------------|--------------------------------|--------|-------|
| `YGPositionTypeStatic` | *Missing* | **DEVIATION** | CSS `position: static` |
| `YGPositionTypeRelative` | `Relative` | **Match** | |
| `YGPositionTypeAbsolute` | `Absolute` | **Match** | |

**Deviation Reason:** `position: static` is the CSS default where elements ignore inset properties. TimeWarp treats `Relative` as the default, which matches Yoga's historical behavior. The `Static` position type was added to Yoga more recently for stricter CSS compliance.

---

### 1.10 FlexWrap / YGWrap

| Yoga (YGWrap) | TimeWarp.Flexbox (FlexWrap) | Status |
|---------------|---------------------------|--------|
| `YGWrapNoWrap` | `NoWrap` | **Match** |
| `YGWrapWrap` | `Wrap` | **Match** |
| `YGWrapWrapReverse` | `WrapReverse` | **Match** |

**No deviations** - Complete match.

---

### 1.11 BoxSizing / YGBoxSizing

| Yoga (YGBoxSizing) | TimeWarp.Flexbox (BoxSizing) | Status |
|--------------------|----------------------------|--------|
| `YGBoxSizingBorderBox` | `BorderBox` | **Match** |
| `YGBoxSizingContentBox` | `ContentBox` | **Match** |

**No deviations** - Complete match.

---

### 1.12 MeasureMode / YGMeasureMode

| Yoga (YGMeasureMode) | TimeWarp.Flexbox (MeasureMode) | Status |
|---------------------|-------------------------------|--------|
| `YGMeasureModeUndefined` | `Undefined` | **Match** |
| `YGMeasureModeExactly` | `Exactly` | **Match** |
| `YGMeasureModeAtMost` | `AtMost` | **Match** |

**No deviations** - Complete match.

---

### 1.13 Missing Yoga Enums

| Yoga Enum | TimeWarp.Flexbox | Notes |
|-----------|------------------|-------|
| `YGDimension` | *Implicit* | Width/Height handled directly |
| `YGErrata` | *Missing* | Backward compatibility flags |
| `YGExperimentalFeature` | *Missing* | Experimental feature toggles |
| `YGGutter` | *Missing* | Gap uses separate RowGap/ColumnGap |
| `YGLogLevel` | *Missing* | Logging infrastructure |
| `YGNodeType` | *Missing* | Default/Text node types |
| `YGPhysicalEdge` | *Internal only* | Used internally, not exposed |

**Deviation Reasons:**
- **YGDimension:** TimeWarp uses Width/Height properties directly rather than an enum.
- **YGErrata:** Errata are backward-compatibility flags for Yoga 1.x behavior. TimeWarp targets modern flexbox spec.
- **YGExperimentalFeature:** Not needed - TimeWarp implements stable features.
- **YGGutter:** TimeWarp uses separate `RowGap`, `ColumnGap`, and `Gap` properties instead of indexed access.
- **YGLogLevel/YGNodeType:** Logging and node type distinctions not implemented.

---

## 2. Value Types Comparison

### 2.1 YGValue vs FlexValue

| Aspect | Yoga (YGValue) | TimeWarp.Flexbox (FlexValue) | Status |
|--------|---------------|------------------------------|--------|
| Type | C struct | C# readonly struct | **Match** |
| Value field | `float value` | `float Value` | **Match** |
| Unit field | `YGUnit unit` | `Unit Unit` | **Match** |
| Undefined constant | `YGValueUndefined` | `FlexValue.Undefined` | **Match** |
| Auto constant | `YGValueAuto` | `FlexValue.Auto` | **Match** |
| Zero constant | `YGValueZero` | *Missing* | **DEVIATION** |
| Point factory | *implicit* | `FlexValue.Point(float)` | **Enhanced** |
| Percent factory | *implicit* | `FlexValue.Percent(float)` | **Enhanced** |
| IsUndefined check | `YGFloatIsUndefined()` | `.IsUndefined` | **Match** |
| IsAuto check | *manual* | `.IsAuto` | **Enhanced** |
| IsDefined check | *manual* | `.IsDefined` | **Enhanced** |
| Equality | `operator==` | `IEquatable<FlexValue>` | **Match** |
| Negation | `operator-` | *Missing* | **DEVIATION** |

**Deviations:**
1. **Missing Zero constant:** Minor - can use `FlexValue.Point(0)`.
2. **Missing negation operator:** Yoga allows `-YGValue` for negating values.

**Enhancements:**
- TimeWarp provides factory methods (`Point`, `Percent`) for cleaner API.
- Boolean properties (`IsUndefined`, `IsAuto`, `IsDefined`) for cleaner checks.

---

### 2.2 EdgeValues vs Yoga Edge Storage

| Aspect | Yoga | TimeWarp.Flexbox | Notes |
|--------|------|------------------|-------|
| Storage | `std::array<StyleValueHandle, 9>` | `EdgeValues<T>` generic struct | Different approach |
| Edge count | 9 (indexed by Edge enum) | 9 fields | **Match** |
| Cascade resolution | In `Style.h` methods | `ComputedLeft/Right/Top/Bottom` | **Match** |
| RTL support | Computed at resolution | `isRtl` parameter | **Match** |
| Start/End mapping | Direction-aware | Direction-aware | **Match** |
| Generic support | N/A | `EdgeValues<T>` | **Enhanced** |

**Enhancement:** TimeWarp's `EdgeValues<T>` is generic, allowing reuse for `FlexValue` (margin, padding, position) and `float` (border).

---

## 3. Node Class Comparison

### 3.1 YGNode / yoga::Node vs FlexNode

#### Core Properties

| Yoga Property | TimeWarp Property | Status | Notes |
|---------------|-------------------|--------|-------|
| `context_` | `Context` | **Match** | User data storage |
| `hasNewLayout_` | *Missing* | **DEVIATION** | Layout change flag |
| `isReferenceBaseline_` | *Missing* | **DEVIATION** | Baseline reference flag |
| `isDirty_` | `IsDirty` | **Match** | Dirty tracking |
| `alwaysFormsContainingBlock_` | *Missing* | **DEVIATION** | CSS containing block |
| `nodeType_` | *Missing* | **DEVIATION** | Default/Text node |
| `measureFunc_` | `MeasureFunc` | **Match** | Measurement callback |
| `baselineFunc_` | `BaselineFunc` | **Match** | Baseline callback |
| `dirtiedFunc_` | `DirtiedFunc` | **Match** | Dirty notification |
| `style_` | *Properties* | **Different** | Style is embedded |
| `layout_` | `Layout` | **Match** | Layout results |
| `lineIndex_` | *Internal* | **Match** | Flex line index |
| `contentsChildrenCount_` | *Missing* | **DEVIATION** | display:contents |
| `owner_` | `Parent` | **Match** (renamed) | Parent node |
| `children_` | `ChildrenInternal` | **Match** | Child nodes |
| `config_` | `Config` | **Match** | Configuration |
| `processedDimensions_` | *Computed* | **Match** | Resolved dimensions |

**Deviations:**
1. **hasNewLayout:** Yoga tracks whether layout has changed since last read. TimeWarp lacks this optimization hint.
2. **isReferenceBaseline:** Used for baseline alignment reference selection.
3. **alwaysFormsContainingBlock:** For CSS transforms creating containing blocks.
4. **nodeType:** Yoga distinguishes Default vs Text nodes for layout rounding.
5. **contentsChildrenCount:** For `display: contents` support.

---

#### Child Management Methods

| Yoga Method | TimeWarp Method | Status | Notes |
|-------------|-----------------|--------|-------|
| `YGNodeInsertChild()` | `InsertChild()` | **Match** | |
| `YGNodeRemoveChild()` | `RemoveChild()` | **Match** | |
| `YGNodeRemoveAllChildren()` | `RemoveAllChildren()` | **Match** | |
| `YGNodeSwapChild()` | `ReplaceChild()` | **Match** (renamed) | |
| `YGNodeGetChild()` | `GetChild()` | **Match** | |
| `YGNodeGetChildCount()` | `ChildCount` | **Match** (property) | |
| `YGNodeGetOwner()` | `Parent` | **Match** (renamed) | |
| `YGNodeGetParent()` | `Parent` | **Match** | |
| `YGNodeSetChildren()` | *Missing* | **DEVIATION** | Bulk set |
| `YGNodeFree()` | *N/A (GC)* | **N/A** | C# uses GC |
| `YGNodeFreeRecursive()` | *N/A (GC)* | **N/A** | C# uses GC |
| `YGNodeClone()` | `Clone()` | **Match** | |
| `YGNodeReset()` | *Missing* | **DEVIATION** | Reset to default |

**Deviations:**
1. **SetChildren:** Yoga allows bulk setting of children array.
2. **Reset:** Yoga can reset a node to initial state.

---

#### Measure/Baseline Methods

| Yoga Method | TimeWarp Method | Status |
|-------------|-----------------|--------|
| `YGNodeSetMeasureFunc()` | `MeasureFunc` setter | **Match** |
| `YGNodeHasMeasureFunc()` | `HasMeasureFunc` | **Match** |
| `YGNodeSetBaselineFunc()` | `BaselineFunc` setter | **Match** |
| `YGNodeHasBaselineFunc()` | *Missing* | **DEVIATION** |
| `YGNodeMarkDirty()` | `MarkDirty()` | **Match** |
| `YGNodeIsDirty()` | `IsDirty` | **Match** |

---

#### Layout Calculation

| Yoga Method | TimeWarp Method | Status | Notes |
|-------------|-----------------|--------|-------|
| `YGNodeCalculateLayout()` | `CalculateLayout()` | **Match** | Entry point |
| `YGNodeGetHasNewLayout()` | *Missing* | **DEVIATION** | |
| `YGNodeSetHasNewLayout()` | *Missing* | **DEVIATION** | |

---

### 3.2 Style Properties Comparison

| Yoga Style Property | TimeWarp Property | Default (Yoga) | Default (TW) | Status |
|--------------------|-------------------|----------------|--------------|--------|
| `direction()` | `Direction` | Inherit | Inherit | **Match** |
| `flexDirection()` | `FlexDirection` | **Column** | **Row** | **DEVIATION** |
| `justifyContent()` | `JustifyContent` | FlexStart | FlexStart | **Match** |
| `alignContent()` | `AlignContent` | FlexStart | FlexStart | **Match** |
| `alignItems()` | `AlignItems` | Stretch | Stretch | **Match** |
| `alignSelf()` | `AlignSelf` | Auto | Auto | **Match** |
| `positionType()` | `PositionType` | Relative | Relative | **Match** |
| `flexWrap()` | `FlexWrap` | NoWrap | NoWrap | **Match** |
| `overflow()` | `Overflow` | Visible | Visible | **Match** |
| `display()` | `Display` | Flex | Flex | **Match** |
| `flex()` | *Missing* | Undefined | *N/A* | **DEVIATION** |
| `flexGrow()` | `FlexGrow` | 0 | 0 | **Match** |
| `flexShrink()` | `FlexShrink` | 0 (web: 1) | 1 | **DEVIATION** |
| `flexBasis()` | `FlexBasis` | Auto | Auto | **Match** |
| `dimension(Width)` | `Width` | Auto | Undefined | **DEVIATION** |
| `dimension(Height)` | `Height` | Auto | Undefined | **DEVIATION** |
| `minDimension()` | `MinWidth/MinHeight` | Undefined | Undefined | **Match** |
| `maxDimension()` | `MaxWidth/MaxHeight` | Undefined | Undefined | **Match** |
| `margin()` | `Margin` | Undefined | Undefined | **Match** |
| `padding()` | `Padding` | Undefined | Undefined | **Match** |
| `border()` | `Border` | Undefined | Undefined | **Match** |
| `position()` | `Position` | Undefined | Undefined | **Match** |
| `gap()` | `Gap/RowGap/ColumnGap` | Undefined | 0 | **DEVIATION** |
| `aspectRatio()` | `AspectRatio` | Undefined | null | **Match** |
| `boxSizing()` | `BoxSizing` | BorderBox | BorderBox | **Match** |

**Critical Deviations:**

1. **FlexDirection Default:**
   - Yoga: `Column` (native app convention)
   - TimeWarp: `Row` (CSS web default)
   - **Reason:** TimeWarp aligns with CSS flexbox spec where `flex-direction: row` is default.

2. **FlexShrink Default:**
   - Yoga: `0` (unless `useWebDefaults`)
   - TimeWarp: `1` (CSS default)
   - **Reason:** TimeWarp follows CSS spec where `flex-shrink: 1` is default.

3. **Width/Height Default:**
   - Yoga: `Auto`
   - TimeWarp: `Undefined`
   - **Reason:** Semantic difference - both behave as "size to content."

4. **Missing `flex` property:**
   - Yoga supports shorthand `flex` property that sets grow/shrink/basis.
   - TimeWarp requires setting properties individually.
   - **Reason:** Simplification - C# doesn't have CSS-style shorthand parsing.

5. **Gap Default:**
   - Yoga: `Undefined` (no gap)
   - TimeWarp: `0` (explicit zero)
   - **Reason:** Clearer semantics for C# API.

---

### 3.3 Layout Results Comparison

| Yoga LayoutResults | TimeWarp LayoutResult | Status | Notes |
|-------------------|----------------------|--------|-------|
| `direction()` | `Direction` | **Different Type** | FlexDirection vs Direction |
| `hadOverflow()` | `HadOverflow` | **Match** | |
| `dimension(Width)` | `Width` | **Match** | |
| `dimension(Height)` | `Height` | **Match** | |
| `position(Left)` | `Left` | **Match** | |
| `position(Top)` | `Top` | **Match** | |
| `position(Right)` | `Right` (computed) | **Different** | Computed property |
| `position(Bottom)` | `Bottom` (computed) | **Different** | Computed property |
| `margin(edge)` | *Missing* | **DEVIATION** | Resolved margins |
| `border(edge)` | `Border*` | **Match** | |
| `padding(edge)` | `Padding*` | **Match** | |
| `measuredDimension()` | *Missing* | **DEVIATION** | Pre-round dimensions |
| `rawDimension()` | *Missing* | **DEVIATION** | Raw dimensions |
| `computedFlexBasis` | *Internal* | **Match** | |
| `computedFlexBasisGeneration` | *Internal* | **Match** | |
| `generationCount` | `Generation` (on node) | **Match** | |
| `configVersion` | *Missing* | **DEVIATION** | Config change tracking |
| `lastOwnerDirection` | *Internal* | **Match** | |
| `cachedMeasurements` | `LayoutCache` (separate) | **Match** | |
| `nextCachedMeasurementsIndex` | *Internal* | **Match** | |

**Deviations:**
1. **Resolved margins not stored:** TimeWarp computes margins on demand rather than storing them.
2. **Missing measured/raw dimensions:** Used for pixel grid rounding in Yoga.

---

## 4. Configuration Comparison

### 4.1 YGConfig vs FlexConfig

| Yoga Config Property | TimeWarp Config Property | Status | Notes |
|---------------------|-------------------------|--------|-------|
| `useWebDefaults()` | `UseWebDefaults` | **Match** | |
| `pointScaleFactor()` | `PointScaleFactor` | **Match** | |
| `errata()` | `UseErrata` | **Simplified** | Boolean vs flags |
| `logger()` | *Missing* | **DEVIATION** | Logging callback |
| `context()` | *Missing* | **DEVIATION** | User context |
| `cloneNodeCallback()` | *Missing* | **DEVIATION** | Node cloning |
| `experimentalFeatures` | *Missing* | **DEVIATION** | Experimental flags |
| `getDefault()` | `Default` | **Match** | |

**Deviations:**
1. **Errata:** Yoga uses flags (`YGErrataNone`, `YGErrataClassic`, etc.) for precise backward compatibility. TimeWarp uses a simple boolean.
2. **Logger:** Yoga supports custom logging; TimeWarp relies on standard .NET diagnostics.
3. **CloneNodeCallback:** Yoga allows custom cloning logic; TimeWarp uses standard `Clone()`.

---

## 5. Layout Algorithm Comparison

### 5.1 Core Algorithm Implementation

| Aspect | Yoga | TimeWarp | Status |
|--------|------|----------|--------|
| Main entry | `calculateLayout()` | `FlexLayoutEngine.CalculateLayout()` | **Match** |
| Direction resolution | `resolveDirection()` | `ResolvedDirection` property | **Match** |
| Flex line collection | Internal | `FlexLines.CollectLines()` | **Match** |
| Flex basis resolution | `processFlexBasis()` | In `CalculateMainAxisSizes()` | **Match** |
| Flexible length resolution | Iterative algorithm | `ResolveFlexibleLengths()` | **Match** |
| Cross axis sizing | Multi-pass | `CalculateCrossAxisSizes()` | **Match** |
| Alignment calculation | Per CSS spec | `CalculateAlignItemsOffset()` | **Match** |
| Absolute positioning | Separate pass | `LayoutAbsoluteChild()` | **Match** |
| Pixel grid rounding | `PixelGrid.h` | `PixelGrid.cs` | **Match** |
| Caching | `CachedMeasurement` | `LayoutCache` | **Match** |

---

### 5.2 Algorithm Differences

1. **Organization:**
   - Yoga: Algorithm split across multiple files (`CalculateLayout.cpp`, `FlexDirection.h`, etc.)
   - TimeWarp: Algorithm in single `FlexLayoutEngine.cs` (~1500 lines)

2. **Recursion:**
   - Both use recursive layout for nested containers
   - TimeWarp may have slight differences in when recursion occurs

3. **Caching:**
   - Yoga: Up to 8 cached measurements per node
   - TimeWarp: Single cached layout entry

---

## 6. API Design Differences

### 6.1 C++ vs C# Idioms

| Aspect | Yoga (C++) | TimeWarp (C#) |
|--------|-----------|---------------|
| Memory management | Manual (`YGNodeFree`) | Garbage collected |
| Property access | Getter/setter functions | C# properties |
| Callbacks | Function pointers | Delegates |
| Null handling | Raw pointers | Nullable reference types |
| Value types | C structs | C# `readonly struct` |
| Collections | `std::vector` | `List<T>` |
| Error handling | Assertions | Exceptions |

### 6.2 API Style

| Aspect | Yoga | TimeWarp |
|--------|------|----------|
| Node creation | `YGNodeNew()` / `YGNodeNewWithConfig()` | `new FlexNode()` |
| Style setting | `YGNodeStyleSetWidth()` | `node.Width = value` |
| Layout reading | `YGNodeLayoutGetLeft()` | `node.Layout.Left` |
| Child access | `YGNodeGetChild(node, index)` | `node.GetChild(index)` or `node.Children[index]` |
| Dirty marking | `YGNodeMarkDirty()` | `node.MarkDirty()` |

---

## 7. Feature Gaps

### 7.1 Missing from TimeWarp.Flexbox

| Feature | Yoga Support | Impact | Priority |
|---------|-------------|--------|----------|
| `display: contents` | Yes | Medium | Low |
| `position: static` | Yes | Low | Low |
| Intrinsic sizes (max-content, etc.) | Yes | Medium | Medium |
| SpaceEvenly for align-content | Yes | Low | High (easy fix) |
| HasNewLayout flag | Yes | Low | Low |
| Node type (Default/Text) | Yes | Low | Low |
| Errata flags | Yes | Low | Low |
| Custom logger | Yes | Low | Low |
| Experimental features | Yes | Low | Low |
| Multiple cached measurements | Yes | Medium | Medium |
| Resolved margins in layout | Yes | Low | Low |

### 7.2 TimeWarp Enhancements

| Feature | Description |
|---------|-------------|
| Generic EdgeValues | Reusable for different value types |
| Factory methods | `FlexValue.Point()`, `FlexValue.Percent()` |
| Boolean properties | `IsUndefined`, `IsAuto`, `IsDefined` |
| Separate alignment enums | Type-safe alignment at compile time |
| Fluent API extensions | Method chaining for configuration |
| Clone method | Deep cloning with single method |

---

## 8. Default Value Summary

| Property | Yoga Default | TimeWarp Default | CSS Default | Notes |
|----------|--------------|------------------|-------------|-------|
| flex-direction | Column | **Row** | row | TW matches CSS |
| flex-wrap | NoWrap | NoWrap | nowrap | All match |
| justify-content | FlexStart | FlexStart | flex-start | All match |
| align-items | Stretch | Stretch | stretch | All match |
| align-content | FlexStart | FlexStart | stretch | Yoga differs from CSS |
| flex-grow | 0 | 0 | 0 | All match |
| flex-shrink | 0 | **1** | 1 | TW matches CSS |
| flex-basis | Auto | Auto | auto | All match |
| width/height | Auto | Undefined | auto | Semantic difference |
| position | Relative | Relative | static | Both differ from CSS |

---

## 9. Recommendations

### High Priority
1. Add `SpaceEvenly` to `AlignContent` enum
2. Consider adding `position: static` support
3. Document default value differences from Yoga

### Medium Priority
1. Add intrinsic sizing keywords (max-content, fit-content, stretch)
2. Implement multiple cached measurements for performance
3. Add `flex` shorthand property support

### Low Priority
1. Add `display: contents` support
2. Implement `HasNewLayout` optimization flag
3. Add node type distinction (Default/Text)
4. Implement full errata compatibility flags

---

## 10. Conclusion

TimeWarp.Flexbox provides a solid C# implementation of the CSS Flexbox layout algorithm, closely following Yoga's architecture while making appropriate adaptations for the .NET ecosystem. Key differences include:

1. **Default values** align more closely with CSS spec (flex-direction: row, flex-shrink: 1)
2. **Type safety** is enhanced through separate alignment enums
3. **API design** follows C# conventions (properties, delegates, garbage collection)
4. **Some advanced features** are omitted (display: contents, intrinsic sizing, errata)

The implementation is suitable for most flexbox layout scenarios and provides a clean, type-safe API for .NET developers.
