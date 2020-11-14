using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetStandard.Logger
{
    internal class FileLogger : ILogger
    {
        private readonly string _appName;
        private readonly StreamWriter _loggerStream;
        private readonly LoggingLevel _loggingLevel;
        private static object _locker;
        private readonly string[] _stringSeparators = new string[] { "\r\n" };

        internal FileLogger(string appName, StreamWriter loggerStream, LoggingLevel loggingLevel, object locker)
        {
            _appName = appName;
            _loggingLevel = loggingLevel;
            _loggerStream = loggerStream;
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
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {message}");
            }
        }

        public void Error(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Error)
            {
                callerClass = TryGetCallerClass(callerClass);
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Error(string additionalMessage, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Error)
            {
                callerClass = TryGetCallerClass(callerClass);
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {additionalMessage}");
            }
        }

        public void Error(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Error)
            {
                callerClass = TryGetCallerClass(callerClass);
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {additionalMessage} - {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Fatal(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Fatal)
            {
                callerClass = TryGetCallerClass(callerClass);
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Fatal(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Fatal)
            {
                callerClass = TryGetCallerClass(callerClass);
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {additionalMessage} - {exception.Message}\r\n{exception.StackTrace}");
            }
        }

        public void Info(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1)
        {
            if (_loggingLevel <= LoggingLevel.Info)
            {
                callerClass = TryGetCallerClass(callerClass);
                WriteAndFlush($"{GetCurrentDateTime()}:{_appName}:[{MethodBase.GetCurrentMethod().Name}]:[{callerClass}]:[{callerMethod}]:[{callerLine}]:: {message}");
            }
        }

        private void WriteAndFlush(string text)
        {
            lock (_locker)
            {
                foreach (var line in text.Split(_stringSeparators, StringSplitOptions.None))
                {
                    _loggerStream.WriteLine(line);
                    _loggerStream.Flush();
                }
            }
        }

        private static string GetCurrentDateTime()
        {
            return DateTime.Now.ToString(@"yyyy-MM-dd");
        }
    }
}
