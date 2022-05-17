using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    // TODO: Document PropertyValueChangedEventArgs class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class PropertyValueChangedEventArgs : PropertyChangedEventArgs
    {
        public PropertyValueChangedEventArgs([AllowNull] string propertyName, [AllowNull] object oldValue, [AllowNull] object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object OldValue { get; }

        public object NewValue { get; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
