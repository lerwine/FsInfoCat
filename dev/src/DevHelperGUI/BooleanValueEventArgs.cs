using System;

namespace DevHelperGUI
{
    public class BooleanValueEventArgs : EventArgs
    {
        public BooleanValueEventArgs(bool value)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
