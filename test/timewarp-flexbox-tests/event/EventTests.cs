/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Tests for Event system
 * Original C++ source: tests/EventsTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.YogaEvents;

/// <summary>
/// Tests for the event system.
/// </summary>
/// <remarks>
/// These tests are ported from the C++ EventsTest.cpp.
/// Since we don't have a full Node implementation yet, some tests
/// are adapted to test the event infrastructure directly.
/// </remarks>
public sealed class EventTests : IDisposable
{
  private readonly List<RecordedEvent> _events = [];

  public EventTests()
  {
    YogaEvent.Subscribe(RecordEvent);
  }

  public void Dispose()
  {
    YogaEvent.Reset();
    _events.Clear();
  }

  private void RecordEvent(object? node, EventType eventType, IEventData eventData)
  {
    _events.Add(new RecordedEvent(node, eventType, eventData));
  }

  private RecordedEvent LastEvent => _events[^1];

  #region Helper Types

  private sealed record RecordedEvent(object? Node, EventType Type, IEventData Data);

  /// <summary>
  /// Mock node for testing events without full Node implementation.
  /// </summary>
  private sealed class MockNode
  {
    public TimeWarp.Flexbox.Config? Config { get; init; }
  }

  #endregion

  #region Subscribe and Reset Tests

  public void ResetShouldRemoveAllSubscribers()
  {
    YogaEvent.Reset();

    // Verify no events are received after reset
    MockNode node = new();
    int countBefore = _events.Count;
    YogaEvent.PublishNodeAllocation(node, null);
    _events.Count.ShouldBe(countBefore); // No new events since subscriber was removed

    // Re-subscribe for other tests
    YogaEvent.Subscribe(RecordEvent);
  }

  public void MultipleSubscribersShouldAllReceiveEvents()
  {
    int subscriber2Count = 0;
    int subscriber3Count = 0;

    YogaEvent.Subscribe((_, _, _) => subscriber2Count++);
    YogaEvent.Subscribe((_, _, _) => subscriber3Count++);

    MockNode node = new();
    YogaEvent.PublishNodeAllocation(node, null);

    // Original subscriber
    _events.Count.ShouldBe(1);
    // Additional subscribers
    subscriber2Count.ShouldBe(1);
    subscriber3Count.ShouldBe(1);
  }

  #endregion

  #region NodeAllocation Event Tests

  public void PublishNodeAllocationShouldPublishEvent()
  {
    MockNode node = new();
    TimeWarp.Flexbox.Config config = new();

    YogaEvent.PublishNodeAllocation(node, config);

    _events.Count.ShouldBeGreaterThan(0);
    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.NodeAllocation);
  }

  public void NodeAllocationEventShouldContainConfig()
  {
    TimeWarp.Flexbox.Config config = new();
    MockNode node = new() { Config = config };

    YogaEvent.PublishNodeAllocation(node, config);

    LastEvent.Data.ShouldBeOfType<NodeAllocationEventData>();
    NodeAllocationEventData data = (NodeAllocationEventData)LastEvent.Data;
    data.Config.ShouldBeSameAs(config);
  }

  public void NodeAllocationEventShouldAllowNullConfig()
  {
    MockNode node = new();

    YogaEvent.PublishNodeAllocation(node, null);

    NodeAllocationEventData data = (NodeAllocationEventData)LastEvent.Data;
    data.Config.ShouldBeNull();
  }

  #endregion

  #region NodeDeallocation Event Tests

  public void PublishNodeDeallocationShouldPublishEvent()
  {
    MockNode node = new();
    TimeWarp.Flexbox.Config config = new();

    YogaEvent.PublishNodeDeallocation(node, config);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.NodeDeallocation);
  }

  public void NodeDeallocationEventShouldContainConfig()
  {
    TimeWarp.Flexbox.Config config = new();
    MockNode node = new() { Config = config };

    YogaEvent.PublishNodeDeallocation(node, config);

    NodeDeallocationEventData data = (NodeDeallocationEventData)LastEvent.Data;
    data.Config.ShouldBeSameAs(config);
  }

  #endregion

  #region NodeLayout Event Tests

  public void PublishNodeLayoutShouldPublishEvent()
  {
    MockNode node = new();

    YogaEvent.PublishNodeLayout(node, LayoutType.Layout);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.NodeLayout);
  }

  public void NodeLayoutEventShouldContainLayoutType()
  {
    MockNode node = new();

    YogaEvent.PublishNodeLayout(node, LayoutType.Measure);

    NodeLayoutEventData data = (NodeLayoutEventData)LastEvent.Data;
    data.LayoutType.ShouldBe(LayoutType.Measure);
  }

  public void NodeLayoutEventShouldDistinguishCachedOperations()
  {
    MockNode node = new();

    YogaEvent.PublishNodeLayout(node, LayoutType.CachedLayout);
    NodeLayoutEventData cachedLayoutData = (NodeLayoutEventData)LastEvent.Data;
    cachedLayoutData.LayoutType.ShouldBe(LayoutType.CachedLayout);

    YogaEvent.PublishNodeLayout(node, LayoutType.CachedMeasure);
    NodeLayoutEventData cachedMeasureData = (NodeLayoutEventData)LastEvent.Data;
    cachedMeasureData.LayoutType.ShouldBe(LayoutType.CachedMeasure);
  }

  #endregion

  #region LayoutPassStart Event Tests

  public void PublishLayoutPassStartShouldPublishEvent()
  {
    MockNode node = new();

    YogaEvent.PublishLayoutPassStart(node);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.LayoutPassStart);
  }

  public void LayoutPassStartEventDataShouldHaveCorrectType()
  {
    MockNode node = new();

    YogaEvent.PublishLayoutPassStart(node);

    LastEvent.Data.ShouldBeOfType<LayoutPassStartEventData>();
    LastEvent.Data.EventType.ShouldBe(EventType.LayoutPassStart);
  }

  #endregion

  #region LayoutPassEnd Event Tests

  public void PublishLayoutPassEndShouldPublishEvent()
  {
    MockNode node = new();
    LayoutData layoutData = new();

    YogaEvent.PublishLayoutPassEnd(node, layoutData);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.LayoutPassEnd);
  }

  public void LayoutPassEndEventShouldContainLayoutData()
  {
    MockNode node = new();
    LayoutData layoutData = new()
    {
      Layouts = 5,
      Measures = 10,
      CachedLayouts = 2,
      CachedMeasures = 3,
      MaxMeasureCache = 15
    };

    YogaEvent.PublishLayoutPassEnd(node, layoutData);

    LayoutPassEndEventData data = (LayoutPassEndEventData)LastEvent.Data;
    data.LayoutData.ShouldNotBeNull();
    data.LayoutData!.Layouts.ShouldBe(5);
    data.LayoutData.Measures.ShouldBe(10);
    data.LayoutData.CachedLayouts.ShouldBe(2);
    data.LayoutData.CachedMeasures.ShouldBe(3);
    data.LayoutData.MaxMeasureCache.ShouldBe(15u);
  }

  public void LayoutPassEndEventShouldAllowNullLayoutData()
  {
    MockNode node = new();

    YogaEvent.PublishLayoutPassEnd(node, null);

    LayoutPassEndEventData data = (LayoutPassEndEventData)LastEvent.Data;
    data.LayoutData.ShouldBeNull();
  }

  #endregion

  #region MeasureCallback Event Tests

  public void PublishMeasureCallbackStartShouldPublishEvent()
  {
    MockNode node = new();

    YogaEvent.PublishMeasureCallbackStart(node);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.MeasureCallbackStart);
  }

  public void PublishMeasureCallbackEndShouldPublishEvent()
  {
    MockNode node = new();

    YogaEvent.PublishMeasureCallbackEnd(
        node,
        width: 100.0f,
        widthMeasureMode: MeasureMode.Exactly,
        height: 200.0f,
        heightMeasureMode: MeasureMode.AtMost,
        measuredWidth: 100.0f,
        measuredHeight: 150.0f,
        reason: LayoutPassReason.Initial);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.MeasureCallbackEnd);
  }

  public void MeasureCallbackEndEventShouldContainMeasurementData()
  {
    MockNode node = new();

    YogaEvent.PublishMeasureCallbackEnd(
        node,
        width: 100.0f,
        widthMeasureMode: MeasureMode.Exactly,
        height: 200.0f,
        heightMeasureMode: MeasureMode.AtMost,
        measuredWidth: 100.0f,
        measuredHeight: 150.0f,
        reason: LayoutPassReason.FlexMeasure);

    MeasureCallbackEndEventData data = (MeasureCallbackEndEventData)LastEvent.Data;
    data.Width.ShouldBe(100.0f);
    data.WidthMeasureMode.ShouldBe(MeasureMode.Exactly);
    data.Height.ShouldBe(200.0f);
    data.HeightMeasureMode.ShouldBe(MeasureMode.AtMost);
    data.MeasuredWidth.ShouldBe(100.0f);
    data.MeasuredHeight.ShouldBe(150.0f);
    data.Reason.ShouldBe(LayoutPassReason.FlexMeasure);
  }

  #endregion

  #region NodeBaseline Event Tests

  public void PublishNodeBaselineStartShouldPublishEvent()
  {
    MockNode node = new();

    YogaEvent.PublishNodeBaselineStart(node);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.NodeBaselineStart);
  }

  public void PublishNodeBaselineEndShouldPublishEvent()
  {
    MockNode node = new();

    YogaEvent.PublishNodeBaselineEnd(node);

    LastEvent.Node.ShouldBeSameAs(node);
    LastEvent.Type.ShouldBe(EventType.NodeBaselineEnd);
  }

  #endregion

  #region LayoutData Tests

  public void LayoutDataCopyShouldCreateIndependentCopy()
  {
    LayoutData original = new()
    {
      Layouts = 5,
      Measures = 10,
      CachedLayouts = 2,
      CachedMeasures = 3,
      MaxMeasureCache = 15,
      MeasureCallbacks = 7
    };
    original.IncrementMeasureCallbackReasonCount(LayoutPassReason.Initial);
    original.IncrementMeasureCallbackReasonCount(LayoutPassReason.FlexLayout);

    LayoutData copy = original.Copy();

    // Verify values are copied
    copy.Layouts.ShouldBe(5);
    copy.Measures.ShouldBe(10);
    copy.CachedLayouts.ShouldBe(2);
    copy.CachedMeasures.ShouldBe(3);
    copy.MaxMeasureCache.ShouldBe(15u);
    copy.MeasureCallbacks.ShouldBe(7);
    copy.GetMeasureCallbackReasonCount(LayoutPassReason.Initial).ShouldBe(1);
    copy.GetMeasureCallbackReasonCount(LayoutPassReason.FlexLayout).ShouldBe(1);

    // Verify independence
    original.Layouts = 100;
    copy.Layouts.ShouldBe(5);

    original.IncrementMeasureCallbackReasonCount(LayoutPassReason.Initial);
    copy.GetMeasureCallbackReasonCount(LayoutPassReason.Initial).ShouldBe(1);
  }

  public void LayoutDataMeasureCallbackReasonCountsShouldBeIndependent()
  {
    LayoutData data = new();

    data.IncrementMeasureCallbackReasonCount(LayoutPassReason.Initial);
    data.IncrementMeasureCallbackReasonCount(LayoutPassReason.Initial);
    data.IncrementMeasureCallbackReasonCount(LayoutPassReason.Stretch);

    data.GetMeasureCallbackReasonCount(LayoutPassReason.Initial).ShouldBe(2);
    data.GetMeasureCallbackReasonCount(LayoutPassReason.Stretch).ShouldBe(1);
    data.GetMeasureCallbackReasonCount(LayoutPassReason.FlexLayout).ShouldBe(0);
  }

  #endregion

  #region LayoutPassReason Extension Tests

  public void LayoutPassReasonToReasonStringShouldReturnCorrectValues()
  {
    LayoutPassReason.Initial.ToReasonString().ShouldBe("initial");
    LayoutPassReason.AbsLayout.ToReasonString().ShouldBe("abs_layout");
    LayoutPassReason.Stretch.ToReasonString().ShouldBe("stretch");
    LayoutPassReason.MultilineStretch.ToReasonString().ShouldBe("multiline_stretch");
    LayoutPassReason.FlexLayout.ToReasonString().ShouldBe("flex_layout");
    LayoutPassReason.MeasureChild.ToReasonString().ShouldBe("measure");
    LayoutPassReason.AbsMeasureChild.ToReasonString().ShouldBe("abs_measure");
    LayoutPassReason.FlexMeasure.ToReasonString().ShouldBe("flex_measure");
  }

  public void LayoutPassReasonToReasonStringShouldReturnUnknownForInvalidValue()
  {
    LayoutPassReason invalidReason = (LayoutPassReason)999;
    invalidReason.ToReasonString().ShouldBe("unknown");
  }

  #endregion

  #region LayoutType Tests

  public void LayoutTypeShouldHaveCorrectValues()
  {
    ((int)LayoutType.Layout).ShouldBe(0);
    ((int)LayoutType.Measure).ShouldBe(1);
    ((int)LayoutType.CachedLayout).ShouldBe(2);
    ((int)LayoutType.CachedMeasure).ShouldBe(3);
  }

  #endregion

  #region EventType Tests

  public void AllEventDataTypesShouldReturnCorrectEventType()
  {
    new NodeAllocationEventData(null).EventType.ShouldBe(EventType.NodeAllocation);
    new NodeDeallocationEventData(null).EventType.ShouldBe(EventType.NodeDeallocation);
    new NodeLayoutEventData(LayoutType.Layout).EventType.ShouldBe(EventType.NodeLayout);
    new LayoutPassStartEventData().EventType.ShouldBe(EventType.LayoutPassStart);
    new LayoutPassEndEventData(null).EventType.ShouldBe(EventType.LayoutPassEnd);
    new MeasureCallbackStartEventData().EventType.ShouldBe(EventType.MeasureCallbackStart);
    new MeasureCallbackEndEventData(0, MeasureMode.Undefined, 0, MeasureMode.Undefined, 0, 0, LayoutPassReason.Initial).EventType.ShouldBe(EventType.MeasureCallbackEnd);
    new NodeBaselineStartEventData().EventType.ShouldBe(EventType.NodeBaselineStart);
    new NodeBaselineEndEventData().EventType.ShouldBe(EventType.NodeBaselineEnd);
  }

  #endregion

  #region Thread Safety Tests (Basic)

  public void ConcurrentPublishShouldNotThrow()
  {
    int eventCount = 0;
    object lockObj = new();

    YogaEvent.Subscribe((_, _, _) =>
    {
      lock (lockObj)
      {
        eventCount++;
      }
    });

    MockNode[] nodes = [.. Enumerable.Range(0, 10).Select(_ => new MockNode())];

    // Publish events concurrently from multiple threads
    Parallel.ForEach(nodes, node =>
    {
      YogaEvent.PublishNodeAllocation(node, null);
      YogaEvent.PublishNodeLayout(node, LayoutType.Layout);
      YogaEvent.PublishNodeDeallocation(node, null);
    });

    // Each node publishes 3 events, 10 nodes = 30 events per subscriber
    // We have 2 subscribers (original + this one)
    eventCount.ShouldBe(30);
  }

  #endregion
}
