using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
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
}
