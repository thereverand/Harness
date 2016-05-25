using System;
using System.Collections.Generic;

namespace Harness.Services
{
    public interface ISessionState : IDictionary<string, object>
    {
        string SessionName { get; }
    }

    public interface ISessionManager
    {
        ISessionState GetSession(string sessionName);

        bool SaveSession(ISessionState session);
    }
}