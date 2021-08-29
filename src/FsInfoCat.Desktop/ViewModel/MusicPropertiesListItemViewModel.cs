using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MusicPropertiesListItemViewModel<TEntity> : MusicPropertiesRowViewModel<TEntity>
        where TEntity : DbEntity, IMusicProperties
    {
        #region Artist Property Members

        private static readonly DependencyPropertyKey ArtistPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Artist), typeof(string), typeof(MusicPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Artist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ArtistProperty = ArtistPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Artist { get => GetValue(ArtistProperty) as string; private set => SetValue(ArtistPropertyKey, value); }

        #endregion
        #region Composer Property Members

        private static readonly DependencyPropertyKey ComposerPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Composer), typeof(string), typeof(MusicPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Composer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ComposerProperty = ComposerPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Composer { get => GetValue(ComposerProperty) as string; private set => SetValue(ComposerPropertyKey, value); }

        #endregion
        #region Conductor Property Members

        private static readonly DependencyPropertyKey ConductorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Conductor), typeof(string), typeof(MusicPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Conductor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConductorProperty = ConductorPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Conductor { get => GetValue(ConductorProperty) as string; private set => SetValue(ConductorPropertyKey, value); }

        #endregion
        #region Genre Property Members

        private static readonly DependencyPropertyKey GenrePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Genre), typeof(string), typeof(MusicPropertiesListItemViewModel<TEntity>),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="Genre"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GenreProperty = GenrePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Genre { get => GetValue(GenreProperty) as string; private set => SetValue(GenrePropertyKey, value); }

        #endregion

        public MusicPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            Artist = entity.Artist.ToNormalizedDelimitedText();
            Composer = entity.Composer.ToNormalizedDelimitedText();
            Conductor = entity.Conductor.ToNormalizedDelimitedText();
            Genre = entity.Genre.ToNormalizedDelimitedText();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IMusicProperties.Artist):
                    Dispatcher.CheckInvoke(() => Artist = Entity.Artist.ToNormalizedDelimitedText());
                    break;
                case nameof(IMusicProperties.Composer):
                    Dispatcher.CheckInvoke(() => Composer = Entity.Composer.ToNormalizedDelimitedText());
                    break;
                case nameof(IMusicProperties.Conductor):
                    Dispatcher.CheckInvoke(() => Conductor = Entity.Conductor.ToNormalizedDelimitedText());
                    break;
                case nameof(IMusicProperties.Genre):
                    Dispatcher.CheckInvoke(() => Genre = Entity.Genre.ToNormalizedDelimitedText());
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
