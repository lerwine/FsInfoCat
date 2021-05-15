using System;

namespace FsInfoCat
{
    public abstract class Coersion<T>
    {
        public static Coersion<T> Default { get; }

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
    }
}
