using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Services
{
    public interface IComparisonService
    {
        ICoersion GetDefaultCoersion(Type type);

        ICoersion<T> GetDefaultCoersion<T>();

        /// <summary>
        /// Gets the optimal <see cref="IEqualityComparer{T}"/> instance for the current generic type.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>
        /// The optimal <see cref="IEqualityComparer{T}"/> instance for the current generic type.
        /// </returns>
        IEqualityComparer<T> GetEqualityComparer<T>();

        /// <summary>
        /// Gets the optimal <see cref="IEqualityComparer{T}"/> instance for the current generic type.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <param name="noPropertyComparer">If set to <see langword="true"/>, do not return a comparar which does uses reflection to compare properties.</param>
        /// <returns>
        /// The optimal <see cref="IEqualityComparer{T}"/> instance for the current generic type.
        /// </returns>
        IEqualityComparer<T> GetEqualityComparer<T>(bool noPropertyComparer);

        /// <summary>
        /// Gets the optimal <see cref="IEqualityComparer{T}"/> instance for the given type.
        /// </summary>
        /// <param name="type">The type of value to be compared.</param>
        /// <returns>
        /// The optimal <see cref="IEqualityComparer{T}"/> instance as a generic <see cref="IEqualityComparer"/>.
        /// </returns>
        IEqualityComparer GetEqualityComparer(Type type);

        /// <summary>
        /// Gets the optimal <see cref="IEqualityComparer{T}"/> instance for the given type.
        /// </summary>
        /// <param name="type">The type of value to be compared.</param>
        /// <param name="noPropertyComparer">If set to <see langword="true"/>, do not return a comparar which does uses reflection to compare properties.</param>
        /// <returns>
        /// The optimal <see cref="IEqualityComparer{T}"/> instance as a generic <see cref="IEqualityComparer"/>.
        /// </returns>
        IEqualityComparer GetEqualityComparer(Type type, bool noPropertyComparer);

        /// <summary>
        /// Gets an <see cref="IEqualityComparer{T}"/> based off of a generic type that implements <see cref="IComparable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>
        /// The <see cref="IEqualityComparer{T}"/> based off of the generic type implementing <see cref="IComparable{T}"/>.
        /// </returns>
        IEqualityComparer<T> GetComparableEqualityComparer<T>() where T : class, IComparable<T>;

        /// <summary>
        /// Gets an <see cref="IEqualityComparer{T}"/> based off of a generic type that implements <see cref="IComparable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>
        /// The <see cref="IEqualityComparer{T}"/> based off of the generic type implementing <see cref="IComparable{T}"/>, returned as a
        /// generic <see cref="IEqualityComparer"/> value.
        /// </returns>
        IEqualityComparer GetComparableEqualityComparer(Type type);

        IEqualityComparer<T> GetConvertingEqualityComparer<T>() where T : class;

        IEqualityComparer GetConvertingEqualityComparer(Type type);

        IComparer<T> GetConvertingComparer<T>() where T : class;

        IComparer GetConvertingComparer(Type type);

        /// <summary>
        /// Gets the <see cref="EqualityComparer{T}.Default"/> <see cref="IEqualityComparer{T}"/> for the given type.
        /// </summary>
        /// <param name="type">The type of value to be compared.</param>
        /// <returns>he <see cref="EqualityComparer{T}.Default"/> <see cref="IEqualityComparer{T}"/> for the given type as a
        /// generic <see cref="IEqualityComparer"/>.</returns>
        IEqualityComparer GetDefaultEqualityComparer(Type type);

        /// <summary>
        /// Gets an <see cref="IEqualityComparer{T}"/> instance which uses object properties or string conversion to assist with comparison for the current generic type.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>
        /// An <see cref="IEqualityComparer{T}"/> instance which uses <see cref="System.ComponentModel.PropertyDescriptor">PropertyDescriptors</see>
        /// or <see cref="System.ComponentModel.TypeConverter.ConvertToInvariantString(object)"/> to assist with comparison for the current generic type.
        /// </returns>
        IEqualityComparer<T> GetComponentEqualityComparer<T>() where T : class;

        /// <summary>
        /// Gets an <see cref="IEqualityComparer{T}"/> instance which uses object properties or string conversion to assist with comparison for a given type.
        /// </summary>
        /// <param name="type">The type to be compared.</param>
        /// <returns>
        /// An <see cref="IEqualityComparer{T}"/> instance which uses object <see cref="System.ComponentModel.PropertyDescriptor">PropertyDescriptors</see>
        /// or <see cref="System.ComponentModel.TypeConverter.ConvertToInvariantString(object)"/>to assist with comparison for the given <paramref name="type"/>,
        /// returned as a generic <see cref="IEqualityComparer"/>.</returns>
        /// <exception cref="TypeLoadException"><paramref name="type"/> is not a reference type.</exception>
        IEqualityComparer GetComponentEqualityComparer(Type type);

        /// <summary>
        /// Gets the optimal <see cref="IEqualityComparer{T}"/> instance for the current generic type.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>
        /// The optimal <see cref="IEqualityComparer{T}"/> instance for the current generic type.
        /// </returns>
        IComparer<T> GetComparer<T>();

        IComparer<T> GetComparer<T>(bool noPropertyComparer);

        /// <summary>
        /// Gets the optimal <see cref="IEqualityComparer{T}"/> instance for the given type.
        /// </summary>
        /// <param name="type">The type to be compared.</param>
        /// <returns>
        /// The optimal <see cref="IEqualityComparer{T}"/> instance for the given type as a generic <see cref="IComparer"/>.
        /// </returns>
        IComparer GetComparer(Type type);

        IComparer GetComparer(Type type, bool noPropertyComparer);

        /// <summary>
        /// Gets an <see cref="IComparer{T}"/> instance which uses object properties or string conversion to assist with comparison for the current generic type.
        /// </summary>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>
        /// An <see cref="IComparer{T}"/> instance which uses <see cref="System.ComponentModel.PropertyDescriptor">PropertyDescriptors</see>
        /// or <see cref="System.ComponentModel.TypeConverter.ConvertToInvariantString(object)"/> to assist with comparison for the current generic type.
        /// </returns>
        IComparer<T> GetComponentComparer<T>() where T : class;

        /// <summary>
        /// Gets an <see cref="IComparer{T}"/> instance which uses object properties or string conversion to assist with comparison for a given type.
        /// </summary>
        /// <param name="type">The type to be compared.</param>
        /// <returns>
        /// An <see cref="IComparer{T}"/> instance which uses object <see cref="System.ComponentModel.PropertyDescriptor">PropertyDescriptors</see>
        /// or <see cref="System.ComponentModel.TypeConverter.ConvertToInvariantString(object)"/>to assist with comparison for the given <paramref name="type"/>,
        /// returned as a generic <see cref="IComparer"/>.</returns>
        /// <exception cref="TypeLoadException"><paramref name="type"/> is not a reference type.</exception>
        IComparer GetComponentComparer(Type type);
    }
}
