using FsInfoCat.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Internal
{
    class ComparisonService : IComparisonService
    {
        public IEqualityComparer<T> GetEqualityComparer<T>() => (IEqualityComparer<T>)GetEqualityComparer(typeof(T));

        public IEqualityComparer GetEqualityComparer(Type type)
        {
            if (type.IsValueType)
                return (IEqualityComparer)typeof(EqualityComparer<>).MakeGenericType(type).GetField(nameof(EqualityComparer<object>.Default)).GetValue(null);
            if (type.Equals(typeof(string)))
                return StringEqualityComparer.Instance;
            if (!typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                PropertyDescriptor[] properties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(p => !p.DesignTimeOnly).ToArray();
                if (properties.Length > 0)
                    return (IEqualityComparer)typeof(EqualityComparer<>).MakeGenericType(type).GetField(nameof(EqualityComparer<object>.Default)).GetValue(null);
            }
            return (IEqualityComparer)typeof(ReferenceEqualityComparer<>).MakeGenericType(type).GetField(nameof(ReferenceEqualityComparer<object>.Default)).GetValue(null);
        }

        public IComparer<T> GetComparer<T>() => (IComparer<T>)GetComparer(typeof(T));

        public IComparer GetComparer(Type type)
        {
            if (type.IsValueType)
                return (IComparer)typeof(Comparer<>).MakeGenericType(type).GetField(nameof(Comparer<object>.Default)).GetValue(null);
            if (type.Equals(typeof(string)))
                return StringEqualityComparer.Instance;
            if (!typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                PropertyDescriptor[] properties = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>().Where(p => !p.DesignTimeOnly).ToArray();
                if (properties.Length > 0)
                    return (IComparer)typeof(Comparer<>).MakeGenericType(type).GetField(nameof(Comparer<object>.Default)).GetValue(null);
            }
            return (IComparer)typeof(ReferenceEqualityComparer<>).MakeGenericType(type).GetField(nameof(ReferenceEqualityComparer<object>.Default)).GetValue(null);
        }
    }
}
