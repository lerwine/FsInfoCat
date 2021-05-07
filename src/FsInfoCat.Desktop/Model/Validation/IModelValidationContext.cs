using FsInfoCat.Desktop.Model.ComponentSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IModelValidationContext : IModelContext, INotifyDataErrorInfo, IValidatableObject
    {
        event EventHandler<ModelErrorsChangedEventArgs> ModelErrorsChanged;

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
