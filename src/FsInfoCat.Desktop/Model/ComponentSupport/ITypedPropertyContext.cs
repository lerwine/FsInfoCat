using System;
using System.Globalization;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public interface ITypedPropertyContext<TValue> : ITypedProperty<TValue>, IPropertyContext
    {
        /// <summary>
        /// Gets the current value of this property.
        /// </summary>
        /// <value>
        /// The value of this property.
        /// </value>
        new TValue Value { get; }

        /// <summary>
        /// Resets the value for this property to the default value.
        /// </summary>
        /// <returns>The value of the property after it was reset.</returns>
        new TValue ResetValue();

        /// <summary>
        /// Sets the value of this property to a different value.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> was <see langword="null"/> and the property mutator threw
        /// an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only.</exception>
        /// <exception cref="InvalidCastException"><paramref name="value"/> was not directly assignable to the current property and could not be converted.</exception>
        void SetValue(TValue value);

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
        new TValue SetValueFromConverted(CultureInfo culture, object value);

        /// <summary>
        /// Sets the value of the current property to a value converted from a <see cref="string"/>-valued representation, in the
        /// current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="text">The culture-invariant <see cref="string"/> to convert.</param>
        /// <returns>The property value that was converted from the given <paramref name="text"/>, which was applied to tthe property.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> was converted as a <see langword="null"/> value and the property mutator
        /// threw an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only or the conversion cannot be performed.</exception>
        new TValue SetValueFromInvariantString(string text);

        /// <summary>
        /// Sets the value of the current property to a value converted from a <see cref="string"/>-valued representation, in the
        /// current <see cref="ITypeDescriptorContext"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>The property value that was converted from the given <paramref name="text"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> was converted as a <see langword="null"/> value and the property mutator
        /// threw an <see cref="ArgumentNullException"/>.</exception>
        /// <exception cref="NotSupportedException">The property is read-only or the conversion cannot be performed.</exception>
        new TValue SetValueFromString(string text);

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
        new TValue SetValueFromString(CultureInfo culture, string text);
    }
}
