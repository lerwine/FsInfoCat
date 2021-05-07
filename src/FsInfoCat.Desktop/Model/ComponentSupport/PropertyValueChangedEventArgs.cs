using System;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Provides data for the <see cref="IModelContext.PropertyValueChanged"/> event.
    /// </summary>
    public class PropertyValueChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Gets the context of the property that was changed.
        /// </summary>
        /// <value>
        /// The <see cref="IPropertyContext"/> object that gives contextual information of the property that was changed.
        /// </value>
        public IPropertyContext Context { get; }

        /// <summary>
        /// Initializes a new <see cref="PropertyValueChangedEventArgs"/> object.
        /// </summary>
        /// <param name="context">The <see cref="IPropertyContext"/> object that gives contextual information of the property that was changed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <see langword="null"/>.</exception>
        public PropertyValueChangedEventArgs(IPropertyContext context) : base((context ?? throw new ArgumentNullException(nameof(context))).Name)
        {
            Context = context;
        }
    }
}
