using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class BinaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem, TRedundantSetEntity, TRedundantSetItem>
        : BinaryPropertySetRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, IBinaryPropertySet
        where TFileEntity : DbEntity, IFileListItemWithAncestorNames
        where TFileItem : FileWithAncestorNamesViewModel<TFileEntity>
        where TRedundantSetEntity : DbEntity, IRedundantSetListItem
        where TRedundantSetItem : RedundantSetListItemViewModel<TRedundantSetEntity>
    {
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, BinaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem, TRedundantSetEntity, TRedundantSetItem>>
            .RegisterEntityMapped<TEntity>(nameof(IBinaryPropertySet.Files))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Files"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilesProperty = FilesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TFileItem> Files => (ReadOnlyObservableCollection<TFileItem>)GetValue(FilesProperty);

        #endregion

        #region RedundantSets Property Members

        protected ObservableCollection<TRedundantSetItem> BackingRedundantSets { get; } = new();

        private static readonly DependencyPropertyKey RedundantSetsPropertyKey = ColumnPropertyBuilder<ObservableCollection<TRedundantSetItem>, BinaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem, TRedundantSetEntity, TRedundantSetItem>>
            .RegisterEntityMapped<TEntity>(nameof(IBinaryPropertySet.RedundantSets))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="RedundantSets"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RedundantSetsProperty = RedundantSetsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TRedundantSetItem> RedundantSets => (ReadOnlyObservableCollection<TRedundantSetItem>)GetValue(RedundantSetsProperty);

        #endregion
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; }

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemFunctionResult(ItemFunctionResult result) => OnItemFunctionResult(new(result, InvocationState));

        #endregion

        public BinaryPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
            SetValue(RedundantSetsPropertyKey, new ReadOnlyObservableCollection<TRedundantSetItem>(BackingRedundantSets));
        }
    }
}
