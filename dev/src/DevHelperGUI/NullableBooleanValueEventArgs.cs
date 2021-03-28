using System;

namespace DevHelperGUI
{
    public class NullableBooleanValueEventArgs : EventArgs
    {
        public NullableBooleanValueEventArgs(bool? value)
        {
            Value = value;
        }

        public bool? Value { get; }
    }
}
