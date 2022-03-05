using System;
using System.Collections.Generic;
using System.Text;

namespace BetaLixT.Logger.Entities
{
    public class LogMessage
    {
        public string LogName { get; set; }
        public int EventId { get; set; }
        public int LogLevel { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public DateTimeOffset EventTime { get; set; }
        public string LogLevelString { get; set; }
        public string NodeName { get; set; }
        public object[] Scopes { get; set; }
    }
}
