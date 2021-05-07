using System;
using System.Collections;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Generic representation of the characteristics of a property represented within a <see cref="IModelDescriptor"/>.
    /// </summary>
    /// <remarks>This is functionally similar to a <see cref="PropertyDescriptor"/> and provides some of the
    /// functionality of the <see cref="TypeConverter"/> for its value type.</remarks>
    public interface IModelPropertyDescriptor : IEquatable<IModelPropertyDescriptor>, IModelProperty, IEqualityComparer
    {
        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        /// <value>
        /// The type of this property.
        /// </value>
        Type PropertyType { get; }

        /// <summary>
        /// Gets a value indicating whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if value change notifications may originate from outside the property descriptor; otherwise, <see langword="false"/>.
        /// </value>
        bool SupportsChangeEvents { get; }

        /// <summary>
        /// Represents the model type that owns this property.
        /// </summary>
        /// <value>
        /// The <see cref="IModelDescriptor"/> that represents model type that owns this property.
        /// </value>
        IModelDescriptor Owner { get; }

        /// <summary>
        /// Gets the value of this property on the specified object.
        /// </summary>
        /// <param name="component">The component that contains the current property.</param>
        /// <returns>The value of this property on the specified <paramref name="component"/> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        /// <exception cref="InvalidCastException"><paramref name="component"/> type does not match the underlying owner type.</exception>
        object GetValue(object component);

        /// <summary>
        /// Returns whether the <see cref="TypeConverter"/> for this property can convert an object of the given type to the type of this property.
        /// </summary>
        /// <param name="sourceType">The <see cref="Type"/> to be converted form.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="TypeConverter"/> for this property can perform the conversion; otherwise, <see langword="false"/>.
        /// </returns>
        bool CanConvertFrom(Type sourceType);

        /// <summary>
        /// Converts the given value to the type of this property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFrom(object value);

        /// <summary>
        /// Converts the given string to the appropriate type for this property, using the invariant culture.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>
        /// The value that represents the converted text.
        /// </returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromInvariantString(string text);

        /// <summary>
        /// Converts the given string to the appropriate type for this property.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>
        /// The value that represents the converted text.
        /// </returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromString(string text);

        /// <summary>
        /// Converts the specified value to a culture-invariant string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// A <see cref="string"/> that represents the converted value.
        /// </returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        /// <exception cref="InvalidCastException">The <paramref name="value"/> was not assignable to the property and cannot not be converted.</exception>
        string ConvertToInvariantString(object value);

        /// <summary>
        /// Converts the specified value to a string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// A <see cref="string"/> that represents the converted value.
        /// </returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        /// <exception cref="InvalidCastException">The <paramref name="value"/> was not assignable to the property and cannot not be converted.</exception>
        string ConvertToString(object value);

        /// <summary>
        /// Determines whether the specified value can be direclty assigned to this property.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// <see langword="true"/> if the specified value can be directly assigned to this property; otherwise, <see langword="false"/>.
        /// </returns>
        bool IsAssignableFrom(object value);
    }
    
    public interface IModelPropertyDescriptor<TModel> : IEquatable<IModelPropertyDescriptor<TModel>>, IModelPropertyDescriptor
        where TModel : class
    {
        /// <summary>
        /// Represents the model type that owns this property.
        /// </summary>
        /// <value>
        /// The <see cref="IModelDescriptor"/> that represents model type that owns this property.
        /// </value>
        new ModelDescriptor<TModel> Owner { get; }

        /// <summary>
        /// Gets the value of this property on the specified object.
        /// </summary>
        /// <param name="component">The component that contains the current property.</param>
        /// <returns>The value of this property on the specified <paramref name="component"/> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        object GetValue(TModel component);
    }
}
