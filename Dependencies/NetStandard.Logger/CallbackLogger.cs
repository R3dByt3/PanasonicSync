using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetStandard.Logger
{
    internal class CallbackLogger : ILogger
    {
        private readonly LoggingLevel _loggingLevel;
        private readonly Action<string> _action;
        private static object _locker;

        public CallbackLogger(Action<string> action, LoggingLevel loggingLevel, object locker)
        {
            _action = action;
            _loggingLevel = loggingLevel;
            _locker = locker;
        }

        private string TryGetCallerClass(string stringIn)
        {
            try
            {
                stringIn = Path.GetFileNameWithoutExtension(stringIn);
            }
            catch
            {
                stringIn = string.Empty;
            }

            return stringIn;
        }

        public void Debug(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Debug)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {message}");
            }
        }

        public void Error(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Error)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Error(string additionalMessage, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Error)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {additionalMessage}");
            }
        }

        public void Error(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Error)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {additionalMessage} - {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Fatal(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Fatal)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Fatal(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Fatal)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {additionalMessage} - {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Info(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Info)
            {
                callerClass = TryGetCallerClass(callerClass);
                Call($"{GetCurrentDateTime()}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {message}");
            }
        }

        private void Call(string text)
        {
            lock (_locker)
            {
                _action(text);
            }
        }

        private static string GetCurrentDateTime()
        {
            return DateTime.Now.ToString(@"yyyy-MM-dd");
        }
    }
}