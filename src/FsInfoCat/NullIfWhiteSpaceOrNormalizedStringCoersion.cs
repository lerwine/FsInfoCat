using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat
{
    public class NullIfWhiteSpaceOrNormalizedStringCoersion : ICoersion<string>
    {
        public static readonly NullIfWhiteSpaceOrNormalizedStringCoersion Default = new();

        IEqualityComparer<string> _backingComparer;

        Type ICoersion.ValueType => typeof(string);

        public NullIfWhiteSpaceOrNormalizedStringCoersion(IEqualityComparer<string> comparer)
        {
            _backingComparer = comparer ?? StringComparer.InvariantCulture;
        }

        private NullIfWhiteSpaceOrNormalizedStringCoersion() : this(null) { }

        public virtual string Cast(object obj) => StringExtensionMethods.NullIfWhiteSpaceOrNormalized((string)obj);

        public virtual string Coerce(object obj) => (obj is null) ? null : StringExtensionMethods.NullIfWhiteSpaceOrNormalized((obj is string text) ? text : obj.ToString());

        public virtual string Normalize(string obj) => obj.NullIfWhiteSpaceOrNormalized();

        object ICoersion.Normalize(object obj) => Normalize((string)obj);

        bool IEqualityComparer.Equals(object x, object y) => TryCast(x, out string a) && TryCast(y, out string b) ? Equals(a, b) :
            Equals(x, y);

        public bool Equals(string x, string y) => (x is null || (x = x.NullIfWhiteSpaceOrNormalized()) is null) ?
            (y is null || y.NullIfWhiteSpaceOrNormalized() is null) : (y is not null && (y = y.NullIfWhiteSpaceOrNormalized()) is not null &&
            _backingComparer.Equals(x, y));

        public int GetHashCode(string obj) => _backingComparer.GetHashCode(obj.NullIfWhiteSpaceOrNormalized() ?? "");

        int IEqualityComparer.GetHashCode(object obj) => TryCast(obj, out string text) ? GetHashCode(text) : ((obj is null) ? 0 : obj.GetHashCode());

        public virtual bool TryCast(object obj, out string result)
        {
            if (obj is null)
                result = null;
            else if (obj is string text)
                result = text.NullIfWhiteSpaceOrNormalized();
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
                result = text.NullIfWhiteSpaceOrNormalized();
            else
            {
                try { result = obj.ToString().NullIfWhiteSpaceOrNormalized(); }
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
}
