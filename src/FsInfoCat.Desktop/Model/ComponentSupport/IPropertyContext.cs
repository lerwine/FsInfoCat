using System;
using System.ComponentModel;
using System.Globalization;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Generic contextual information and access to a property on an instance of a model class, represented through a <see cref="IModelContext"/>.
    /// </summary>
    /// <remarks>In addition to implementing the <see cref="ITypeDescriptorContext"/> interface for the target property, this includes functionality that is derived from
    /// its <see cref="PropertyDescriptor"/>, including that of the <see cref="TypeConverter"/> for its value type.</remarks>
    public interface IPropertyContext : IModelProperty, ITypeDescriptorContext, INotifyPropertyChanged
    {
        event ValueChangedEventHandler ValueChanged;

        /// <summary>
        /// Gets the current value of this property.
        /// </summary>
        /// <value>
        /// The value of this property.
        /// </value>
        object Value { get; }

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        /// <value>
        /// The type of this property.
        /// </value>
        Type PropertyType { get; }

        /// <summary>
        /// Gets the model instance which contains this property.
        /// </summary>
        /// <value>
        /// The <see cref="IModelContext"/> that represents the owner model instance.
        /// </value>
        IModelContext Owner { get; }

        /// <summary>
        /// Determines whether resetting the property changes its value.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if resetting the property changes its value; otherwise, <see langword="false"/>.
        /// </returns>
        bool CanResetValue();

        /// <summary>
        /// Resets the value for this property to the default value.
        /// </summary>
        /// <returns>The value of the property after it was reset.</returns>
        object ResetValue();

        /// <summary>
        /// Sets the value of this property to a different value.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> was <see langword="null"/> and the property mutator threw
        /// an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only.</exception>
        /// <exception cref="InvalidCastException"><paramref name="value"/> was not directly assignable to the current property and could not be converted.</exception>
        void SetValue(object value);

        /// <summary>
        /// Determines whether the <see cref="TypeConverter"/> of the associated <see cref="PropertyDescriptor"/> can convert an object of the given type to one that
        /// is assignable to this property, in the current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="sourceType">The <see cref="Type"/> to be converted from.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="TypeConverter"/> of the associated <see cref="PropertyDescriptor"/> can perform the conversion;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="sourceType"/> is null.</exception>
        bool CanConvertFrom(Type sourceType);

        /// <summary>
        /// Sets the value of the current property to a value that is converted to a compatible type, in the specified culture and the
        /// current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value that was applied to the property.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> was converted as a <see langword="null"/> value and the property mutator
        /// threw an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only or the conversion cannot be performed.</exception>
        object SetValueFromConverted(CultureInfo culture, object value);

        /// <summary>
        /// Sets the value of the current property to a value converted from a <see cref="string"/>-valued representation, in the
        /// current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="text">The culture-invariant <see cref="string"/> to convert.</param>
        /// <returns>The property value that was converted from the given <paramref name="text"/>, which was applied to tthe property.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> was converted as a <see langword="null"/> value and the property mutator
        /// threw an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only or the conversion cannot be performed.</exception>
        object SetValueFromInvariantString(string text);

        /// <summary>
        /// Sets the value of the current property to a value converted from a <see cref="string"/>-valued representation, in the
        /// current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>The property value that was converted from the given <paramref name="text"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> was converted as a <see langword="null"/> value and the property mutator
        /// threw an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only or the conversion cannot be performed.</exception>
        object SetValueFromString(string text);

        /// <summary>
        /// Sets the value of the current property to a value converted from a <see cref="string"/>-valued representation, in the specified culture and
        /// the current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to be used in the conversion or <see langword="null"/> to assume the current culture.</param>
        /// <param name="text">The culture-specific <see cref="string"/> to convert.</param>
        /// <returns>The property value that was converted from the given <paramref name="text"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> was converted as a <see langword="null"/> value and the property mutator
        /// threw an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only or the conversion cannot be performed.</exception>
        object SetValueFromString(CultureInfo culture, string text);

        /// <summary>
        /// Converts the current property value to a culture-invariant string representation, in the current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <returns>A culture-invariant string representation of the current property value or <see langword="null"/> if the underlying <see cref="TypeConverter"/>
        /// returned a <see langword="null"/> value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string GetInvariantStringValue();

        /// <summary>
        /// Converts the current property value to a string representation, in the specified culture and the current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to be used in the conversion or <see langword="null"/> to assume the current culture.</param>
        /// <returns>A culture-specific string representation of the current property value or <see langword="null"/> if the underlying <see cref="TypeConverter"/>
        /// returned a <see langword="null"/> value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string GetStringValue(CultureInfo culture);

        /// <summary>
        /// Converts the given value to a string representation, in the current <see cref="ITypeDescriptorContext"/> .
        /// </summary>
        /// <returns>A string representation of the current property value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string GetStringValue();

        /// <summary>
        /// Converts the given value to a string representation, in the current <see cref="ITypeDescriptorContext"/> .
        /// </summary>
        /// <returns>A string representation of the current property value.</returns>
        /// <remarks>In order to ensure compatibilty with the <see cref="object.ToString"/> override, this will return an empty string if the
        /// underlying <see cref="TypeConverter"/> returns a <see langword="null"/> value. If an exception is thrown during conversion, this will return the value from
        /// the <see cref="object.ToString"/> method on the property value itself, returning an empty string if the property value is null.
        /// <para>To get the actual <see cref="string"/> produced by the  <see cref="TypeConverter"/>, use the <see cref="GetStringValue"/> method.</para></remarks>
        string ToString();

        /// <summary>
        /// Indicates whether the given value can be assigned to this property and for the current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="value">The value to test for validity.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <paramref name="value"/> can be assigned to this property type or can be converted to a compatible value;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        bool IsAssignableFrom(object value);

        bool CheckPropertyChange();
    }

    public interface IPropertyContext<TInstance> : IPropertyContext
        where TInstance : class
    {
        /// <summary>
        /// Gets the model instance which contains this property.
        /// </summary>
        /// <value>
        /// The <see cref="IModelContext"/> that represents the owner model instance.
        /// </value>
        new IModelContext<TInstance> Owner { get; }
    }
}
