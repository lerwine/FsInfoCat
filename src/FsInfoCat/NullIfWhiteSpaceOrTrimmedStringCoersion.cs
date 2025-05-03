using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat
{
    // TODO: Document NullIfWhiteSpaceOrTrimmedStringCoersion class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NullIfWhiteSpaceOrTrimmedStringCoersion(IEqualityComparer<string> comparer) : ICoersion<string>
    {
        public static readonly NullIfWhiteSpaceOrTrimmedStringCoersion Default = new();
        readonly IEqualityComparer<string> _backingComparer = comparer ?? StringComparer.InvariantCulture;

        Type ICoersion.ValueType => typeof(string);

        private NullIfWhiteSpaceOrTrimmedStringCoersion() : this(null) { }

        public virtual string Cast(object obj) => StringExtensionMethods.TrimmedOrNullIfWhiteSpace((string)obj);

        public virtual string Coerce(object obj) => (obj is null) ? "" : StringExtensionMethods.TrimmedOrNullIfWhiteSpace((obj is string text) ? text : obj.ToString());

        public virtual string Normalize(string obj) => StringExtensionMethods.TrimmedOrNullIfWhiteSpace(obj);

        object ICoersion.Normalize(object obj) => Normalize((string)obj);

        bool IEqualityComparer.Equals(object x, object y) => TryCast(x, out string a) && TryCast(y, out string b) ? Equals(a, b) :
            Equals(x, y);

        public bool Equals(string x, string y) => string.IsNullOrWhiteSpace(x) ? string.IsNullOrWhiteSpace(y) : (!(y is null) && _backingComparer.Equals(x, y));

        public int GetHashCode(string obj) => _backingComparer.GetHashCode(obj ?? "");

        int IEqualityComparer.GetHashCode(object obj) => TryCast(obj, out string text) ? GetHashCode(text) : ((obj is null) ? 0 : obj.GetHashCode());

        public virtual bool TryCast(object obj, out string result)
        {
            if (obj is null)
                result = null;
            else if (obj is string text)
                result = StringExtensionMethods.TrimmedOrNullIfWhiteSpace(text);
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        bool ICoersion.TryCast(object obj, out object result)
        {
            bool r = TryCast(obj, out string text);
            result = text;
            return r;
        }

        public virtual bool TryCoerce(object obj, out string result)
        {
            if (obj is null)
                result = null;
            else if (obj is string text)
                result = StringExtensionMethods.TrimmedOrNullIfWhiteSpace(text);
            else
            {
                try { result = StringExtensionMethods.TrimmedOrNullIfWhiteSpace(obj.ToString()); }
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
            bool r = TryCoerce(obj, out string text);
            result = text;
            return r;
        }

        object ICoersion.Cast(object obj) => Cast(obj);

        object ICoersion.Coerce(object obj) => Coerce(obj);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
