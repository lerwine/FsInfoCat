using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Util
{
    public static class CompatibilityHelper
    {
        public static bool EndsWith(this string s, char c) => !string.IsNullOrEmpty(s) && s[s.Length - 1] == c;

        public static string[] Split(this string s, char c, int count) => (s is null) ? new string[0] : s.Split(new char[] { c }, count);

        public static bool TryDequeue<T>(this Queue<T> source, out T result)
        {
            if (source is null || source.Count == 0)
            {
                result = default(T);
                return false;
            }
            try
            {
                result = source.Dequeue();
            }
            catch (InvalidOperationException)
            {
                result = default(T);
                return false;
            }
            return true;
        }
    }
}
