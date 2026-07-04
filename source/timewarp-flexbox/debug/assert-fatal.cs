namespace TimeWarp.Flexbox;

/// <summary>
/// Exception thrown when a fatal Yoga assertion fails.
/// </summary>
/// <remarks>
/// Corresponds to the std::logic_error thrown by fatalWithMessage in
/// yoga/debug/AssertFatal.cpp. C++ Yoga throws (or terminates when exceptions
/// are disabled); in C# we always throw.
/// </remarks>
public class YogaAssertException : Exception
{
  /// <summary>Initializes a new instance of the <see cref="YogaAssertException"/> class.</summary>
  public YogaAssertException()
  {
  }

  /// <summary>Initializes a new instance of the <see cref="YogaAssertException"/> class with a message.</summary>
  /// <param name="message">The assertion failure message.</param>
  public YogaAssertException(string message) : base(message)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="YogaAssertException"/> class with a message and inner exception.</summary>
  /// <param name="message">The assertion failure message.</param>
  /// <param name="innerException">The inner exception.</param>
  public YogaAssertException(string message, Exception innerException) : base(message, innerException)
  {
  }
}

/// <summary>
/// Fatal assertion utilities used throughout the layout engine.
/// </summary>
/// <remarks>
/// Corresponds to yoga/debug/AssertFatal.h and AssertFatal.cpp
/// (assertFatal, assertFatalWithNode, assertFatalWithConfig).
/// </remarks>
internal static class YogaAssert
{
  /// <summary>
  /// Throws a <see cref="YogaAssertException"/> with the given message.
  /// </summary>
  /// <param name="message">The failure message.</param>
  /// <remarks>Corresponds to fatalWithMessage in AssertFatal.cpp.</remarks>
  [DoesNotReturn]
  public static void FatalWithMessage(string message) =>
    throw new YogaAssertException(message);

  /// <summary>
  /// Asserts that a condition is true, throwing a <see cref="YogaAssertException"/> otherwise.
  /// </summary>
  /// <param name="condition">The condition that must hold.</param>
  /// <param name="message">The failure message.</param>
  /// <remarks>Corresponds to assertFatal in AssertFatal.cpp.</remarks>
  public static void Assert([DoesNotReturnIf(false)] bool condition, string message)
  {
    if (!condition)
    {
      YogaLog.Log(LogLevel.Fatal, message);
      FatalWithMessage(message);
    }
  }

  /// <summary>
  /// Asserts that a condition is true, logging against a node context on failure.
  /// </summary>
  /// <param name="node">The node context, if any.</param>
  /// <param name="condition">The condition that must hold.</param>
  /// <param name="message">The failure message.</param>
  /// <remarks>Corresponds to assertFatalWithNode in AssertFatal.cpp.</remarks>
  public static void Assert(Node? node, [DoesNotReturnIf(false)] bool condition, string message)
  {
    if (!condition)
    {
      YogaLog.Log(node, LogLevel.Fatal, message);
      FatalWithMessage(message);
    }
  }

  /// <summary>
  /// Asserts that a condition is true, logging against a config context on failure.
  /// </summary>
  /// <param name="config">The config context, if any.</param>
  /// <param name="condition">The condition that must hold.</param>
  /// <param name="message">The failure message.</param>
  /// <remarks>Corresponds to assertFatalWithConfig in AssertFatal.cpp.</remarks>
  public static void Assert(Config? config, [DoesNotReturnIf(false)] bool condition, string message)
  {
    if (!condition)
    {
      YogaLog.Log(config, LogLevel.Fatal, message);
      FatalWithMessage(message);
    }
  }
}
