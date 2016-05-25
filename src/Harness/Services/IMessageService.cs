using System;

namespace Harness.Services
{
    public interface IMessageService
    {
        void ReceiveMessage(string address, string subject, Action<string, object, Func<string, object>> handler);

        void SendMessage(string address, string subject, object sender, object args);
    }
}