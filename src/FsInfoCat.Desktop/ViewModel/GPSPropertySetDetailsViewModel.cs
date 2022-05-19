using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class GPSPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : GPSPropertiesRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IGPSPropertySet, Model.IGPSProperties
        where TFileEntity : Model.DbEntity, Model.IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region VersionID Property Members

        private static readonly DependencyPropertyKey VersionIDPropertyKey = DependencyPropertyBuilder<GPSPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<byte>>
            .Register(nameof(VersionID))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="VersionID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VersionIDProperty = VersionIDPropertyKey.DependencyProperty;

        public ObservableCollection<byte> VersionID { get => (ObservableCollection<byte>)GetValue(VersionIDProperty); private set => SetValue(VersionIDPropertyKey, value); }

        #endregion
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, GPSPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSPropertySet.Files))
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

        public GPSPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
            ObservableCollection<byte> target = new();
            ReadOnlyCollection<byte> source = entity.VersionID;
            if (source is not null)
                foreach (byte value in source)
                    target.Add(value);
        }
    }
}
