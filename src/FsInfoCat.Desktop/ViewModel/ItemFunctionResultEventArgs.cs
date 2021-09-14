using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemFunctionResultEventArgs : EventArgs
    {
        public ItemFunctionResult FunctionResult { get; }

        public object State { get; }

        public ItemFunctionResultEventArgs(ItemFunctionResult functionResult, object state = null)
        {
            FunctionResult = functionResult;
        }
    }
}
