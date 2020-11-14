using System;
using System.Collections.Generic;

namespace NetStandard.Logger
{
    public interface ILoggerFactory
    {
        ILogger CreateFileLogger();
        ILogger CreateCallbackLogger(Action<string> action);
        ILogger CreateCompositeLogger(IEnumerable<ILogger> loggers);
        void Dispose();
    }
}