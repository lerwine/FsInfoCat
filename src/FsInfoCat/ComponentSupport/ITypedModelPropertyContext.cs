using System.Collections.Generic;

namespace FsInfoCat.ComponentSupport
{
    public interface ITypedModelPropertyContext<TValue> : ITypedModelProperty<TValue>, IModelPropertyContext
    {
        new TValue Value { get; set; }

        new IReadOnlyList<TValue> StandardValues { get; }

        new ITypedModelPropertyDescriptor<TValue> Descriptor { get; }
    }
}
