using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat.ComponentSupport
{
    public interface IModelContext : IModelDescriptor
    {
        object Instance { get; }

        IModelTypeDescriptor Descriptor { get; }

        new IReadOnlyList<IModelPropertyContext> Properties { get; }
    }

    public interface IModelContext<TModel> : IModelDescriptor<TModel>, IModelContext
        where TModel : class
    {
        new TModel Instance { get; }

        new IModelTypeDescriptor<TModel> Descriptor { get; }

        new ReadOnlyObservableCollection<IModelPropertyContext<TModel>> Properties { get; }
    }
}
