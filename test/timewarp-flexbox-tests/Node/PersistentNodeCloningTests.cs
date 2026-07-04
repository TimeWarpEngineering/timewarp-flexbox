/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ tests: tests/YGPersistentNodeCloningTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Node_;

using FlexConfig = TimeWarp.Flexbox.Config;
using FlexNode = TimeWarp.Flexbox.Node;

/// <summary>
/// Tests for persistent node cloning via the clone node callback.
/// Ported from C++ YGPersistentNodeCloningTest.cpp.
/// </summary>
public class PersistentNodeCloningTests
{
  /// <summary>
  /// C# equivalent of the C++ test fixture's NodeWrapper: pairs a yoga node with
  /// the wrapper objects of its children, wiring the wrapper into the node context.
  /// </summary>
  private sealed class NodeWrapper
  {
    public FlexNode Node { get; }
    public List<NodeWrapper> Children { get; }

    // Fresh node, optionally with children.
    public NodeWrapper(FlexConfig config, List<NodeWrapper>? children = null)
    {
      Node = new FlexNode(config);
      Node.Context = this;
      Children = children ?? [];
      AttachChildren();
    }

    // Clone, with current children, for mutation.
    public NodeWrapper(NodeWrapper other)
    {
      Node = other.Node.Clone();
      Node.Context = this;
      Node.Owner = null;
      Children = [.. other.Children];
    }

    // Clone, with new children.
    public NodeWrapper(NodeWrapper other, List<NodeWrapper> children)
    {
      Node = other.Node.Clone();
      Node.Context = this;
      Node.Owner = null;
      Children = children;
      Node.SetDirty(true);
      AttachChildren();
    }

    private void AttachChildren()
    {
      List<FlexNode> childNodes = [];
      foreach (NodeWrapper child in Children)
      {
        childNodes.Add(child.Node);
      }

      Node.SetChildren(childNodes);

      // Claim first ownership of not yet owned nodes, to avoid immediately
      // cloning them.
      foreach (NodeWrapper child in Children)
      {
        child.Node.Owner ??= Node;
      }
    }
  }

  /// <summary>
  /// Holds the per-test onClone observer so it can be swapped mid-test,
  /// mirroring the C++ fixture's static onClone function.
  /// </summary>
  private sealed class CloneObserver
  {
    public Action<FlexNode, FlexNode?, int> OnClone { get; set; } = (_, _, _) => { };
  }

  private static FlexConfig CreateConfig(CloneObserver observer)
  {
    FlexConfig config = new();
    config.SetCloneNodeCallback((oldNode, owner, childIndex) =>
    {
      FlexNode oldFlexNode = (FlexNode)oldNode;
      FlexNode ownerNode = (FlexNode)owner!;
      observer.OnClone(oldFlexNode, ownerNode, childIndex);

      NodeWrapper ownerWrapper = (NodeWrapper)ownerNode.Context!;
      NodeWrapper oldWrapper = (NodeWrapper)oldFlexNode.Context!;

      ownerWrapper.Children[childIndex] = new NodeWrapper(oldWrapper);
      return ownerWrapper.Children[childIndex].Node;
    });
    return config;
  }

  public void changing_sibling_height_does_not_clone_neighbors()
  {
    // <ScrollView>
    //   <View id="Sibling" style={{ height: 1 }} />
    //   <View id="A" style={{ height: 1 }}>
    //     <View id="B">
    //       <View id="C">
    //         <View id="D"/>
    //       </View>
    //     </View>
    //   </View>
    // </ScrollView>

    CloneObserver observer = new();
    FlexConfig config = CreateConfig(observer);

    NodeWrapper sibling = new(config);
    sibling.Node.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(1f));

    NodeWrapper d = new(config);
    NodeWrapper c = new(config, [d]);
    NodeWrapper b = new(config, [c]);
    NodeWrapper a = new(config, [b]);
    a.Node.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(1f));

    NodeWrapper scrollContentView = new(config, [sibling, a]);
    scrollContentView.Node.Style.PositionType = PositionType.Absolute;

    NodeWrapper scrollView = new(config, [scrollContentView]);
    scrollView.Node.Style.SetDimension(Dimension.Width, StyleSizeLength.Points(100f));
    scrollView.Node.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(100f));

    // We don't expect any cloning during the first layout
    observer.OnClone = (_, _, _) =>
        throw new ShouldAssertException("Unexpected node clone during first layout");

    CalculateLayout.Calculate(scrollView.Node, float.NaN, float.NaN, Direction.LTR);

    NodeWrapper siblingPrime = new(config);
    siblingPrime.Node.Style.SetDimension(Dimension.Height, StyleSizeLength.Points(2f));

    NodeWrapper scrollContentViewPrime = new(scrollContentView, [siblingPrime, a]);
    NodeWrapper scrollViewPrime = new(scrollView, [scrollContentViewPrime]);

    List<NodeWrapper> nodesCloned = [];
    // We should only need to clone "A"
    observer.OnClone = (oldNode, _, _) =>
        nodesCloned.Add((NodeWrapper)oldNode.Context!);

    CalculateLayout.Calculate(scrollViewPrime.Node, float.NaN, float.NaN, Direction.LTR);

    nodesCloned.Count.ShouldBe(1);
    nodesCloned[0].ShouldBeSameAs(a);
  }

  public void clone_leaf_display_contents_node()
  {
    // <View id="A">
    //   <View id="B" style={{ display: 'contents' }} />
    // </View>

    CloneObserver observer = new();
    FlexConfig config = CreateConfig(observer);

    NodeWrapper b = new(config);
    NodeWrapper a = new(config, [b]);
    b.Node.Style.Display = Display.Contents;

    // We don't expect any cloning during the first layout
    observer.OnClone = (_, _, _) =>
        throw new ShouldAssertException("Unexpected node clone during first layout");

    CalculateLayout.Calculate(a.Node, float.NaN, float.NaN, Direction.LTR);

    NodeWrapper aPrime = new(config, [b]);

    List<NodeWrapper> nodesCloned = [];
    // We should clone "B"
    observer.OnClone = (oldNode, _, _) =>
        nodesCloned.Add((NodeWrapper)oldNode.Context!);

    CalculateLayout.Calculate(aPrime.Node, 100f, 100f, Direction.LTR);

    nodesCloned.Count.ShouldBe(1);
    nodesCloned[0].ShouldBeSameAs(b);
  }
}
