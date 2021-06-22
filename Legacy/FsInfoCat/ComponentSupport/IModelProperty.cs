using System;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Describes a property of a model object type.
    /// </summary>
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
        /// Gets the type of this property.
        /// </summary>
        /// <value>
        /// The type of this property.
        /// </value>
        Type PropertyType { get; }

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
        /// <see langword="true" /> if <see cref="GetStandardValues" /> should be called to find a common set of values the property supports; otherwise, <see langword="false" />.
        /// </value>
        bool AreStandardValuesSupported { get; }

        /// <summary>
        /// Describes the model type that owns this property.
        /// </summary>
        IModelDescriptor Owner { get; }
    }

    /// <summary>
    /// Describes a property of a model object type.
    /// </summary>
    /// <typeparam name="TModel">The type of model that owns the property.</typeparam>
    public interface IModelProperty<TModel> : IModelProperty
        where TModel : class
    {
        /// <summary>
        /// Describes the model type that owns this property.
        /// </summary>
        new IModelDescriptor<TModel> Owner { get; }
    }

    /// <summary>
    /// Describes a property of a model object type.
    /// </summary>
    /// <typeparam name="TModel">The type of model that owns the property.</typeparam>
    /// <typeparam name="TValue">The type of the property's value.</typeparam>
    public interface IModelProperty<TModel, TValue> : ITypedModelProperty<TValue>, IModelProperty<TModel>
        where TModel : class
    {
    }
}
