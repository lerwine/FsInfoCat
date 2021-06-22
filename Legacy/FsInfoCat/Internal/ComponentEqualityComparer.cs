using FsInfoCat.Collections;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Internal
{
    internal class ComponentEqualityComparer<T> : EqualityComparerBase<T>, IGeneralizableOrderAndEqualityComparer<T> where T : class
    {
        private static readonly Coersion<T> _coersion = Coersion<T>.Default;
        private static IGeneralizableOrderAndEqualityComparer<T>[] _propertyComparers;
        private static readonly TypeConverter _converter;
        public static readonly IGeneralizableOrderAndEqualityComparer<T> Default = new ComponentEqualityComparer<T>();

        static ComponentEqualityComparer()
        {
            Type type = typeof(T);
            Type g = typeof(PropertyComparer<,>);
            PropertyDescriptor dp = TypeDescriptor.GetDefaultProperty(type);
            _propertyComparers = ((dp is null) ? TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>() : new PropertyDescriptor[] { dp }
                .Concat(TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(p => !p.Name.Equals(dp.Name))))
                .Where(p => !p.DesignTimeOnly)
                .Select(p => (IGeneralizableOrderAndEqualityComparer<T>)Activator.CreateInstance(g.MakeGenericType(type, p.PropertyType), new object[] { p }))
                .ToArray();
            _converter = TypeDescriptor.GetConverter(type);
        }

        public ComponentEqualityComparer() { }

        public override int GetHashCode(T obj)
        {
            if (_propertyComparers.Length == 0)
                return _converter.ConvertToInvariantString(obj).GetHashCode();
            return _propertyComparers.Select(c => c.GetHashCode(obj)).ToAggregateHashCode();
        }

        public override bool Equals(T x, T y)
        {
            if (_propertyComparers.Length == 0)
                return _converter.ConvertToInvariantString(x).Equals(_converter.ConvertToInvariantString(y));
            return (x is null) ? y is null : (!(y is null) && (ReferenceEquals(x, y) || _propertyComparers.All(c => c.Equals(x, y))));
        }

        public int Compare(T x, T y)
        {
            if (_propertyComparers.Length == 0)
                return _converter.ConvertToInvariantString(x).CompareTo(_converter.ConvertToInvariantString(y));
            return _propertyComparers.Select(p => p.Compare(x, y)).Where(i => i != 0).DefaultIfEmpty(0).First();
        }

        int IComparer.Compare(object x, object y) => _coersion.TryCoerce(x, out T a) ?
            (_coersion.TryCoerce(y, out T b) ? Compare(a, b) : -1) : (_coersion.TryCoerce(y, out _) ? 1 :
            ((x is null) ? ((y is null) ? 0 : -1) : (ReferenceEquals(x, y) ? 0 : Comparer.Default.Compare(x, y))));
    }
}
