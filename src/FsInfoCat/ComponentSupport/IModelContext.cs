using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Describes a model from the context of an instantiated object.
    /// </summary>
    public interface IModelContext : IModelDescriptor
    {
        /// <summary>
        /// The instantiated model object.
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// The <see cref="IModelTypeDescriptor"/> that was used to create this context object.
        /// </summary>
        IModelTypeDescriptor Descriptor { get; }

        /// <summary>
        /// Represents the properties of the current model object <see cref="Instance"/>.
        /// </summary>
        new IReadOnlyList<IModelPropertyContext> Properties { get; }
    }

    /// <summary>
    /// Describes a model from the context of an instantiated object.
    /// </summary>
    /// <typeparam name="TModel">The instantiated model type.</typeparam>
    public interface IModelContext<TModel> : IModelDescriptor<TModel>, IModelContext
        where TModel : class
    {
        /// <summary>
        /// The instantiated model object.
        /// </summary>
        new TModel Instance { get; }

        /// <summary>
        /// The <see cref="IModelTypeDescriptor{TModel}"/> that was used to create this context object.
        /// </summary>
        new IModelTypeDescriptor<TModel> Descriptor { get; }

        /// <summary>
        /// Represents the properties of the current model object <see cref="Instance"/>.
        /// </summary>
        new ReadOnlyObservableCollection<IModelPropertyContext<TModel>> Properties { get; }
    }
}
