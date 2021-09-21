using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MediaPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : MediaPropertiesRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : DbEntity, IMediaPropertySet, IMediaProperties
        where TFileEntity : DbEntity, IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Producer Property Members

        private static readonly DependencyPropertyKey ProducerPropertyKey = DependencyPropertyBuilder<MediaPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Producer))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Producer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProducerProperty = ProducerPropertyKey.DependencyProperty;

        public ObservableCollection<string> Producer { get => (ObservableCollection<string>)GetValue(ProducerProperty); private set => SetValue(ProducerPropertyKey, value); }

        #endregion
        #region Writer Property Members

        private static readonly DependencyPropertyKey WriterPropertyKey = DependencyPropertyBuilder<MediaPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Writer))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Writer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WriterProperty = WriterPropertyKey.DependencyProperty;

        public ObservableCollection<string> Writer { get => (ObservableCollection<string>)GetValue(WriterProperty); private set => SetValue(WriterPropertyKey, value); }

        #endregion
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, MediaPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaPropertySet.Files))
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

        public MediaPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
            ObservableCollection<string> target = new();
            Producer = target;
            ReadOnlyCollection<string> items = entity.Producer;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
            target = new();
            Writer = target;
            items = entity.Writer;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
        }
    }
}
