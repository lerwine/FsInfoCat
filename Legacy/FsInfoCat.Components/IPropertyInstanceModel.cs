using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FsInfoCat.Components
{
    /// <summary>
    /// Exposes methods and properties from a <seealso cref="PropertyDescriptor"/> and the associated <seealso cref="TypeConverter"/>
    /// from the <seealso cref="ITypeDescriptorContext">Context</seealso> of an instance of a component object.
    /// </summary>
    public interface IPropertyInstanceModel : IPropertyModel
    {
        event EventHandler PropertyValueChanged;

        /// <summary>
        /// The current property value.
        /// </summary>
        object PropertyValue { get; }

        /// <summary>
        /// Indicates whether this object supports a <see cref="GetStandardValues">standard set of values</see> that can be picked from a list, using the current <see cref="Context"/>.
        /// </summary>
        bool HasStandardValues { get; }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="GetStandardValues"/> is an exclusive list of possible values, using the current <see cref="Context"/>.
        /// </summary>
        bool AreStandardValuesExclusive { get; }

        /// <summary>
        /// The context that references the component which contains the current property.
        /// </summary>
        ITypeDescriptorContext Context { get; }

        /// <summary>
        /// Indicates whether resetting the value for this property of the component changes its value.
        /// </summary>
        /// <returns><see langword="true"/> if resetting this property would change its value; otherwise, <see langword="false"/>.</returns>
        bool CanResetValue();

        /// <summary>
        /// resets the value for this property of the component to the default value.
        /// </summary>
        void ResetValue();

        /// <summary>
        /// Sets the value of the component to a different value.
        /// </summary>
        /// <param name="value">The new property value.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> and the propery's mutator threw an exception of this type.</exception>
        /// <exception cref="NotSupportedException">The current property is read-only.</exception>
        /// <exception cref="InvalidCastException">The <paramref name="value"/> was not assignable as the current property type and could not be converted.</exception>
        void SetValue(object value);

        /// <summary>
        /// Converts the given value to the type of this property.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFrom(object value);

        /// <summary>
        /// Converts the given object to the type of this property, using the specified culture information and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The property value to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFrom(CultureInfo culture, object value);

        /// <summary>
        /// Converts the given string to the type of this property, using the invariant culture and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromInvariantString(string text);

        /// <summary>
        /// Converts the given string to the type of this property, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromString(string text);

        /// <summary>
        /// Converts the given string to the type of this property, using the specified <see cref="CultureInfo">culture</see> and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromString(CultureInfo culture, string text);

        /// <summary>
        /// Converts the specified value to a culture-invariant string representation, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string ConvertToInvariantString(object value);

        /// <summary>
        /// Converts the given value to a string representation, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string ConvertToString(object value);

        /// <summary>
        /// Converts the given value to a string representation, using the specified <see cref="CultureInfo">culture</see> and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string ConvertToString(CultureInfo culture, object value);

        /// <summary>
        /// Indicates whether the given value's type can be assigned as the property's type or if it can be converted to the property type, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> can be directly assigned or converted and then assigned to the current property type.</returns>
        bool IsCompatibleType(object value);

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for in the current <see cref="Context"/>.
        /// </summary>
        /// <returns>A <see cref="TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or <see langword="null"/> if the property does not support a standard set of values.</returns>
        TypeConverter.StandardValuesCollection GetStandardValues();
    }

    public interface IPropertyInstanceModel<TInstance> : IPropertyInstanceModel
        where TInstance : class
    {
        /// <summary>
        /// The context that references the component which contains the current property.
        /// </summary>
        new IPropertyDescriptorContext<TInstance> Context { get; }
    }
}
