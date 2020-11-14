using System;
using System.Collections.Generic;
using System.IO;

namespace NetStandard.Logger
{
    public class LoggerFactory : IDisposable, ILoggerFactory
    {
        private readonly string _loggerFilePath;
        private readonly string _appName;
        private readonly LoggingLevel _loggingLevel;
        private readonly StreamWriter _loggerStream;
        private readonly FileStream _fileStream;
        private static readonly object _fileLocker = new object();
        private static readonly object _callbackLocker = new object();

        public LoggerFactory(string appName, LoggingLevel loggingLevel, bool running_on_unix = false)
        {
            _appName = appName;
            _loggingLevel = loggingLevel;

            if (running_on_unix)
            {
                _loggerFilePath = Environment.CurrentDirectory + @"\Logs\";
            }
            else
            {
                _loggerFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"\{_appName}\Logs\";
            }

            if (!Directory.Exists(_loggerFilePath)) Directory.CreateDirectory(_loggerFilePath);
            _loggerFilePath += DateTime.Now.ToString(@"dd_MM_yyyy_hh_mm_ss") + @".txt";

            if (!File.Exists(_loggerFilePath))
                _fileStream = new FileStream(_loggerFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite);
            else
                _fileStream = new FileStream(_loggerFilePath, FileMode.Append, FileAccess.Write,
                    FileShare.ReadWrite);

            _loggerStream = new StreamWriter(_fileStream);
        }

        public ILogger CreateFileLogger()
        {
            return new FileLogger(_appName, _loggerStream, _loggingLevel, _fileLocker);
        }
        public ILogger CreateCallbackLogger(Action<string> action)
        {
            return new CallbackLogger(action, _loggingLevel, _callbackLocker);
        }

        public ILogger CreateCompositeLogger(IEnumerable<ILogger> loggers)
        {
            return new CompositeLogger(loggers);
        }

        ~LoggerFactory()
        {
            Dispose(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _loggerStream.Close();
                    _fileStream.Close();
                    _loggerStream.Dispose();
                    _fileStream.Dispose();
                    try
                    {
                        if (new FileInfo(_loggerFilePath).Length == 0)
                            File.Delete(_loggerFilePath);
                    }
                    catch
                    {
                        //File is in use or deleted already
                    }
                }
                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~NetStandard.Logger() {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
