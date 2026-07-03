namespace TimeWarp.Flexbox;

/// <summary>
/// Custom log handler invoked for Yoga log messages.
/// </summary>
/// <param name="context">The node or config the message relates to, if any.</param>
/// <param name="level">The severity of the message.</param>
/// <param name="message">The log message.</param>
/// <remarks>
/// Corresponds to YGLogger in the C++ public API. C# uses a formatted string
/// instead of a printf format plus va_list.
/// </remarks>
public delegate void YogaLogHandler(object? context, LogLevel level, string message);

/// <summary>
/// Logging utilities used throughout the layout engine.
/// </summary>
/// <remarks>
/// Corresponds to yoga/debug/Log.h and Log.cpp (the log overloads and
/// getDefaultLogger).
/// </remarks>
public static class YogaLog
{
  /// <summary>
  /// Logs a message with no node or config context.
  /// </summary>
  /// <param name="level">The severity of the message.</param>
  /// <param name="message">The log message.</param>
  /// <remarks>Corresponds to log(LogLevel, const char*, ...) in Log.cpp.</remarks>
  public static void Log(LogLevel level, string message) =>
    VLog(null, null, level, message);

  /// <summary>
  /// Logs a message associated with a node, routing through the node's config.
  /// </summary>
  /// <param name="node">The node context, if any.</param>
  /// <param name="level">The severity of the message.</param>
  /// <param name="message">The log message.</param>
  /// <remarks>Corresponds to log(const Node*, LogLevel, const char*, ...) in Log.cpp.</remarks>
  public static void Log(Node? node, LogLevel level, string message) =>
    VLog(node?.Config, node, level, message);

  /// <summary>
  /// Logs a message associated with a config.
  /// </summary>
  /// <param name="config">The config context, if any.</param>
  /// <param name="level">The severity of the message.</param>
  /// <param name="message">The log message.</param>
  /// <remarks>Corresponds to log(const Config*, LogLevel, const char*, ...) in Log.cpp.</remarks>
  public static void Log(Config? config, LogLevel level, string message) =>
    VLog(config, null, level, message);

  /// <summary>
  /// The default logger: errors and fatals go to standard error, everything else
  /// to standard output.
  /// </summary>
  /// <param name="context">The node or config the message relates to, if any. Unused, matching C++.</param>
  /// <param name="level">The severity of the message.</param>
  /// <param name="message">The log message.</param>
  /// <remarks>Corresponds to getDefaultLogger in Log.cpp (non-Android branch).</remarks>
  public static void DefaultLog(object? context, LogLevel level, string message)
  {
    _ = context;
    switch (level)
    {
      case LogLevel.Error:
      case LogLevel.Fatal:
        Console.Error.WriteLine(message);
        break;
      case LogLevel.Warn:
      case LogLevel.Info:
      case LogLevel.Debug:
      case LogLevel.Verbose:
      default:
        Console.WriteLine(message);
        break;
    }
  }

  /// <remarks>Corresponds to the anonymous vlog helper in Log.cpp.</remarks>
  private static void VLog(Config? config, Node? node, LogLevel level, string message)
  {
    if (config is null)
    {
      DefaultLog(node, level, message);
    }
    else
    {
      config.Log(node, level, message);
    }
  }
}
