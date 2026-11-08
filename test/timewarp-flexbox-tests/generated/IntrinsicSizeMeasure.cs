/*
 * TimeWarp.Flexbox - C# port of Facebook Yoga
 *
 * Hand-written port of the IntrinsicSizeMeasure helper from Yoga's
 * tests/util/TestUtil.cpp: simulates word-wrapping text at 10px per
 * character for the generated IntrinsicSize conformance tests. The text
 * is passed via Node.Context (mirroring YGNodeSetContext in the C++).
 */

namespace TimeWarp.Flexbox.Tests.Generated;

using FlexNode = TimeWarp.Flexbox.Node;

internal static class IntrinsicSizeMeasure
{
  private const float WidthPerChar = 10f;
  private const float HeightPerChar = 10f;

  public static YGSize Measure(
      FlexNode node,
      float width,
      MeasureMode widthMode,
      float height,
      MeasureMode heightMode)
  {
    string innerText = (string)node.Context!;
    float measuredWidth;
    float measuredHeight;

    if (widthMode == MeasureMode.Exactly)
    {
      measuredWidth = width;
    }
    else if (widthMode == MeasureMode.AtMost)
    {
      measuredWidth = Math.Min(innerText.Length * WidthPerChar, width);
    }
    else
    {
      measuredWidth = innerText.Length * WidthPerChar;
    }

    if (heightMode == MeasureMode.Exactly)
    {
      measuredHeight = height;
    }
    else if (heightMode == MeasureMode.AtMost)
    {
      measuredHeight = Math.Min(
          CalculateHeight(
              innerText,
              node.Style.FlexDirection == FlexDirection.Column
                  ? measuredWidth
                  : Math.Max(LongestWordWidth(innerText), measuredWidth)),
          height);
    }
    else
    {
      measuredHeight = CalculateHeight(
          innerText,
          node.Style.FlexDirection == FlexDirection.Column
              ? measuredWidth
              : Math.Max(LongestWordWidth(innerText), measuredWidth));
    }

    return new YGSize(measuredWidth, measuredHeight);
  }

  private static float LongestWordWidth(string text)
  {
    int maxLength = 0;
    int currentLength = 0;
    foreach (char c in text)
    {
      if (c == ' ')
      {
        maxLength = Math.Max(currentLength, maxLength);
        currentLength = 0;
      }
      else
      {
        currentLength++;
      }
    }

    return Math.Max(currentLength, maxLength) * WidthPerChar;
  }

  private static float CalculateHeight(string text, float measuredWidth)
  {
    if (text.Length * WidthPerChar <= measuredWidth)
    {
      return HeightPerChar;
    }

    string[] words = text.Split(' ');
    float lines = 1;
    float currentLineLength = 0;
    foreach (string word in words)
    {
      float wordWidth = word.Length * WidthPerChar;
      if (wordWidth > measuredWidth)
      {
        if (currentLineLength > 0)
        {
          lines++;
        }

        lines++;
        currentLineLength = 0;
      }
      else if (currentLineLength + wordWidth <= measuredWidth)
      {
        currentLineLength += wordWidth + WidthPerChar;
      }
      else
      {
        lines++;
        currentLineLength = wordWidth + WidthPerChar;
      }
    }

    return (currentLineLength == 0 ? lines - 1 : lines) * HeightPerChar;
  }
}
