using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface IDependencyPropertyBuilder<TProperty>
    {
        string PropertyName { get; }
        Type OwnerType { get; }
        PropertyDescriptor EntityProperty { get; }
        PropertyDescriptor TargetProperty { get; }
        IDependencyPropertyBuilder<TProperty> DefaultValue(TProperty defaultValue);
        IDependencyPropertyBuilder<TProperty> OnChanged([DisallowNull] Action<DependencyObject, TProperty, TProperty> propertyChangedCallback);
        IDependencyPropertyBuilder<TProperty> CoerseWith([DisallowNull] ICoersion<TProperty> coersion);
        IDependencyPropertyBuilder<TProperty> CoerseWith([DisallowNull] Func<DependencyObject, object, TProperty> coersion);
        IDependencyPropertyBuilder<TProperty> ValidateWith([DisallowNull] Func<TProperty, bool> func);
        DependencyProperty AsReadWrite();
        DependencyPropertyKey AsReadOnly();
    }
}
