using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    /// <summary>
    /// Provides contextual information about a property on an instantiated model object as well as generic access to its properties.
    /// </summary>
    /// <remarks>This implements the <see cref="ITypeDescriptorContext"/> interface whereby
    /// the <see cref="ITypeDescriptorContext.PropertyDescriptor"/> is <see langword="null"/>.</remarks>
    public interface IModelContext : ITypeDescriptorContext, IModelInfo
    {
        /// <summary>
        /// Occurs when the value of a property on the underlying model <see cref="ITypeDescriptorContext.Instance"/> has changed.
        /// </summary>
        event PropertyValueChangedEventHandler PropertyValueChanged;

        /// <summary>
        /// Gets the context objects that represent the properties of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="IPropertyContext"/> objects that represent the properties of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        new IReadOnlyList<IPropertyContext> Properties { get; }

        /// <summary>
        /// Gets the descriptor for the type of the underlying model instance.
        /// </summary>
        /// <value>
        /// The <see cref="IModelDescriptor"/> that describes the of the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </value>
        IModelDescriptor ModelDescriptor { get; }

        /// <summary>
        /// Returns the hash code for the underlying model <see cref="ITypeDescriptorContext.Instance"/>.
        /// </summary>
        /// <returns>
        /// The hash code for underlying model <see cref="ITypeDescriptorContext.Instance"/>, suitable for use in hashing algorithms and data structures like
        /// a hash table. 
        /// </returns>
        int GetHashCode();
    }

    public interface IModelContext<TInstance> : IModelContext
        where TInstance : class
    {
        new TInstance Instance { get; }

        new ModelDescriptor<TInstance> ModelDescriptor { get; }

        new IReadOnlyList<IPropertyContext<TInstance>> Properties { get; }
    }
}
