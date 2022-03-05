using BetaLixT.Logger.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BetaLixT.Logger
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>();
        private readonly LogProcessor _logProcessor;

        public LoggerProvider(
            ILogRepository logRepository)
        {
            this._logProcessor = new LogProcessor(logRepository);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this._loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            this._logProcessor.Dispose();
        }

        private Logger CreateLoggerImplementation(string name)
        {
            return new Logger(name, this._logProcessor);
        }

    }
}
