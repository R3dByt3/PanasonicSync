using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetStandard.Logger
{
    public class CompositeLogger : ILogger
    {
        private readonly IList<ILogger> _loggers;

        public CompositeLogger(IEnumerable<ILogger> loggers)
        {
            _loggers = new List<ILogger>(loggers);
        }

        public void Debug(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach(var logger in _loggers)
            {
                logger.Debug(message, callerClass, callerMethod, callerLine);
            }
        }

        public void Error(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(exception, callerClass, callerMethod, callerLine);
            }
        }

        public void Error(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(additionalMessage, exception, callerClass, callerMethod, callerLine);
            }
        }

        public void Error(string additionalMessage, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(additionalMessage, callerClass, callerMethod, callerLine);
            }
        }

        public void Fatal(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal(exception, callerClass, callerMethod, callerLine);
            }
        }

        public void Fatal(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach (var logger in _loggers)
            {
                logger.Fatal(additionalMessage, exception, callerClass, callerMethod, callerLine);
            }
        }

        public void Info(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            foreach (var logger in _loggers)
            {
                logger.Info(message, callerClass, callerMethod, callerLine);
            }
        }
    }
}
