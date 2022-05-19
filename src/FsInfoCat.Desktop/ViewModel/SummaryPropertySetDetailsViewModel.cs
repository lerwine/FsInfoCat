using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SummaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : SummaryPropertiesRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.ISummaryPropertySet, Model.ISummaryProperties
        where TFileEntity : Model.DbEntity, Model.IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Author Property Members

        private static readonly DependencyPropertyKey AuthorPropertyKey = DependencyPropertyBuilder<SummaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Author))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Author"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AuthorProperty = AuthorPropertyKey.DependencyProperty;

        public ObservableCollection<string> Author { get => (ObservableCollection<string>)GetValue(AuthorProperty); private set => SetValue(AuthorPropertyKey, value); }

        #endregion
        #region ItemAuthors Property Members

        private static readonly DependencyPropertyKey ItemAuthorsPropertyKey = DependencyPropertyBuilder<SummaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(ItemAuthors))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ItemAuthors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemAuthorsProperty = ItemAuthorsPropertyKey.DependencyProperty;

        public ObservableCollection<string> ItemAuthors { get => (ObservableCollection<string>)GetValue(ItemAuthorsProperty); private set => SetValue(ItemAuthorsPropertyKey, value); }

        #endregion
        #region Keywords Property Members

        private static readonly DependencyPropertyKey KeywordsPropertyKey = DependencyPropertyBuilder<SummaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Keywords))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Keywords"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KeywordsProperty = KeywordsPropertyKey.DependencyProperty;

        public ObservableCollection<string> Keywords { get => (ObservableCollection<string>)GetValue(KeywordsProperty); private set => SetValue(KeywordsPropertyKey, value); }

        #endregion
        #region Kind Property Members

        private static readonly DependencyPropertyKey KindPropertyKey = DependencyPropertyBuilder<SummaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Kind))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Kind"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty KindProperty = KindPropertyKey.DependencyProperty;

        public ObservableCollection<string> Kind { get => (ObservableCollection<string>)GetValue(KindProperty); private set => SetValue(KindPropertyKey, value); }

        #endregion
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, SummaryPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(Model.ISummaryPropertySet.Files))
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

        public SummaryPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
            ObservableCollection<string> target = new();
            Author = target;
            ReadOnlyCollection<string> source = entity.Author;
            if (source is not null)
                foreach (string s in source)
                    target.Add(s ?? "");
            target = new();
            ItemAuthors = target;
            source = entity.ItemAuthors;
            if (source is not null)
                foreach (string s in source)
                    target.Add(s ?? "");
            target = new();
            Keywords = target;
            source = entity.Keywords;
            if (source is not null)
                foreach (string s in source)
                    target.Add(s ?? "");
            target = new();
            Kind = target;
            source = entity.Kind;
            if (source is not null)
                foreach (string s in source)
                    target.Add(s ?? "");
        }
    }
}
