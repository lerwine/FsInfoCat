using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    public class DetailsViewModel : TagDefinitionRowViewModel<SharedTagDefinition>, IItemFunctionViewModel<SharedTagDefinition>
    {
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; }

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemInsertedResult([DisallowNull] Model.DbEntity entity) => OnItemFunctionResult(new(ItemFunctionResult.Inserted, entity, InvocationState));

        protected void RaiseItemUpdatedResult() => OnItemFunctionResult(new(ItemFunctionResult.ChangesSaved, Entity, InvocationState));

        protected void RaiseItemDeletedResult() => OnItemFunctionResult(new(ItemFunctionResult.Deleted, Entity, InvocationState));

        protected void RaiseItemUnmodifiedResult() => OnItemFunctionResult(new(ItemFunctionResult.Unmodified, Entity, InvocationState));

        #endregion

        public DetailsViewModel(SharedTagDefinition entity, SharedTagDefinitionListItem listItemEntity) : base(entity)
        {
            // TODO: Implement DetailsViewModel
        }
    }
}
