using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FsInfoCat.ComponentSupport
{
    internal sealed class PropertyContext<TModel, TValue> : IModelPropertyContext<TModel, TValue>, ITypeDescriptorContext where TModel : class
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ReadOnlyObservableCollection<TValue> StandardValues { get; }

        IReadOnlyList<TValue> ITypedModelPropertyContext<TValue>.StandardValues => StandardValues;

        ICollection IModelPropertyContext.StandardValues => StandardValues;

        internal ModelPropertyDescriptor<TModel, TValue> Descriptor { get; }

        private readonly TypeConverter _converter;
        private readonly PropertyDescriptor _propertyDescriptor;

        IModelPropertyDescriptor<TModel, TValue> IModelPropertyContext<TModel, TValue>.Descriptor => Descriptor;

        ITypedModelPropertyDescriptor<TValue> ITypedModelPropertyContext<TValue>.Descriptor => Descriptor;

        IModelPropertyDescriptor<TModel> IModelPropertyContext<TModel>.Descriptor => Descriptor;

        IModelPropertyDescriptor IModelPropertyContext.Descriptor => Descriptor;

        public TValue Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        object IModelPropertyContext.Value => throw new NotImplementedException();

        internal ModelContext<TModel> Owner { get; }

        IModelContext<TModel> IModelPropertyContext<TModel>.Owner => Owner;

        IModelDescriptor<TModel> IModelProperty<TModel>.Owner => Owner;

        IModelContext IModelPropertyContext.Owner => Owner;

        IModelDescriptor IModelProperty.Owner => Owner;

        public string Name => Descriptor.Name;

        public Type PropertyType => Descriptor.PropertyType;

        public string Description => Descriptor.Description;

        public string Category => Descriptor.Category;

        public string DisplayName => Descriptor.DisplayName;

        public bool IsReadOnly => Descriptor.IsReadOnly;

        public bool AreStandardValuesExclusive => _converter.GetStandardValuesExclusive(this);

        public bool AreStandardValuesSupported => _converter.GetStandardValuesSupported(this);

        public bool HasErrors => throw new NotImplementedException();

        IContainer ITypeDescriptorContext.Container => null;

        object ITypeDescriptorContext.Instance => Owner.Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => _propertyDescriptor;

        internal PropertyContext(ModelContext<TModel> owner, ModelPropertyDescriptor<TModel, TValue> descriptor)
        {
            Owner = owner;
            Descriptor = descriptor;
            _converter = (_propertyDescriptor = descriptor.Descriptor).Converter;
            ObservableCollection<TValue> collection = new ObservableCollection<TValue>();
            if (_converter.GetStandardValuesSupported(this))
            {
                TypeConverter.StandardValuesCollection standardValues = _converter.GetStandardValues(this);
                if (!(standardValues is null))
                {
                    foreach (object obj in standardValues)
                    {
                        if (obj is TValue v)
                            collection.Add(v);
                    }
                }
            }
            StandardValues = new ReadOnlyObservableCollection<TValue>(collection);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        void ITypeDescriptorContext.OnComponentChanged() { }

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}
