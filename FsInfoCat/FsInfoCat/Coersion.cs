using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    internal abstract class Coersion<T> : ICoersion<T>
    {
        protected TypeConverter Converter { get; } = TypeDescriptor.GetConverter(typeof(T));

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

        public virtual T Convert(object obj)
        {
            if (TryCoerce(obj, out T result))
                return result;
            return (T)Converter.ConvertFrom(obj);
        }

        public virtual bool TryConvert(object obj, out T result)
        {
            if (TryCoerce(obj, out result))
                return true;
            if (obj is null)
            {
                result = default;
                return false;
            }
            if (Converter.CanConvertFrom(obj.GetType()))
                try
                {
                    result = (T)Converter.ConvertFrom(obj);
                    return true;
                }
                catch { /* ignored on purpose */ }
            result = default;
            return false;
        }

        object ICoersion.Convert(object obj) => Convert(obj);

        bool ICoersion.TryConvert(object obj, out object result)
        {
            bool returnValue = TryConvert(obj, out T t);
            result = t;
            return returnValue;
        }

        public virtual bool TryCast(object obj, out T result)
        {
            try
            {
                result = (T)obj;
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        bool ICoersion.TryCast(object obj, out object result)
        {
            bool returnValue = TryConvert(obj, out T t);
            result = t;
            return returnValue;
        }

        public abstract bool Equals(T x, T y);
        public abstract int GetHashCode(T obj);
        bool IEqualityComparer.Equals(object x, object y) => (x is T a && y is T b) ? Equals(a, b) : Equals(x, y);
        int IEqualityComparer.GetHashCode(object obj) => (obj is T t) ? GetHashCode(t) : ((obj is null) ? 0 : obj.GetHashCode());
    }

}
