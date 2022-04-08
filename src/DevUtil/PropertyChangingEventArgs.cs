using System.ComponentModel;

namespace DevUtil
{
    public class PropertyChangingEventArgs<T> : PropertyChangingEventArgs
    {
        public T CurrentValue { get; }

        public T NewValue { get; }

        public PropertyChangingEventArgs(T currentValue, T newValue, string propertyName) : base(propertyName)
        {
            CurrentValue = currentValue;
            NewValue = newValue;
        }
    }
}
