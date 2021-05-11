using System;
using System.Collections.Generic;

namespace FsInfoCat.ComponentSupport
{
    public interface IModelDescriptor
    {
        Type ModelType { get; }

        IReadOnlyList<IModelProperty> Properties { get; }
    }

    public interface IModelDescriptor<TModel> : IModelDescriptor
        where TModel : class
    {
        new IReadOnlyList<IModelProperty<TModel>> Properties { get; }
    }
}
