using System;
using System.IO;
using System.Collections.Generic;

public static class Logging
{
    public static void Main()
    {
        PathFinder pathFinder_1 = new PathFinder(new FileLogger());
        PathFinder pathFinder_2 = new PathFinder(new ConsoleLogger());
        PathFinder pathFinder_3 = new PathFinder(new SecureLogger(new FileLogger(), new FridayLoggingPolicy()));
        PathFinder pathFinder_4 = new PathFinder(new SecureLogger(new ConsoleLogger(), new FridayLoggingPolicy())));
        PathFinder pathFinder_5 = new PathFinder(new ConsoleLogger(), new SecureLogger(new FileLogger(), new FridayLoggingPolicy()));
        pathFinder_1.Find();
        pathFinder_2.Find();
        pathFinder_3.Find();
        pathFinder_4.Find();
        pathFinder_5.Find();
        Console.ReadKey();
    }
}

public interface ILogger
{
    void WriteError(string message);
}

public class ConsoleLogger : ILogger
{
    public void WriteError(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        Console.WriteLine(message);
    }
}

public class FileLogger : ILogger
{
    private readonly string _path = "log.txt";

    public void WriteError(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

        File.WriteAllText(_path, message);
    }
}

public class SecureLogger : ILogger
{
    private readonly ILogger _logger;
    private readonly ILoggingPolicy _loggingPolicy;

    public SecureLogger(ILogger logger, ILoggingPolicy loggingPolicy)
    {
        if (logger == null)
            throw new ArgumentNullException(nameof(logger));

        if (loggingPolicy == null)
            throw new ArgumentNullException(nameof(loggingPolicy));

        _logger = logger;
        _loggingPolicy = loggingPolicy;
    }

    public void WriteError(string message)
    {
        if (_loggingPolicy.CanWriteError())
            _logger.WriteError(message);
    }
}

public interface ILoggingPolicy
{
    bool CanWriteError();
}

public class FridayLoggingPolicy
{
    public bool CanWriteError() 
        => DateTime.Now.DayOfWeek == DayOfWeek.Friday
}

public class PathFinder
{
    private readonly string _message = "...";
    private readonly ILogger[] _loggers;

    public PathFinder(params ILogger[] loggers)
    {
        if (loggers == null)
            throw new ArgumentNullException(nameof(loggers));

        if (loggers.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(loggers));

        _loggers = loggers;
    }

    public void Find()
    {
        foreach (var logger in _loggers)
            logger.WriteError(_message);
    }
}