using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    /// <summary>
    /// Base class for object value coersion.
    /// </summary>
    /// <typeparam name="T">The coersion targetoutput type.</typeparam>
    /// <seealso cref="ICoersion{T}" />
    public abstract class Coersion<T> : ICoersion<T>
    {
        /// <summary>
        /// Gets the <see cref="TypeConverter"/> for type <typeparamref name="T"/>.
        /// </summary>
        /// <value>The the <see cref="TypeConverter"/> for type <typeparamref name="T"/>.</value>
        [NotNull]
        protected TypeConverter Converter { get; } = TypeDescriptor.GetConverter(typeof(T));

        /// <summary>
        /// Gets the default <see cref="Coersion{T}"/> instance.
        /// </summary>
        /// <value>The default <see cref="Coersion{T}"/> instance.</value>
        [NotNull]
        public static Coersion<T> Default { get; }

        Type ICoersion.ValueType => typeof(T);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Initializes static members of the <see cref="Coersion{T}"/> class.
        /// </summary>
        static Coersion()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Type type = typeof(T);
            if (type.IsValueType)
                Default = type.IsGenericType && typeof(Nullable<>).Equals(type.GetGenericTypeDefinition())
#pragma warning disable CS8604 // Possible null reference argument - preceding condition ensures GetUnderlyingType will not return null.
                    ? (Coersion<T>)Activator.CreateInstance(typeof(NullableCoersion<>).MakeGenericType(Nullable.GetUnderlyingType(type)))
                    : (Coersion<T>)Activator.CreateInstance(typeof(ValueCoersion<>).MakeGenericType(type));
#pragma warning restore CS8604 // Possible null reference argument.
            else if (type.IsArray && type.GetArrayRank() == 1)
            {
                if (type.Equals(typeof(byte[])))
                    Default = (Coersion<T>)(object)ByteArrayCoersion.Default;
                else
                {
#pragma warning disable CS8604 // Possible null reference argument - type.IsArray ensures GetElementType will not return null.
                    type = typeof(ArrayCoersion<>).MakeGenericType(type.GetElementType());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference - using nameof ensures GetField will not return null.
                    Default = (Coersion<T>)type.GetField(nameof(ArrayCoersion<object>.Default)).GetValue(null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
            else
                Default = (Coersion<T>)Activator.CreateInstance(typeof(ReferenceCoersion<>).MakeGenericType(type));
        }

        /// <summary>
        /// Casts the specified object as type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><paramref name="obj"/> cast as type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException"><paramref name="obj"/> could not be cast as type <typeparamref name="T"/>.</exception>
        public virtual T Cast(object obj) => (T)obj;

        /// <summary>
        /// Attempts to coerce an object to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="result">The value cast or converted to type <typeparamref name="T"/>, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast or converted to type <typeparamref name="T"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool TryCoerce(object obj, out T result)
        {
            if (TryCast(obj, out result))
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

        object ICoersion.Cast(object obj) => Cast(obj);

        bool ICoersion.TryCoerce(object obj, out object result)
        {
            bool returnValue = TryCoerce(obj, out T t);
            result = t;
            return returnValue;
        }

        /// <summary>
        /// Coerces the specified object to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns><paramref name="obj"/> coerced as type <typeparamref name="T"/>.</returns>\
        /// <exception cref="NotSupportedException"><paramref name="obj"/> could not be converted to type <typeparamref name="T"/>.</exception>
        public virtual T Coerce(object obj)
        {
            if (TryCast(obj, out T result))
                return result;
            return OnConvert(obj);
        }

        protected virtual T OnConvert(object obj) => (T)Converter.ConvertFrom(obj);

        object ICoersion.Coerce(object obj) => Coerce(obj);

        /// <summary>
        /// Attempts to cast an object as type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="result">The cast value, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as type <typeparamref name="T"/>; otherwise, <see langword="false"/>.</returns>
        public abstract bool TryCast(object obj, out T result);

        bool ICoersion.TryCast(object obj, out object result)
        {
            bool returnValue = TryCast(obj, out T t);
            result = t;
            return returnValue;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <paramref name="T" /> to compare.</param>
        /// <param name="y">The second object of type <paramref name="T" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public abstract bool Equals(T x, T y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public abstract int GetHashCode(T obj);

        bool IEqualityComparer.Equals(object x, object y) => (x is T a && y is T b) ? Equals(a, b) : Equals(x, y);

        int IEqualityComparer.GetHashCode(object obj) => (obj is T t) ? GetHashCode(t) : ((obj is null) ? 0 : obj.GetHashCode());

        /// <summary>
        /// Normalizes the specified value.
        /// </summary>
        /// <param name="obj">The value to normalize.</param>
        /// <returns>The normalized value.</returns>
        public virtual T Normalize(T obj) => obj;

        object ICoersion.Normalize(object obj) => Normalize((T)obj);
    }

}
