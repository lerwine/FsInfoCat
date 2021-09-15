using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>
        : VolumeRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, IVolume
        where TFileSystemEntity : DbEntity, IFileSystemListItem
        where TFileSystemItem : FileSystemListItemViewModel<TFileSystemEntity>
        where TSubdirectoryEntity : DbEntity, ISubdirectoryListItem
        where TSubdirectoryItem : SubdirectoryListItemViewModel<TSubdirectoryEntity>
        where TAccessErrorEntity : DbEntity, IVolumeAccessError
        where TAccessErrorItem : AccessErrorRowViewModel<TAccessErrorEntity>
        where TPersonalTagEntity : DbEntity, IItemTagListItem
        where TPersonalTagItem : ItemTagListItemViewModel<TPersonalTagEntity>
        where TSharedTagEntity : DbEntity, IItemTagListItem
        where TSharedTagItem : ItemTagListItemViewModel<TSharedTagEntity>
    {
        #region FileSystem Property Members

        /// <summary>
        /// Identifies the <see cref="FileSystem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemProperty = ColumnPropertyBuilder<TFileSystemItem, VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>>
            .RegisterEntityMapped<TEntity>(nameof(IVolume.FileSystem))
            .OnChanged((d, oldValue, newValue) => (d as VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>)?.OnFileSystemPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public TFileSystemItem FileSystem { get => GetValue(FileSystemProperty) as TFileSystemItem; set => SetValue(FileSystemProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FileSystem"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FileSystem"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FileSystem"/> property.</param>
        protected virtual void OnFileSystemPropertyChanged(TFileSystemItem oldValue, TFileSystemItem newValue) { }

        #endregion
        #region RootDirectory Property Members

        /// <summary>
        /// Identifies the <see cref="RootDirectory"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootDirectoryProperty = ColumnPropertyBuilder<TSubdirectoryItem, VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>>
            .RegisterEntityMapped<TEntity>(nameof(IVolume.RootDirectory))
            .OnChanged((d, oldValue, newValue) => (d as VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>)?.OnRootDirectoryPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public TSubdirectoryItem RootDirectory { get => GetValue(RootDirectoryProperty) as TSubdirectoryItem; set => SetValue(RootDirectoryProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="RootDirectory"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="RootDirectory"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="RootDirectory"/> property.</param>
        protected virtual void OnRootDirectoryPropertyChanged(TSubdirectoryItem oldValue, TSubdirectoryItem newValue) { }

        #endregion
        #region AccessErrors Property Members

        protected ObservableCollection<TAccessErrorItem> BackingAccessErrors { get; } = new();

        private static readonly DependencyPropertyKey AccessErrorsPropertyKey = ColumnPropertyBuilder<ObservableCollection<TAccessErrorItem>, VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>>
            .RegisterEntityMapped<TEntity>(nameof(IVolume.AccessErrors))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="AccessErrors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorsProperty = AccessErrorsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TAccessErrorItem> AccessErrors => (ReadOnlyObservableCollection<TAccessErrorItem>)GetValue(AccessErrorsProperty);

        #endregion
        #region PersonalTags Property Members

        protected ObservableCollection<TPersonalTagItem> BackingPersonalTags { get; } = new();

        private static readonly DependencyPropertyKey PersonalTagsPropertyKey = ColumnPropertyBuilder<ObservableCollection<TPersonalTagItem>, VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>>
            .RegisterEntityMapped<TEntity>(nameof(IVolume.PersonalTags))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PersonalTags"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagsProperty = PersonalTagsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TPersonalTagItem> PersonalTags => (ReadOnlyObservableCollection<TPersonalTagItem>)GetValue(PersonalTagsProperty);

        #endregion
        #region SharedTags Property Members

        protected ObservableCollection<TSharedTagItem> BackingSharedTags { get; } = new();

        private static readonly DependencyPropertyKey SharedTagsPropertyKey = ColumnPropertyBuilder<ObservableCollection<TSharedTagItem>, VolumeDetailsViewModel<TEntity, TFileSystemEntity, TFileSystemItem, TSubdirectoryEntity, TSubdirectoryItem, TAccessErrorEntity, TAccessErrorItem, TPersonalTagEntity, TPersonalTagItem, TSharedTagEntity, TSharedTagItem>>
            .RegisterEntityMapped<TEntity>(nameof(IVolume.SharedTags))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="SharedTags"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagsProperty = SharedTagsPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<TSharedTagItem> SharedTags => (ReadOnlyObservableCollection<TSharedTagItem>)GetValue(SharedTagsProperty);

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

        public VolumeDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(AccessErrorsPropertyKey, new ReadOnlyObservableCollection<TAccessErrorItem>(BackingAccessErrors));
            SetValue(PersonalTagsPropertyKey, new ReadOnlyObservableCollection<TPersonalTagItem>(BackingPersonalTags));
            SetValue(SharedTagsPropertyKey, new ReadOnlyObservableCollection<TSharedTagItem>(BackingSharedTags));
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            base.OnEntityPropertyChanged(propertyName);
            // TODO: Update properties
        }
    }
}
