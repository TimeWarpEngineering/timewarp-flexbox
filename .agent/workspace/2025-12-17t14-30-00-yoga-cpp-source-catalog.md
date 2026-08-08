# Comprehensive Catalog of Facebook Yoga C++ Source Code

**Generated:** 2025-12-17  
**Source:** `/home/steventcramer/worktrees/github.com/facebook/yoga/main/yoga/`  
**Purpose:** Reference for verifying C# port completeness

---

## Executive Summary

This document provides a complete catalog of all classes, structs, enums, properties, and methods in the Facebook Yoga C++ flexbox layout library. It serves as a reference to verify that a C# port has all necessary components.

## Scope

- All header files (.h) in the yoga directory
- All implementation files (.cpp) 
- Focus on algorithm, node, style, numeric, config, and event subsystems

## Methodology

Direct file reading and analysis of the C++ source code in the Facebook Yoga repository.

---

## 1. ENUMS (yoga/enums/)

### Align (Align.h)
```cpp
enum class Align : uint8_t {
  Auto = 0,
  FlexStart = 1,
  Center = 2,
  FlexEnd = 3,
  Stretch = 4,
  Baseline = 5,
  SpaceBetween = 6,
  SpaceAround = 7,
  SpaceEvenly = 8
};
// Ordinal count: 9
```

### BoxSizing (BoxSizing.h)
```cpp
enum class BoxSizing : uint8_t {
  BorderBox = 0,
  ContentBox = 1
};
// Ordinal count: 2
```

### Dimension (Dimension.h)
```cpp
enum class Dimension : uint8_t {
  Width = 0,
  Height = 1
};
// Ordinal count: 2
```

### Direction (Direction.h)
```cpp
enum class Direction : uint8_t {
  Inherit = 0,
  LTR = 1,
  RTL = 2
};
// Ordinal count: 3
```

### Display (Display.h)
```cpp
enum class Display : uint8_t {
  Flex = 0,
  None = 1,
  Contents = 2
};
// Ordinal count: 3
```

### Edge (Edge.h)
```cpp
enum class Edge : uint8_t {
  Left = 0,
  Top = 1,
  Right = 2,
  Bottom = 3,
  Start = 4,
  End = 5,
  Horizontal = 6,
  Vertical = 7,
  All = 8
};
// Ordinal count: 9
```

### Errata (Errata.h)
```cpp
enum class Errata : uint32_t {
  None = 0,
  StretchFlexBasis = 1,
  AbsolutePositionWithoutInsetsExcludesPadding = 2,
  AbsolutePercentAgainstInnerSize = 4,
  All = 2147483647,
  Classic = 2147483646
};
// Supports bitwise operators (|, &, ~)
```

### ExperimentalFeature (ExperimentalFeature.h)
```cpp
enum class ExperimentalFeature : uint8_t {
  WebFlexBasis = 0
};
// Ordinal count: 1
```

### FlexDirection (FlexDirection.h)
```cpp
enum class FlexDirection : uint8_t {
  Column = 0,
  ColumnReverse = 1,
  Row = 2,
  RowReverse = 3
};
// Ordinal count: 4
```

### Gutter (Gutter.h)
```cpp
enum class Gutter : uint8_t {
  Column = 0,
  Row = 1,
  All = 2
};
// Ordinal count: 3
```

### Justify (Justify.h)
```cpp
enum class Justify : uint8_t {
  FlexStart = 0,
  Center = 1,
  FlexEnd = 2,
  SpaceBetween = 3,
  SpaceAround = 4,
  SpaceEvenly = 5
};
// Ordinal count: 6
```

### LogLevel (LogLevel.h)
```cpp
enum class LogLevel : uint8_t {
  Error = 0,
  Warn = 1,
  Info = 2,
  Debug = 3,
  Verbose = 4,
  Fatal = 5
};
// Ordinal count: 6
```

### MeasureMode (MeasureMode.h)
```cpp
enum class MeasureMode : uint8_t {
  Undefined = 0,
  Exactly = 1,
  AtMost = 2
};
// Ordinal count: 3
```

### NodeType (NodeType.h)
```cpp
enum class NodeType : uint8_t {
  Default = 0,
  Text = 1
};
// Ordinal count: 2
```

### Overflow (Overflow.h)
```cpp
enum class Overflow : uint8_t {
  Visible = 0,
  Hidden = 1,
  Scroll = 2
};
// Ordinal count: 3
```

### PhysicalEdge (PhysicalEdge.h)
```cpp
enum class PhysicalEdge : uint32_t {
  Left = 0,
  Top = 1,
  Right = 2,
  Bottom = 3
};
```

### PositionType (PositionType.h)
```cpp
enum class PositionType : uint8_t {
  Static = 0,
  Relative = 1,
  Absolute = 2
};
// Ordinal count: 3
```

### Unit (Unit.h)
```cpp
enum class Unit : uint8_t {
  Undefined = 0,
  Point = 1,
  Percent = 2,
  Auto = 3,
  MaxContent = 4,
  FitContent = 5,
  Stretch = 6
};
// Ordinal count: 7
```

### Wrap (Wrap.h)
```cpp
enum class Wrap : uint8_t {
  NoWrap = 0,
  Wrap = 1,
  WrapReverse = 2
};
// Ordinal count: 3
```

---

## 2. NUMERIC TYPES (yoga/numeric/)

### FloatOptional (FloatOptional.h)

**Struct representing an optional float value using NaN for undefined**

```cpp
struct FloatOptional {
private:
  float value_ = std::numeric_limits<float>::quiet_NaN();

public:
  // Constructors
  explicit constexpr FloatOptional(float value);
  constexpr FloatOptional() = default;

  // Methods
  constexpr float unwrap() const;
  constexpr float unwrapOrDefault(float defaultValue) const;
  constexpr bool isUndefined() const;
  constexpr bool isDefined() const;
};

// Operators (free functions)
constexpr bool operator==(FloatOptional lhs, FloatOptional rhs);
constexpr bool operator!=(FloatOptional lhs, FloatOptional rhs);
constexpr bool operator==(FloatOptional lhs, float rhs);
constexpr bool operator!=(FloatOptional lhs, float rhs);
constexpr bool operator==(float lhs, FloatOptional rhs);
constexpr bool operator!=(float lhs, FloatOptional rhs);
constexpr FloatOptional operator+(FloatOptional lhs, FloatOptional rhs);
constexpr bool operator>(FloatOptional lhs, FloatOptional rhs);
constexpr bool operator<(FloatOptional lhs, FloatOptional rhs);
constexpr bool operator>=(FloatOptional lhs, FloatOptional rhs);
constexpr bool operator<=(FloatOptional lhs, FloatOptional rhs);
constexpr FloatOptional maxOrDefined(FloatOptional lhs, FloatOptional rhs);
inline bool inexactEquals(FloatOptional lhs, FloatOptional rhs);
```

### Comparison.h - Utility Functions

```cpp
constexpr bool isUndefined(std::floating_point auto value);
constexpr bool isDefined(std::floating_point auto value);
constexpr bool isinf(auto value);
constexpr auto maxOrDefined(std::floating_point auto a, std::floating_point auto b);
constexpr auto minOrDefined(std::floating_point auto a, std::floating_point auto b);
inline bool inexactEquals(float a, float b);  // epsilon = 0.0001f
inline bool inexactEquals(double a, double b);
template <std::size_t Size, typename ElementT>
bool inexactEquals(const std::array<ElementT, Size>& val1, const std::array<ElementT, Size>& val2);
```

---

## 3. STYLE TYPES (yoga/style/)

### StyleLength (StyleLength.h)

**Represents CSS length values (points, percent, auto, undefined)**

```cpp
class StyleLength {
public:
  constexpr StyleLength() = default;

  // Static factory methods
  constexpr static StyleLength points(float value);
  constexpr static StyleLength percent(float value);
  constexpr static StyleLength ofAuto();
  constexpr static StyleLength undefined();

  // Query methods
  constexpr bool isAuto() const;
  constexpr bool isUndefined() const;
  constexpr bool isPoints() const;
  constexpr bool isPercent() const;
  constexpr bool isDefined() const;
  constexpr FloatOptional value() const;
  constexpr FloatOptional resolve(float referenceLength);

  // Operators
  explicit constexpr operator YGValue() const;
  constexpr bool operator==(const StyleLength& rhs) const;
  constexpr bool inexactEquals(const StyleLength& other) const;

private:
  FloatOptional value_{};
  Unit unit_{Unit::Undefined};
};
```

### StyleSizeLength (StyleSizeLength.h)

**Like StyleLength but also supports MaxContent, FitContent, Stretch**

```cpp
class StyleSizeLength {
public:
  constexpr StyleSizeLength() = default;

  // Static factory methods
  constexpr static StyleSizeLength points(float value);
  constexpr static StyleSizeLength percent(float value);
  constexpr static StyleSizeLength ofAuto();
  constexpr static StyleSizeLength ofMaxContent();
  constexpr static StyleSizeLength ofFitContent();
  constexpr static StyleSizeLength ofStretch();
  constexpr static StyleSizeLength undefined();

  // Query methods
  constexpr bool isAuto() const;
  constexpr bool isMaxContent() const;
  constexpr bool isFitContent() const;
  constexpr bool isStretch() const;
  constexpr bool isUndefined() const;
  constexpr bool isDefined() const;
  constexpr bool isPoints() const;
  constexpr bool isPercent() const;
  constexpr FloatOptional value() const;
  constexpr FloatOptional resolve(float referenceLength);

  // Operators
  explicit constexpr operator YGValue() const;
  constexpr bool operator==(const StyleSizeLength& rhs) const;
  constexpr bool inexactEquals(const StyleSizeLength& other) const;

private:
  FloatOptional value_{};
  Unit unit_{Unit::Undefined};
};
```

### StyleValueHandle (StyleValueHandle.h)

**16-bit handle to a length or number in a style (for compact storage)**

```cpp
class StyleValueHandle {
public:
  static constexpr StyleValueHandle ofAuto();
  constexpr bool isUndefined() const;
  constexpr bool isDefined() const;
  constexpr bool isAuto() const;

private:
  friend class StyleValuePool;

  enum class Type : uint8_t {
    Undefined, Point, Percent, Number, Auto, Keyword
  };
  enum class Keyword : uint8_t { MaxContent, FitContent, Stretch };

  constexpr bool isKeyword(Keyword keyword) const;
  constexpr Type type() const;
  constexpr void setType(Type handleType);
  constexpr uint16_t value() const;
  constexpr void setValue(uint16_t value);
  constexpr bool isValueIndexed() const;
  constexpr void setValueIsIndexed();

  uint16_t repr_{0};
};
```

### StyleValuePool (StyleValuePool.h)

**Compact storage for style values**

```cpp
class StyleValuePool {
public:
  void store(StyleValueHandle& handle, StyleLength length);
  void store(StyleValueHandle& handle, StyleSizeLength sizeValue);
  void store(StyleValueHandle& handle, FloatOptional number);
  
  StyleLength getLength(StyleValueHandle handle) const;
  StyleSizeLength getSize(StyleValueHandle handle) const;
  FloatOptional getNumber(StyleValueHandle handle) const;

private:
  SmallValueBuffer<4> buffer_;
};
```

### SmallValueBuffer (SmallValueBuffer.h)

**Fixed buffer with overflow to heap for compact value storage**

```cpp
template <size_t BufferSize>
class SmallValueBuffer {
public:
  SmallValueBuffer() = default;
  SmallValueBuffer(const SmallValueBuffer& other);
  SmallValueBuffer(SmallValueBuffer&& other) noexcept = default;

  uint16_t push(uint32_t value);
  uint16_t push(uint64_t value);
  [[nodiscard]] uint16_t replace(uint16_t index, uint32_t value);
  [[nodiscard]] uint16_t replace(uint16_t index, uint64_t value);
  uint32_t get32(uint16_t index) const;
  uint64_t get64(uint16_t index) const;

private:
  uint16_t count_{0};
  std::array<uint32_t, BufferSize> buffer_{};
  std::bitset<BufferSize> wideElements_;
  std::unique_ptr<Overflow> overflow_;
};
```

### Style (Style.h)

**Main style class containing all CSS flexbox properties**

```cpp
class Style {
public:
  using Length = StyleLength;
  using SizeLength = StyleSizeLength;

  // Constants
  static constexpr float DefaultFlexGrow = 0.0f;
  static constexpr float DefaultFlexShrink = 0.0f;
  static constexpr float WebDefaultFlexShrink = 1.0f;

  // Direction and Layout Mode Properties (get/set)
  Direction direction() const;
  void setDirection(Direction value);
  FlexDirection flexDirection() const;
  void setFlexDirection(FlexDirection value);
  Justify justifyContent() const;
  void setJustifyContent(Justify value);
  Align alignContent() const;
  void setAlignContent(Align value);
  Align alignItems() const;
  void setAlignItems(Align value);
  Align alignSelf() const;
  void setAlignSelf(Align value);
  PositionType positionType() const;
  void setPositionType(PositionType value);
  Wrap flexWrap() const;
  void setFlexWrap(Wrap value);
  Overflow overflow() const;
  void setOverflow(Overflow value);
  Display display() const;
  void setDisplay(Display value);
  BoxSizing boxSizing() const;
  void setBoxSizing(BoxSizing value);

  // Flex Properties
  FloatOptional flex() const;
  void setFlex(FloatOptional value);
  FloatOptional flexGrow() const;
  void setFlexGrow(FloatOptional value);
  FloatOptional flexShrink() const;
  void setFlexShrink(FloatOptional value);
  SizeLength flexBasis() const;
  void setFlexBasis(SizeLength value);

  // Edge Properties (margin, position, padding, border)
  Length margin(Edge edge) const;
  void setMargin(Edge edge, Length value);
  Length position(Edge edge) const;
  void setPosition(Edge edge, Length value);
  Length padding(Edge edge) const;
  void setPadding(Edge edge, Length value);
  Length border(Edge edge) const;
  void setBorder(Edge edge, Length value);

  // Gap Properties
  Length gap(Gutter gutter) const;
  void setGap(Gutter gutter, Length value);

  // Dimension Properties
  SizeLength dimension(Dimension axis) const;
  void setDimension(Dimension axis, SizeLength value);
  SizeLength minDimension(Dimension axis) const;
  void setMinDimension(Dimension axis, SizeLength value);
  SizeLength maxDimension(Dimension axis) const;
  void setMaxDimension(Dimension axis, SizeLength value);
  FloatOptional resolvedMinDimension(Direction dir, Dimension axis, float refLen, float ownerWidth) const;
  FloatOptional resolvedMaxDimension(Direction dir, Dimension axis, float refLen, float ownerWidth) const;

  // Aspect Ratio
  FloatOptional aspectRatio() const;
  void setAspectRatio(FloatOptional value);

  // Insets Query Methods
  bool horizontalInsetsDefined() const;
  bool verticalInsetsDefined() const;
  bool isFlexStartPositionDefined(FlexDirection axis, Direction direction) const;
  bool isFlexStartPositionAuto(FlexDirection axis, Direction direction) const;
  bool isInlineStartPositionDefined(FlexDirection axis, Direction direction) const;
  bool isInlineStartPositionAuto(FlexDirection axis, Direction direction) const;
  bool isFlexEndPositionDefined(FlexDirection axis, Direction direction) const;
  bool isFlexEndPositionAuto(FlexDirection axis, Direction direction) const;
  bool isInlineEndPositionDefined(FlexDirection axis, Direction direction) const;
  bool isInlineEndPositionAuto(FlexDirection axis, Direction direction) const;

  // Computed Position Methods
  float computeFlexStartPosition(FlexDirection axis, Direction direction, float axisSize) const;
  float computeInlineStartPosition(FlexDirection axis, Direction direction, float axisSize) const;
  float computeFlexEndPosition(FlexDirection axis, Direction direction, float axisSize) const;
  float computeInlineEndPosition(FlexDirection axis, Direction direction, float axisSize) const;

  // Computed Margin Methods
  float computeFlexStartMargin(FlexDirection axis, Direction direction, float widthSize) const;
  float computeInlineStartMargin(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexEndMargin(FlexDirection axis, Direction direction, float widthSize) const;
  float computeInlineEndMargin(FlexDirection axis, Direction direction, float widthSize) const;

  // Computed Border Methods
  float computeFlexStartBorder(FlexDirection axis, Direction direction) const;
  float computeInlineStartBorder(FlexDirection axis, Direction direction) const;
  float computeFlexEndBorder(FlexDirection axis, Direction direction) const;
  float computeInlineEndBorder(FlexDirection axis, Direction direction) const;

  // Computed Padding Methods
  float computeFlexStartPadding(FlexDirection axis, Direction direction, float widthSize) const;
  float computeInlineStartPadding(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexEndPadding(FlexDirection axis, Direction direction, float widthSize) const;
  float computeInlineEndPadding(FlexDirection axis, Direction direction, float widthSize) const;

  // Combined Padding and Border Methods
  float computeInlineStartPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexStartPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize) const;
  float computeInlineEndPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexEndPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize) const;
  float computePaddingAndBorderForDimension(Direction direction, Dimension dimension, float widthSize) const;

  // Axis-based Computation Methods
  float computeBorderForAxis(FlexDirection axis) const;
  float computeMarginForAxis(FlexDirection axis, float widthSize) const;
  float computeGapForAxis(FlexDirection axis, float ownerSize) const;

  // Auto Margin Check Methods
  bool flexStartMarginIsAuto(FlexDirection axis, Direction direction) const;
  bool flexEndMarginIsAuto(FlexDirection axis, Direction direction) const;

  // Operators
  bool operator==(const Style& other) const;
  bool operator!=(const Style& other) const;

private:
  // Bit-packed enum fields
  Direction direction_ : bitCount<Direction>() = Direction::Inherit;
  FlexDirection flexDirection_ : bitCount<FlexDirection>() = FlexDirection::Column;
  Justify justifyContent_ : bitCount<Justify>() = Justify::FlexStart;
  Align alignContent_ : bitCount<Align>() = Align::FlexStart;
  Align alignItems_ : bitCount<Align>() = Align::Stretch;
  Align alignSelf_ : bitCount<Align>() = Align::Auto;
  PositionType positionType_ : bitCount<PositionType>() = PositionType::Relative;
  Wrap flexWrap_ : bitCount<Wrap>() = Wrap::NoWrap;
  Overflow overflow_ : bitCount<Overflow>() = Overflow::Visible;
  Display display_ : bitCount<Display>() = Display::Flex;
  BoxSizing boxSizing_ : bitCount<BoxSizing>() = BoxSizing::BorderBox;

  // Value handles (stored compactly via pool)
  StyleValueHandle flex_{};
  StyleValueHandle flexGrow_{};
  StyleValueHandle flexShrink_{};
  StyleValueHandle flexBasis_{StyleValueHandle::ofAuto()};
  Edges margin_{};
  Edges position_{};
  Edges padding_{};
  Edges border_{};
  Gutters gap_{};
  Dimensions dimensions_{StyleValueHandle::ofAuto(), StyleValueHandle::ofAuto()};
  Dimensions minDimensions_{};
  Dimensions maxDimensions_{};
  StyleValueHandle aspectRatio_{};

  StyleValuePool pool_;
};
```

---

## 4. NODE TYPES (yoga/node/)

### CachedMeasurement (CachedMeasurement.h)

```cpp
struct CachedMeasurement {
  float availableWidth{-1};
  float availableHeight{-1};
  SizingMode widthSizingMode{SizingMode::MaxContent};
  SizingMode heightSizingMode{SizingMode::MaxContent};
  float computedWidth{-1};
  float computedHeight{-1};

  bool operator==(CachedMeasurement measurement) const;
};
```

### LayoutResults (LayoutResults.h)

**Stores layout computation results for a node**

```cpp
struct LayoutResults {
  static constexpr int32_t MaxCachedMeasurements = 8;

  // Public fields
  uint32_t computedFlexBasisGeneration = 0;
  FloatOptional computedFlexBasis = {};
  uint32_t generationCount = 0;
  uint32_t configVersion = 0;
  Direction lastOwnerDirection = Direction::Inherit;
  uint32_t nextCachedMeasurementsIndex = 0;
  std::array<CachedMeasurement, MaxCachedMeasurements> cachedMeasurements = {};
  CachedMeasurement cachedLayout{};

  // Direction
  Direction direction() const;
  void setDirection(Direction direction);

  // Overflow
  bool hadOverflow() const;
  void setHadOverflow(bool hadOverflow);

  // Dimensions (width/height after layout)
  float dimension(Dimension axis) const;
  void setDimension(Dimension axis, float dimension);

  // Measured Dimensions
  float measuredDimension(Dimension axis) const;
  void setMeasuredDimension(Dimension axis, float dimension);

  // Raw Dimensions
  float rawDimension(Dimension axis) const;
  void setRawDimension(Dimension axis, float dimension);

  // Position (Left, Top, Right, Bottom)
  float position(PhysicalEdge physicalEdge) const;
  void setPosition(PhysicalEdge physicalEdge, float dimension);

  // Margin
  float margin(PhysicalEdge physicalEdge) const;
  void setMargin(PhysicalEdge physicalEdge, float dimension);

  // Border
  float border(PhysicalEdge physicalEdge) const;
  void setBorder(PhysicalEdge physicalEdge, float dimension);

  // Padding
  float padding(PhysicalEdge physicalEdge) const;
  void setPadding(PhysicalEdge physicalEdge, float dimension);

  // Operators
  bool operator==(LayoutResults layout) const;
  bool operator!=(LayoutResults layout) const;

private:
  Direction direction_ : bitCount<Direction>() = Direction::Inherit;
  bool hadOverflow_ : 1 = false;
  std::array<float, 2> dimensions_ = {{YGUndefined, YGUndefined}};
  std::array<float, 2> measuredDimensions_ = {{YGUndefined, YGUndefined}};
  std::array<float, 2> rawDimensions_ = {{YGUndefined, YGUndefined}};
  std::array<float, 4> position_ = {};
  std::array<float, 4> margin_ = {};
  std::array<float, 4> border_ = {};
  std::array<float, 4> padding_ = {};
};
```

### LayoutableChildren (LayoutableChildren.h)

**Iterator over child nodes, skipping display:none and expanding display:contents**

```cpp
template <typename T>
class LayoutableChildren {
public:
  struct Iterator {
    using iterator_category = std::input_iterator_tag;
    using difference_type = std::ptrdiff_t;
    using value_type = T*;
    using pointer = T*;
    using reference = T*;

    Iterator() = default;
    Iterator(const T* node, size_t childIndex);

    T* operator*() const;
    Iterator& operator++();
    Iterator operator++(int);

    friend bool operator==(const Iterator& a, const Iterator& b);
    friend bool operator!=(const Iterator& a, const Iterator& b);

  private:
    void next();
    void skipContentsNodes();
    const T* node_{nullptr};
    size_t childIndex_{0};
    std::forward_list<std::pair<const T*, size_t>> backtrack_;
  };

  explicit LayoutableChildren(const T* node);
  Iterator begin() const;
  Iterator end() const;

private:
  const T* node_;
};
```

### Node (Node.h)

**Main node class representing a flexbox element**

```cpp
class Node : public ::YGNode {
public:
  using LayoutableChildren = yoga::LayoutableChildren<Node>;

  // Constructors
  Node();
  explicit Node(const Config* config);
  Node(Node&& node) noexcept;
  Node(const Node& node) = default;
  Node& operator=(const Node&) = delete;

  // Context
  void* getContext() const;
  void setContext(void* context);

  // Containing Block
  bool alwaysFormsContainingBlock() const;
  void setAlwaysFormsContainingBlock(bool alwaysFormsContainingBlock);

  // New Layout Flag
  bool getHasNewLayout() const;
  void setHasNewLayout(bool hasNewLayout);

  // Node Type
  NodeType getNodeType() const;
  void setNodeType(NodeType nodeType);

  // Measure Function
  bool hasMeasureFunc() const noexcept;
  void setMeasureFunc(YGMeasureFunc measureFunc);
  YGSize measure(float availableWidth, MeasureMode widthMode, float availableHeight, MeasureMode heightMode);

  // Baseline Function
  bool hasBaselineFunc() const noexcept;
  void setBaselineFunc(YGBaselineFunc baseLineFunc);
  float baseline(float width, float height) const;

  // Dirtied Function
  YGDirtiedFunc getDirtiedFunc() const;
  void setDirtiedFunc(YGDirtiedFunc dirtiedFunc);

  // Dimension Helpers
  float dimensionWithMargin(FlexDirection axis, float widthSize);
  bool isLayoutDimensionDefined(FlexDirection axis);
  bool hasDefiniteLength(Dimension dimension, float ownerSize);

  // Errata
  bool hasErrata(Errata errata) const;

  // Contents Children
  bool hasContentsChildren() const;

  // Style Access
  Style& style();
  const Style& style() const;
  void setStyle(const Style& style);

  // Layout Access
  LayoutResults& getLayout();
  const LayoutResults& getLayout() const;
  void setLayout(const LayoutResults& layout);

  // Line Index
  size_t getLineIndex() const;
  void setLineIndex(size_t lineIndex);

  // Reference Baseline
  bool isReferenceBaseline() const;
  void setIsReferenceBaseline(bool isReferenceBaseline);

  // Owner (Parent in tree)
  Node* getOwner() const;
  void setOwner(Node* owner);

  // Children
  const std::vector<Node*>& getChildren() const;
  Node* getChild(size_t index) const;
  size_t getChildCount() const;
  LayoutableChildren getLayoutChildren() const;
  size_t getLayoutChildCount() const;
  void setChildren(const std::vector<Node*>& children);
  void clearChildren();
  void replaceChild(Node* oldChild, Node* newChild);
  void replaceChild(Node* child, size_t index);
  void insertChild(Node* child, size_t index);
  bool removeChild(Node* child);
  void removeChild(size_t index);

  // Config
  const Config* getConfig() const;
  void setConfig(Config* config);

  // Dirty State
  bool isDirty() const;
  void setDirty(bool isDirty);

  // Processed Dimensions
  Style::SizeLength getProcessedDimension(Dimension dimension) const;
  FloatOptional getResolvedDimension(Direction direction, Dimension dimension, float referenceLength, float ownerWidth) const;

  // Layout Setters
  void setLayoutLastOwnerDirection(Direction direction);
  void setLayoutComputedFlexBasis(FloatOptional computedFlexBasis);
  void setLayoutComputedFlexBasisGeneration(uint32_t computedFlexBasisGeneration);
  void setLayoutMeasuredDimension(float measuredDimension, Dimension dimension);
  void setLayoutHadOverflow(bool hadOverflow);
  void setLayoutDimension(float lengthValue, Dimension dimension);
  void setLayoutDirection(Direction direction);
  void setLayoutMargin(float margin, PhysicalEdge edge);
  void setLayoutBorder(float border, PhysicalEdge edge);
  void setLayoutPadding(float padding, PhysicalEdge edge);
  void setLayoutPosition(float position, PhysicalEdge edge);
  void setPosition(Direction direction, float ownerWidth, float ownerHeight);

  // Flex Resolution
  Style::SizeLength processFlexBasis() const;
  FloatOptional resolveFlexBasis(Direction direction, FlexDirection flexDirection, float referenceLength, float ownerWidth) const;
  void processDimensions();
  Direction resolveDirection(Direction ownerDirection);
  float resolveFlexGrow() const;
  float resolveFlexShrink() const;
  bool isNodeFlexible();

  // Tree Operations
  void cloneChildrenIfNeeded();
  void cloneContentsChildrenIfNeeded();
  void markDirtyAndPropagate();
  void reset();

private:
  float relativePosition(FlexDirection axis, Direction direction, float axisSize) const;
  void useWebDefaults();

  // Bit-packed fields
  bool hasNewLayout_ : 1 = true;
  bool isReferenceBaseline_ : 1 = false;
  bool isDirty_ : 1 = true;
  bool alwaysFormsContainingBlock_ : 1 = false;
  NodeType nodeType_ : bitCount<NodeType>() = NodeType::Default;

  // Pointers and callbacks
  void* context_ = nullptr;
  YGMeasureFunc measureFunc_ = nullptr;
  YGBaselineFunc baselineFunc_ = nullptr;
  YGDirtiedFunc dirtiedFunc_ = nullptr;

  // Core data
  Style style_;
  LayoutResults layout_;
  size_t lineIndex_ = 0;
  size_t contentsChildrenCount_ = 0;
  Node* owner_ = nullptr;
  std::vector<Node*> children_;
  const Config* config_;
  std::array<Style::SizeLength, 2> processedDimensions_;
};
```

---

## 5. CONFIG (yoga/config/)

### Config (Config.h)

```cpp
class Config : public ::YGConfig {
public:
  explicit Config(YGLogger logger);

  // Web Defaults
  void setUseWebDefaults(bool useWebDefaults);
  bool useWebDefaults() const;

  // Experimental Features
  void setExperimentalFeatureEnabled(ExperimentalFeature feature, bool enabled);
  bool isExperimentalFeatureEnabled(ExperimentalFeature feature) const;
  ExperimentalFeatureSet getEnabledExperiments() const;

  // Errata
  void setErrata(Errata errata);
  void addErrata(Errata errata);
  void removeErrata(Errata errata);
  Errata getErrata() const;
  bool hasErrata(Errata errata) const;

  // Point Scale Factor (for pixel rounding)
  void setPointScaleFactor(float pointScaleFactor);
  float getPointScaleFactor() const;

  // Context
  void setContext(void* context);
  void* getContext() const;

  // Version (for cache invalidation)
  uint32_t getVersion() const noexcept;

  // Logger
  void setLogger(YGLogger logger);
  void log(const Node* node, LogLevel logLevel, const char* format, va_list args) const;

  // Clone Node Callback
  void setCloneNodeCallback(YGCloneNodeFunc cloneNode);
  YGNodeRef cloneNode(YGNodeConstRef node, YGNodeConstRef owner, size_t childIndex) const;

  // Default Config
  static const Config& getDefault();

private:
  YGCloneNodeFunc cloneNodeCallback_{nullptr};
  YGLogger logger_{};
  bool useWebDefaults_ : 1 = false;
  uint32_t version_ = 0;
  ExperimentalFeatureSet experimentalFeatures_{};
  Errata errata_ = Errata::None;
  float pointScaleFactor_ = 1.0f;
  void* context_ = nullptr;
};

// Free function
bool configUpdateInvalidatesLayout(const Config& oldConfig, const Config& newConfig);
```

---

## 6. ALGORITHM (yoga/algorithm/)

### SizingMode (SizingMode.h)

```cpp
enum class SizingMode {
  StretchFit,   // Fill available space exactly
  MaxContent,   // Ideal size with infinite space
  FitContent    // clamp(min-content, stretch-fit, max-content)
};

inline MeasureMode measureMode(SizingMode mode);
inline SizingMode sizingMode(MeasureMode mode);
```

### FlexDirection Helpers (FlexDirection.h)

```cpp
inline bool isRow(const FlexDirection flexDirection);
inline bool isColumn(const FlexDirection flexDirection);
inline FlexDirection resolveDirection(FlexDirection flexDirection, Direction direction);
inline FlexDirection resolveCrossDirection(FlexDirection flexDirection, Direction direction);
inline PhysicalEdge flexStartEdge(FlexDirection flexDirection);
inline PhysicalEdge flexEndEdge(FlexDirection flexDirection);
inline PhysicalEdge inlineStartEdge(FlexDirection flexDirection, Direction direction);
inline PhysicalEdge inlineEndEdge(FlexDirection flexDirection, Direction direction);
inline Dimension dimension(FlexDirection flexDirection);
```

### BoundAxis (BoundAxis.h)

```cpp
inline float paddingAndBorderForAxis(const Node* node, FlexDirection axis, Direction direction, float widthSize);

inline FloatOptional boundAxisWithinMinAndMax(
    const Node* node, Direction direction, FlexDirection axis,
    FloatOptional value, float axisSize, float widthSize);

inline float boundAxis(
    const Node* node, FlexDirection axis, Direction direction,
    float value, float axisSize, float widthSize);
```

### Align (Align.h)

```cpp
inline Align resolveChildAlignment(const Node* node, const Node* child);
constexpr Align fallbackAlignment(Align align);
constexpr Justify fallbackAlignment(Justify align);
```

### Baseline (Baseline.h, Baseline.cpp)

```cpp
float calculateBaseline(const Node* node);
bool isBaselineLayout(const Node* node);
```

### Cache (Cache.h, Cache.cpp)

```cpp
bool canUseCachedMeasurement(
    SizingMode widthMode, float availableWidth,
    SizingMode heightMode, float availableHeight,
    SizingMode lastWidthMode, float lastAvailableWidth,
    SizingMode lastHeightMode, float lastAvailableHeight,
    float lastComputedWidth, float lastComputedHeight,
    float marginRow, float marginColumn,
    const Config* config);
```

### TrailingPosition (TrailingPosition.h)

```cpp
inline float getPositionOfOppositeEdge(
    float position, FlexDirection axis,
    const Node* containingNode, const Node* node);

inline void setChildTrailingPosition(const Node* node, Node* child, FlexDirection axis);

inline bool needsTrailingPosition(FlexDirection axis);
```

### FlexLine (FlexLine.h, FlexLine.cpp)

```cpp
struct FlexLineRunningLayout {
  float totalFlexGrowFactors{0.0f};
  float totalFlexShrinkScaledFactors{0.0f};
  float remainingFreeSpace{0.0f};
  float mainDim{0.0f};
  float crossDim{0.0f};
};

struct FlexLine {
  const std::vector<Node*> itemsInFlow{};
  const float sizeConsumed{0.0f};
  const size_t numberOfAutoMargins{0};
  FlexLineRunningLayout layout{};
};

FlexLine calculateFlexLine(
    Node* node, Direction ownerDirection, float ownerWidth,
    float mainAxisOwnerSize, float availableInnerWidth,
    float availableInnerMainDim,
    Node::LayoutableChildren::Iterator& iterator, size_t lineCount);
```

### PixelGrid (PixelGrid.h, PixelGrid.cpp)

```cpp
float roundValueToPixelGrid(double value, double pointScaleFactor, bool forceCeil, bool forceFloor);

void roundLayoutResultsToPixelGrid(Node* node, double absoluteLeft, double absoluteTop);
```

### AbsoluteLayout (AbsoluteLayout.h, AbsoluteLayout.cpp)

```cpp
void layoutAbsoluteChild(
    const Node* containingNode, const Node* node, Node* child,
    float containingBlockWidth, float containingBlockHeight,
    SizingMode widthMode, Direction direction,
    LayoutData& layoutMarkerData, uint32_t depth, uint32_t generationCount);

bool layoutAbsoluteDescendants(
    Node* containingNode, Node* currentNode,
    SizingMode widthSizingMode, Direction currentNodeDirection,
    LayoutData& layoutMarkerData, uint32_t currentDepth, uint32_t generationCount,
    float currentNodeMainOffsetFromContainingBlock,
    float currentNodeCrossOffsetFromContainingBlock,
    float containingNodeAvailableInnerWidth,
    float containingNodeAvailableInnerHeight);
```

### CalculateLayout (CalculateLayout.h, CalculateLayout.cpp)

**Main layout algorithm entry points**

```cpp
// Global generation counter for cache invalidation
extern std::atomic<uint32_t> gCurrentGenerationCount;

// Main public entry point
void calculateLayout(
    Node* node,
    float ownerWidth,
    float ownerHeight,
    Direction ownerDirection);

// Internal recursive layout function
bool calculateLayoutInternal(
    Node* node,
    float availableWidth,
    float availableHeight,
    Direction ownerDirection,
    SizingMode widthSizingMode,
    SizingMode heightSizingMode,
    float ownerWidth,
    float ownerHeight,
    bool performLayout,
    LayoutPassReason reason,
    LayoutData& layoutMarkerData,
    uint32_t depth,
    uint32_t generationCount);
```

**Static helper functions in CalculateLayout.cpp:**

| Function | Description |
|----------|-------------|
| `constrainMaxSizeForMode` | Constrains size based on max dimension |
| `computeFlexBasisForChild` | Computes flex basis for a single child |
| `measureNodeWithMeasureFunc` | Measures node using custom measure function |
| `measureNodeWithoutChildren` | Measures leaf node without children |
| `measureNodeWithFixedSize` | Fast path for fixed-size nodes |
| `zeroOutLayoutRecursively` | Zeros layout for display:none nodes |
| `cleanupContentsNodesRecursively` | Handles display:contents cleanup |
| `calculateAvailableInnerDimension` | Calculates available inner space |
| `computeFlexBasisForChildren` | Iterates children computing flex basis |
| `distributeFreeSpaceFirstPass` | First pass of flex distribution |
| `distributeFreeSpaceSecondPass` | Second pass of flex distribution |
| `resolveFlexibleLength` | Resolves final flex item sizes |
| `justifyMainAxis` | Applies justify-content |
| `calculateLayoutImpl` | Main implementation (~900 lines) |

---

## 7. EVENT SYSTEM (yoga/event/)

### event.h / event.cpp

```cpp
enum struct LayoutType : int {
  kLayout = 0,
  kMeasure = 1,
  kCachedLayout = 2,
  kCachedMeasure = 3
};

enum struct LayoutPassReason : int {
  kInitial = 0,
  kAbsLayout = 1,
  kStretch = 2,
  kMultilineStretch = 3,
  kFlexLayout = 4,
  kMeasureChild = 5,
  kAbsMeasureChild = 6,
  kFlexMeasure = 7,
  COUNT
};

struct LayoutData {
  int layouts = 0;
  int measures = 0;
  uint32_t maxMeasureCache = 0;
  int cachedLayouts = 0;
  int cachedMeasures = 0;
  int measureCallbacks = 0;
  std::array<int, static_cast<uint8_t>(LayoutPassReason::COUNT)> measureCallbackReasonsCount;
};

const char* LayoutPassReasonToString(LayoutPassReason value);

struct Event {
  enum Type {
    NodeAllocation,
    NodeDeallocation,
    NodeLayout,
    LayoutPassStart,
    LayoutPassEnd,
    MeasureCallbackStart,
    MeasureCallbackEnd,
    NodeBaselineStart,
    NodeBaselineEnd
  };

  class Data;
  using Subscriber = void(YGNodeConstRef, Type, Data);
  using Subscribers = std::vector<std::function<Subscriber>>;

  template <Type E>
  struct TypedData {};

  class Data {
    const void* data_;
  public:
    template <Type E> explicit Data(const TypedData<E>& data);
    template <Type E> const TypedData<E>& get() const;
  };

  static void reset();
  static void subscribe(std::function<Subscriber>&& subscriber);
  
  template <Type E>
  static void publish(YGNodeConstRef node, const TypedData<E>& eventData = {});

private:
  static void publish(YGNodeConstRef node, Type eventType, const Data& eventData);
};

// TypedData specializations
template <> struct Event::TypedData<Event::NodeAllocation> { YGConfigConstRef config; };
template <> struct Event::TypedData<Event::NodeDeallocation> { YGConfigConstRef config; };
template <> struct Event::TypedData<Event::LayoutPassEnd> { LayoutData* layoutData; };
template <> struct Event::TypedData<Event::MeasureCallbackEnd> {
  float width;
  YGMeasureMode widthMeasureMode;
  float height;
  YGMeasureMode heightMeasureMode;
  float measuredWidth;
  float measuredHeight;
  const LayoutPassReason reason;
};
template <> struct Event::TypedData<Event::NodeLayout> { LayoutType layoutType; };
```

---

## 8. DEBUG UTILITIES (yoga/debug/)

### AssertFatal.h

```cpp
[[noreturn]] void fatalWithMessage(const char* message);
void assertFatal(bool condition, const char* message);
void assertFatalWithNode(const Node* node, bool condition, const char* message);
void assertFatalWithConfig(const Config* config, bool condition, const char* message);
```

### Log.h

```cpp
void log(LogLevel level, const char* format, ...) noexcept;
void log(const Node* node, LogLevel level, const char* format, ...) noexcept;
void log(const Config* config, LogLevel level, const char* format, ...) noexcept;
YGLogger getDefaultLogger();
```

---

## 9. YGValue (YGValue.h)

**C-compatible value struct**

```cpp
// Constant for undefined float
constexpr float YGUndefined = std::numeric_limits<float>::quiet_NaN();

typedef struct YGValue {
  float value;
  YGUnit unit;
} YGValue;

// Constants
extern const YGValue YGValueAuto;
extern const YGValue YGValueUndefined;
extern const YGValue YGValueZero;

// Helper function
bool YGFloatIsUndefined(float value);

// C++ operators
bool operator==(const YGValue& lhs, const YGValue& rhs);
bool operator!=(const YGValue& lhs, const YGValue& rhs);
YGValue operator-(const YGValue& value);
```

---

## Summary

This catalog covers the complete Yoga C++ implementation with:

| Category | Count |
|----------|-------|
| **Enums** | 18 |
| **Structs/Classes** | 15 |
| **Major Classes** | Node, Style, Config, LayoutResults |
| **Algorithm Functions** | 20+ |
| **Helper Functions** | 40+ |

### Key Classes for C# Port

1. **Node** - The main flexbox node with style, layout results, children
2. **Style** - All CSS flexbox properties with compact storage
3. **Config** - Configuration for errata, web defaults, point scale factor
4. **LayoutResults** - Output of layout calculation
5. **FloatOptional** - Optional float using NaN for undefined
6. **StyleLength / StyleSizeLength** - CSS length values
7. **FlexLine** - Represents a line in flex layout

### Algorithm Entry Points

- `calculateLayout()` - Main entry point
- `calculateLayoutInternal()` - Recursive layout with caching
- `layoutAbsoluteDescendants()` - Absolute positioning
- `roundLayoutResultsToPixelGrid()` - Pixel snapping

---

## Recommendations

To verify C# port completeness, ensure:

1. All 18 enums are implemented with matching values
2. `FloatOptional` handles NaN correctly for undefined values
3. `Style` class has all 50+ properties and computed methods
4. `Node` class has all tree operations and layout setters
5. `LayoutResults` stores all 8 cached measurements
6. Layout algorithm implements all 10+ steps from `calculateLayoutImpl`
7. `Config` tracks version for cache invalidation

---

## References

- [Yoga GitHub Repository](https://github.com/facebook/yoga)
- [CSS Flexbox Specification](https://www.w3.org/TR/css-flexbox-1/)
- [CSS Sizing Specification](https://www.w3.org/TR/css-sizing-3/)
