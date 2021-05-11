using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Parameters for creating an instance of <see cref="IModelProperty{TModel}"/>.
    /// </summary>
    /// <typeparam name="TOwner">The type of the object that owns the target property.</typeparam>
    public interface IPropertyBuilder<TOwner>
        where TOwner : class
    {
        /// <summary>
        /// If <see langword="true"/>, do not include property in the result <see cref="IModelDescriptor{TModel}.Properties"/> collection; otherwise, <see langword="false"/> to include it.
        /// </summary>
        bool IgnoreProperty { get; set; }

        /// <summary>
        /// Use culture-invariant conversion when converting to and from string representations.
        /// </summary>
        bool UseInvariantStringConversion { get; set; }

        /// <summary>
        /// The <see cref="PropertyDescriptor"/> for the target property.
        /// </summary>
        PropertyDescriptor Descriptor { get; }

        /// <summary>
        /// The <see cref="IModelDescriptor{TModel}"/> that represents the model type of the target property.
        /// </summary>
        IModelDescriptor<TOwner> Owner { get; }

        /// <summary>
        /// Validation attributes to be used with property validation.
        /// </summary>
        /// This will be initially populated with the validation attributes that were declared with the property.
        IList<ValidationAttribute>  ValidationAttributes { get; }
    }

    /// <summary>
    /// Parameters for creating an instance of <see cref="IModelProperty{TModel, TValue}"/>.
    /// </summary>
    /// <typeparam name="TOwner">The type of the object that owns the target property.</typeparam>
    /// <typeparam name="TValue">The type of the property's value.</typeparam>
    public interface IPropertyBuilder<TOwner, TValue> : IPropertyBuilder<TOwner>
        where TOwner : class
    {
        /// <summary>
        /// The equality comparer to use for comparing property values or <see langword="null"/> to use the <see cref="EqualityComparer{T}.Default"/> comparer.
        /// </summary>
        IEqualityComparer<TValue> Comparer { get; set; }
    }
}
