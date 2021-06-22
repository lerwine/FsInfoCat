using System;
using System.ComponentModel;
using System.Globalization;

namespace FsInfoCat.Components
{
    public class PropertyInstanceModel<TInstance, TProperty> : IPropertyInstanceModel<TInstance>
        where TInstance : class
    {
        private readonly TypeConverter _converter;
        private readonly PropertyDescriptor _descriptor;

        public event EventHandler PropertyValueChanged;

        /// <summary>
        /// The current property value.
        /// </summary>
        public TProperty PropertyValue => (TProperty)_descriptor.GetValue(Context.Instance);

        object IPropertyInstanceModel.PropertyValue => PropertyValue;

        /// <summary>
        /// Indicates whether this object supports a <see cref="GetStandardValues">standard set of values</see> that can be picked from a list.
        /// </summary>
        public bool HasStandardValues => _descriptor.Converter.GetStandardValuesSupported();

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="GetStandardValues"/> is an exclusive list of possible values.
        /// </summary>
        public bool AreStandardValuesExclusive => _descriptor.Converter.GetStandardValuesExclusive();

        /// <summary>
        /// The context that references the component which contains the current property.
        /// </summary>
        public PropertyDescriptorContext<TInstance, TProperty> Context { get; }

        IPropertyDescriptorContext<TInstance> IPropertyInstanceModel<TInstance>.Context => Context;

        ITypeDescriptorContext IPropertyInstanceModel.Context => Context;

        /// <summary>
        /// The <see cref="MemberDescriptor.Name">name</see> (identifier) of the property.
        /// </summary>
        public string Name => _descriptor.Name;

        /// <summary>
        /// The name to display in the user interface.
        /// </summary>
        /// <remarks>For types that derive from <see cref="System.Data.Common.DbConnectionStringBuilder"/> this also contains the key that is used within the connection string.
        /// <para>This value returns the value of the <see cref="DisplayNameAttribute">DisplayName</see> attribute or the <see cref="MemberDescriptor.Name"/> of
        /// the property if no <see cref="DisplayNameAttribute">DisplayName</see> was used or its value was empty.</para></remarks>
        public string DisplayName => string.IsNullOrWhiteSpace(_descriptor.DisplayName) ? _descriptor.Name : _descriptor.DisplayName;

        /// <summary>
        /// The name of the category to which the member belongs
        /// </summary>
        /// <remarks>This value returns the value of the <see cref="CategoryAttribute">Category</see> attribute or the value of
        /// the <see cref="CategoryAttribute.Default">Default Category (<c>&quot;Misc&quot;</c>)</see> if
        /// no <see cref="CategoryAttribute">Category</see> was used or its value was empty.</remarks>
        public string Category => string.IsNullOrWhiteSpace(_descriptor.Category) ? CategoryAttribute.Default.Category : _descriptor.DisplayName;

        /// <summary>
        /// The description of the property.
        /// </summary>
        /// <remarks>This value returns the value of the <see cref="DescriptionAttribute">Description</see> attribute or an empty string
        /// (<c>&quot;&quot;</c>) if no <see cref="DescriptionAttribute">Description</see> was used.</remarks>
        public string Description => _descriptor.Description ?? "";

        /// <summary>
        /// Indicates whether this property is read-only.
        /// </summary>
        /// <remarks>If this is <see langword="true"/>, that may only be due to the usage of the <see cref="ReadOnlyAttribute">ReadOnly</see> and
        /// does not necessarily mean that the property has no mutator.</remarks>
        public bool IsReadOnly => _descriptor.IsReadOnly;

        /// <summary>
        /// The full <see cref="PropertyDescriptor.PropertyType">type</see> name for the property.
        /// </summary>
        public string TypeFullName => _descriptor.PropertyType.FullName;

        /// <summary>
        /// The simple <see cref="PropertyDescriptor.PropertyType">type</see> name for the property.
        /// </summary>
        public string TypeSimpleName => _descriptor.PropertyType.Name;

        /// <summary>
        /// Creates a new instance property model.
        /// </summary>
        /// <param name="context">The context of the property and its component.</param>
        public PropertyInstanceModel(PropertyDescriptorContext<TInstance, TProperty> context)
        {
            _descriptor = (Context = context ?? throw new ArgumentNullException(nameof(context))).PropertyDescriptor;
            _converter = _descriptor.Converter;
        }

        /// <summary>
        /// Indicates whether resetting the value for this property of the component changes its value.
        /// </summary>
        /// <returns><see langword="true"/> if resetting this property would change its value; otherwise, <see langword="false"/>.</returns>
        public bool CanResetValue() => _descriptor.CanResetValue(Context.Instance);

        /// <summary>
        /// Converts the given value to the type of this property.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFrom(object value) => (TProperty)_converter.ConvertFrom(value);

        object IPropertyInstanceModel.ConvertFrom(object value) => ConvertFrom(value);

        /// <summary>
        /// Converts the given object to the type of this property, using the specified culture information and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The property value to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFrom(CultureInfo culture, object value) => (TProperty)_converter.ConvertFrom(Context, culture, value);

        object IPropertyInstanceModel.ConvertFrom(CultureInfo culture, object value) => ConvertFrom(culture, value);

        /// <summary>
        /// Converts the given string to the type of this property, using the invariant culture and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFromInvariantString(string text) => (TProperty)_converter.ConvertFromInvariantString(Context, text);

        object IPropertyInstanceModel.ConvertFromInvariantString(string text) => ConvertFromInvariantString(text);

        /// <summary>
        /// Converts the given string to the type of this property, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFromString(string text) => (TProperty)_converter.ConvertFromString(Context, text);

        object IPropertyInstanceModel.ConvertFromString(string text) => ConvertFromString(text);

        /// <summary>
        /// Converts the given string to the type of this property, using the specified <see cref="CultureInfo">culture</see> and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFromString(CultureInfo culture, string text) => (TProperty)_converter.ConvertFromString(Context, culture, text);

        object IPropertyInstanceModel.ConvertFromString(CultureInfo culture, string text) => ConvertFromString(culture, text);

        /// <summary>
        /// Converts the specified value to a culture-invariant string representation, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public string ConvertToInvariantString(TProperty value) => _converter.ConvertToInvariantString(Context, value);

        string IPropertyInstanceModel.ConvertToInvariantString(object value) => ConvertToString(ConvertFrom(value));

        /// <summary>
        /// Converts the given value to a string representation, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public string ConvertToString(TProperty value) => _converter.ConvertToString(Context, value);

        string IPropertyInstanceModel.ConvertToString(object value) => ConvertToString(ConvertFrom(value));

        /// <summary>
        /// Converts the given value to a string representation, using the specified <see cref="CultureInfo">culture</see> and the current <see cref="Context"/>.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public string ConvertToString(CultureInfo culture, TProperty value) => _converter.ConvertToString(Context, culture, value);

        string IPropertyInstanceModel.ConvertToString(CultureInfo culture, object value) => ConvertToString(culture, ConvertFrom(value));

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for in the current <see cref="Context"/>.
        /// </summary>
        /// <returns>A <see cref="TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or <see langword="null"/> if the property does not support a standard set of values.</returns>
        public TypeConverter.StandardValuesCollection GetStandardValues() => (TypeConverter.StandardValuesCollection)_converter.GetStandardValues();

        /// <summary>
        /// Indicates whether the given value's type can be assigned as the property's type or if it can be converted to the property type, using the current <see cref="Context"/>.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> can be directly assigned or converted and then assigned to the current property type.</returns>
        public bool IsCompatibleType(object value) => _converter.IsValid(Context, value);

        /// <summary>
        /// resets the value for this property of the component to the default value.
        /// </summary>
        public void ResetValue() => _descriptor.ResetValue(Context.Instance);

        /// <summary>
        /// Sets the value of the component to a different value.
        /// </summary>
        /// <param name="value">The new property value.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> and the propery's mutator threw an exception of this type.</exception>
        /// <exception cref="NotSupportedException">The current property is read-only.</exception>
        public void SetValue(TProperty value)
        {
            if (_descriptor.IsReadOnly)
                throw new NotSupportedException();
            _descriptor.SetValue(Context.Instance, value);
        }

        void IPropertyInstanceModel.SetValue(object value) => SetValue(ConvertFrom(value));
    }
}
