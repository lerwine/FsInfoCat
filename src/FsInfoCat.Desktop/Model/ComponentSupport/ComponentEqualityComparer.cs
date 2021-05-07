using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public class ComponentEqualityComparer<TComponent> : IEqualityComparer<TComponent>
        where TComponent : class
    {
        public static readonly IEqualityComparer<TComponent> Default;
        public static readonly ComponentEqualityComparer<TComponent> Instance;
        private readonly PropertyComparer[] _propertyComparers;

        static ComponentEqualityComparer()
        {
            Type t = typeof(TComponent);
            PropertyComparer[] propertyComparers = TypeDescriptor.GetProperties(t).Cast<PropertyDescriptor>().Where(pd => !pd.DesignTimeOnly)
                .Select(pd => PropertyComparer.Create(pd)).ToArray();
            Instance = new ComponentEqualityComparer<TComponent>(propertyComparers);
            if (typeof(IEquatable<TComponent>).IsAssignableFrom(t) || propertyComparers.Length == 0)
                Default = EqualityComparer<TComponent>.Default;
            else
                Default = Instance;
        }

        private ComponentEqualityComparer(PropertyComparer[] propertyComparers)
        {
            _propertyComparers = propertyComparers;
        }

        public bool Equals(TComponent x, TComponent y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            if (_propertyComparers.Length == 0)
                return false;
            foreach (PropertyComparer pc in _propertyComparers)
            {
                if (!pc.AreEqual(x, y))
                    return false;
            }
            return true;
        }

        public int GetHashCode(TComponent obj) =>
            _propertyComparers.Select(pc => pc.GetPropertyHashCode(obj)).Where(h => h.HasValue).Select(h => h.Value).ToAggregateHashCode();

        private abstract class PropertyComparer
        {
            private PropertyDescriptor _propertyDescriptor;

            protected PropertyComparer(PropertyDescriptor propertyDescriptor)
            {
                _propertyDescriptor = propertyDescriptor;
            }

            internal int? GetPropertyHashCode(TComponent obj)
            {
                object value;
                try { value = _propertyDescriptor.GetValue(obj); }
                catch { return null; }
                return GetHashCode(value);
            }

            protected abstract int? GetHashCode(object obj);

            internal bool AreEqual(TComponent x, TComponent y)
            {
                object a;
                try { a = _propertyDescriptor.GetValue(x); }
                catch (Exception exc)
                {
                    try { _propertyDescriptor.GetValue(y); }
                    catch (Exception e) { return exc.GetType().Equals(e.GetType()); }
                    return false;
                }
                object b;
                try { b = _propertyDescriptor.GetValue(y); }
                catch { return false; }
                return AreValuesEqual(a, b);
            }

            protected abstract bool AreValuesEqual(object x, object y);

            internal static PropertyComparer Create(PropertyDescriptor pd)
            {
                Type pt = pd.PropertyType;
                if (pt.IsValueType)
                {
                    if (pt.IsGenericType && pt.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                        return (PropertyComparer)Activator.CreateInstance(typeof(NullablePropertyComparer<>).MakeGenericType(Nullable.GetUnderlyingType(pt)),
                            new object[] { pd });
                    return (PropertyComparer)Activator.CreateInstance(typeof(ValuePropertyComparer<>).MakeGenericType(pt), new object[] { pd });
                }
                return (PropertyComparer)Activator.CreateInstance(typeof(ObjectPropertyComparer<>).MakeGenericType(pt), new object[] { pd });
            }
        }

        private class ObjectPropertyComparer<T> : PropertyComparer
        {
            private readonly EqualityComparer<T> _backingComparer;

            internal ObjectPropertyComparer(PropertyDescriptor propertyDescriptor) : base(propertyDescriptor)
            {
                _backingComparer = EqualityComparer<T>.Default;
            }

            protected override bool AreValuesEqual(object x, object y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                if (ReferenceEquals(x, y))
                    return true;
                if (x is T a)
                {
                    if (y is T b)
                        return _backingComparer.Equals(a, b);
                    return false;
                }
                return !(y is T) && x.Equals(y);
            }

            protected override int? GetHashCode(object obj)
            {
                if (obj is null)
                    return 0;
                if (obj is T t)
                    return _backingComparer.GetHashCode(t);
                return null;
            }
        }

        private class ValuePropertyComparer<T> : PropertyComparer
            where T : struct
        {
            private readonly EqualityComparer<T> _backingComparer;

            internal ValuePropertyComparer(PropertyDescriptor propertyDescriptor) : base(propertyDescriptor)
            {
                _backingComparer = EqualityComparer<T>.Default;
            }

            protected override bool AreValuesEqual(object x, object y)
            {
                if (x is T a)
                {
                    if (y is T b)
                        return _backingComparer.Equals(a, b);
                    return false;
                }
                return !(y is T) && x.Equals(y);
            }

            protected override int? GetHashCode(object obj)
            {
                if (obj is T t)
                    return _backingComparer.GetHashCode(t);
                return null;
            }
        }

        private class NullablePropertyComparer<T> : PropertyComparer
            where T : struct
        {
            private readonly EqualityComparer<T> _backingComparer;

            internal NullablePropertyComparer(PropertyDescriptor propertyDescriptor) : base(propertyDescriptor)
            {
                _backingComparer = EqualityComparer<T>.Default;
            }

            protected override bool AreValuesEqual(object x, object y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                if (x is T a)
                {
                    if (y is T b)
                        return _backingComparer.Equals(a, b);
                    return false;
                }
                return !(y is T) && x.Equals(y);
            }

            protected override int? GetHashCode(object obj)
            {
                if (obj is null)
                    return 0;
                if (obj is T t)
                    return _backingComparer.GetHashCode(t);
                return null;
            }
        }
    }
}
