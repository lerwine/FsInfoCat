using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    /// <summary>
    /// Provides data for a property value validation event.
    /// </summary>
    /// <typeparam name="T">The type of value being validated.</typeparam>
    /// <seealso cref="PropertyChangedEventArgs" />
    public class PropertyValidatingEventArgs<T> : PropertyChangedEventArgs
    {
        private string _validationMessage = "";

        /// <summary>
        /// Gets the value being validated.
        /// </summary>
        /// <value>
        /// The value being validated.
        /// </value>
        public T Value { get; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message which is <see cref="string.Empty"/> if the property is validated.
        /// </value>
        /// <remarks>Values that are <see langword="null"/> converted to <see cref="string.Empty"/>; otherwise extraneous whitespace will be trimmed.</remarks>
        public string ValidationMessage
        {
            get => _validationMessage;
            set => _validationMessage = value.AsNonNullTrimmed();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValidatingEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The value being validated.</param>
        /// <param name="propertyName">Name of the property being validated, which may be null if not applicable.</param>
        /// <param name="validationMessage">The optional initial validation message. If this parameter is not specified,
        /// the initial <see cref="ValidationMessage"/> will be <see cref="string.Empty"/>.</param>
        public PropertyValidatingEventArgs([AllowNull] T value, [AllowNull] string propertyName, string validationMessage = null) : base(propertyName)
        {
            Value = value;
            ValidationMessage = validationMessage;
        }
    }

    /// <summary>
    /// Provides data for a property value validation event.
    /// </summary>
    /// <typeparam name="TValue">The type of value being validated.</typeparam>
    /// <typeparam name="TContext">The type context information for the validation event.</typeparam>
    /// <seealso cref="PropertyChangedEventArgs" />
    public class PropertyValidatingEventArgs<TValue, TContext> : PropertyValidatingEventArgs<TValue>
    {
        /// <summary>
        /// Gets the contextual information for the validation event.
        /// </summary>
        /// <value>
        /// The value that represents contextual information for the validation event.
        /// </value>
        public TContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValidatingEventArgs{TValue, TContext}"/> class.
        /// </summary>
        /// <param name="value">The value being nvalidated.</param>
        /// <param name="propertyName">Name of the property being validated, which may be null if not applicable.</param>
        /// <param name="context">The value that represents contextual information for the validation event.</param>
        /// <param name="validationMessage">The optional initial validation message. If this parameter is not specified,
        /// the initial <see cref="ValidationMessage"/> will be <see cref="string.Empty"/>.</param>
        public PropertyValidatingEventArgs([AllowNull] TValue value, [AllowNull] string propertyName, [AllowNull] TContext context, string validationMessage = null)
            : base(value, propertyName, validationMessage)
        {
            Context = context;
        }
    }

}
