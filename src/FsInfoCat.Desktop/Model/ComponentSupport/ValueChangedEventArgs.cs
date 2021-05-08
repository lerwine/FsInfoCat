using System;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public class ValueChangedEventArgs : EventArgs
    {
        public object OldValue { get; }

        public object NewValue { get; }

        public ValueChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
