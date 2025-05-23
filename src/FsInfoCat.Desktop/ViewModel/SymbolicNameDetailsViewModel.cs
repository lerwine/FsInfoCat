using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SymbolicNameDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemModel>([DisallowNull] TEntity entity) : SymbolicNameRowViewModel<TEntity>(entity), IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ISymbolicName
        where TFileSystemEntity : Model.DbEntity, Model.IFileSystemRow
        where TFileSystemModel : FileSystemRowViewModel<TFileSystemEntity>
    {
        #region FileSystem Property Members

        /// <summary>
        /// Identifies the <see cref="FileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemProperty = DependencyProperty.Register(nameof(FileSystem), typeof(TFileSystemModel),
            typeof(SymbolicNameDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemModel>), new PropertyMetadata(null,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SymbolicNameDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemModel>)?.OnFileSystemPropertyChanged((TFileSystemModel)e.OldValue,
                    (TFileSystemModel)e.NewValue)));

        public TFileSystemModel FileSystem { get => (TFileSystemModel)GetValue(FileSystemProperty); set => SetValue(FileSystemProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystem"/> property.</param>
        private void OnFileSystemPropertyChanged(TFileSystemModel oldValue, TFileSystemModel newValue) { }

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

        protected void SetFileSystem(Guid id)
        {
            // TODO: Implement SetFileSystem
            throw new NotImplementedException("SetFileSystem not implemented");
        }

        protected void SetFileSystem(Model.IFileSystemRow fileSystem)
        {
            // TODO: Implement SetFileSystem
            throw new NotImplementedException("SetFileSystem not implemented");
        }
    }
}
