using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemFunctionResultEventArgs : EventArgs
    {
        public ItemFunctionResult FunctionResult { get; }

        public object State { get; }

        public DbEntity Entity { get; }

        [Obsolete("Use constructor with DbEntity, instead")]
        public ItemFunctionResultEventArgs(ItemFunctionResult functionResult, object state = null)
        {
            FunctionResult = functionResult;
        }

        public ItemFunctionResultEventArgs(ItemFunctionResult functionResult, DbEntity entity, object state = null)
        {
            FunctionResult = functionResult;
        }
    }
}
