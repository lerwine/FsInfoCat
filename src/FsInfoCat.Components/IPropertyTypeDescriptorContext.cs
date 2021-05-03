using System.ComponentModel;

namespace FsInfoCat.Components
{
    public interface IPropertyTypeDescriptorContext : ITypeDescriptorContext
    {
        IModelInstance ComponentContext { get; }
    }

    public interface IPropertyTypeDescriptorContext<TInstance> : IPropertyTypeDescriptorContext
        where TInstance : class
    {
        new TInstance Instance { get; }
        new ModelInstance<TInstance> ComponentContext { get; }
    }
}
