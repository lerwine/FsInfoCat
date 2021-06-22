using System.Collections.Generic;

namespace FsInfoCat.ComponentSupport
{
    public interface ITypedModelPropertyContext<TValue> : ITypedModelProperty<TValue>, IModelPropertyContext
    {
        new event ValueChangedEventHandler<TValue> ValueChanged;

        new TValue RawValue { get; set; }

        new IReadOnlyList<IDisplayValue<TValue>> StandardValues { get; }

        new ITypedModelPropertyDescriptor<TValue> Descriptor { get; }
    }
}
