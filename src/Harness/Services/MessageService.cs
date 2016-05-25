using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Harness.Services
{
    public class MessageService : IMessageService
    {
        private readonly string _anyAddress = "*";
        private readonly string _anySubject = "*";
        private readonly IDictionary<string, IDictionary<string, IList<Action<string, object, Func<string, object>>>>> _handlers;

        public MessageService()
        {
            _handlers = new Dictionary<string, IDictionary<string, IList<Action<string, object, Func<string, object>>>>>();
        }

        public void ReceiveMessage(string address, string subject, Action<string, object, Func<string, object>> handler)
        {
            if (string.IsNullOrEmpty(address)) address = _anyAddress;
            if (string.IsNullOrEmpty(subject)) subject = _anySubject;
            if (!_handlers.ContainsKey(address)) _handlers.Add(address, new Dictionary<string, IList<Action<string, object, Func<string, object>>>>());
            if (!_handlers[address].ContainsKey(subject)) _handlers[address].Add(subject, new List<Action<string, object, Func<string, object>>>());
            _handlers[address][subject].Add(handler);
        }

        public void SendMessage(string address, string subject, object sender, object args)
        {
            if (string.IsNullOrEmpty(subject)) subject = _anySubject;
            if (!_handlers.ContainsKey(address)) return;

            var mHandlers = new List<Action<string, object, Func<string, object>>>();

            if (_handlers[address].ContainsKey(subject))
                mHandlers.AddRange(_handlers[address][subject]);

            if (_handlers[address].ContainsKey(_anySubject))
                mHandlers.AddRange(_handlers[address][_anySubject]);

            foreach (var handler in mHandlers)
            {
                Task.Run(() => handler(subject, sender, key => key == "*" ? args : args?.GetType().GetRuntimeProperty(key)?.GetValue(args)));
            }
        }
    }
}