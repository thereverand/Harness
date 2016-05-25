using System;
using System.Diagnostics;
using System.Globalization;

namespace Harness.Services
{
    public interface ILogWriter
    {
        void Log(string subject, string id, string source, string message);
    }

    public class DebugLogWriter : ILogWriter
    {
        public DebugLogWriter(IMessageService messaging)
        {
            messaging.ReceiveMessage("logger", null, LogHandler);
        }

        public void Log(string subject, string id, string source, string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {subject} > {id} : {source} : {message}");
        }

        private void LogHandler(string subject, object origin, Func<string, object> args)
        {
            Log(subject, $"{args("Id")}", $"{args("Source")}", $"{args("Message")}");
        }
    }
}