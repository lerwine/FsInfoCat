using System.ComponentModel;

namespace FsInfoCat.Components
{
    public interface IPropertyDescriptorContext : ITypeDescriptorContext
    {
        ITypeInstanceModel ComponentContext { get; }
    }

    public interface IPropertyDescriptorContext<TInstance> : IPropertyDescriptorContext
        where TInstance : class
    {
        new TInstance Instance { get; }
        new TypeInstanceModel<TInstance> ComponentContext { get; }
    }
}
