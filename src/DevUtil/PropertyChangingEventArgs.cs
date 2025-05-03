using System.ComponentModel;

namespace DevUtil
{
    public class PropertyChangingEventArgs<T>(T currentValue, T newValue, string propertyName) : PropertyChangingEventArgs(propertyName)
    {
        public T CurrentValue { get; } = currentValue;

        public T NewValue { get; } = newValue;
    }
}
