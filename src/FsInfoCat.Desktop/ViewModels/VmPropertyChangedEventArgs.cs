using System;
using System.ComponentModel;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModels
{
    public class VmPropertyChangedEventArgs<T> : PropertyChangedEventArgs
    {
        private static string GetPropertyName(DependencyProperty property)
        {
            if (null == property)
                throw new ArgumentNullException("property");
            if (string.IsNullOrWhiteSpace(property.Name))
                throw new ArgumentException("Property does not have a name", "property");
            return property.Name;
        }

        public DependencyProperty Property { get; }

        public T OldValue { get; }

        public T NewValue { get; }

        public VmPropertyChangedEventArgs(DependencyProperty property, T oldValue, T newValue) : base(GetPropertyName(property))
        {
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class VmPropertyChangedEventArgs<T1, T2> : VmPropertyChangedEventArgs<T1>
    {

        public T2 Value2 { get; }

        public VmPropertyChangedEventArgs(DependencyProperty property, T1 oldValue, T1 newValue, T2 value2) : base(property, oldValue, newValue)
        {
            Value2 = value2;
        }
    }

    public class VmPropertyChangedEventArgs<T1, T2, T3> : VmPropertyChangedEventArgs<T1, T2>
    {

        public T3 Value3 { get; }

        public VmPropertyChangedEventArgs(DependencyProperty property, T1 oldValue, T1 newValue, T2 value2, T3 value3) : base(property, oldValue, newValue, value2)
        {
            Value3 = Value3;
        }
    }
}
