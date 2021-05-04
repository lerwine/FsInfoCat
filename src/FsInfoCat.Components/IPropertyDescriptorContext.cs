using System.ComponentModel;

namespace FsInfoCat.Components
{
    public interface IPropertyDescriptorContext : ITypeDescriptorContext
    {
        ITypeInstanceModel OwnerContext { get; }
    }

    public interface IPropertyDescriptorContext<TInstance> : IPropertyDescriptorContext
        where TInstance : class
    {
        new TInstance Instance { get; }
        new TypeInstanceModel<TInstance> OwnerContext { get; }
    }
}
