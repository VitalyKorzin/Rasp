using System;
using System.Collections.Generic;

public static class Logging
{
    public static void Main()
    {
        PathFinder pathFinder_1 = new PathFinder(ChainOfLogging.Create(new FileLogger()));
        PathFinder pathFinder_2 = new PathFinder(ChainOfLogging.Create(new ConsoleLogger()));
        PathFinder pathFinder_3 = new PathFinder(ChainOfLogging.Create(new SecureLogger(new FileLogger())));
        PathFinder pathFinder_4 = new PathFinder(ChainOfLogging.Create(new SecureLogger(new ConsoleLogger())));
        PathFinder pathFinder_5 = new PathFinder(ChainOfLogging.Create(new ConsoleLogger(), new SecureLogger(new FileLogger())));
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

    public SecureLogger(ILogger logger)
    {
        if (logger == null)
            throw new ArgumentNullException();

        _logger = logger;
    }

    public void WriteError(string message)
    {
        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            _logger.WriteError(message);
    }
}

public class ChainOfLogging
{
    private readonly IEnumerable<ILogger> _loggers;

    public ChainOfLogging(IEnumerable<ILogger> loggers)
    {
        if (loggers == null)
            throw new ArgumentNullException();

        _loggers = loggers;
    }

    public void WriteError(string message)
    {
        foreach (var logger in _loggers)
            logger.WriteError(message);
    }

    public static ChainOfLogging Create(params ILogger[] loggers)
        => new ChainOfLogging(loggers);
}

public class PathFinder
{
    private readonly string _message = "...";
    private readonly ChainOfLogging _chain;

    public PathFinder(ChainOfLogging chain)
    {
        if (chain == null)
            throw new ArgumentNullException();

        _chain = chain;
    }

    public void Find() => _chain.WriteError(_message);
}