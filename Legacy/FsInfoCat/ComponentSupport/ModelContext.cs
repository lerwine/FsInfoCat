using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FsInfoCat.ComponentSupport
{
    internal class ModelContext<TModel> : IModelContext<TModel>, ITypeDescriptorContext where TModel : class
    {
        public TModel Instance { get; }

        object IModelContext.Instance => Instance;

        public Type ModelType { get; }

        public ReadOnlyObservableCollection<IModelPropertyContext<TModel>> Properties { get; }

        IReadOnlyList<IModelProperty<TModel>> IModelDescriptor<TModel>.Properties => Properties;

        IReadOnlyList<IModelPropertyContext> IModelContext.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelDescriptor.Properties => Properties;

        IContainer ITypeDescriptorContext.Container => null;

        object ITypeDescriptorContext.Instance => Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null;

        internal ModelDescriptor<TModel> Descriptor { get; }

        IModelTypeDescriptor<TModel> IModelContext<TModel>.Descriptor => Descriptor;

        IModelTypeDescriptor IModelContext.Descriptor => Descriptor;

        internal ModelContext(ModelDescriptor<TModel> descriptor, TModel instance)
        {
            Descriptor = descriptor;
            Instance = instance;
            ObservableCollection<IModelPropertyContext<TModel>> collection = new ObservableCollection<IModelPropertyContext<TModel>>();
            Properties = new ReadOnlyObservableCollection<IModelPropertyContext<TModel>>(collection);
            foreach (ModelPropertyDescriptor<TModel> property in descriptor.Properties)
                collection.Add(property.CreateContext(this));
        }

        void ITypeDescriptorContext.OnComponentChanged() { }

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}
