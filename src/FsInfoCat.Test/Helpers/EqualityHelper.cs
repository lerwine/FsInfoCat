using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat.Test.Helpers
{
    public class EqualityHelper<T>
    {
        public static IEqualityComparer<T> Default { get; }
        public static IEqualityComparer<T> DefaultNoEnumerate { get; }
        private static GenericComparer _default = new GenericComparer();

        private static IEqualityComparer<T> GetDefaultReferenceComparer(Type t) => (IEqualityComparer<T>)typeof(ReferenceEqualityHelper<>).MakeGenericType(t).GetField("Default").GetValue(null);

        static EqualityHelper()
        {
            Type t = typeof(T);
            if (t.IsValueType)
            {
                if (t.IsGenericType && typeof(Nullable<>).Equals(t.GetGenericTypeDefinition()))
                    Default = (IEqualityComparer<T>)typeof(StructEqualityHelper<>).MakeGenericType(t).GetField("Default").GetValue(null);
                else
                    Default = EqualityComparer<T>.Default;
            }
            else if (t.IsClass)
            {
                if (t.Equals(typeof(object)))
                    Default = _default;
                else if (t.Equals(typeof(string)))
                    Default = EqualityComparer<T>.Default;
                else
                {
                    if (t.IsArray)
                    {
                        Default = (IEqualityComparer<T>)typeof(ArrayEqualityHelper<>).MakeGenericType(t).GetField("Default").GetValue(null);
                        DefaultNoEnumerate = GetDefaultReferenceComparer(t);
                        return;
                    }

                    Type[] interfaces = t.GetInterfaces();
                    if (interfaces.Length > 0)
                    {
                        Type g = typeof(IDictionary<,>);
                        Type a = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(g));
                        if (!(a is null))
                        {
                            Type[] kv = a.GetGenericArguments();
                            Default = (IEqualityComparer<T>)typeof(DictionaryEqualityHelper<,,>).MakeGenericType(t, kv[0], kv[1]).GetField("Default").GetValue(null);
                            DefaultNoEnumerate = GetDefaultReferenceComparer(t);
                            return;
                        }
                        g = typeof(ICollection<>);
                        a = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(g));
                        if (!(a is null))
                        {
                            Default = (IEqualityComparer<T>)typeof(EnumerableEqualityHelper<,>).MakeGenericType(t, a.GetGenericArguments()[0]).GetField("Default").GetValue(null);
                            DefaultNoEnumerate = GetDefaultReferenceComparer(t);
                            return;
                        }
                        g = typeof(IDictionary);
                        a = interfaces.FirstOrDefault(i => i.Equals(g));
                        if (!(a is null))
                        {
                            Default = (IEqualityComparer<T>)typeof(DictionaryEqualityHelper<>).MakeGenericType(t).GetField("Default").GetValue(null);
                            DefaultNoEnumerate = GetDefaultReferenceComparer(t);
                            return;
                        }
                        g = typeof(ICollection);
                        a = interfaces.FirstOrDefault(i => i.Equals(g));
                        if (!(a is null))
                        {
                            Default = (IEqualityComparer<T>)typeof(EnumerableEqualityHelper<>).MakeGenericType(t).GetField("Default").GetValue(null);
                            DefaultNoEnumerate = GetDefaultReferenceComparer(t);
                            return;
                        }
                    }
                }
                Default = (IEqualityComparer<T>)typeof(ReferenceEqualityHelper<>).MakeGenericType(t).GetField("Default").GetValue(null);
            }
            else
                Default = _default;
            DefaultNoEnumerate = Default;
        }

        public class GenericComparer : IEqualityComparer<T>
        {
            public bool Equals([AllowNull] T x, [AllowNull] T y)
            {
                if ((object)x is null)
                    return (object)y is null;
                if ((object)y is null)
                    return false;
                Type t = x.GetType();
                return t.Equals(y.GetType()) && ((t.IsValueType) ? x.Equals(y) : ReferenceEquals(x, y));
            }

            public int GetHashCode(T obj) => (obj is null) ? 0 : obj.GetHashCode();
        }
    }
}
