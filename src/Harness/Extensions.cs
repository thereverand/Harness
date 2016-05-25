using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Harness
{
    public static class Extensions
    {
        public static bool MatchesKey<T>(this IDictionary<string, T> dictionary, string input)
        {
            return dictionary.Keys.Any(input.IsMatch);
        }

        public static IEnumerable<TY> WithKeyMatch<TY>(this IDictionary<string, TY> dictionary, string input)
        {
            return dictionary.Where(i => input.IsMatch(i.Key)).Select(i => i.Value);
        }

        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
    }
}