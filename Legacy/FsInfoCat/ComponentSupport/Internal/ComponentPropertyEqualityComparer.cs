using FsInfoCat.Collections;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.ComponentSupport.Internal
{
    internal class ComponentPropertyEqualityComparer<T> : IGeneralizableEqualityComparer<T>
        where T : class
    {
        internal static readonly IGeneralizableEqualityComparer<T> Default;
        private readonly Tuple<PropertyDescriptor, IEqualityComparer>[] _comparers;

        static ComponentPropertyEqualityComparer()
        {
            Type type = typeof(T);
            if (!(typeof(IEquatable<T>).IsAssignableFrom(type) || typeof(IComparable<T>).IsAssignableFrom(type)))
            {
                PropertyDescriptor[] properties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(p => !p.DesignTimeOnly).ToArray();
                if (properties.Length > 0)
                {
                    Default = new ComponentPropertyEqualityComparer<T>(properties);
                    return;
                }
            }
            Default = CollectionExtensions.Defaults<T>.EqualityComparer;
        }

        private ComponentPropertyEqualityComparer(PropertyDescriptor[] properties)
        {
            _comparers = properties.Select(p => new Tuple<PropertyDescriptor, IEqualityComparer>(p,
                (IEqualityComparer)typeof(CollectionExtensions.Defaults<>).MakeGenericType(p.PropertyType)
                .GetField(nameof(CollectionExtensions.Defaults<T>.EqualityComparer)).GetValue(null))).ToArray();
        }

        public bool Equals(T x, T y)
        {
            if (x is null)
                return y is null;
            return !(y is null) && (ReferenceEquals(x, y) || _comparers.All(c =>
            {
                object a = c.Item1.GetValue(x);
                object b = c.Item1.GetValue(y);
                return c.Item2.Equals(a, b);
            }));
        }

        bool IEqualityComparer.Equals(object x, object y) => (x is null) ? y is null : (!(y is null) && ((x is T a) ? (y is T b && Equals(a, b)) :
            (!(y is T) && x.Equals(y))));

        public int GetHashCode(T obj) => (obj is null) ? 0 : _comparers.Select(t =>
        {
            object o = t.Item1.GetValue(obj);
            return (o is null) ? 0 : t.Item2.GetHashCode(o);
        }).GetAggregateHashCode();

        int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : (obj is T t) ? GetHashCode(t) : obj.GetHashCode();
    }
}
