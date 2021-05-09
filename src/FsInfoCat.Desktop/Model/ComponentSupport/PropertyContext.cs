using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public abstract class PropertyContext<TInstance, TValue, TOwner> : ITypedPropertyContext<TValue>, IPropertyContext<TInstance>
        where TInstance : class
        where TOwner : class, IModelContext<TInstance>
    {
        private readonly TypeConverter _converter;
        private readonly PropertyDescriptor _propertyDescriptor;
        private object _oldValue;

        public event ValueChangedEventHandler ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public TValue Value => (TValue)_propertyDescriptor.GetValue(Owner.Instance);

        public TOwner Owner { get; }

        public string Name => _propertyDescriptor.Name;

        public string Description => _propertyDescriptor.Description ?? "";

        public string Category => string.IsNullOrWhiteSpace(_propertyDescriptor.Category) ? CategoryAttribute.Default.Category : _propertyDescriptor.Category;

        public string DisplayName => string.IsNullOrWhiteSpace(_propertyDescriptor.DisplayName) ? _propertyDescriptor.Name : _propertyDescriptor.DisplayName;

        public bool IsReadOnly => _propertyDescriptor.IsReadOnly;

        public bool AreStandardValuesExclusive => _converter.GetStandardValuesExclusive(this);

        public bool AreStandardValuesSupported => _converter.GetStandardValuesSupported(this);

        public Type PropertyType => _propertyDescriptor.PropertyType;

        public PropertyContext(TOwner owner, PropertyDescriptor propertyDescriptor)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            _converter = (_propertyDescriptor = propertyDescriptor ?? throw new ArgumentNullException(nameof(propertyDescriptor))).Converter;
            _propertyDescriptor.AddValueChanged(owner.Instance, OnInstancePropertyValueChanged);
            _oldValue = _propertyDescriptor.GetValue(owner.Instance);
            WeakComponentPropertyChangedManager.AddValueChanged(owner.Instance, propertyDescriptor, OnInstancePropertyValueChanged);
        }

        private void OnInstancePropertyValueChanged(object sender, EventArgs e)
        {
            object oldValue = _oldValue;
            _oldValue = _propertyDescriptor.GetValue(Owner.Instance);
            OnInstancePropertyValueChanged(oldValue, _oldValue);
        }

        protected virtual void OnInstancePropertyValueChanged(object oldValue, object newValue)
        {
            try { ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue)); }
            finally { RaisePropertyChanged(nameof(Value)); }
        }

        protected void RaisePropertyChanged(string propertyName) => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        public bool CanConvertFrom(Type sourceType) => _converter.CanConvertFrom(this, sourceType);

        public bool CanResetValue() => _propertyDescriptor.CanResetValue(Owner.Instance);

        public string GetInvariantStringValue() => _converter.ConvertToInvariantString(this, Value);

        public IEnumerable<TValue> GetStandardValues() => _converter.GetStandardValues(this).OfType<TValue>();

        public string GetStringValue(CultureInfo culture) => _converter.ConvertToString(this, culture, Value);

        public string GetStringValue() => _converter.ConvertToString(this, Value);

        public TValue ResetValue()
        {
            _propertyDescriptor.ResetValue(Owner.Instance);
            return Value;
        }

        public void SetValue(TValue value) => _propertyDescriptor.SetValue(Owner.Instance, value);

        public TValue SetValueFromConverted(object value)
        {
            if (Value is TValue v)
                SetValue(v);
            else
            {
                try { v = (TValue)_converter.ConvertFrom(value); }
                catch (NotSupportedException exc) { throw new InvalidCastException(string.IsNullOrWhiteSpace(exc.Message) ? "Unable to convert value" : exc.Message, exc); }
                SetValue(v);
            }
            return Value;
        }

        public TValue SetValueFromConverted(CultureInfo culture, object value)
        {
            if (Value is TValue v)
                SetValue(v);
            else
            {
                try { v = (TValue)_converter.ConvertFrom(this, culture, value); }
                catch (NotSupportedException exc) { throw new InvalidCastException(string.IsNullOrWhiteSpace(exc.Message) ? "Unable to convert value" : exc.Message, exc); }
                SetValue(v);
            }
            return Value;
        }

        public TValue SetValueFromInvariantString(string text)
        {
            SetValue((TValue)_converter.ConvertFromInvariantString(this, text));
            return Value;
        }

        public TValue SetValueFromString(string text)
        {
            SetValue((TValue)_converter.ConvertFromString(this, text));
            return Value;
        }

        public TValue SetValueFromString(CultureInfo culture, string text)
        {
            SetValue((TValue)_converter.ConvertFromString(this, culture, text));
            return Value;
        }

        #region Explicit Members

        object IPropertyContext.Value => Value;

        IModelContext<TInstance> IPropertyContext<TInstance>.Owner => Owner;

        IModelContext IPropertyContext.Owner => Owner;

        IContainer ITypeDescriptorContext.Container => null;

        object ITypeDescriptorContext.Instance => Owner.Instance;

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => _propertyDescriptor;

        object IServiceProvider.GetService(Type serviceType) => null;

        ICollection IModelProperty.GetStandardValues() => _converter.GetStandardValues(this);

        bool IPropertyContext.IsAssignableFrom(object value) => _converter.IsValid(this, value);

        bool ITypeDescriptorContext.OnComponentChanging() => false;

        void ITypeDescriptorContext.OnComponentChanged() { }

        object IPropertyContext.ResetValue() => ResetValue();

        void IPropertyContext.SetValue(object value) => SetValueFromConverted(value);

        object IPropertyContext.SetValueFromConverted(CultureInfo culture, object value) => SetValueFromConverted(culture, value);

        object IPropertyContext.SetValueFromInvariantString(string text) => SetValueFromInvariantString(text);

        object IPropertyContext.SetValueFromString(string text) => SetValueFromString(text);

        object IPropertyContext.SetValueFromString(CultureInfo culture, string text) => SetValueFromString(culture, text);

        #endregion
    }

    public sealed class PropertyContext<TInstance, TValue> : PropertyContext<TInstance, TValue, ModelContext<TInstance>>
        where TInstance : class
    {
        public PropertyContext(ModelContext<TInstance> owner, PropertyDescriptor propertyDescriptor) : base(owner, propertyDescriptor)
        {
        }

        public override string ToString()
        {
            return $@"{nameof(PropertyContext<TInstance, TValue>)}<{Owner.SimpleName}, {PropertyType.Name}> {{
    Name = {TypeHelper.ToPseudoCsText(Name)},
    Category = {TypeHelper.ToPseudoCsText(Category)},
    DisplayName = {TypeHelper.ToPseudoCsText(DisplayName)},
    IsReadOnly = {TypeHelper.ToPseudoCsText(IsReadOnly)},
    sValue = {TypeHelper.ToPseudoCsText(Value)}
}}";
        }
    }
}
