/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: yoga/event/event.h, yoga/event/event.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox;

#region Enums

/// <summary>
/// Specifies the type of layout operation performed on a node.
/// </summary>
[OrdinalCount(4)]
public enum LayoutType
{
  /// <summary>Full layout calculation.</summary>
  Layout = 0,

  /// <summary>Measurement pass.</summary>
  Measure = 1,

  /// <summary>Layout result retrieved from cache.</summary>
  CachedLayout = 2,

  /// <summary>Measurement result retrieved from cache.</summary>
  CachedMeasure = 3
}

/// <summary>
/// Specifies the reason for a layout pass.
/// </summary>
[OrdinalCount(8)]
public enum LayoutPassReason
{
  /// <summary>Initial layout pass.</summary>
  Initial = 0,

  /// <summary>Absolute positioning layout.</summary>
  AbsLayout = 1,

  /// <summary>Stretch alignment.</summary>
  Stretch = 2,

  /// <summary>Multiline stretch alignment.</summary>
  MultilineStretch = 3,

  /// <summary>Flex layout pass.</summary>
  FlexLayout = 4,

  /// <summary>Measure child pass.</summary>
  MeasureChild = 5,

  /// <summary>Absolute measure child pass.</summary>
  AbsMeasureChild = 6,

  /// <summary>Flex measure pass.</summary>
  FlexMeasure = 7
}

#endregion

#region LayoutPassReason Extensions

/// <summary>
/// Extension methods for LayoutPassReason.
/// </summary>
public static class LayoutPassReasonExtensions
{
  /// <summary>
  /// Converts a LayoutPassReason to its string representation.
  /// </summary>
  /// <param name="value">The layout pass reason.</param>
  /// <returns>The string representation.</returns>
  public static string ToReasonString(this LayoutPassReason value) => value switch
  {
    LayoutPassReason.Initial => "initial",
    LayoutPassReason.AbsLayout => "abs_layout",
    LayoutPassReason.Stretch => "stretch",
    LayoutPassReason.MultilineStretch => "multiline_stretch",
    LayoutPassReason.FlexLayout => "flex_layout",
    LayoutPassReason.MeasureChild => "measure",
    LayoutPassReason.AbsMeasureChild => "abs_measure",
    LayoutPassReason.FlexMeasure => "flex_measure",
    _ => "unknown"
  };
}

#endregion

#region LayoutData

/// <summary>
/// Statistics collected during a layout pass.
/// </summary>
public sealed class LayoutData
{
  /// <summary>
  /// Number of full layout calculations performed.
  /// </summary>
  public int Layouts { get; set; }

  /// <summary>
  /// Number of measurement passes performed.
  /// </summary>
  public int Measures { get; set; }

  /// <summary>
  /// Maximum number of cached measurements.
  /// </summary>
  public uint MaxMeasureCache { get; set; }

  /// <summary>
  /// Number of cached layout results used.
  /// </summary>
  public int CachedLayouts { get; set; }

  /// <summary>
  /// Number of cached measurement results used.
  /// </summary>
  public int CachedMeasures { get; set; }

  /// <summary>
  /// Number of measure callback invocations.
  /// </summary>
  public int MeasureCallbacks { get; set; }

  private readonly int[] MeasureCallbackReasonsCount = new int[YogaEnums.OrdinalCount<LayoutPassReason>()];

  /// <summary>
  /// Gets the count of measure callbacks for a specific reason.
  /// </summary>
  /// <param name="reason">The layout pass reason.</param>
  /// <returns>The count of measure callbacks for the specified reason.</returns>
  public int GetMeasureCallbackReasonCount(LayoutPassReason reason) => MeasureCallbackReasonsCount[(int)reason];

  /// <summary>
  /// Increments the count of measure callbacks for a specific reason.
  /// </summary>
  /// <param name="reason">The layout pass reason.</param>
  public void IncrementMeasureCallbackReasonCount(LayoutPassReason reason) => MeasureCallbackReasonsCount[(int)reason]++;

  /// <summary>
  /// Creates a copy of this LayoutData.
  /// </summary>
  /// <returns>A new LayoutData with the same values.</returns>
  public LayoutData Copy()
  {
    LayoutData copy = new()
    {
      Layouts = Layouts,
      Measures = Measures,
      MaxMeasureCache = MaxMeasureCache,
      CachedLayouts = CachedLayouts,
      CachedMeasures = CachedMeasures,
      MeasureCallbacks = MeasureCallbacks
    };
    Array.Copy(MeasureCallbackReasonsCount, copy.MeasureCallbackReasonsCount, MeasureCallbackReasonsCount.Length);
    return copy;
  }
}

#endregion

#region Event Types

/// <summary>
/// Specifies the type of event being published.
/// </summary>
public enum EventType
{
  /// <summary>A node was allocated.</summary>
  NodeAllocation,

  /// <summary>A node was deallocated.</summary>
  NodeDeallocation,

  /// <summary>A node completed a layout operation.</summary>
  NodeLayout,

  /// <summary>A layout pass started.</summary>
  LayoutPassStart,

  /// <summary>A layout pass ended.</summary>
  LayoutPassEnd,

  /// <summary>A measure callback started.</summary>
  MeasureCallbackStart,

  /// <summary>A measure callback ended.</summary>
  MeasureCallbackEnd,

  /// <summary>A baseline calculation started.</summary>
  NodeBaselineStart,

  /// <summary>A baseline calculation ended.</summary>
  NodeBaselineEnd
}

#endregion

#region Event Data

/// <summary>
/// Base interface for event data.
/// </summary>
public interface IEventData
{
  /// <summary>
  /// Gets the type of event this data represents.
  /// </summary>
  EventType EventType { get; }
}

/// <summary>
/// Event data for node allocation events.
/// </summary>
/// <param name="Config">The configuration associated with the node.</param>
public readonly record struct NodeAllocationEventData(Config? Config) : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.NodeAllocation;
}

/// <summary>
/// Event data for node deallocation events.
/// </summary>
/// <param name="Config">The configuration associated with the node.</param>
public readonly record struct NodeDeallocationEventData(Config? Config) : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.NodeDeallocation;
}

/// <summary>
/// Event data for node layout events.
/// </summary>
/// <param name="LayoutType">The type of layout operation performed.</param>
public readonly record struct NodeLayoutEventData(LayoutType LayoutType) : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.NodeLayout;
}

/// <summary>
/// Event data for layout pass start events.
/// </summary>
public readonly record struct LayoutPassStartEventData() : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.LayoutPassStart;
}

/// <summary>
/// Event data for layout pass end events.
/// </summary>
/// <param name="LayoutData">Statistics from the layout pass.</param>
public readonly record struct LayoutPassEndEventData(LayoutData? LayoutData) : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.LayoutPassEnd;
}

/// <summary>
/// Event data for measure callback start events.
/// </summary>
public readonly record struct MeasureCallbackStartEventData() : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.MeasureCallbackStart;
}

/// <summary>
/// Event data for measure callback end events.
/// </summary>
/// <param name="Width">The width constraint.</param>
/// <param name="WidthMeasureMode">The width measure mode.</param>
/// <param name="Height">The height constraint.</param>
/// <param name="HeightMeasureMode">The height measure mode.</param>
/// <param name="MeasuredWidth">The measured width result.</param>
/// <param name="MeasuredHeight">The measured height result.</param>
/// <param name="Reason">The reason for the measure call.</param>
public readonly record struct MeasureCallbackEndEventData(
    float Width,
    MeasureMode WidthMeasureMode,
    float Height,
    MeasureMode HeightMeasureMode,
    float MeasuredWidth,
    float MeasuredHeight,
    LayoutPassReason Reason) : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.MeasureCallbackEnd;
}

/// <summary>
/// Event data for node baseline start events.
/// </summary>
public readonly record struct NodeBaselineStartEventData() : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.NodeBaselineStart;
}

/// <summary>
/// Event data for node baseline end events.
/// </summary>
public readonly record struct NodeBaselineEndEventData() : IEventData
{
  /// <inheritdoc />
  public EventType EventType => EventType.NodeBaselineEnd;
}

#endregion

#region Event Publisher

/// <summary>
/// Delegate for event subscribers.
/// </summary>
/// <param name="node">The node associated with the event (may be null for some events).</param>
/// <param name="eventType">The type of event.</param>
/// <param name="eventData">The event data.</param>
public delegate void EventSubscriber(object? node, EventType eventType, IEventData eventData);

/// <summary>
/// Static event publisher for Yoga layout events.
/// </summary>
/// <remarks>
/// This is a thread-safe implementation using a lock-free linked list for subscribers,
/// matching the C++ implementation's behavior.
///
/// Design Decision: The C++ implementation uses an atomic linked list for thread safety.
/// In C#, we use a similar approach with Interlocked operations for the linked list head.
/// The class is named YogaEvent instead of Event to avoid conflict with C# reserved keyword.
/// </remarks>
public static class YogaEvent
{
  private sealed class SubscriberNode
  {
    public EventSubscriber Subscriber { get; }
    public SubscriberNode? Next { get; set; }

    public SubscriberNode(EventSubscriber subscriber)
    {
      Subscriber = subscriber;
    }
  }

  private static SubscriberNode? Subscribers;
  private static readonly Lock SubscribersLock = new();

  /// <summary>
  /// Subscribes to all events.
  /// </summary>
  /// <param name="subscriber">The subscriber delegate.</param>
  public static void Subscribe(EventSubscriber subscriber)
  {
    ArgumentNullException.ThrowIfNull(subscriber);
    Push(new SubscriberNode(subscriber));
  }

  /// <summary>
  /// Resets all subscribers, removing all registered callbacks.
  /// </summary>
  public static void Reset()
  {
    lock (SubscribersLock)
    {
      Subscribers = null;
    }
  }

  /// <summary>
  /// Publishes a node allocation event.
  /// </summary>
  /// <param name="node">The allocated node.</param>
  /// <param name="config">The configuration associated with the node.</param>
  public static void PublishNodeAllocation(object node, Config? config)
  {
    Publish(node, EventType.NodeAllocation, new NodeAllocationEventData(config));
  }

  /// <summary>
  /// Publishes a node deallocation event.
  /// </summary>
  /// <param name="node">The deallocated node.</param>
  /// <param name="config">The configuration associated with the node.</param>
  public static void PublishNodeDeallocation(object node, Config? config)
  {
    Publish(node, EventType.NodeDeallocation, new NodeDeallocationEventData(config));
  }

  /// <summary>
  /// Publishes a node layout event.
  /// </summary>
  /// <param name="node">The node that was laid out.</param>
  /// <param name="layoutType">The type of layout operation.</param>
  public static void PublishNodeLayout(object node, LayoutType layoutType)
  {
    Publish(node, EventType.NodeLayout, new NodeLayoutEventData(layoutType));
  }

  /// <summary>
  /// Publishes a layout pass start event.
  /// </summary>
  /// <param name="node">The root node of the layout pass.</param>
  public static void PublishLayoutPassStart(object node)
  {
    Publish(node, EventType.LayoutPassStart, new LayoutPassStartEventData());
  }

  /// <summary>
  /// Publishes a layout pass end event.
  /// </summary>
  /// <param name="node">The root node of the layout pass.</param>
  /// <param name="layoutData">Statistics from the layout pass.</param>
  public static void PublishLayoutPassEnd(object node, LayoutData? layoutData)
  {
    Publish(node, EventType.LayoutPassEnd, new LayoutPassEndEventData(layoutData));
  }

  /// <summary>
  /// Publishes a measure callback start event.
  /// </summary>
  /// <param name="node">The node being measured.</param>
  public static void PublishMeasureCallbackStart(object node)
  {
    Publish(node, EventType.MeasureCallbackStart, new MeasureCallbackStartEventData());
  }

  /// <summary>
  /// Publishes a measure callback end event.
  /// </summary>
  /// <param name="node">The node that was measured.</param>
  /// <param name="width">The width constraint.</param>
  /// <param name="widthMeasureMode">The width measure mode.</param>
  /// <param name="height">The height constraint.</param>
  /// <param name="heightMeasureMode">The height measure mode.</param>
  /// <param name="measuredWidth">The measured width result.</param>
  /// <param name="measuredHeight">The measured height result.</param>
  /// <param name="reason">The reason for the measure call.</param>
  public static void PublishMeasureCallbackEnd(
      object node,
      float width,
      MeasureMode widthMeasureMode,
      float height,
      MeasureMode heightMeasureMode,
      float measuredWidth,
      float measuredHeight,
      LayoutPassReason reason)
  {
    Publish(node, EventType.MeasureCallbackEnd, new MeasureCallbackEndEventData(
        width, widthMeasureMode, height, heightMeasureMode,
        measuredWidth, measuredHeight, reason));
  }

  /// <summary>
  /// Publishes a node baseline start event.
  /// </summary>
  /// <param name="node">The node being measured for baseline.</param>
  public static void PublishNodeBaselineStart(object node)
  {
    Publish(node, EventType.NodeBaselineStart, new NodeBaselineStartEventData());
  }

  /// <summary>
  /// Publishes a node baseline end event.
  /// </summary>
  /// <param name="node">The node that was measured for baseline.</param>
  public static void PublishNodeBaselineEnd(object node)
  {
    Publish(node, EventType.NodeBaselineEnd, new NodeBaselineEndEventData());
  }

  /// <summary>
  /// Publishes an event to all subscribers.
  /// </summary>
  /// <param name="node">The node associated with the event.</param>
  /// <param name="eventType">The type of event.</param>
  /// <param name="eventData">The event data.</param>
  public static void Publish(object? node, EventType eventType, IEventData eventData)
  {
    SubscriberNode? current;
    lock (SubscribersLock)
    {
      current = Subscribers;
    }

    while (current is not null)
    {
      current.Subscriber(node, eventType, eventData);
      current = current.Next;
    }
  }

  private static void Push(SubscriberNode newNode)
  {
    lock (SubscribersLock)
    {
      newNode.Next = Subscribers;
      Subscribers = newNode;
    }
  }

  /// <summary>
  /// Gets the number of registered subscribers (for testing purposes).
  /// </summary>
  internal static int SubscriberCount
  {
    get
    {
      int count = 0;
      lock (SubscribersLock)
      {
        SubscriberNode? current = Subscribers;
        while (current is not null)
        {
          count++;
          current = current.Next;
        }
      }

      return count;
    }
  }
}

#endregion
