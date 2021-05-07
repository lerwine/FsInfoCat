using System;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.Validation
{
    /// <summary>
    /// Provides data for the <see cref="IModelValidationContext.ModelErrorsChanged"/> event.
    /// </summary>
    public class ModelErrorsChangedEventArgs : DataErrorsChangedEventArgs
    {
        /// <summary>
        /// Gets the context of the property that where the error state changed.
        /// </summary>
        /// <value>
        /// The <see cref="IPropertyValidationContext"/> object that gives contextual information of the property's validation state or <see langword="null"/> if the
        /// error is at the <see cref="ModelContext"/> level.
        /// </value>
        public IPropertyValidationContext PropertyContext { get; }

        /// <summary>
        /// Gets the context of the model instance upon which the error state changed.
        /// </summary>
        /// <value>
        /// The <see cref="IModelValidationContext"/> object that gives contextual information of the object's overall validation state.
        /// </value>
        public IModelValidationContext ModelContext { get; }

        /// <summary>
        /// Initializes a new <see cref="IModelValidationContext.ModelErrorsChangedEventArgs"/> object for a property-level error state change.
        /// </summary>
        /// <param name="context">The <see cref="IModelValidationContext"/> object that gives contextual information of the property's validation state.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        public ModelErrorsChangedEventArgs(IPropertyValidationContext context) : base((context ?? throw new ArgumentNullException(nameof(context))).Name)
        {
            PropertyContext = context;
            ModelContext = context.Owner;
        }

        /// <summary>
        /// Initializes a new <see cref="IModelValidationContext.ModelErrorsChangedEventArgs"/> object for a object-level error state change.
        /// </summary>
        /// <param name="context">The <see cref="IModelValidationContext"/> object that gives contextual information of the object's overall validation state.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        public ModelErrorsChangedEventArgs(IModelValidationContext context) : base(null)
        {
            PropertyContext = null;
            ModelContext = context;
        }
    }
}
