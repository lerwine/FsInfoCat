using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ItemFunctionResultEventArgs(ItemFunctionResult functionResult, Model.DbEntity entity, object state = null) : EventArgs
    {
        public ItemFunctionResult FunctionResult { get; } = functionResult;

        public object State { get; } = state;

        public Model.DbEntity Entity { get; } = entity;
    }
}
