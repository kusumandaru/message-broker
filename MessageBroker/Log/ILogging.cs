using System;
namespace MessageBroker.Log
{
    public interface ILogging
    {
        void Debug(string message, string section = "");
        void Info(string message, string section = "");
        void Error(Exception e);
    }
}
