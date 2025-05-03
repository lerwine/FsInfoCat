using System.ComponentModel;

namespace DevUtil
{
    public class PropertyChangedEventArgs<T>(T oldValue, T currentValue, string propertyName) : PropertyChangedEventArgs(propertyName)
    {
        public T OldValue { get; } = oldValue;

        public T CurrentValue { get; } = currentValue;
    }
}
