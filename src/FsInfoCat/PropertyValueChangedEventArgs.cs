using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    // TODO: Document PropertyValueChangedEventArgs class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PropertyValueChangedEventArgs([AllowNull] string propertyName, [AllowNull] object oldValue, [AllowNull] object newValue) : PropertyChangedEventArgs(propertyName)
    {
        public object OldValue { get; } = oldValue;

        public object NewValue { get; } = newValue;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
