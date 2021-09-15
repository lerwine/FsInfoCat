using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemFunctionResultEventArgs : EventArgs
    {
        public ItemFunctionResult FunctionResult { get; }

        public object State { get; }

        public DbEntity Entity { get; }

        public ItemFunctionResultEventArgs(ItemFunctionResult functionResult, DbEntity entity, object state = null)
        {
            FunctionResult = functionResult;
            Entity = entity;
            State = state;
        }
    }
}
