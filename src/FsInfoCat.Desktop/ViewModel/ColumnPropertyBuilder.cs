using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class ColumnPropertyBuilder<TProperty, TTarget> : IDependencyPropertyBuilder<TProperty>
        where TTarget : DependencyObject
    {
        private readonly IDependencyPropertyBuilder<TProperty> _backingBuilder;
        private string _displayName;
        private string _shortName;
        private string _prompt;
        private string _groupName;
        private string _description;
        private int? _order;

        public string PropertyName => _backingBuilder.PropertyName;

        public Type OwnerType => _backingBuilder.OwnerType;

        public PropertyDescriptor EntityProperty => _backingBuilder.EntityProperty;

        public PropertyDescriptor TargetProperty => _backingBuilder.TargetProperty;

        private ColumnPropertyBuilder(IDependencyPropertyBuilder<TProperty> backingBuilder)
        {
            _backingBuilder = backingBuilder;
            PropertyDescriptor sourceDescriptor = backingBuilder.EntityProperty ?? backingBuilder.TargetProperty;
            if (sourceDescriptor is null)
            {
                _displayName = _shortName = backingBuilder.PropertyName;
                _prompt = $"{backingBuilder.PropertyName}: ";
                _groupName = _description = "";
            }
            else
            {
                _displayName = sourceDescriptor.GetDisplay(out string shortName, out string prompt, out string groupName, out string description, out int? order);
                _shortName = shortName;
                _prompt = prompt;
                _groupName = groupName;
                _description = description;
                _order = order;
            }
        }

        public static ColumnPropertyBuilder<TProperty, TTarget> RegisterEntityMapped<TEntity>([DisallowNull] string propertyName, string entityPropertyName = null)
        {;
            return new ColumnPropertyBuilder<TProperty, TTarget>(DependencyPropertyBuilder<TTarget, TProperty>.RegisterEntityMapped<TEntity>(propertyName, entityPropertyName));
        }

        public static ColumnPropertyBuilder<TProperty, TTarget> RegisterAttachedEntityMapped<TEntity, TOwner>(string propertyName, string entityPropertyName = null)
            where TOwner : class
        {
            return new ColumnPropertyBuilder<TProperty, TTarget>(DependencyPropertyBuilder<TOwner, TProperty>.RegisterAttachedEntityMapped<TEntity>(propertyName, entityPropertyName));
        }

        public static ColumnPropertyBuilder<TProperty, TTarget> Register(string propertyName)
        {
            return new ColumnPropertyBuilder<TProperty, TTarget>(DependencyPropertyBuilder<TTarget, TProperty>.Register(propertyName));
        }

        public static ColumnPropertyBuilder<TProperty, TTarget> RegisterAttached<TOwner>(string propertyName)
            where TOwner : class
        {
            return new ColumnPropertyBuilder<TProperty, TTarget>(DependencyPropertyBuilder<TOwner, TProperty>.RegisterAttached(propertyName));
        }

        public ColumnPropertyBuilder<TProperty, TTarget> DisplayName(string displayName, string shortName = null, string prompt = null)
        {
            if (!string.IsNullOrWhiteSpace(displayName))
                _displayName = displayName;
            if (!string.IsNullOrWhiteSpace(shortName))
                _shortName = shortName;
            if (!string.IsNullOrWhiteSpace(prompt))
                _prompt = prompt;
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> ShortName(string shortName)
        {
            if (!string.IsNullOrWhiteSpace(shortName))
                _shortName = shortName;
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> Prompt(string prompt)
        {
            if (!string.IsNullOrWhiteSpace(prompt))
                _prompt = prompt;
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> Order(int order)
        {
            _order = order;
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> DefaultOrder(int order)
        {
            if (!_order.HasValue)
                _order = order;
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> GroupName(string name)
        {
            _groupName = name.EmptyIfNullOrWhiteSpace();
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> DefaultGroupName(string name)
        {
            if (string.IsNullOrWhiteSpace(_groupName))
                _groupName = name.EmptyIfNullOrWhiteSpace();
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> Description(string description)
        {
            _description = description.EmptyIfNullOrWhiteSpace();
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> DefaultValue(TProperty defaultValue)
        {
            _backingBuilder.DefaultValue(defaultValue);
            return this;
        }
        public ColumnPropertyBuilder<TProperty, TTarget> OnChanged([DisallowNull] Action<DependencyObject, TProperty, TProperty> propertyChangedCallback)
        {
            _backingBuilder.OnChanged(propertyChangedCallback);
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> CoerseWith([DisallowNull] ICoersion<TProperty> coersion)
        {
            _backingBuilder.CoerseWith(coersion);
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> CoerseWith([DisallowNull] Func<DependencyObject, object, TProperty> coersion)
        {
            _backingBuilder.CoerseWith(coersion);
            return this;
        }

        public ColumnPropertyBuilder<TProperty, TTarget> ValidateWith([DisallowNull] Func<TProperty, bool> func)
        {
            _backingBuilder.ValidateWith(func);
            return this;
        }

        public DependencyProperty AsReadWrite()
        {
            DependencyProperty dependencyProperty = _backingBuilder.AsReadWrite();
            ColumnProperty.Add(dependencyProperty, _displayName, _shortName, _prompt, _groupName, _description, _order, typeof(TTarget));
            return dependencyProperty;
        }

        public DependencyPropertyKey AsReadOnly()
        {
            DependencyPropertyKey dependencyProperty = _backingBuilder.AsReadOnly();
            ColumnProperty.Add(dependencyProperty.DependencyProperty, _displayName, _shortName, _prompt, _groupName, _description, _order, typeof(TTarget));
            return dependencyProperty;
        }

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.DefaultValue(TProperty defaultValue) => DefaultValue(defaultValue);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.OnChanged(Action<DependencyObject, TProperty, TProperty> propertyChangedCallback) => OnChanged(propertyChangedCallback);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.CoerseWith(ICoersion<TProperty> coersion) => CoerseWith(coersion);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.CoerseWith(Func<DependencyObject, object, TProperty> coersion) => CoerseWith(coersion);

        IDependencyPropertyBuilder<TProperty> IDependencyPropertyBuilder<TProperty>.ValidateWith(Func<TProperty, bool> func) => ValidateWith(func);
    }
}
