using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Describes a property of a model object type, not yet associated with an instance object.
    /// </summary>
    public interface IModelPropertyDescriptor : IModelProperty
    {
        /// <summary>
        /// Gets a value indicating whether value change notifications for this property may originate from outside the property descriptor.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if value change notifications may originate from outside the property descriptor; otherwise, <see langword="false"/>.
        /// </value>
        bool SupportsChangeEvents { get; }

        /// <summary>
        /// Use culture-invariant conversion when converting to and from string representations.
        /// </summary>
        bool UseInvariantStringConversion { get; }

        /// <summary>
        /// Gets the attributes for property validation.
        /// </summary>
        /// <value>
        /// The <see cref="ValidationAttribute"/> objects used for property validation.
        /// </value>
        IReadOnlyList<ValidationAttribute> ValidationAttributes { get; }

        /// <summary>
        /// Describes the model type that owns this property.
        /// </summary>
        new IModelTypeDescriptor Owner { get; }

        /// <summary>
        /// Gets the property value from a model object.
        /// </summary>
        /// <param name="model">The instantiated model object.</param>
        /// <returns>he property value from the <paramref name="model"/> object.</returns>
        object GetValue(object model);
    }

    /// <summary>
    /// Describes a property of a model object type, not yet associated with an instance object.
    /// </summary>
    /// <typeparam name="TModel">The model object type.</typeparam>
    public interface IModelPropertyDescriptor<TModel> : IModelProperty<TModel>, IModelPropertyDescriptor where TModel : class
    {
        /// <summary>
        /// Describes the model type that owns this property.
        /// </summary>
        new IModelTypeDescriptor<TModel> Owner { get; }

        /// <summary>
        /// Gets the property value from a model object.
        /// </summary>
        /// <param name="model">The instantiated model object.</param>
        /// <returns>he property value from the <paramref name="model"/> object.</returns>
        object GetValue(TModel model);
    }

    /// <summary>
    /// Describes a property of a model object type, not yet associated with an instance object.
    /// </summary>
    /// <typeparam name="TModel">The model object type.</typeparam>
    /// <typeparam name="TValue">The property value type.</typeparam>
    public interface IModelPropertyDescriptor<TModel, TValue> : IModelProperty<TModel, TValue>, ITypedModelPropertyDescriptor<TValue>, IModelPropertyDescriptor<TModel>
        where TModel : class
    {
        /// <summary>
        /// Gets the property value from a model object.
        /// </summary>
        /// <param name="model">The instantiated model object.</param>
        /// <returns>he property value from the <paramref name="model"/> object.</returns>
        new TValue GetValue(TModel model);
    }
}
