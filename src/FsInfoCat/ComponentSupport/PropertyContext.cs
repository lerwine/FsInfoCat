using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FsInfoCat.ComponentSupport
{
    internal sealed class PropertyContext<TModel, TValue> : IModelPropertyContext<TModel, TValue>, ITypeDescriptorContext where TModel : class
    {
        private IEqualityComparer<TValue> _comparer;
        private readonly TypeConverter _converter;
        private TValue _previousValue;
        private readonly PropertyDescriptor _propertyDescriptor;
        private readonly ValidationResultCollection _validationResults = new ValidationResultCollection();
        private readonly ValidationMessageCollection _errorMessages;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // TODO: Change this to items that contain the actual value as well as a string representation.
        public ReadOnlyObservableCollection<TValue> StandardValues { get; }

        IReadOnlyList<TValue> ITypedModelPropertyContext<TValue>.StandardValues => StandardValues;

        ICollection IModelPropertyContext.StandardValues => StandardValues;

        internal ModelPropertyDescriptor<TModel, TValue> Descriptor { get; }

        IModelPropertyDescriptor<TModel, TValue> IModelPropertyContext<TModel, TValue>.Descriptor => Descriptor;

        ITypedModelPropertyDescriptor<TValue> ITypedModelPropertyContext<TValue>.Descriptor => Descriptor;

        IModelPropertyDescriptor<TModel> IModelPropertyContext<TModel>.Descriptor => Descriptor;

        IModelPropertyDescriptor IModelPropertyContext.Descriptor => Descriptor;

        public TValue Value
        {
            get
            {
                TValue currentValue = (TValue)_propertyDescriptor.GetValue(Owner.Instance);
                if (!_comparer.Equals(currentValue, _previousValue))
                {
                    _previousValue = currentValue;
                    RaiseValueChanged();
                }
                return currentValue;
            }

            set
            {
                if (_comparer.Equals(value, _previousValue))
                    _previousValue = value;
                else
                {
                    _previousValue = value;
                    RaiseValueChanged();
                }
            }
        }

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

        public bool HasErrors { get; private set; }

        IContainer ITypeDescriptorContext.Container => null;

        object ITypeDescriptorContext.Instance => Owner.Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => _propertyDescriptor;

        internal PropertyContext(ModelContext<TModel> owner, ModelPropertyDescriptor<TModel, TValue> descriptor)
        {
            Owner = owner;
            Descriptor = descriptor;
            _comparer = descriptor.Comparer ?? EqualityComparer<TValue>.Default;
            _converter = (_propertyDescriptor = descriptor.Descriptor).Converter;
            _previousValue = (TValue)_propertyDescriptor.GetValue(Owner.Instance);
            _errorMessages = new ValidationMessageCollection(_validationResults);
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
            HasErrors = _validationResults.Count > 0;
        }

        private void RaiseValueChanged()
        {
            RaisePropertyChanged(nameof(Value));
        }

        private void RaisePropertyChanged(string propertyName)
        {
            throw new NotImplementedException();
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
