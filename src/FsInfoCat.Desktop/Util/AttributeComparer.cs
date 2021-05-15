using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Desktop.Util
{
    public class AttributeComparer : AttributeComparer<Attribute>
    {
    }

    public class AttributeComparer<T> : IEqualityComparer<T>
        where T : Attribute
    {
        public static readonly AttributeComparer<T> Instance = new AttributeComparer<T>();

        protected AttributeComparer() { }

        public static bool? AllowMultiple(Type type, bool inherit)
        {
            AttributeUsageAttribute usageAttribute = type.GetCustomAttribute<AttributeUsageAttribute>(inherit);
            if (usageAttribute is null)
                return null;
            return usageAttribute.AllowMultiple;
        }

        public static Type GetBaseComparisonType(Type type)
        {
            if (AllowMultiple(type, true) ?? false)
            {
                Type dt = typeof(object);
                return type.GetProperties().Select(p => p.DeclaringType).OrderByDescending(t =>
                {
                    int result = -1;
                    while (!(t.BaseType is null || t.BaseType.Equals(dt)))
                    {
                        result++;
                        t = t.BaseType;
                    }
                    return result;
                }).DefaultIfEmpty(type).First();
            }
            bool? allowMultiple = AllowMultiple(type, false);
            Type at = typeof(T);
            Type baseType = type.BaseType;
            if (baseType is null || baseType.Equals(at) || !at.IsAssignableFrom(baseType))
                return type;
            while (!allowMultiple.HasValue)
            {
                if (baseType.BaseType.Equals(at))
                    return baseType;
                type = baseType;
                baseType = baseType.BaseType;
                allowMultiple = AllowMultiple(baseType, false);
            }
            if (allowMultiple.Value)
                return type;
            do
            {
                type = baseType;
                if ((baseType = baseType.BaseType).Equals(at))
                    return type;
                allowMultiple = AllowMultiple(baseType, true);
            } while (allowMultiple.HasValue && !allowMultiple.Value);
            return type;
        }

        public static bool AllowMultiple(T attribute) => AllowMultiple(attribute.GetType(), true) ?? false;

        public bool Equals(T x, T y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            Type a = x.GetType();
            Type b = y.GetType();

            if (AllowMultiple(x))
            {
                if (!AllowMultiple(y))
                    return false;
                if (!a.Equals(b))
                {
                    a = GetBaseComparisonType(a);
                    b = GetBaseComparisonType(b);
                    if (!(a.IsAssignableFrom(b) || b.IsAssignableFrom(a)))
                        return false;
                }
            }
            else
            {
                if (AllowMultiple(y))
                    return false;
                a = GetBaseComparisonType(a);
                b = GetBaseComparisonType(b);
                return a.IsAssignableFrom(b) || b.IsAssignableFrom(a);
            }
            IEnumerable<PropertyDescriptor> p1 = TypeDescriptor.GetProperties(x).Cast<PropertyDescriptor>();
            IEnumerable<PropertyDescriptor> p2 = TypeDescriptor.GetProperties(y).Cast<PropertyDescriptor>();
            if (p1.Count() != p2.Count())
                return false;
            if (!p1.Any())
                return true;
            return (p1 = p1.OrderBy(p => p.Name)).Select(p => p.Name)
                .SequenceEqual((p2 = p2.OrderBy(p => p.Name)).Select(p => p.Name)) &&
                p1.Select(p => p.PropertyType).SequenceEqual(p2.Select(p => p.PropertyType)) &&
                p1.Select(p => p.IsReadOnly).SequenceEqual(p2.Select(p => p.IsReadOnly)) &&
                p1.Select(p => p.GetValue(x)).SequenceEqual(p2.Select(p => p.GetValue(y)));
        }

        public int GetHashCode(T obj)
        {
            if (obj is null)
                return 0;
            if (AllowMultiple(obj))
            {
                IEnumerable<PropertyDescriptor> p1 = TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>().OrderBy(p => p.Name);
                if (p1.Any())
                    return p1.Select(p => p.GetValue(obj)).GetAggregateHashCode();
            }

            return GetBaseComparisonType(obj.GetType()).FullName.GetHashCode();
        }
    }
}
