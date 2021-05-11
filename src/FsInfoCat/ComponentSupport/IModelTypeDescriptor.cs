using System.Collections.Generic;

namespace FsInfoCat.ComponentSupport
{
    public interface IModelTypeDescriptor : IModelDescriptor
    {
        new IReadOnlyList<IModelPropertyDescriptor> Properties { get; }

        IModelContext CreateContext(object model);
    }

    public interface IModelTypeDescriptor<TModel> : IModelDescriptor<TModel>, IModelTypeDescriptor
        where TModel : class
    {
        new IReadOnlyList<IModelPropertyDescriptor<TModel>> Properties { get; }

        IModelContext<TModel> CreateContext(TModel model);
    }
}
