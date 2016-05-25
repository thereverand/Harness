using Harness.Properties;
using Harness.Services;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Harness
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        protected string Id { get; } = Guid.NewGuid().ToString();

        protected IMessageService Messaging { get; } = X.Get<IMessageService>();

        protected bool LoggingEnabled { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Log(string subject, string message)
        {
            if (LoggingEnabled)
            {
                Messaging?.SendMessage("logger", subject, this, new { Id, Source = (GetType().Name), Message = message });
            }
        }

        [NotifyPropertyChangedInvocator]
        protected void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Log("PropertyChanged", propertyName);
        }

        protected void NotifyOfPropertyChange(string propertyName, Action handler)
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propertyName) handler();
            };
        }

        protected MemberInfo ExpressionToMember<T>(Expression<Func<T>> member)
        {
            var body = member.Body as MemberExpression;

            if (body != null) return body.Member;

            var uBody = member.Body as UnaryExpression;
            var mExp = uBody?.Operand as MemberExpression;
            return mExp?.Member;
        }

        protected void NotifyOfPropertyChange<T>(Expression<Func<T>> member)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            NotifyOfPropertyChange(ExpressionToMember(member)?.Name);
        }

        protected void NotifyOfPropertyChange<T>(Expression<Func<T>> member, Action handler)
        {
            NotifyOfPropertyChange(ExpressionToMember(member)?.Name, handler);
        }
    }
}