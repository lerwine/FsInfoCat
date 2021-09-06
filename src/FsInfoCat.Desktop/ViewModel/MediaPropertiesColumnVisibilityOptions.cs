using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : DbEntity, IMediaPropertiesListItem
        where TViewModel : MediaPropertiesListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region ContentDistributor Property Members

        /// <summary>
        /// Identifies the <see cref="ContentDistributor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentDistributorProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ContentDistributor))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ContentDistributor { get => (bool)GetValue(ContentDistributorProperty); set => SetValue(ContentDistributorProperty, value); }

        #endregion
        #region CreatorApplication Property Members

        /// <summary>
        /// Identifies the <see cref="CreatorApplication"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatorApplicationProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CreatorApplication))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CreatorApplication { get => (bool)GetValue(CreatorApplicationProperty); set => SetValue(CreatorApplicationProperty, value); }

        #endregion
        #region CreatorApplicationVersion Property Members

        /// <summary>
        /// Identifies the <see cref="CreatorApplicationVersion"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatorApplicationVersionProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CreatorApplicationVersion))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CreatorApplicationVersion { get => (bool)GetValue(CreatorApplicationVersionProperty); set => SetValue(CreatorApplicationVersionProperty, value); }

        #endregion
        #region DateReleased Property Members

        /// <summary>
        /// Identifies the <see cref="DateReleased"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateReleasedProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DateReleased))
            .DefaultValue(false)
            .AsReadWrite();

        public bool DateReleased { get => (bool)GetValue(DateReleasedProperty); set => SetValue(DateReleasedProperty, value); }

        #endregion
        #region Duration Property Members

        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Duration))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Duration { get => (bool)GetValue(DurationProperty); set => SetValue(DurationProperty, value); }

        #endregion
        #region DVDID Property Members

        /// <summary>
        /// Identifies the <see cref="DVDID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DVDIDProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DVDID))
            .DefaultValue(false)
            .AsReadWrite();

        public bool DVDID { get => (bool)GetValue(DVDIDProperty); set => SetValue(DVDIDProperty, value); }

        #endregion
        #region FrameCount Property Members

        /// <summary>
        /// Identifies the <see cref="FrameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameCountProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(FrameCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool FrameCount { get => (bool)GetValue(FrameCountProperty); set => SetValue(FrameCountProperty, value); }

        #endregion
        #region Producer Property Members

        /// <summary>
        /// Identifies the <see cref="Producer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProducerProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Producer))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Producer { get => (bool)GetValue(ProducerProperty); set => SetValue(ProducerProperty, value); }

        #endregion
        #region ProtectionType Property Members

        /// <summary>
        /// Identifies the <see cref="ProtectionType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProtectionTypeProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ProtectionType))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ProtectionType { get => (bool)GetValue(ProtectionTypeProperty); set => SetValue(ProtectionTypeProperty, value); }

        #endregion
        #region ProviderRating Property Members

        /// <summary>
        /// Identifies the <see cref="ProviderRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderRatingProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ProviderRating))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ProviderRating { get => (bool)GetValue(ProviderRatingProperty); set => SetValue(ProviderRatingProperty, value); }

        #endregion
        #region ProviderStyle Property Members

        /// <summary>
        /// Identifies the <see cref="ProviderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderStyleProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ProviderStyle))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ProviderStyle { get => (bool)GetValue(ProviderStyleProperty); set => SetValue(ProviderStyleProperty, value); }

        #endregion
        #region Publisher Property Members

        /// <summary>
        /// Identifies the <see cref="Publisher"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PublisherProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Publisher))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Publisher { get => (bool)GetValue(PublisherProperty); set => SetValue(PublisherProperty, value); }

        #endregion
        #region Subtitle Property Members

        /// <summary>
        /// Identifies the <see cref="Subtitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Subtitle))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Subtitle { get => (bool)GetValue(SubtitleProperty); set => SetValue(SubtitleProperty, value); }

        #endregion
        #region Writer Property Members

        /// <summary>
        /// Identifies the <see cref="Writer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WriterProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Writer))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Writer { get => (bool)GetValue(WriterProperty); set => SetValue(WriterProperty, value); }

        #endregion
        #region Year Property Members

        /// <summary>
        /// Identifies the <see cref="Year"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YearProperty = DependencyPropertyBuilder<MediaPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Year))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Year { get => (bool)GetValue(YearProperty); set => SetValue(YearProperty, value); }

        #endregion

        protected MediaPropertiesColumnVisibilityOptions() : base() { }
    }
}
