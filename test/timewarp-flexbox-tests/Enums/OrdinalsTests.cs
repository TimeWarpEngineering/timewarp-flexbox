/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Original C++ source: tests/OrdinalsTest.cpp
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * Licensed under the MIT license.
 */

namespace TimeWarp.Flexbox.Tests.Enums;

/// <summary>
/// Tests for YogaEnums utilities.
/// Ported from C++ tests/OrdinalsTest.cpp
/// </summary>
public class OrdinalsTests
{
    /// <summary>
    /// Test that iterating through ordinals produces all expected enum values in order.
    /// Equivalent to C++ TEST(Ordinals, iteration)
    /// </summary>
    public void OrdinalsIterationProducesAllEdgeValuesInOrder()
    {
        // Arrange
        Queue<Edge> expectedEdges = new(new[]
        {
            Edge.Left,
            Edge.Top,
            Edge.Right,
            Edge.Bottom,
            Edge.Start,
            Edge.End,
            Edge.Horizontal,
            Edge.Vertical,
            Edge.All
        });

        // Act & Assert
        foreach (Edge edge in YogaEnums.Ordinals<Edge>())
        {
            expectedEdges.Count.ShouldBeGreaterThan(0, "More edges iterated than expected");
            edge.ShouldBe(expectedEdges.Dequeue());
        }

        expectedEdges.Count.ShouldBe(0, "Not all expected edges were iterated");
    }

    /// <summary>
    /// Test that OrdinalCount returns correct value for Edge enum.
    /// </summary>
    public void OrdinalCountEdgeReturns9()
    {
        YogaEnums.OrdinalCount<Edge>().ShouldBe(9);
    }

    /// <summary>
    /// Test that BitCount returns correct value for Edge enum (9 values needs 4 bits).
    /// </summary>
    public void BitCountEdgeReturns4()
    {
        // 9 ordinals (0-8) require 4 bits to represent (max value 8 = 1000 in binary)
        YogaEnums.BitCount<Edge>().ShouldBe(4);
    }

    /// <summary>
    /// Test that ToUnderlying converts enum values correctly.
    /// </summary>
    public void ToUnderlyingEdgeValuesReturnsCorrectIntegers()
    {
        YogaEnums.ToUnderlying(Edge.Left).ShouldBe(0);
        YogaEnums.ToUnderlying(Edge.Top).ShouldBe(1);
        YogaEnums.ToUnderlying(Edge.Right).ShouldBe(2);
        YogaEnums.ToUnderlying(Edge.Bottom).ShouldBe(3);
        YogaEnums.ToUnderlying(Edge.Start).ShouldBe(4);
        YogaEnums.ToUnderlying(Edge.End).ShouldBe(5);
        YogaEnums.ToUnderlying(Edge.Horizontal).ShouldBe(6);
        YogaEnums.ToUnderlying(Edge.Vertical).ShouldBe(7);
        YogaEnums.ToUnderlying(Edge.All).ShouldBe(8);
    }

    /// <summary>
    /// Test that OrdinalCount throws for enums without the attribute.
    /// </summary>
    public void OrdinalCountEnumWithoutAttributeThrows()
    {
        Should.Throw<InvalidOperationException>(() => YogaEnums.OrdinalCount<DayOfWeek>());
    }
}
