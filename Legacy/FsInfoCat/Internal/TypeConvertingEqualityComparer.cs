using FsInfoCat.Collections;
using System.Collections;
using System.ComponentModel;

namespace FsInfoCat.Internal
{
    internal class TypeConvertingEqualityComparer<T> : EqualityComparerBase<T>, IGeneralizableOrderAndEqualityComparer<T>
    {
        private static readonly Coersion<T> _coersion = Coersion<T>.Default;
        public static readonly IGeneralizableOrderAndEqualityComparer<T> Default = new TypeConvertingEqualityComparer<T>();
        private readonly TypeConverter _converter;

        public TypeConvertingEqualityComparer() : this(null) { }

        public TypeConvertingEqualityComparer(TypeConverter converter)
        {
            _converter = converter ?? TypeDescriptor.GetConverter(typeof(T));
        }

        public int Compare(T x, T y) => _converter.ConvertToInvariantString(x).CompareTo(_converter.ConvertToInvariantString(y));

        int IComparer.Compare(object x, object y) => _coersion.TryCoerce(x, out T a) ?
            (_coersion.TryCoerce(y, out T b) ? Compare(a, b) : -1) : (_coersion.TryCoerce(y, out _) ? 1 :
            ((x is null) ? ((y is null) ? 0 : -1) : (ReferenceEquals(x, y) ? 0 : Comparer.Default.Compare(x, y))));

        public override bool Equals(T x, T y) => _converter.ConvertToInvariantString(x).Equals(_converter.ConvertToInvariantString(y));

        public override int GetHashCode(T obj) => _converter.ConvertToInvariantString(obj).GetHashCode();
    }
}
