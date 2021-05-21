using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public class NonNullStringCoersion : ICoersion<string>
    {
        IEqualityComparer<string> _backingComparer;

        Type ICoersion.ValueType => typeof(string);

        public NonNullStringCoersion(IEqualityComparer<string> comparer)
        {
            _backingComparer = comparer ?? StringComparer.InvariantCulture;
        }

        public NonNullStringCoersion() : this(null) { }

        public string Cast(object obj) => (string)obj ?? "";

        public string Convert(object obj) => (obj is null) ? "" : ((obj is string text) ? text : obj.ToString());

        bool System.Collections.IEqualityComparer.Equals(object x, object y) => TryCast(x, out string a) && TryCast(y, out string b) ? Equals(a, b) :
            Equals(x, y);

        public bool Equals(string x, string y) => string.IsNullOrEmpty(x) ? string.IsNullOrEmpty(y) : (!(y is null) && _backingComparer.Equals(x, y));

        public int GetHashCode(string obj) => _backingComparer.GetHashCode(obj ?? "");

        int System.Collections.IEqualityComparer.GetHashCode(object obj) => TryCast(obj, out string text) ? GetHashCode(text) : ((obj is null) ? 0 : obj.GetHashCode());

        public bool TryCast(object obj, out string result)
        {
            if (obj is null)
                result = "";
            else if (obj is string text)
                result = text;
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        bool ICoersion.TryCast(object obj, out object result)
        {
            if (TryCast(obj, out string text))
            {
                result = text;
                return true;
            }
            result = null;
            return false;
        }

        public bool TryCoerce(object obj, out string result)
        {
            if (obj is null)
                result = "";
            else if (obj is string text)
                result = text;
            else
            {
                try { result = obj.ToString(); }
                catch
                {
                    result = null;
                    return false;
                }
                return !(result is null);
            }
            return true;
        }

        bool ICoersion.TryCoerce(object obj, out object result)
        {
            if (TryCoerce(obj, out string text))
            {
                result = text;
                return true;
            }
            result = null;
            return false;
        }

        public bool TryConvert(object obj, out string result) => TryCoerce(obj, out result);

        bool ICoersion.TryConvert(object obj, out object result)
        {
            if (TryConvert(obj, out string text))
            {
                result = text;
                return true;
            }
            result = null;
            return false;
        }

        object ICoersion.Cast(object obj) => Cast(obj);

        object ICoersion.Convert(object obj) => Convert(obj);
    }
}
