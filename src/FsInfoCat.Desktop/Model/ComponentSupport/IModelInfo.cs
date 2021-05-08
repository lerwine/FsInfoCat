using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Provides information about a compont and its properties.
    /// </summary>
    public interface IModelInfo
    {
        /// <summary>
        /// Gets the simple name for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="RuntimeType.Name"/> for the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        string SimpleName { get; }

        /// <summary>
        /// Gets the full name for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="Type.FullName"/> for the type of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        string FullName { get; }

        /// <summary>
        /// Gets information about properties of the target object.
        /// </summary>
        /// <value>
        /// Information about properties of the target object.
        /// </value>
        IReadOnlyList<IModelProperty> Properties { get; }
    }
}
