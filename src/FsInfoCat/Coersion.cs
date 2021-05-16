using System;

namespace FsInfoCat
{
    internal abstract class Coersion<T> : ICoersion<T>
    {
        public static Coersion<T> Default { get; }

        Type ICoersion.ValueType => typeof(T);

        static Coersion()
        {
            Type type = typeof(T);
            if (type.IsValueType)
            {
                if (type.IsGenericType && typeof(Nullable<>).Equals(type.GetGenericTypeDefinition()))
                    Default = (Coersion<T>)Activator.CreateInstance(typeof(NullableCoersion<>).MakeGenericType(Nullable.GetUnderlyingType(type)));
                else
                    Default = (Coersion<T>)Activator.CreateInstance(typeof(ValueCoersion<>).MakeGenericType(type));
            }
            else
                Default = (Coersion<T>)Activator.CreateInstance(typeof(ReferenceCoersion<>).MakeGenericType(type));
        }

        public virtual T Cast(object obj) => (T)obj;

        public abstract bool TryCoerce(object obj, out T result);

        object ICoersion.Cast(object obj) => Cast(obj);

        bool ICoersion.TryCoerce(object obj, out object result)
        {
            bool returnValue = TryCoerce(obj, out T t);
            result = t;
            return returnValue;
        }
    }
}
