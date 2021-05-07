using System.Collections;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Generic representation of the characteristics of a property on a model class.
    /// </summary>
    /// <remarks>This is functionally similar to some aspects of the <see cref="System.ComponentModel.PropertyDescriptor"/>.</remarks>
    public interface IModelProperty
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the description of the property.
        /// </summary>
        /// <value>
        /// The description of the property, as specified in the <see cref="System.ComponentModel.DescriptionAttribute"/>.
        /// If there is no <see cref="System.ComponentModel.DescriptionAttribute"/>, an empty string (<code>&quot;&quot;</code>) is returned.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the name of the category to which the property belongs.
        /// </summary>
        /// <value>
        /// The name of the category to which the property belongs, as specified in the <see cref="System.ComponentModel.CategoryAttribute"/>.
        /// If there is no <see cref="System.ComponentModel.CategoryAttribute"/> or it is null or empty,
        /// the <see cref="System.ComponentModel.CategoryAttribute.Default">default category</see>, <code>&quot;Misc&quot;</code>, is returned.
        /// </value>
        string Category { get; }

        /// <summary>
        /// Gets the name that can be displayed in the user interface.
        /// </summary>
        /// <value>
        /// The name to display for the property, as specified in the <see cref="System.ComponentModel.DisplayNameAttribute"/>.
        /// If there is no <see cref="System.ComponentModel.DisplayNameAttribute"/> or it is null or empty, the property name is returned.
        /// </value>
        string DisplayName { get; }

        /// <summary>
        /// Gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the property is read-only; otherwise, <see langword="false"/>.
        /// </value>
        bool IsReadOnly { get; }

        /// <summary>
        /// Indicates whether the collection of standard values returned from <see cref="GetStandardValues" /> is an exclusive list.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if the <see cref="TypeConverter.StandardValuesCollection" /> returned from <see cref="GetStandardValues" /> is an exhaustive list of possible values;
        /// <see langword="false" /> if other values are possible.
        /// </value>
        bool AreStandardValuesExclusive { get; }

        /// <summary>
        /// Indicates whether this property's type supports a standard set of values that can be picked from a list.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if <see cref="GetStandardValues" /> should be called to find a common set of values the property supports; otherwise, <see langword="false" />.
        /// </value>
        bool AreStandardValuesSupported { get; }

        /// <summary>
        /// Returns a collection of standard values from the default context for the property.
        /// </summary>
        /// <returns>
        /// A <see cref="ICollection" /> containing a standard set of valid values, or <see langword="null" /> if the property does not support a standard
        /// set of values.
        /// </returns>
        ICollection GetStandardValues();
    }
}
