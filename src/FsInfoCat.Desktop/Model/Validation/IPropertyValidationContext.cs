using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Desktop.Model.Validation
{
    /// <summary>
    /// Generic contextual validation information and access to a property on an instance of a model class, represented through a <see cref="IModelValidationContext"/>.
    /// </summary>
    public interface IPropertyValidationContext : IPropertyContext, IValidatableObject, INotifyDataErrorInfo
    {
        ///// <summary>
        ///// Occurs when the validation errors have changed for this property.
        ///// </summary>
        //event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Occurs when the <see cref="HasErrors"/> property has changed.
        /// </summary>
        event EventHandler HasErrorsChanged;

        ///// <summary>
        ///// Gets a value indicating whether this property has errors.
        ///// </summary>
        ///// <value>
        ///// <see langword="true"/> if this instance has errors; otherwise, <see langword="false"/>.
        ///// </value>
        //bool HasErrors { get; }

        /// <summary>
        /// Gets the model instance which contains this property.
        /// </summary>
        /// <value>
        /// The <see cref="IModelValidationContext"/> that represents the owner model instance.
        /// </value>
        new IModelValidationContext Owner { get; }

        ///// <summary>
        ///// Gets the validation errors for this property.
        ///// </summary>
        ///// <returns>The validation errors for this property.</returns>
        //IEnumerable<string> GetErrors();

        ValidationResult Validate();

        void Invalidate();

        ValidationResult Revalidate();
    }

    public interface IPropertyValidationContext<TInstance> : IPropertyContext<TInstance>, IPropertyValidationContext
        where TInstance : class
    {
        new ModelValidationContext<TInstance> Owner { get; }
    }
}
