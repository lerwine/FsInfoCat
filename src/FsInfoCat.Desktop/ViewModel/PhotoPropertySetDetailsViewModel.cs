using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class PhotoPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : PhotoPropertiesRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, IPhotoPropertySet, IPhotoProperties
        where TFileEntity : DbEntity, IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Event Property Members

        private static readonly DependencyPropertyKey EventPropertyKey = DependencyPropertyBuilder<PhotoPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Event))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Event"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventProperty = EventPropertyKey.DependencyProperty;

        public ObservableCollection<string> Event { get => (ObservableCollection<string>)GetValue(EventProperty); private set => SetValue(EventPropertyKey, value); }

        #endregion
        #region PeopleNames Property Members

        private static readonly DependencyPropertyKey PeopleNamesPropertyKey = DependencyPropertyBuilder<PhotoPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(PeopleNames))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PeopleNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeopleNamesProperty = PeopleNamesPropertyKey.DependencyProperty;

        public ObservableCollection<string> PeopleNames { get => (ObservableCollection<string>)GetValue(PeopleNamesProperty); private set => SetValue(PeopleNamesPropertyKey, value); }

        #endregion
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, PhotoPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoPropertySet.Files))
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

        protected void RaiseItemInsertedResult([DisallowNull] DbEntity entity) => OnItemFunctionResult(new(ItemFunctionResult.Inserted, entity, InvocationState));

        protected void RaiseItemUpdatedResult() => OnItemFunctionResult(new(ItemFunctionResult.ChangesSaved, Entity, InvocationState));

        protected void RaiseItemDeletedResult() => OnItemFunctionResult(new(ItemFunctionResult.Deleted, Entity, InvocationState));

        protected void RaiseItemUnmodifiedResult() => OnItemFunctionResult(new(ItemFunctionResult.Unmodified, Entity, InvocationState));

        #endregion

        public PhotoPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
            ObservableCollection<string> target = new();
            Event = target;
            ReadOnlyCollection<string> items = entity.Event;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
            target = new();
            PeopleNames = target;
            items = entity.PeopleNames;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
        }
    }
}
