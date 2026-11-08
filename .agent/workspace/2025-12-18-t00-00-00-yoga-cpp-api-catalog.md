# Comprehensive Catalog of Facebook Yoga C++ Source Code

## Overview

This document catalogs all classes, structs, enums, properties, and methods in the Facebook Yoga C++ flexbox layout library located at `/home/steventcramer/worktrees/github.com/facebook/yoga/main/yoga/`.

---

## 1. ENUMS (yoga/enums/)

### Align (Align.h)
```cpp
enum class Align : uint8_t {
  Auto = 0, FlexStart = 1, Center = 2, FlexEnd = 3, Stretch = 4,
  Baseline = 5, SpaceBetween = 6, SpaceAround = 7, SpaceEvenly = 8
};
```

### BoxSizing (BoxSizing.h)
```cpp
enum class BoxSizing : uint8_t { BorderBox = 0, ContentBox = 1 };
```

### Dimension (Dimension.h)
```cpp
enum class Dimension : uint8_t { Width = 0, Height = 1 };
```

### Direction (Direction.h)
```cpp
enum class Direction : uint8_t { Inherit = 0, LTR = 1, RTL = 2 };
```

### Display (Display.h)
```cpp
enum class Display : uint8_t { Flex = 0, None = 1, Contents = 2 };
```

### Edge (Edge.h)
```cpp
enum class Edge : uint8_t {
  Left = 0, Top = 1, Right = 2, Bottom = 3, Start = 4, End = 5,
  Horizontal = 6, Vertical = 7, All = 8
};
```

### Errata (Errata.h)
```cpp
enum class Errata : uint32_t {
  None = 0, StretchFlexBasis = 1, 
  AbsolutePositionWithoutInsetsExcludesPadding = 2,
  AbsolutePercentAgainstInnerSize = 4,
  All = 2147483647, Classic = 2147483646
};
```

### FlexDirection (FlexDirection.h)
```cpp
enum class FlexDirection : uint8_t {
  Column = 0, ColumnReverse = 1, Row = 2, RowReverse = 3
};
```

### Gutter (Gutter.h)
```cpp
enum class Gutter : uint8_t { Column = 0, Row = 1, All = 2 };
```

### Justify (Justify.h)
```cpp
enum class Justify : uint8_t {
  FlexStart = 0, Center = 1, FlexEnd = 2,
  SpaceBetween = 3, SpaceAround = 4, SpaceEvenly = 5
};
```

### MeasureMode (MeasureMode.h)
```cpp
enum class MeasureMode : uint8_t { Undefined = 0, Exactly = 1, AtMost = 2 };
```

### NodeType (NodeType.h)
```cpp
enum class NodeType : uint8_t { Default = 0, Text = 1 };
```

### Overflow (Overflow.h)
```cpp
enum class Overflow : uint8_t { Visible = 0, Hidden = 1, Scroll = 2 };
```

### PhysicalEdge (PhysicalEdge.h)
```cpp
enum class PhysicalEdge : uint32_t { Left = 0, Top = 1, Right = 2, Bottom = 3 };
```

### PositionType (PositionType.h)
```cpp
enum class PositionType : uint8_t { Static = 0, Relative = 1, Absolute = 2 };
```

### SizingMode (SizingMode.h)
```cpp
enum class SizingMode { StretchFit, MaxContent, FitContent };
```

### Unit (Unit.h)
```cpp
enum class Unit : uint8_t {
  Undefined = 0, Point = 1, Percent = 2, Auto = 3,
  MaxContent = 4, FitContent = 5, Stretch = 6
};
```

### Wrap (Wrap.h)
```cpp
enum class Wrap : uint8_t { NoWrap = 0, Wrap = 1, WrapReverse = 2 };
```

---

## 2. NUMERIC TYPES (yoga/numeric/)

### FloatOptional (FloatOptional.h)
```cpp
struct FloatOptional {
  float value_ = NaN;
  
  explicit constexpr FloatOptional(float value);
  constexpr FloatOptional() = default;
  
  constexpr float unwrap() const;
  constexpr float unwrapOrDefault(float defaultValue) const;
  constexpr bool isUndefined() const;
  constexpr bool isDefined() const;
};

// Operators
FloatOptional operator+(FloatOptional lhs, FloatOptional rhs);
FloatOptional maxOrDefined(FloatOptional lhs, FloatOptional rhs);
bool inexactEquals(FloatOptional lhs, FloatOptional rhs);
```

### Comparison.h
```cpp
constexpr bool isUndefined(float value);
constexpr bool isDefined(float value);
constexpr auto maxOrDefined(float a, float b);
constexpr auto minOrDefined(float a, float b);
inline bool inexactEquals(float a, float b);  // epsilon = 0.0001f
```

---

## 3. STYLE TYPES (yoga/style/)

### StyleLength (StyleLength.h)
```cpp
class StyleLength {
  FloatOptional value_{};
  Unit unit_{Unit::Undefined};
  
public:
  static StyleLength points(float value);
  static StyleLength percent(float value);
  static StyleLength ofAuto();
  static StyleLength undefined();
  
  bool isAuto() const;
  bool isUndefined() const;
  bool isPoints() const;
  bool isPercent() const;
  bool isDefined() const;
  FloatOptional value() const;
  FloatOptional resolve(float referenceLength);
};
```

### StyleSizeLength (StyleSizeLength.h)
```cpp
class StyleSizeLength {
  // Like StyleLength but also supports MaxContent, FitContent, Stretch
  static StyleSizeLength ofMaxContent();
  static StyleSizeLength ofFitContent();
  static StyleSizeLength ofStretch();
  
  bool isMaxContent() const;
  bool isFitContent() const;
  bool isStretch() const;
};
```

### Style (Style.h)
```cpp
class Style {
  // Constants
  static constexpr float DefaultFlexGrow = 0.0f;
  static constexpr float DefaultFlexShrink = 0.0f;
  static constexpr float WebDefaultFlexShrink = 1.0f;

  // Properties (get/set pairs)
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
  
  FloatOptional flex() const;
  void setFlex(FloatOptional value);
  
  FloatOptional flexGrow() const;
  void setFlexGrow(FloatOptional value);
  
  FloatOptional flexShrink() const;
  void setFlexShrink(FloatOptional value);
  
  SizeLength flexBasis() const;
  void setFlexBasis(SizeLength value);
  
  Length margin(Edge edge) const;
  void setMargin(Edge edge, Length value);
  
  Length position(Edge edge) const;
  void setPosition(Edge edge, Length value);
  
  Length padding(Edge edge) const;
  void setPadding(Edge edge, Length value);
  
  Length border(Edge edge) const;
  void setBorder(Edge edge, Length value);
  
  Length gap(Gutter gutter) const;
  void setGap(Gutter gutter, Length value);
  
  SizeLength dimension(Dimension axis) const;
  void setDimension(Dimension axis, SizeLength value);
  
  SizeLength minDimension(Dimension axis) const;
  void setMinDimension(Dimension axis, SizeLength value);
  
  SizeLength maxDimension(Dimension axis) const;
  void setMaxDimension(Dimension axis, SizeLength value);
  
  FloatOptional aspectRatio() const;
  void setAspectRatio(FloatOptional value);
  
  // Computed methods
  float computeFlexStartPosition(FlexDirection axis, Direction direction, float axisSize) const;
  float computeFlexEndPosition(FlexDirection axis, Direction direction, float axisSize) const;
  float computeFlexStartMargin(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexEndMargin(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexStartBorder(FlexDirection axis, Direction direction) const;
  float computeFlexEndBorder(FlexDirection axis, Direction direction) const;
  float computeFlexStartPadding(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexEndPadding(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexStartPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize) const;
  float computeFlexEndPaddingAndBorder(FlexDirection axis, Direction direction, float widthSize) const;
  float computeBorderForAxis(FlexDirection axis) const;
  float computeMarginForAxis(FlexDirection axis, float widthSize) const;
  float computeGapForAxis(FlexDirection axis, float ownerSize) const;
  
  bool flexStartMarginIsAuto(FlexDirection axis, Direction direction) const;
  bool flexEndMarginIsAuto(FlexDirection axis, Direction direction) const;
  bool isFlexStartPositionDefined(FlexDirection axis, Direction direction) const;
  bool isFlexEndPositionDefined(FlexDirection axis, Direction direction) const;
  
  FloatOptional resolvedMinDimension(Direction dir, Dimension axis, float refLen, float ownerWidth) const;
  FloatOptional resolvedMaxDimension(Direction dir, Dimension axis, float refLen, float ownerWidth) const;
};
```

---

## 4. NODE (yoga/node/)

### CachedMeasurement (CachedMeasurement.h)
```cpp
struct CachedMeasurement {
  float availableWidth{-1};
  float availableHeight{-1};
  SizingMode widthSizingMode{SizingMode::MaxContent};
  SizingMode heightSizingMode{SizingMode::MaxContent};
  float computedWidth{-1};
  float computedHeight{-1};
};
```

### LayoutResults (LayoutResults.h)
```cpp
struct LayoutResults {
  static constexpr int32_t MaxCachedMeasurements = 8;
  
  // Cache fields
  uint32_t computedFlexBasisGeneration = 0;
  FloatOptional computedFlexBasis = {};
  uint32_t generationCount = 0;
  uint32_t configVersion = 0;
  Direction lastOwnerDirection = Direction::Inherit;
  uint32_t nextCachedMeasurementsIndex = 0;
  std::array<CachedMeasurement, MaxCachedMeasurements> cachedMeasurements = {};
  CachedMeasurement cachedLayout{};
  
  // Methods
  Direction direction() const;
  void setDirection(Direction direction);
  
  bool hadOverflow() const;
  void setHadOverflow(bool hadOverflow);
  
  float dimension(Dimension axis) const;
  void setDimension(Dimension axis, float dimension);
  
  float measuredDimension(Dimension axis) const;
  void setMeasuredDimension(Dimension axis, float dimension);
  
  float rawDimension(Dimension axis) const;
  void setRawDimension(Dimension axis, float dimension);
  
  float position(PhysicalEdge edge) const;
  void setPosition(PhysicalEdge edge, float dimension);
  
  float margin(PhysicalEdge edge) const;
  void setMargin(PhysicalEdge edge, float dimension);
  
  float border(PhysicalEdge edge) const;
  void setBorder(PhysicalEdge edge, float dimension);
  
  float padding(PhysicalEdge edge) const;
  void setPadding(PhysicalEdge edge, float dimension);
};
```

### Node (Node.h)
```cpp
class Node {
public:
  // Constructors
  Node();
  explicit Node(const Config* config);
  
  // Context
  void* getContext() const;
  void setContext(void* context);
  
  // Containing Block
  bool alwaysFormsContainingBlock() const;
  void setAlwaysFormsContainingBlock(bool value);
  
  // New Layout Flag
  bool getHasNewLayout() const;
  void setHasNewLayout(bool hasNewLayout);
  
  // Node Type
  NodeType getNodeType() const;
  void setNodeType(NodeType nodeType);
  
  // Measure Function
  bool hasMeasureFunc() const noexcept;
  void setMeasureFunc(YGMeasureFunc measureFunc);
  YGSize measure(float width, MeasureMode widthMode, float height, MeasureMode heightMode);
  
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
  
  // Line Index
  size_t getLineIndex() const;
  void setLineIndex(size_t lineIndex);
  
  // Reference Baseline
  bool isReferenceBaseline() const;
  void setIsReferenceBaseline(bool value);
  
  // Owner (Parent)
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
  FloatOptional getResolvedDimension(Direction dir, Dimension dim, float refLen, float ownerWidth) const;
  
  // Layout Setters
  void setLayoutLastOwnerDirection(Direction direction);
  void setLayoutComputedFlexBasis(FloatOptional computedFlexBasis);
  void setLayoutComputedFlexBasisGeneration(uint32_t generation);
  void setLayoutMeasuredDimension(float dimension, Dimension axis);
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
  FloatOptional resolveFlexBasis(Direction dir, FlexDirection flexDir, float refLen, float ownerWidth) const;
  void processDimensions();
  Direction resolveDirection(Direction ownerDirection);
  float resolveFlexGrow() const;
  float resolveFlexShrink() const;
  bool isNodeFlexible();
  
  // Tree Operations
  void cloneChildrenIfNeeded();
  void markDirtyAndPropagate();
  void reset();
};
```

---

## 5. CONFIG (yoga/config/)

### Config (Config.h)
```cpp
class Config {
public:
  explicit Config(YGLogger logger);
  
  void setUseWebDefaults(bool useWebDefaults);
  bool useWebDefaults() const;
  
  void setExperimentalFeatureEnabled(ExperimentalFeature feature, bool enabled);
  bool isExperimentalFeatureEnabled(ExperimentalFeature feature) const;
  
  void setErrata(Errata errata);
  void addErrata(Errata errata);
  void removeErrata(Errata errata);
  Errata getErrata() const;
  bool hasErrata(Errata errata) const;
  
  void setPointScaleFactor(float factor);
  float getPointScaleFactor() const;
  
  void setContext(void* context);
  void* getContext() const;
  
  uint32_t getVersion() const noexcept;
  
  void setLogger(YGLogger logger);
  void log(const Node* node, LogLevel level, const char* format, va_list args) const;
  
  void setCloneNodeCallback(YGCloneNodeFunc cloneNode);
  YGNodeRef cloneNode(YGNodeConstRef node, YGNodeConstRef owner, size_t childIndex) const;
  
  static const Config& getDefault();
};
```

---

## 6. ALGORITHM (yoga/algorithm/)

### FlexLine (FlexLine.h)
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

### BoundAxis (BoundAxis.h)
```cpp
float paddingAndBorderForAxis(const Node* node, FlexDirection axis, Direction dir, float widthSize);
FloatOptional boundAxisWithinMinAndMax(const Node* node, Direction dir, FlexDirection axis,
    FloatOptional value, float axisSize, float widthSize);
float boundAxis(const Node* node, FlexDirection axis, Direction dir,
    float value, float axisSize, float widthSize);
```

### Baseline (Baseline.h)
```cpp
float calculateBaseline(const Node* node);
bool isBaselineLayout(const Node* node);
```

### Cache (Cache.h)
```cpp
bool canUseCachedMeasurement(
    SizingMode widthMode, float availableWidth,
    SizingMode heightMode, float availableHeight,
    SizingMode lastWidthMode, float lastAvailableWidth,
    SizingMode lastHeightMode, float lastAvailableHeight,
    float lastComputedWidth, float lastComputedHeight,
    float marginRow, float marginColumn, const Config* config);
```

### CalculateLayout (CalculateLayout.h)
```cpp
extern std::atomic<uint32_t> gCurrentGenerationCount;

void calculateLayout(Node* node, float ownerWidth, float ownerHeight, Direction ownerDirection);

bool calculateLayoutInternal(
    Node* node, float availableWidth, float availableHeight,
    Direction ownerDirection, SizingMode widthSizingMode, SizingMode heightSizingMode,
    float ownerWidth, float ownerHeight, bool performLayout,
    LayoutPassReason reason, LayoutData& layoutMarkerData,
    uint32_t depth, uint32_t generationCount);
```

### Key Static Functions in CalculateLayout.cpp
```cpp
static void constrainMaxSizeForMode(...);
static void computeFlexBasisForChild(...);
static void measureNodeWithMeasureFunc(...);
static void measureNodeWithoutChildren(...);
static bool measureNodeWithFixedSize(...);
static void zeroOutLayoutRecursively(...);
static void cleanupContentsNodesRecursively(...);
static float calculateAvailableInnerDimension(...);
static float computeFlexBasisForChildren(...);
static void distributeFreeSpaceFirstPass(...);
static float distributeFreeSpaceSecondPass(...);
static void resolveFlexibleLength(...);
static void justifyMainAxis(...);
static void calculateLayoutImpl(...);
```

### AbsoluteLayout (AbsoluteLayout.h)
```cpp
void layoutAbsoluteChild(...);
bool layoutAbsoluteDescendants(...);
```

### PixelGrid (PixelGrid.h)
```cpp
float roundValueToPixelGrid(double value, double pointScaleFactor, bool forceCeil, bool forceFloor);
void roundLayoutResultsToPixelGrid(Node* node, double absoluteLeft, double absoluteTop);
```

### TrailingPosition (TrailingPosition.h)
```cpp
float getPositionOfOppositeEdge(float position, FlexDirection axis, const Node* containingNode, const Node* node);
void setChildTrailingPosition(const Node* node, Node* child, FlexDirection axis);
bool needsTrailingPosition(FlexDirection axis);
```

---

## Summary

| Category | Count |
|----------|-------|
| **Enums** | 18 |
| **Numeric Types** | 2 (FloatOptional, Comparison) |
| **Style Types** | 4 (StyleLength, StyleSizeLength, StyleValueHandle, Style) |
| **Node Types** | 4 (CachedMeasurement, LayoutResults, LayoutableChildren, Node) |
| **Config** | 1 |
| **Algorithm Functions** | 20+ |
