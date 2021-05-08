using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Desktop.Util
{
    public static class ComponentModelHelper
    {
        public static bool AllowMultiple(this IEnumerable<AttributeUsageAttribute> attributes) => attributes is null || attributes.All(a => a.AllowMultiple);

        public static bool AllowMultiple(this Attribute attribute) => attribute.GetType().GetCustomAttributes<AttributeUsageAttribute>(true).AllowMultiple();

        public static IEnumerable<T> MergeAttributes<T>(this IEnumerable<T> source, IEnumerable<T> other)
            where T : Attribute
        {
            if (other is null)
                return (source is null) ? Array.Empty<T>() : source.Where(a => !(a is null)).Distinct();
            if (source is null || ReferenceEquals(source, other))
                return other.Where(a => !(a is null)).Distinct();
            if (!(other = other.Where(a => !(a is null))).Any())
                return source.Where(a => !(a is null)).Distinct();
            if (!(source = source.Where(a => !(a is null))).Any())
                return other;
            source = source.Concat(other).Distinct();
            if (!source.SkipWhile(a => a.AllowMultiple()).Take(1).Any(a => !a.AllowMultiple()))
                return source;
            List<T> result = new List<T>();
            foreach (T a in source)
            {
                Type t = a.GetType();
                if (!t.GetCustomAttributes<AttributeUsageAttribute>(true).AllowMultiple())
                {
                    int index = result.FindIndex(m => t.IsInstanceOfType(m));
                    if (index > -1)
                        result.RemoveAt(index);
                }
                result.Add(a);
            }
            return result;
        }

        public static IEnumerable<PropertyDescriptor> GetModelProperties(this IEnumerable<PropertyDescriptor> source, bool includeReadOnly = false) =>
            (source is null) ? source : (includeReadOnly ? source.Where(pd => !(pd is null || pd.DesignTimeOnly)) :
                source.Where(pd => !(pd is null || pd.IsReadOnly || pd.DesignTimeOnly)));

        public static IEnumerable<PropertyDescriptor> GetModelProperties(this PropertyDescriptorCollection source, bool includeReadOnly = false) =>
            (source is null) ? null : source.OfType<PropertyDescriptor>().GetModelProperties(includeReadOnly);

        public static IEnumerable<PropertyDescriptor> GetModelProperties(this Type componentType, bool includeReadOnly = false) =>
            TypeDescriptor.GetProperties(componentType).GetModelProperties(includeReadOnly);

        public static IEnumerable<PropertyDescriptor> GetModelProperties<TComponent>(bool includeReadOnly = false) =>
            GetModelProperties(typeof(TComponent), includeReadOnly);

        public static IEnumerable<PropertyDescriptor> GetModelProperties(object component, bool includeReadOnly = false)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));
            return TypeDescriptor.GetProperties(component).GetModelProperties(includeReadOnly);
        }
    }
}
