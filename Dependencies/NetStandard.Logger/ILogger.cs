using System;
using System.Runtime.CompilerServices;

namespace NetStandard.Logger
{
    public interface ILogger
    {
        void Debug(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
        void Error(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
        void Error(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
        void Error(string additionalMessage, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
        void Fatal(Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
        void Fatal(string additionalMessage, Exception exception, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
        void Info(string message, [CallerFilePath] string callerClass = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = -1);
    }
}