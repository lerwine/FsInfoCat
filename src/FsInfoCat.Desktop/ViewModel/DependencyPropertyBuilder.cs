using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DependencyPropertyBuilder<TOwner, TProperty> : IDependencyPropertyBuilder<TProperty>
        where TOwner : class
    {
        public Type OwnerType => typeof(TOwner);
        public string PropertyName { get; }
        public PropertyDescriptor TargetProperty { get; }
        public PropertyDescriptor EntityProperty { get; }
        private bool _hasDefaultValue;
        private TProperty _defaultValue;
        private PropertyChangedCallback _propertyChangedCallback;
        private CoerceValueCallback _coerceValueCallback;
        private ValidateValueCallback _validationCallback;

        private DependencyPropertyBuilder([DisallowNull] string propertyName, PropertyDescriptor targetProperty, PropertyDescriptor entityProperty)
        {
            PropertyName = propertyName;
            TargetProperty = targetProperty;
            EntityProperty = entityProperty;
            PropertyDescriptor sourceDescriptor = entityProperty ?? targetProperty;
            if (sourceDescriptor is not null && targetProperty.TryGetDefaultValue(out object defaultValue))
            {
                if (defaultValue is null)
                {
                    Type type = typeof(TProperty);
                    if (!type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
                    {
                        _hasDefaultValue = true;
                        _defaultValue = (TProperty)defaultValue;
                    }
                }
                else if (defaultValue is TProperty value)
                {
                    _hasDefaultValue = true;
                    _defaultValue = value;
                }
            }
        }

        public static DependencyPropertyBuilder<TOwner, TProperty> RegisterEntityMapped<TEntity>([DisallowNull] string propertyName, string entityPropertyName = null)
        {
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));
            PropertyDescriptor targetProperty = TypeDescriptor.GetProperties(typeof(TOwner)).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == propertyName);
            if (targetProperty is null)
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property \"{propertyName}\" not found on type {nameof(TOwner)}");
            PropertyDescriptor sourceProperty;
            if (string.IsNullOrEmpty(entityPropertyName))
            {
                if ((sourceProperty = TypeDescriptor.GetProperties(typeof(TEntity)).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == propertyName)) is null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property \"{propertyName}\" not found on type {nameof(TEntity)}");
            }
            else
            if ((sourceProperty = TypeDescriptor.GetProperties(typeof(TEntity)).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == entityPropertyName)) is null)
                throw new ArgumentOutOfRangeException(nameof(entityPropertyName), $"Property \"{entityPropertyName}\" not found on type {nameof(TEntity)}");
            return new DependencyPropertyBuilder<TOwner, TProperty>(propertyName, targetProperty, sourceProperty);
        }

        public static DependencyPropertyBuilder<TOwner, TProperty> RegisterAttachedEntityMapped<TEntity>(string propertyName, string entityPropertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            PropertyDescriptor sourceProperty;
            if (string.IsNullOrEmpty(entityPropertyName))
            {
                if ((sourceProperty = TypeDescriptor.GetProperties(typeof(TEntity)).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == propertyName)) is null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property \"{propertyName}\" not found on type {nameof(TEntity)}");
            }
            else if (char.IsLetter(propertyName[0]) && propertyName.Skip(1).All(c => char.IsLetterOrDigit(c)))
            {
                if ((sourceProperty = TypeDescriptor.GetProperties(typeof(TEntity)).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == entityPropertyName)) is null)
                    throw new ArgumentOutOfRangeException(nameof(entityPropertyName), $"Property \"{entityPropertyName}\" not found on type {nameof(TEntity)}");
            }
            else
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property name \"{propertyName}\" is not valid.");
            return new DependencyPropertyBuilder<TOwner, TProperty>(propertyName, null, sourceProperty);
        }

        public static DependencyPropertyBuilder<TOwner, TProperty> Register(string propertyName)
        {
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));
            PropertyDescriptor targetProperty = TypeDescriptor.GetProperties(typeof(TOwner)).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == propertyName);
            if (targetProperty is null)
                throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property \"{propertyName}\" not found on type {nameof(TOwner)}");
            return new DependencyPropertyBuilder<TOwner, TProperty>(propertyName, targetProperty, null);
        }

        public static DependencyPropertyBuilder<TOwner, TProperty> RegisterAttached(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or whitespace.", nameof(propertyName));
            if (char.IsLetter(propertyName[0]) && propertyName.Skip(1).All(c => char.IsLetterOrDigit(c)))
                return new DependencyPropertyBuilder<TOwner, TProperty>(propertyName, null, null);
            throw new ArgumentOutOfRangeException(nameof(propertyName), $"Property name \"{propertyName}\" is not valid.");
        }

        public DependencyPropertyBuilder<TOwner, TProperty> DefaultValue(TProperty defaultValue)
        {
            _hasDefaultValue = true;
            _defaultValue = defaultValue;
            return this;
        }

        public DependencyPropertyBuilder<TOwner, TProperty> OnChanged([DisallowNull] Action<DependencyObject, TProperty, TProperty> propertyChangedCallback)
        {
            if (propertyChangedCallback is null)
                throw new ArgumentNullException(nameof(propertyChangedCallback));
            _propertyChangedCallback = (DependencyObject d, DependencyPropertyChangedEventArgs e) => propertyChangedCallback(d, (TProperty)e.OldValue, (TProperty)e.NewValue);
            return this;
        }

        public DependencyPropertyBuilder<TOwner, TProperty> OnChanged([DisallowNull] PropertyChangedCallback propertyChangedCallback)
        {
            if (propertyChangedCallback is null)
                throw new ArgumentNullException(nameof(propertyChangedCallback));
            _propertyChangedCallback = propertyChangedCallback;
            return this;
        }

        public DependencyPropertyBuilder<TOwner, TProperty> CoerseWith([DisallowNull] ICoersion<TProperty> coersion)
        {
            if (coersion is null)
                throw new ArgumentNullException(nameof(coersion));
            _coerceValueCallback = coersion.ToCoerceValueCallback();
            return this;
        }

        public DependencyPropertyBuilder<TOwner, TProperty> CoerseWith([DisallowNull] Func<DependencyObject, object, TProperty> coersion)
        {
            if (coersion is null)
                throw new ArgumentNullException(nameof(coersion));
            _coerceValueCallback = (DependencyObject d, object baseValue) => coersion(d, baseValue);
            return this;
        }

        public DependencyPropertyBuilder<TOwner, TProperty> CoerseWith([DisallowNull] Func<object, TProperty> coersion)
        {
            if (coersion is null)
                throw new ArgumentNullException(nameof(coersion));
            _coerceValueCallback = (DependencyObject d, object baseValue) => coersion(baseValue);
            return this;
        }

        public DependencyPropertyBuilder<TOwner, TProperty> ValidateWith([DisallowNull] Func<TProperty, bool> func)
        {
            if (func is null)
                throw new ArgumentNullException(nameof(func));
            _validationCallback = o => func((TProperty)o);
            return this;
        }

        private PropertyMetadata GetMetaData()
        {
            PropertyMetadata typeMetadata;
            if (_hasDefaultValue)
            {
                if (_propertyChangedCallback is null)
                    typeMetadata = (_coerceValueCallback is null) ? new(_defaultValue) : new(_defaultValue) { CoerceValueCallback = _coerceValueCallback };
                else if (_coerceValueCallback is null)
                    typeMetadata = new PropertyMetadata(_defaultValue, _propertyChangedCallback);
                else
                    typeMetadata = new PropertyMetadata(_defaultValue, _propertyChangedCallback, _coerceValueCallback);
            }
            else if (_propertyChangedCallback is null)
                typeMetadata = (_coerceValueCallback is null) ? new() : new() { CoerceValueCallback = _coerceValueCallback };
            else
                typeMetadata = (_coerceValueCallback is null) ? new(_propertyChangedCallback) : new(_propertyChangedCallback) { CoerceValueCallback = _coerceValueCallback };
            return typeMetadata;
        }

        public DependencyProperty AsReadWrite()
        {
            PropertyMetadata typeMetadata = GetMetaData();
            if (TargetProperty is null)
            {
                if (_validationCallback is null)
                    return DependencyProperty.RegisterAttached(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata);
                return DependencyProperty.RegisterAttached(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata, _validationCallback);
            }
            if (_validationCallback is null)
                return DependencyProperty.Register(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata);
            return DependencyProperty.Register(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata, _validationCallback);
        }

        public DependencyPropertyKey AsReadOnly()
        {
            PropertyMetadata typeMetadata = GetMetaData();
            if (TargetProperty is null)
            {
                if (_validationCallback is null)
                    return DependencyProperty.RegisterAttachedReadOnly(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata);
                return DependencyProperty.RegisterAttachedReadOnly(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata, _validationCallback);
            }
            if (_validationCallback is null)
                return DependencyProperty.RegisterReadOnly(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata);
            return DependencyProperty.RegisterReadOnly(PropertyName, typeof(TProperty), typeof(TOwner), typeMetadata, _validationCallback);
        }

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.DefaultValue(TProperty defaultValue) => DefaultValue(defaultValue);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.OnChanged(Action<DependencyObject, TProperty, TProperty> propertyChangedCallback) => OnChanged(propertyChangedCallback);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.OnChanged(PropertyChangedCallback propertyChangedCallback) => OnChanged(propertyChangedCallback);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.CoerseWith(ICoersion<TProperty> coersion) => CoerseWith(coersion);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.CoerseWith(Func<DependencyObject, object, TProperty> coersion) => CoerseWith(coersion);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.ValidateWith(Func<TProperty, bool> func) => ValidateWith(func);
    }
}
