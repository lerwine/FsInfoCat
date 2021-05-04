using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Components
{
    public class ModelInstance<TComponent> : IModelInstance
        where TComponent : class
    {
        public TComponent Instance { get; }

        object ITypeDescriptorContext.Instance => Instance;

        public ReadOnlyCollection<IInstanceProperty<TComponent>> Properties { get; }

        IReadOnlyList<IInstanceProperty> IModelInstance.Properties => Properties;

        public IContainer Container { get; }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        void ITypeDescriptorContext.OnComponentChanged() { }

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}
