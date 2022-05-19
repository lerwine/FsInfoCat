using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AudioPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : AudioPropertiesRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IAudioPropertySet, Model.IAudioProperties
        where TFileEntity : Model.DbEntity, Model.IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, AudioPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioPropertySet.Files))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Files"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilesProperty = FilesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TFileItem> Files => (ReadOnlyObservableCollection<TFileItem>)GetValue(FilesProperty);

        #endregion
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

        public AudioPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
        }
    }
}
