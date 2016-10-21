using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Harness
{
    public static class Extensions
    {
        public static MemberInfo ToMember<T>(this Expression<Func<T>> member)
        {
            var body = member.Body as MemberExpression;

            if (body != null) return body.Member;

            var uBody = member.Body as UnaryExpression;
            var mExp = uBody?.Operand as MemberExpression;
            return mExp?.Member;
        }

        public static bool Is<T>(this object obj)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo());
        }
    }
}