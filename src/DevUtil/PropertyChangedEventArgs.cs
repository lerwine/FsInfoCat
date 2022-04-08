using System.ComponentModel;

namespace DevUtil
{
    public class PropertyChangedEventArgs<T> : PropertyChangedEventArgs
    {
        public T OldValue { get; }

        public T CurrentValue { get; }

        public PropertyChangedEventArgs(T oldValue, T currentValue, string propertyName) : base(propertyName)
        {
            OldValue = oldValue;
            CurrentValue = currentValue;
        }

    }
}
