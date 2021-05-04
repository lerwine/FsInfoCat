using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FsInfoCat.Components
{
    /// <summary>
    /// Exposes select, normalized properties of a <seealso cref="PropertyDescriptor"/>.
    /// </summary>
    public class PropertyDefinitionModel<TOwner, TProperty> : IPropertyDefinitionModel<TOwner>
        where TOwner : class
    {
        private readonly PropertyDescriptor _descriptor;

        /// <summary>
        /// Attributes used to validate the property value.
        /// </summary>
        public ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        /// <summary>
        /// Indicates whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        public bool SupportsChangeEvents => _descriptor.SupportsChangeEvents;

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
        /// Indicates whether this object supports a <see cref="GetStandardValues">standard set of values</see> that can be picked from a list.
        /// </summary>
        public bool HasStandardValues => _descriptor.Converter.GetStandardValuesSupported();

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="GetStandardValues"/> is an exclusive list of possible values.
        /// </summary>
        public bool AreStandardValuesExclusive => _descriptor.Converter.GetStandardValuesExclusive();

        public TypeDefinitionModel<TOwner> Owner { get; }

        ITypeDefinitionModel IPropertyDefinitionModel.Owner => Owner;

        /// <summary>
        /// Creates a new property definition object.
        /// </summary>
        /// <param name="descriptor">The property descriptor.</param>
        /// <param name="validationAttributes">Use this parameter to specify validation attributes which take precedence over any attributes applied to the property.</param>
        public PropertyDefinitionModel(TypeDefinitionModel<TOwner> component, PropertyDescriptor descriptor, IEnumerable<ValidationAttribute> validationAttributes = null)
        {
            Owner = component ?? throw new ArgumentNullException(nameof(component));
            _descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
            ValidationAttributes = new ReadOnlyCollection<ValidationAttribute>(((validationAttributes is null || !(validationAttributes = validationAttributes.Where(a => !(a is null))).Any()) ?
                descriptor.Attributes.OfType<ValidationAttribute>() :
                validationAttributes.Concat(descriptor.Attributes.OfType<ValidationAttribute>()).Distinct(AttributeComparer.Instance)).ToArray());
        }

        /// <summary>
        /// Gets the <see cref="PropertyDescriptor"/> wrapped by thsi object.
        /// </summary>
        /// <returns>The <see cref="PropertyDescriptor"/> wrapped by thsi object.</returns>
        public PropertyDescriptor GetDescriptor() => _descriptor;

        /// <summary>
        /// Converts the given value to the type of this property.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFrom(object value) => (TProperty)_descriptor.Converter.ConvertFrom(value);

        object IPropertyDefinitionModel.ConvertFrom(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given string to the type of this property, using the invariant culture.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFromInvariantString(string text) => (TProperty)_descriptor.Converter.ConvertFromInvariantString(text);

        object IPropertyDefinitionModel.ConvertFromInvariantString(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given string to the type of this property.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public TProperty ConvertFromString(string text) => (TProperty)_descriptor.Converter.ConvertFromString(text);

        object IPropertyDefinitionModel.ConvertFromString(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the specified value to a culture-invariant string representation.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public string ConvertToInvariantString(TProperty value) => _descriptor.Converter.ConvertToInvariantString(value);

        public string ConvertToInvariantString(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the given value to a string representation.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        public string ConvertToString(TProperty value) => _descriptor.Converter.ConvertToString(value);

        public string ConvertToString(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates whether the given value's type can be assigned as the property's type or if it can be converted to the property type.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> can be directly assigned or converted and then assigned to the current property type.</returns>
        public bool IsCompatibleType(object value) => _descriptor.Converter.IsValid(value);

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for.
        /// </summary>
        /// <returns>A <see cref="TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or <see langword="null"/> if the property does not support a standard set of values.</returns>
        public TypeConverter.StandardValuesCollection GetStandardValues() => (TypeConverter.StandardValuesCollection)_descriptor.Converter.GetStandardValues();

        public PropertyInstanceModel<TOwner, TProperty> ToInstanceProperty(TypeInstanceModel<TOwner>  componentContext) => new PropertyInstanceModel<TOwner, TProperty>(new PropertyDescriptorContext<TOwner, TProperty>(componentContext, this));

        IPropertyInstanceModel<TOwner> IPropertyDefinitionModel<TOwner>.ToInstanceProperty(TypeInstanceModel<TOwner> component) => ToInstanceProperty(component);

        IPropertyInstanceModel IPropertyDefinitionModel.ToInstanceProperty(ITypeInstanceModel component) => ToInstanceProperty((TypeInstanceModel<TOwner>)component);

        class AttributeComparer : IEqualityComparer<ValidationAttribute>
        {
            internal readonly static AttributeComparer Instance = new AttributeComparer();
            private AttributeComparer() { }
            public bool Equals(ValidationAttribute x, ValidationAttribute y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                if (ReferenceEquals(x, y))
                    return true;
                Type a = x.GetType();
                Type b = y.GetType();
                if (!a.Equals(b) || a.GetCustomAttributes<AttributeUsageAttribute>().Any(u => !u.AllowMultiple))
                    return false;

                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(a))
                {
                    object o1 = pd.GetValue(x);
                    object o2 = pd.GetValue(y);
                    if (o1 is null)
                    {
                        if (!(o2 is null))
                            return false;
                    }
                    else if (!(bool)typeof(EqualityComparer<>).MakeGenericType(pd.PropertyType).GetMethod("Equals", new Type[] { pd.PropertyType, pd.PropertyType }).Invoke(null, new object[] { o1, o2 }))
                        return false;
                }
                return true;
            }

            public int GetHashCode(ValidationAttribute obj) => obj.ToString().GetHashCode();
        }

    }
}
