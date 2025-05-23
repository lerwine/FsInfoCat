using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MusicPropertiesListItemViewModel<TEntity> : MusicPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IMusicPropertiesListItem
    {
        #region Open Command Property Members

        /// <summary>
        /// Occurs when the <see cref="Open"/> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> OpenCommand;

        private static readonly DependencyPropertyKey OpenPropertyKey = DependencyPropertyBuilder<MusicPropertiesListItemViewModel<TEntity>, Commands.RelayCommand>
            .Register(nameof(Open))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Open"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OpenProperty = OpenPropertyKey.DependencyProperty;

        public Commands.RelayCommand Open => (Commands.RelayCommand)GetValue(OpenProperty);

        /// <summary>
        /// Called when the Open event is raised by <see cref="Open" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Open" />.</param>
        protected void RaiseOpenCommand(object parameter) => OpenCommand?.Invoke(this, new(parameter));

        #endregion
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(MusicPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        public Commands.RelayCommand Edit => (Commands.RelayCommand)GetValue(EditProperty);

        /// <summary>
        /// Called when the Edit event is raised by <see cref="Edit" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Edit" />.</param>
        protected virtual void RaiseEditCommand(object parameter) => EditCommand?.Invoke(this, new(parameter));

        #endregion
        #region Delete Property Members

        /// <summary>
        /// Occurs when the <see cref="Delete">Delete Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> DeleteCommand;

        private static readonly DependencyPropertyKey DeletePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Delete),
            typeof(Commands.RelayCommand), typeof(MusicPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region Artist Property Members

        private static readonly DependencyPropertyKey ArtistPropertyKey = ColumnPropertyBuilder<string, MusicPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertiesListItem.Artist))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Artist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ArtistProperty = ArtistPropertyKey.DependencyProperty;

        public string Artist { get => GetValue(ArtistProperty) as string; private set => SetValue(ArtistPropertyKey, value); }

        #endregion
        #region Composer Property Members

        private static readonly DependencyPropertyKey ComposerPropertyKey = ColumnPropertyBuilder<string, MusicPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertiesListItem.Composer))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Composer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComposerProperty = ComposerPropertyKey.DependencyProperty;

        public string Composer { get => GetValue(ComposerProperty) as string; private set => SetValue(ComposerPropertyKey, value); }

        #endregion
        #region Conductor Property Members

        private static readonly DependencyPropertyKey ConductorPropertyKey = ColumnPropertyBuilder<string, MusicPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertiesListItem.Conductor))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Conductor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConductorProperty = ConductorPropertyKey.DependencyProperty;

        public string Conductor { get => GetValue(ConductorProperty) as string; private set => SetValue(ConductorPropertyKey, value); }

        #endregion
        #region Genre Property Members

        private static readonly DependencyPropertyKey GenrePropertyKey = ColumnPropertyBuilder<string, MusicPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertiesListItem.Genre))
            .DefaultValue("")
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="Genre"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GenreProperty = GenrePropertyKey.DependencyProperty;

        public string Genre { get => GetValue(GenreProperty) as string; private set => SetValue(GenrePropertyKey, value); }

        #endregion
        #region ExistingFileCount Property Members

        private static readonly DependencyPropertyKey ExistingFileCountPropertyKey = ColumnPropertyBuilder<long, MusicPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertiesListItem.ExistingFileCount))
            .DefaultValue(0L)
            .OnChanged((DependencyObject d, long oldValue, long newValue) =>
                (d as MusicPropertiesListItemViewModel<TEntity>).OnExistingFileCountPropertyChanged(newValue))
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="ExistingFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExistingFileCountProperty = ExistingFileCountPropertyKey.DependencyProperty;

        public long ExistingFileCount { get => (long)GetValue(ExistingFileCountProperty); private set => SetValue(ExistingFileCountPropertyKey, value); }

        /// <summary>
        /// Called when the value of the <see cref="ExistingFileCount"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="ExistingFileCount"/> property.</param>
        ///
        private void OnExistingFileCountPropertyChanged(long newValue) => Delete.IsEnabled = newValue == 0L;

        #endregion
        #region TotalFileCount Property Members

        private static readonly DependencyPropertyKey TotalFileCountPropertyKey = ColumnPropertyBuilder<long, MusicPropertiesListItemViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicPropertiesListItem.TotalFileCount))
            .DefaultValue(0L)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = TotalFileCountPropertyKey.DependencyProperty;

        public long TotalFileCount { get => (long)GetValue(TotalFileCountProperty); private set => SetValue(TotalFileCountPropertyKey, value); }

        #endregion

        public MusicPropertiesListItemViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            SetValue(OpenPropertyKey, new Commands.RelayCommand(RaiseOpenCommand));
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
            Artist = entity.Artist.ToNormalizedDelimitedText();
            Composer = entity.Composer.ToNormalizedDelimitedText();
            Conductor = entity.Conductor.ToNormalizedDelimitedText();
            Genre = entity.Genre.ToNormalizedDelimitedText();
            ExistingFileCount = entity.ExistingFileCount;
            TotalFileCount = entity.TotalFileCount;
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            CommonAttached.SetListItemTitle(this, CalculateDisplayText());
        }
    }
}
