using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace FsInfoCat.ComponentSupport
{
    internal sealed class PropertyContext<TModel, TValue> : IModelPropertyContext<TModel, TValue>, ITypeDescriptorContext where TModel : class
    {
        private const string ErrorMessage_Invalid_Format = "Invalid Format";
        private readonly object _syncRoot = new object();
        private IEqualityComparer<TValue> _comparer;
        private readonly TypeConverter _converter;
        private string _textValue;
        private bool _hasConvertFromStringError;
        private TValue _previousValue;
        private readonly PropertyDescriptor _propertyDescriptor;
        private readonly ValidationResultCollection _validationResults = new ValidationResultCollection();
        private readonly ValidationMessageCollection _errorMessages;
        private event EventHandler _valueChanged;

        public event ValueChangedEventHandler<TValue> ValueChanged;
        event EventHandler IModelPropertyContext.ValueChanged
        {
            add => _valueChanged += value;
            remove => _valueChanged -= value;
        }

        public event ValueChangedEventHandler<bool> HasErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ReadOnlyObservableCollection<DisplayValue<TValue>> StandardValues { get; }

        IReadOnlyList<IDisplayValue<TValue>> ITypedModelPropertyContext<TValue>.StandardValues => StandardValues;

        IReadOnlyList<IDisplayValue> IModelPropertyContext.StandardValues => StandardValues;

        internal ModelPropertyDescriptor<TModel, TValue> Descriptor { get; }

        IModelPropertyDescriptor<TModel, TValue> IModelPropertyContext<TModel, TValue>.Descriptor => Descriptor;

        ITypedModelPropertyDescriptor<TValue> ITypedModelPropertyContext<TValue>.Descriptor => Descriptor;

        IModelPropertyDescriptor<TModel> IModelPropertyContext<TModel>.Descriptor => Descriptor;

        IModelPropertyDescriptor IModelPropertyContext.Descriptor => Descriptor;

        public string TextValue
        {
            get
            {
                CheckUpdateValue((TValue)_propertyDescriptor.GetValue(Owner.Instance));
                return _textValue;
            }
            set
            {
                string textValue = value ?? "";
                bool raiseValueChanged, hadConversionError;
                Monitor.Enter(_syncRoot);
                TValue newValue;
                TValue oldValue = newValue = _previousValue;
                try
                {
                    if (textValue.Equals(_textValue))
                        return;
                    hadConversionError = _hasConvertFromStringError;
                    _textValue = textValue;
                    try
                    {
                        newValue = (TValue)((_useInvariantStringConversion) ? _converter.ConvertFromInvariantString(this, textValue) : _converter.ConvertFromString(this, textValue));
                        raiseValueChanged = !_comparer.Equals(newValue, _previousValue);
                        _previousValue = newValue;
                        _hasConvertFromStringError = false;

                    }
                    catch
                    {
                        newValue = _previousValue;
                        raiseValueChanged = false;
                        _hasConvertFromStringError = true;
                    }
                }
                finally { Monitor.Exit(_syncRoot); }

                try
                {
                    if (raiseValueChanged)
                        RaiseValueChanged(oldValue, newValue);
                    else if (_hasConvertFromStringError)
                    {
                        if (!hadConversionError)
                            _validationResults.SetSingle(new ValidationResult(ErrorMessage_Invalid_Format));
                    }
                    else if (hadConversionError)
                        _validationResults.Clear();
                }
                finally { RaisePropertyChanged(nameof(TextValue)); }
            }
        }

        public TValue RawValue
        {
            get
            {
                TValue value = (TValue)_propertyDescriptor.GetValue(Owner.Instance);
                CheckUpdateValue(value);
                return value;
            }
            set => CheckUpdateValue(value);
        }

        object IModelPropertyContext.RawValue => RawValue;

        private readonly bool _useInvariantStringConversion;

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

        ReadOnlyObservableCollection<IDisplayValue<TValue>> IModelPropertyContext<TModel, TValue>.StandardValues => throw new NotImplementedException();

        internal PropertyContext(ModelContext<TModel> owner, ModelPropertyDescriptor<TModel, TValue> descriptor)
        {
            _useInvariantStringConversion = descriptor.UseInvariantStringConversion;
            Owner = owner;
            Descriptor = descriptor;
            _comparer = descriptor.Comparer ?? EqualityComparer<TValue>.Default;
            _converter = (_propertyDescriptor = descriptor.Descriptor).Converter;
            _previousValue = (TValue)_propertyDescriptor.GetValue(Owner.Instance);
            _errorMessages = new ValidationMessageCollection(_validationResults);
            ObservableCollection<DisplayValue<TValue>> standardValues = new ObservableCollection<DisplayValue<TValue>>();
            if (_converter.GetStandardValuesSupported(this))
            {
                TypeConverter.StandardValuesCollection svc = _converter.GetStandardValues(this);
                if (!(svc is null))
                {
                    if (_useInvariantStringConversion)
                        foreach (object obj in svc)
                        {
                            if (obj is TValue value)
                                standardValues.Add(new DisplayValue<TValue>(value, _converter.ConvertToInvariantString(this, value)));
                        }
                    else
                        foreach (object obj in svc)
                        {
                            if (obj is TValue value)
                                standardValues.Add(new DisplayValue<TValue>(value, _converter.ConvertToString(this, value)));
                        }
                }
            }
            StandardValues = new ReadOnlyObservableCollection<DisplayValue<TValue>>(standardValues);
            Validate((TValue)_propertyDescriptor.GetValue(Owner.Instance));
            HasErrors = _validationResults.Count > 0;
            _validationResults.CollectionChanged += ValidationResults_CollectionChanged;
        }

        public ValidationResult[] Validate()
        {
            CheckUpdateValue((TValue)_propertyDescriptor.GetValue(Owner.Instance));
            return _validationResults.Cast<ValidationResult>().ToArray();
        }

        public ValidationResult[] Revalidate()
        {
            TValue value = (TValue)_propertyDescriptor.GetValue(Owner.Instance);
            if (!CheckUpdateValue(value))
                Validate(value);
            return _validationResults.Cast<ValidationResult>().ToArray();
        }

        private bool CheckUpdateValue(TValue newValue)
        {
            bool valueChanged;
            bool textChanged;
            TValue oldValue;
            bool hadConversionError;
            Monitor.Enter(_syncRoot);
            try
            {
                hadConversionError = _hasConvertFromStringError;
                _hasConvertFromStringError = false;
                oldValue = _previousValue;
                valueChanged = !_comparer.Equals(newValue, oldValue);
                _previousValue = newValue;
                if (valueChanged)
                {
                    string textValue;
                    try { textValue = _useInvariantStringConversion ? _converter.ConvertToInvariantString(this, newValue) : _converter.ConvertToString(this, newValue); }
                    catch
                    {
                        try { textValue = newValue.ToString(); } catch { textValue = ""; }
                    }
                    textChanged = !textValue.Equals(_textValue);
                    _textValue = textValue;
                }
                else
                    textChanged = false;
            }
            finally { Monitor.Exit(_syncRoot); }
            try
            {
                if (valueChanged)
                    RaiseValueChanged(oldValue, newValue);
                else if (hadConversionError)
                    _validationResults.Clear();
            }
            finally
            {
                if (textChanged)
                    RaisePropertyChanged(nameof(TextValue));
            }
            return valueChanged;
        }

        private void RaiseValueChanged(TValue oldValue, TValue newValue)
        {
            try
            {
                try { RaisePropertyChanged(nameof(RawValue)); }
                finally
                {
                    ValueChangedEventArgs<TValue> args = new ValueChangedEventArgs<TValue>(oldValue, newValue);
                    ValueChanged?.Invoke(this, args);
                    _valueChanged?.Invoke(this, args);
                }
            }
            finally { Validate(newValue); }
        }

        private void Validate(TValue value)
        {
            if (Descriptor.ValidationAttributes.Count == 0)
            {
                if (_validationResults.Count > 0)
                    _validationResults.Clear();
            }
            else
            {
                Collection<ValidationResult> validationResults = new Collection<ValidationResult>();
                Validator.TryValidateValue(value, new ValidationContext(this), validationResults, Descriptor.ValidationAttributes);
                _validationResults.SetItems(validationResults);
            }
        }

        private void ValidationResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool hasErrorsChanged;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    hasErrorsChanged = !HasErrors;
                    if (hasErrorsChanged)
                        HasErrors = true;
                    break;
                case NotifyCollectionChangedAction.Remove:
                    hasErrorsChanged = _validationResults.Count == 0;
                    if (hasErrorsChanged)
                        HasErrors = false;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (_validationResults.Count > 0)
                    {
                        hasErrorsChanged = !HasErrors;
                        if (hasErrorsChanged)
                            HasErrors = true;
                    }
                    else
                    {
                        hasErrorsChanged = HasErrors;
                        if (hasErrorsChanged)
                            HasErrors = false;
                    }
                    break;
                default:
                    hasErrorsChanged = false;
                    break;
            }
            try
            {
                EventHandler<DataErrorsChangedEventArgs> errorsChanged = ErrorsChanged;
                if (!(errorsChanged is null))
                    try { errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(nameof(RawValue))); }
                    finally { errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(nameof(TextValue))); }
            }
            finally
            {
                if (hasErrorsChanged)
                    RaiseHasErrorsChanged(HasErrors);
            }
        }

        private void RaiseHasErrorsChanged(bool hasErrors)
        {
            try { RaisePropertyChanged(nameof(HasErrors)); }
            finally { HasErrorsChanged?.Invoke(this, new ValueChangedEventArgs<bool>(!hasErrors, hasErrors)); }
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName is null)
                return null;
            switch (propertyName)
            {
                case nameof(TextValue):
                case nameof(RawValue):
                    return _errorMessages;
            }
            return null;
        }

        void ITypeDescriptorContext.OnComponentChanged() { }

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        object IServiceProvider.GetService(Type serviceType) => null;
    }
}
