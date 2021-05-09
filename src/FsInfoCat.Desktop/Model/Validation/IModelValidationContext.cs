using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Desktop.Model.Validation
{
    /// <summary>
    /// Provides contextual validation information for an object
    /// </summary>
    /// <seealso cref="IModelContext" />
    /// <seealso cref="INotifyDataErrorInfo" />
    /// <seealso cref="IValidatableObject" />
    public interface IModelValidationContext : IModelContext
    {
        /// <summary>
        /// Occurs when error messages have changed.
        /// </summary>
        event EventHandler<ModelErrorsChangedEventArgs> ModelErrorsChanged;

        /// <summary>
        /// Gets the context objects that represent the properties of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="IPropertyValidationContext"/> objects that represent the properties of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        new IReadOnlyList<IPropertyValidationContext> Properties { get; }

        /// <summary>
        /// Gets validation results for all properties of the underlying <see cref="ITypeDescriptorContext.Instance"/>.
        /// </summary>
        /// <returns>Zero or more ValidationResult objects that contain validation failure information.</returns>
        /// <remarks>This will return the results of the last validation for each property unless the property value has changed since the last validation
        /// or the value has never been validated. If a property validation occurs because of this invocation, a new <see cref="ValidationContext"/>
        /// will be created with default values for this context.</remarks>
        IEnumerable<ValidationResult> Validate();

        /// <summary>
        /// Gets validation results for all properties of the underlying <see cref="ITypeDescriptorContext.Instance"/>, re-evaluating each property using
        /// a <see cref="ValidationContext"/> with default values for this context.
        /// </summary>
        /// <returns>Zero or more ValidationResult objects that contain validation failure information.</returns>
        IEnumerable<ValidationResult> Revalidate();
    }
}
