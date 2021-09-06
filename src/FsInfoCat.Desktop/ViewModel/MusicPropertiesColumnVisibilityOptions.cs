using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : DbEntity, IMusicPropertiesListItem
        where TViewModel : MusicPropertiesListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region AlbumArtist Property Members

        /// <summary>
        /// Identifies the <see cref="AlbumArtist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumArtistProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(AlbumArtist))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool AlbumArtist { get => (bool)GetValue(AlbumArtistProperty); set => SetValue(AlbumArtistProperty, value); }

        #endregion
        #region AlbumTitle Property Members

        /// <summary>
        /// Identifies the <see cref="AlbumTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumTitleProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(AlbumTitle))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool AlbumTitle { get => (bool)GetValue(AlbumTitleProperty); set => SetValue(AlbumTitleProperty, value); }

        #endregion
        #region Artist Property Members

        /// <summary>
        /// Identifies the <see cref="Artist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ArtistProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Artist))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Artist { get => (bool)GetValue(ArtistProperty); set => SetValue(ArtistProperty, value); }

        #endregion
        #region ChannelCount Property Members

        /// <summary>
        /// Identifies the <see cref="ChannelCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChannelCountProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ChannelCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ChannelCount { get => (bool)GetValue(ChannelCountProperty); set => SetValue(ChannelCountProperty, value); }

        #endregion
        #region Composer Property Members

        /// <summary>
        /// Identifies the <see cref="Composer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComposerProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Composer))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Composer { get => (bool)GetValue(ComposerProperty); set => SetValue(ComposerProperty, value); }

        #endregion
        #region Conductor Property Members

        /// <summary>
        /// Identifies the <see cref="Conductor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConductorProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Conductor))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Conductor { get => (bool)GetValue(ConductorProperty); set => SetValue(ConductorProperty, value); }

        #endregion
        #region DisplayArtist Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayArtist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayArtistProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DisplayArtist))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool DisplayArtist { get => (bool)GetValue(DisplayArtistProperty); set => SetValue(DisplayArtistProperty, value); }

        #endregion
        #region Genre Property Members

        /// <summary>
        /// Identifies the <see cref="Genre"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GenreProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Genre))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Genre { get => (bool)GetValue(GenreProperty); set => SetValue(GenreProperty, value); }

        #endregion
        #region PartOfSet Property Members

        /// <summary>
        /// Identifies the <see cref="PartOfSet"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PartOfSetProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(PartOfSet))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool PartOfSet { get => (bool)GetValue(PartOfSetProperty); set => SetValue(PartOfSetProperty, value); }

        #endregion
        #region Period Property Members

        /// <summary>
        /// Identifies the <see cref="Period"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeriodProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Period))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Period { get => (bool)GetValue(PeriodProperty); set => SetValue(PeriodProperty, value); }

        #endregion
        #region TrackNumber Property Members

        /// <summary>
        /// Identifies the <see cref="TrackNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackNumberProperty = DependencyPropertyBuilder<MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TrackNumber))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as MusicPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool TrackNumber { get => (bool)GetValue(TrackNumberProperty); set => SetValue(TrackNumberProperty, value); }

        #endregion

        protected MusicPropertiesColumnVisibilityOptions() : base() { }
    }
}
