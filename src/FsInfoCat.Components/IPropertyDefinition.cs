using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Components
{
    /// <summary>
    /// Exposes select, normalized properties of a <seealso cref="PropertyDescriptor"/>.
    /// </summary>
    public interface IPropertyDefinition : IPropertyModel
    {
        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="GetStandardValues"/> is an exclusive list of possible values.
        /// </summary>
        bool AreStandardValuesExclusive { get; }

        /// <summary>
        /// Indicates whether this object supports a <see cref="GetStandardValues">standard set of values</see> that can be picked from a list.
        /// </summary>
        bool HasStandardValues { get; }

        /// <summary>
        /// Indicates whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        bool SupportsChangeEvents { get; }

        /// <summary>
        /// Attributes used to validate the property value.
        /// </summary>
        ReadOnlyCollection<ValidationAttribute> ValidationAttributes { get; }

        PropertyDescriptor GetDescriptor();

        /// <summary>
        /// Converts the given value to the type of this property.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFrom(object value);

        /// <summary>
        /// Converts the given string to the type of this property, using the invariant culture.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromInvariantString(string text);

        /// <summary>
        /// Converts the given string to the type of this property.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to convert.</param>
        /// <returns>An object with the same type as this property.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        object ConvertFromString(string text);

        /// <summary>
        /// Converts the specified value to a culture-invariant string representation.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string ConvertToInvariantString(object value);

        /// <summary>
        /// Converts the given value to a string representation.
        /// </summary>
        /// <param name="value">The property value to convert.</param>
        /// <returns>A <see cref="string"/> that represents the converted value.</returns>
        /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
        string ConvertToString(object value);

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for.
        /// </summary>
        /// <returns>A <see cref="TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or <see langword="null"/> if the property does not support a standard set of values.</returns>
        TypeConverter.StandardValuesCollection GetStandardValues();

        /// <summary>
        /// Indicates whether the given value's type can be assigned as the property's type or if it can be converted to the property type.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> can be directly assigned or converted and then assigned to the current property type.</returns>
        bool IsCompatibleType(object value);

        IInstanceProperty ToInstanceProperty(IModelInstance componentContext);

        IModelDefinition ComponentDefinition { get; }
    }

    public interface IPropertyDefinition<TComponent> : IPropertyDefinition
        where TComponent : class
    {
        IInstanceProperty<TComponent> ToInstanceProperty(ModelInstance<TComponent> componentContext);

        new ModelDefinition<TComponent> ComponentDefinition { get; }
    }

}
