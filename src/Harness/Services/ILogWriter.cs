using System;
using System.Diagnostics;
using System.Globalization;

namespace Harness.Services
{
    public interface ILogWriter
    {
        void Log(string source, string message, Exception ex);
    }

    public class DebugLogWriter : ILogWriter
    {
        public void Log(string source, string message, Exception ex = null)
        {
            Debug.WriteLine($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {source} > {message} ! {ex?.Message}");
        }
    }
}