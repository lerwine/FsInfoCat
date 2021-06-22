using System.ComponentModel;

namespace FsInfoCat.Models
{
    public class PropertyValueChangedEventArgs<T> : PropertyChangedEventArgs, IPropertyValueChangeEventArgs<T>
    {
        public PropertyValueChangedEventArgs(string propertyName, T oldValue, T newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue { get; }

        object IPropertyValueChangeEventArgs.OldValue => OldValue;

        public T NewValue { get; }

        object IPropertyValueChangeEventArgs.NewValue => NewValue;
    }
}
