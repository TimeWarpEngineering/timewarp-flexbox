# Yoga C++ Dependency Map for C# Port

## Level 0 - Zero Dependencies (Pure Leaves)

| C++ File                   | Purpose                                 | C# Target                      |
| -------------------------- | --------------------------------------- | ------------------------------ |
| `YGMacros.h`               | C/C++ macros                            | Not needed - use C# attributes |
| `enums/YogaEnums.h`        | Enum utilities (ordinalCount, bitCount) | `YogaEnums.cs`                 |
| `style/SmallValueBuffer.h` | Memory-efficient value storage          | `SmallValueBuffer.cs`          |

## Level 1 - Depends only on Level 0

| C++ File               | Dependencies | C# Target       |
| ---------------------- | ------------ | --------------- |
| `YGEnums.h`            | YGMacros     | `YGEnums.cs`    |
| `numeric/Comparison.h` | (std only)   | `Comparison.cs` |

## Level 2 - Depends on Level 0-1

| C++ File                      | Dependencies       | C# Target                |
| ----------------------------- | ------------------ | ------------------------ |
| `numeric/FloatOptional.h`     | Comparison         | `FloatOptional.cs`       |
| `YGValue.h`                   | YGEnums, YGMacros  | `YGValue.cs`             |
| `YGConfig.h`                  | YGEnums, YGMacros  | `YGConfig.cs`            |
| `enums/Edge.h`                | YGEnums, YogaEnums | `Edge.cs`                |
| `enums/Dimension.h`           | YGEnums, YogaEnums | `Dimension.cs`           |
| `enums/Direction.h`           | YGEnums, YogaEnums | `Direction.cs`           |
| `enums/FlexDirection.h`       | YGEnums, YogaEnums | `FlexDirection.cs`       |
| `enums/MeasureMode.h`         | YGEnums, YogaEnums | `MeasureMode.cs`         |
| `enums/Align.h`               | YGEnums, YogaEnums | `Align.cs`               |
| `enums/Justify.h`             | YGEnums, YogaEnums | `Justify.cs`             |
| `enums/Wrap.h`                | YGEnums, YogaEnums | `Wrap.cs`                |
| `enums/Overflow.h`            | YGEnums, YogaEnums | `Overflow.cs`            |
| `enums/Display.h`             | YGEnums, YogaEnums | `Display.cs`             |
| `enums/PositionType.h`        | YGEnums, YogaEnums | `PositionType.cs`        |
| `enums/Unit.h`                | YGEnums, YogaEnums | `Unit.cs`                |
| `enums/Gutter.h`              | YGEnums, YogaEnums | `Gutter.cs`              |
| `enums/BoxSizing.h`           | YGEnums, YogaEnums | `BoxSizing.cs`           |
| `enums/Errata.h`              | YGEnums, YogaEnums | `Errata.cs`              |
| `enums/ExperimentalFeature.h` | YGEnums, YogaEnums | `ExperimentalFeature.cs` |
| `enums/LogLevel.h`            | YGEnums, YogaEnums | `LogLevel.cs`            |
| `enums/NodeType.h`            | YGEnums, YogaEnums | `NodeType.cs`            |

## Level 3 - Depends on Level 0-2

| C++ File                    | Dependencies                | C# Target                |
| --------------------------- | --------------------------- | ------------------------ |
| `enums/PhysicalEdge.h`      | Edge                        | `PhysicalEdge.cs`        |
| `style/StyleLength.h`       | Unit, FloatOptional         | `StyleLength.cs`         |
| `style/StyleSizeLength.h`   | Unit, FloatOptional         | `StyleSizeLength.cs`     |
| `YGNode.h`                  | YGConfig, YGEnums, YGMacros | `YGNode.cs` (public API) |
| `YGPixelGrid.h`             | YGConfig, YGEnums, YGMacros | `YGPixelGrid.cs`         |
| `YGNodeLayout.h`            | YGConfig, YGEnums, YGMacros | `YGNodeLayout.cs`        |
| `node/LayoutableChildren.h` | Display                     | `LayoutableChildren.cs`  |
| `Yoga.h`                    | (aggregates all YG\*.h)     | Not needed               |

## Level 4 - Depends on Level 0-3

| C++ File                   | Dependencies                                 | C# Target             |
| -------------------------- | -------------------------------------------- | --------------------- |
| `style/StyleValueHandle.h` | FloatOptional, SmallValueBuffer, StyleLength | `StyleValueHandle.cs` |
| `debug/AssertFatal.h`      | Yoga                                         | `AssertFatal.cs`      |
| `debug/Log.h`              | Yoga, Config, LogLevel                       | `Log.cs`              |
| `event/event.h`            | Yoga                                         | `Event.cs`            |
| `config/Config.h`          | Errata, ExperimentalFeature, LogLevel        | `Config.cs`           |
| `YGNodeStyle.h`            | YGNode, YGValue                              | `YGNodeStyle.cs`      |
| `algorithm/SizingMode.h`   | AssertFatal, MeasureMode                     | `SizingMode.cs`       |

## Level 5 - Depends on Level 0-4

| C++ File                    | Dependencies                                                                    | C# Target               |
| --------------------------- | ------------------------------------------------------------------------------- | ----------------------- |
| `style/StyleValuePool.h`    | FloatOptional, SmallValueBuffer, StyleLength, StyleSizeLength, StyleValueHandle | `StyleValuePool.cs`     |
| `algorithm/FlexDirection.h` | AssertFatal, Dimension, Direction, Edge, FlexDirection, PhysicalEdge            | `FlexDirectionUtils.cs` |
| `node/CachedMeasurement.h`  | SizingMode, Comparison                                                          | `CachedMeasurement.cs`  |

## Level 6 - Depends on Level 0-5

| C++ File               | Dependencies                                                                                | C# Target          |
| ---------------------- | ------------------------------------------------------------------------------------------- | ------------------ |
| `node/LayoutResults.h` | AssertFatal, Dimension, Direction, Edge, PhysicalEdge, CachedMeasurement, FloatOptional     | `LayoutResults.cs` |
| `algorithm/Cache.h`    | SizingMode, Config                                                                          | `Cache.cs`         |
| `style/Style.h`        | FlexDirection(algo), ALL enums, FloatOptional, StyleLength, StyleSizeLength, StyleValuePool | `Style.cs`         |

## Level 7 - Depends on Level 0-6

| C++ File      | Dependencies                                                 | C# Target |
| ------------- | ------------------------------------------------------------ | --------- |
| `node/Node.h` | LayoutableChildren, Config, many enums, LayoutResults, Style | `Node.cs` |

## Level 8 - Depends on Level 0-7

| C++ File                       | Dependencies                                              | C# Target                     |
| ------------------------------ | --------------------------------------------------------- | ----------------------------- |
| `algorithm/PixelGrid.h`        | Node                                                      | `PixelGrid.cs`                |
| `algorithm/Baseline.h`         | Node                                                      | `Baseline.cs`                 |
| `algorithm/FlexLine.h`         | Node                                                      | `FlexLine.cs`                 |
| `algorithm/Align.h`            | FlexDirection, Node                                       | `AlignUtils.cs`               |
| `algorithm/AbsoluteLayout.h`   | event, Node                                               | `AbsoluteLayout.cs`           |
| `algorithm/BoundAxis.h`        | FlexDirection, Dimension, Node, Comparison, FloatOptional | `BoundAxis.cs`                |
| `algorithm/TrailingPosition.h` | FlexDirection, event, Node                                | `TrailingPosition.cs`         |
| `algorithm/CalculateLayout.h`  | FlexDirection, event, Node                                | `CalculateLayout.cs` (header) |

## Level 9 - The Trunk (Top-level implementations)

| C++ File                        | Dependencies                                                        | C# Target                   |
| ------------------------------- | ------------------------------------------------------------------- | --------------------------- |
| `algorithm/CalculateLayout.cpp` | **EVERYTHING**                                                      | `CalculateLayout.cs` (impl) |
| `algorithm/AbsoluteLayout.cpp`  | AbsoluteLayout, Align, BoundAxis, CalculateLayout, TrailingPosition | (in AbsoluteLayout.cs)      |
| `YGNode.cpp`                    | Cache, CalculateLayout, AssertFatal, Log, event, Node               | (in Node.cs)                |
