using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileSystemDetailsViewModel<TEntity, TVolumeEntity, TVolumeItem, TSymbolicNameEntity, TSymbolicNameItem>
        : FileSystemRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, IFileSystem, IFileSystemRow
        where TVolumeEntity : DbEntity, IVolumeListItem
        where TVolumeItem : VolumeListItemViewModel<TVolumeEntity>
        where TSymbolicNameEntity : DbEntity, ISymbolicNameRow
        where TSymbolicNameItem : SymbolicNameRowViewModel<TSymbolicNameEntity>
    {
        #region Volumes Property Members

        protected ObservableCollection<TVolumeItem> BackingVolumes { get; } = new();

        private static readonly DependencyPropertyKey VolumesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Volumes),
            typeof(ReadOnlyObservableCollection<TVolumeItem>),
            typeof(FileSystemDetailsViewModel<TEntity, TVolumeEntity, TVolumeItem, TSymbolicNameEntity, TSymbolicNameItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Volumes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumesProperty = VolumesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TVolumeItem> Volumes => (ReadOnlyObservableCollection<TVolumeItem>)GetValue(VolumesProperty);

        #endregion
        #region SymbolicNames Property Members

        protected ObservableCollection<TSymbolicNameItem> BackingSymbolicNames { get; } = new();

        private static readonly DependencyPropertyKey SymbolicNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SymbolicNames),
            typeof(ReadOnlyObservableCollection<TSymbolicNameItem>),
            typeof(FileSystemDetailsViewModel<TEntity, TVolumeEntity, TVolumeItem, TSymbolicNameEntity, TSymbolicNameItem>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SymbolicNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SymbolicNamesProperty = SymbolicNamesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TSymbolicNameItem> SymbolicNames => (ReadOnlyObservableCollection<TSymbolicNameItem>)GetValue(SymbolicNamesProperty);

        #endregion
        #region Completed Event Members

        public event EventHandler<ItemFunctionResultEventArgs> Completed;

        internal object InvocationState { get; }

        object IItemFunctionViewModel.InvocationState => InvocationState;

        protected virtual void OnItemFunctionResult(ItemFunctionResultEventArgs args) => Completed?.Invoke(this, args);

        protected void RaiseItemInsertedResult([DisallowNull] DbEntity entity) => OnItemFunctionResult(new(ItemFunctionResult.Inserted, entity, InvocationState));

        protected void RaiseItemUpdatedResult() => OnItemFunctionResult(new(ItemFunctionResult.ChangesSaved, Entity, InvocationState));

        protected void RaiseItemDeletedResult() => OnItemFunctionResult(new(ItemFunctionResult.Deleted, Entity, InvocationState));

        protected void RaiseItemUnmodifiedResult() => OnItemFunctionResult(new(ItemFunctionResult.Unmodified, Entity, InvocationState));

        #endregion

        public FileSystemDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(VolumesPropertyKey, new ReadOnlyObservableCollection<TVolumeItem>(BackingVolumes));
            SetValue(SymbolicNamesPropertyKey, new ReadOnlyObservableCollection<TSymbolicNameItem>(BackingSymbolicNames));
        }
    }
}
