using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Internal
{
    class StringEqualityComparer : IEqualityComparer<string>, IEqualityComparer, IComparer<string>, IComparer
    {
        public static readonly StringEqualityComparer Instance = new StringEqualityComparer();

        public int Compare(string x, string y) => (x is null) ? ((y is null) ? 0 : 1) : ((y is null) ? 1 : x.CompareTo(y));

        public bool Equals(string x, string y) => x == y;

        public int GetHashCode(string obj) => (obj is null) ? 0 : obj.GetHashCode();

        int IComparer.Compare(object x, object y) => (x is null) ? ((y is null) ? 0 : 1) : ((y is string) ? -1 : x.ToString().CompareTo(y.ToString()));

        bool IEqualityComparer.Equals(object x, object y) => (x is null) ? y is null : ((x is string a) ? (y is string b) && a == b : !(y is string) && x.Equals(y));

        int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : obj.GetHashCode();
    }
}
