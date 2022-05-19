using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MusicPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem> : MusicPropertiesRowViewModel<TEntity>, IItemFunctionViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IMusicPropertySet, Model.IMusicProperties
        where TFileEntity : Model.DbEntity, Model.IFileListItemWithBinaryPropertiesAndAncestorNames
        where TFileItem : FileWithBinaryPropertiesAndAncestorNamesViewModel<TFileEntity>
    {
        #region Artist Property Members

        private static readonly DependencyPropertyKey ArtistPropertyKey = DependencyPropertyBuilder<MusicPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Artist))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Artist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ArtistProperty = ArtistPropertyKey.DependencyProperty;

        public ObservableCollection<string> Artist { get => (ObservableCollection<string>)GetValue(ArtistProperty); private set => SetValue(ArtistPropertyKey, value); }

        #endregion
        #region Composer Property Members

        private static readonly DependencyPropertyKey ComposerPropertyKey = DependencyPropertyBuilder<MusicPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Composer))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Composer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComposerProperty = ComposerPropertyKey.DependencyProperty;

        public ObservableCollection<string> Composer { get => (ObservableCollection<string>)GetValue(ComposerProperty); private set => SetValue(ComposerPropertyKey, value); }

        #endregion
        #region Conductor Property Members

        private static readonly DependencyPropertyKey ConductorPropertyKey = DependencyPropertyBuilder<MusicPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Conductor))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Conductor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConductorProperty = ConductorPropertyKey.DependencyProperty;

        public ObservableCollection<string> Conductor { get => (ObservableCollection<string>)GetValue(ConductorProperty); private set => SetValue(ConductorPropertyKey, value); }

        #endregion
        #region Genre Property Members

        private static readonly DependencyPropertyKey GenrePropertyKey = DependencyPropertyBuilder<MusicPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>, ObservableCollection<string>>
            .Register(nameof(Genre))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Genre"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GenreProperty = GenrePropertyKey.DependencyProperty;

        public ObservableCollection<string> Genre { get => (ObservableCollection<string>)GetValue(GenreProperty); private set => SetValue(GenrePropertyKey, value); }

        #endregion
        #region Files Property Members

        protected ObservableCollection<TFileItem> BackingFiles { get; } = new();

        private static readonly DependencyPropertyKey FilesPropertyKey = ColumnPropertyBuilder<ObservableCollection<TFileItem>, MusicPropertySetDetailsViewModel<TEntity, TFileEntity, TFileItem>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertySet.Files))
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

        public MusicPropertySetDetailsViewModel([DisallowNull] TEntity entity, object state = null) : base(entity)
        {
            InvocationState = state;
            SetValue(FilesPropertyKey, new ReadOnlyObservableCollection<TFileItem>(BackingFiles));
            ObservableCollection<string> target = new();
            Artist = target;
            ReadOnlyCollection<string> items = entity.Artist;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
            target = new();
            Composer = target;
            items = entity.Composer;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
            target = new();
            Conductor = target;
            items = entity.Conductor;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
            target = new();
            Genre = target;
            items = entity.Genre;
            if (items is not null)
                foreach (string s in items)
                    target.Add(s);
        }
    }
}
