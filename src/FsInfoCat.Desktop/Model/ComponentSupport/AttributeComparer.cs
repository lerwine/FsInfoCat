using FsInfoCat.Desktop.Util;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public abstract class AttributeComparer : IEqualityComparer<Attribute>
    {
        public abstract bool Equals(Attribute x, Attribute y);

        public abstract int GetHashCode(Attribute obj);

        protected abstract Type GetAttributeType();

        public static IEqualityComparer<TAttribute> GetComparer<TAttribute>(TAttribute attribute)
            where TAttribute : Attribute
        {
            Type t = attribute.GetType();
            if (t.Equals(typeof(TAttribute)))
                return Typed<TAttribute>.Default;
            AttributeComparer comparer = (AttributeComparer)typeof(Typed<>).MakeGenericType(t).GetField(nameof(Typed<TAttribute>.Default))
                .GetValue(null);
            if (comparer is IEqualityComparer<TAttribute> equalityComparer)
                return equalityComparer;
            return (IEqualityComparer<TAttribute>)Activator.CreateInstance(typeof(RelayComparer<,>)
                .MakeGenericType(t, comparer.GetAttributeType()), comparer);
        }

        public class Typed<TAttribute> : AttributeComparer, IEqualityComparer<TAttribute>
            where TAttribute : Attribute
        {
            public static readonly Typed<TAttribute> Default;

            private readonly IEqualityComparer<TAttribute> _backingComparer;

            static Typed()
            {
                Type t = typeof(TAttribute);
                AttributeUsageAttribute usageAttribute = t.GetCustomAttribute<AttributeUsageAttribute>(true);
                if (usageAttribute is null || !usageAttribute.AllowMultiple)
                    Default = new Typed<TAttribute>(new SingleUsage<TAttribute>());
                else
                    Default = new Typed<TAttribute>(ModelDescriptor<TAttribute>.Create());
            }

            private Typed(IEqualityComparer<TAttribute> backingComparer)
            {
                _backingComparer = backingComparer;
            }

            protected override Type GetAttributeType() => typeof(TAttribute);

            public bool Equals(TAttribute x, TAttribute y) => _backingComparer.Equals(x, y);

            public override bool Equals(Attribute x, Attribute y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                if (ReferenceEquals(x, y))
                    return true;
                if (x is TAttribute a)
                {
                    if (y is TAttribute b)
                        return _backingComparer.Equals(a, b);
                    return false;
                }
                if (y is TAttribute)
                    return false;
                Type t = x.GetType();
                Type u = y.GetType();
                if (!t.IsAssignableFrom(u))
                {
                    if (u.IsAssignableFrom(t))
                        t = u;
                    else
                        return false;
                }
                u = typeof(Typed<>).MakeGenericType(t);
                return (bool)u.GetMethod(nameof(Equals), new Type[] { t, t }).Invoke(u.GetField(nameof(Default)), new object[] { x, y });
            }

            public int GetHashCode(TAttribute obj) => _backingComparer.GetHashCode(obj);

            public override int GetHashCode(Attribute obj)
            {
                if (obj is null)
                    return _backingComparer.GetHashCode(null);
                if (obj is TAttribute attribute)
                    return _backingComparer.GetHashCode(attribute);
                Type t = obj.GetType();
                Type u = typeof(Typed<>).MakeGenericType(t);
                return (int)u.GetMethod(nameof(GetHashCode), new Type[] { t }).Invoke(u.GetField(nameof(Default)), new object[] { obj });
            }
        }

        private class SingleUsage<TAttribute> : IEqualityComparer<TAttribute>
            where TAttribute : Attribute
        {
            public SingleUsage() { }
            public bool Equals(TAttribute x, TAttribute y) => (x is null) == (y is null);
            public int GetHashCode(TAttribute obj) => (obj is null) ? 0 : 1;
        }
    }
}
