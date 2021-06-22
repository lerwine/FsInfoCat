using System.Collections.Generic;

namespace FsInfoCat.ComponentSupport
{
    /// <summary>
    /// Describes a model type not yet associated with an instance object.
    /// </summary>
    public interface IModelTypeDescriptor : IModelDescriptor
    {
        /// <summary>
        /// The properties of the model type.
        /// </summary>
        new IReadOnlyList<IModelPropertyDescriptor> Properties { get; }

        /// <summary>
        /// Creates an <see cref="IModelContext"/> object that represents the model type in the context of an instance object.
        /// </summary>
        /// <param name="model">The instantiated model object.</param>
        /// <returns>The <see cref="IModelContext"/> that represents the model type in the context of an instance object.</returns>
        IModelContext CreateContext(object model);
    }

    /// <summary>
    /// Describes a model type not yet associated with an instance object.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public interface IModelTypeDescriptor<TModel> : IModelDescriptor<TModel>, IModelTypeDescriptor
        where TModel : class
    {
        /// <summary>
        /// The properties of the model type.
        /// </summary>
        new IReadOnlyList<IModelPropertyDescriptor<TModel>> Properties { get; }

        /// <summary>
        /// Creates an <see cref="IModelContext{TModel}"/> object that represents the model type in the context of an instance object.
        /// </summary>
        /// <param name="model">The instantiated model object.</param>
        /// <returns>The <see cref="IModelContext{TModel}"/> that represents the model type in the context of an instance object.</returns>
        IModelContext<TModel> CreateContext(TModel model);
    }
}
