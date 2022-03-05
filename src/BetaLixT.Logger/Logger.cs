using BetaLixT.Logger.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BetaLixT.Logger
{
    class Logger : ILogger
    {
        private readonly LogProcessor _logProcessor;
        private readonly LoggerScope _scopeProvider;
        private readonly string NodeName;
        

        public string Name { get; }

        public Logger(string name, LogProcessor logProcessor)
        {
            this.Name = name;
            this.NodeName = $"{System.Environment.MachineName} {System.AppDomain.CurrentDomain.FriendlyName}";
            this._scopeProvider = new LoggerScope();
            this._logProcessor = logProcessor;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this._scopeProvider?.Push(state) ?? NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }

            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, Name, eventId.Id, message, exception, this._scopeProvider.GetScopes());
            }
        }

        public virtual void WriteMessage(LogLevel logLevel, string logName, int eventId, string message, Exception exception, object[] scopes)
        {
            // Queue log message
            this._logProcessor.EnqueueMessage(new LogMessage{
                LogName = logName,
                NodeName = this.NodeName,
                EventId = eventId,
                LogLevel = (int)logLevel,
                LogLevelString = logLevel.ToString(),
                Exception = exception,
                Message = message,
                EventTime = DateTimeOffset.UtcNow,
                Scopes = scopes
            });
        }
    }
}
