using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace ScaffoldSharp.Logging.Impl
{
    internal class DefaultConsoleLoggerImpl : Logger, ILoggerImplementationProvider
    {
        public DefaultConsoleLoggerImpl() { }

        public Logger CreateInstance(string name)
        {
            return new DefaultConsoleLoggerImpl();
        }

        private static object writingLock = new object();

        private LogLevel level = LogLevel.GLOBAL;
        public override LogLevel Level { get => (level == LogLevel.GLOBAL_CONSOLE ? (Logger.GlobalConsoleLogLevel == LogLevel.GLOBAL ? Logger.GlobalLogLevel : Logger.GlobalConsoleLogLevel) : (level == LogLevel.GLOBAL ? Logger.GlobalLogLevel : level)); set => level = value; }

        public override void Log(LogLevel level, string message)
        {
            if (Level != LogLevel.QUIET && Level >= level)
            {
                lock (writingLock)
                {
                    if (level <= LogLevel.WARN)
                    {
                        Console.Error.WriteLine(GlobalMessagePrefix + message);
                        if (Debugger.IsAttached)
                            System.Diagnostics.Debug.WriteLine(GlobalMessagePrefix + message);
                    }
                    else
                    {
                        Console.WriteLine(GlobalMessagePrefix + message);
                        if (Debugger.IsAttached)
                            System.Diagnostics.Debug.WriteLine(GlobalMessagePrefix + message);
                    }
                }
            }
        }

        public override void Log(LogLevel level, string message, Exception exception)
        {
            if (Level != LogLevel.QUIET && Level >= level)
            {
                lock (writingLock)
                {
                    if (level <= LogLevel.WARN)
                    {
                        Console.Error.WriteLine(GlobalMessagePrefix + message);
                        Console.Error.WriteLine("Exception: " + exception.GetType().FullName + (exception.Message != null ? ": " + exception.Message : ""));
                        Console.Error.WriteLine(exception.StackTrace);
                        Exception e = exception.InnerException;
                        while (e != null)
                        {
                            Console.Error.WriteLine("Caused by: " + e.GetType().FullName + (e.Message != null ? ": " + e.Message : ""));
                            Console.Error.WriteLine(e.StackTrace);
                            e = e.InnerException;
                        }
                        if (Debugger.IsAttached)
                        {
                            System.Diagnostics.Debug.WriteLine(GlobalMessagePrefix + message);
                            System.Diagnostics.Debug.WriteLine("Exception: " + exception.GetType().FullName + (exception.Message != null ? ": " + exception.Message : ""));
                            System.Diagnostics.Debug.WriteLine(exception.StackTrace);
                            Exception e2 = exception.InnerException;
                            while (e2 != null)
                            {
                                System.Diagnostics.Debug.WriteLine("Caused by: " + e2.GetType().FullName + (e2.Message != null ? ": " + e2.Message : ""));
                                System.Diagnostics.Debug.WriteLine(e2.StackTrace);
                                e2 = e2.InnerException;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(GlobalMessagePrefix + message);
                        Console.WriteLine("Exception: " + exception.GetType().FullName + (exception.Message != null ? ": " + exception.Message : ""));
                        Console.WriteLine(exception.StackTrace);
                        Exception e = exception.InnerException;
                        while (e != null)
                        {
                            Console.WriteLine("Caused by: " + e.GetType().FullName + (e.Message != null ? ": " + e.Message : ""));
                            Console.WriteLine(e.StackTrace);
                            e = e.InnerException;
                        }
                        if (Debugger.IsAttached)
                        {
                            System.Diagnostics.Debug.WriteLine(GlobalMessagePrefix + message);
                            System.Diagnostics.Debug.WriteLine("Exception: " + exception.GetType().FullName + (exception.Message != null ? ": " + exception.Message : ""));
                            System.Diagnostics.Debug.WriteLine(exception.StackTrace);
                            Exception e2 = exception.InnerException;
                            while (e2 != null)
                            {
                                System.Diagnostics.Debug.WriteLine("Caused by: " + e2.GetType().FullName + (e2.Message != null ? ": " + e2.Message : ""));
                                System.Diagnostics.Debug.WriteLine(e2.StackTrace);
                                e2 = e2.InnerException;
                            }
                        }
                    }
                }
            }
        }
    }
}