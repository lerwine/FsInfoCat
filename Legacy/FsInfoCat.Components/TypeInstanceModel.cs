using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FsInfoCat.Components
{
    public class TypeInstanceModel<TInstance> : ITypeInstanceModel
        where TInstance : class
    {
        public TInstance Instance { get; }

        object ITypeDescriptorContext.Instance => Instance;

        public ReadOnlyCollection<IPropertyInstanceModel<TInstance>> Properties { get; }

        IReadOnlyList<IPropertyInstanceModel> ITypeInstanceModel.Properties => Properties;

        public IContainer Container { get; }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        void ITypeDescriptorContext.OnComponentChanged() { }

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}
