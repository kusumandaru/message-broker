using System;
using System.Threading;
using NLog;

namespace MessageBroker.Log
{
    public class NLogging : ILogging
    {
        public void Debug(string message, string section = "")
        {
            LocalDataStoreSlot sessionIDSlot = Thread.GetNamedDataSlot("sessionID");

            var logger = LogManager.GetLogger(section);
            var logEvent = new LogEventInfo(LogLevel.Debug, section, message);

            logEvent.Properties["SessionID"] = Thread.GetData(sessionIDSlot);
            logger.Log(logEvent);
        }

        public void Info(string message, string section = "")
        {
            LocalDataStoreSlot sessionIDSlot = Thread.GetNamedDataSlot("sessionID");

            var logger = LogManager.GetLogger(section);
            var logEvent = new LogEventInfo(LogLevel.Info, section, message);

            logEvent.Properties["SessionID"] = Thread.GetData(sessionIDSlot);
            logger.Log(logEvent);
        }

        public void Error(Exception e)
        {
            LocalDataStoreSlot sessionIDSlot = Thread.GetNamedDataSlot("sessionID");

            var logger = LogManager.GetLogger("error");
            var logEvent = new LogEventInfo();

            logEvent.Level = LogLevel.Error;
            logEvent.LoggerName = "error";
            logEvent.Message = e.ToString();
            logEvent.Properties["SessionID"] = Thread.GetData(sessionIDSlot);
            logger.Log(logEvent);
        }
    }
}
