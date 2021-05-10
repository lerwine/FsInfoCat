using System.ComponentModel;

namespace FsInfoCat.Models
{
    public class PropertyValueChangingEventArgs<T> : PropertyChangingEventArgs, IPropertyValueChangeEventArgs<T>
    {
        public PropertyValueChangingEventArgs(string propertyName, T oldValue, T newValue) : base(propertyName)
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
