using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MediaPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IMediaProperties
    {
        #region ContentDistributor Property Members

        /// <summary>
        /// Identifies the <see cref="ContentDistributor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentDistributorProperty = DependencyProperty.Register(nameof(ContentDistributor), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnContentDistributorPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ContentDistributor { get => GetValue(ContentDistributorProperty) as string; set => SetValue(ContentDistributorProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ContentDistributor"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ContentDistributor"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ContentDistributor"/> property.</param>
        private void OnContentDistributorPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region CreatorApplication Property Members

        /// <summary>
        /// Identifies the <see cref="CreatorApplication"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatorApplicationProperty = DependencyProperty.Register(nameof(CreatorApplication), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnCreatorApplicationPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CreatorApplication { get => GetValue(CreatorApplicationProperty) as string; set => SetValue(CreatorApplicationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CreatorApplication"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CreatorApplication"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CreatorApplication"/> property.</param>
        private void OnCreatorApplicationPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region CreatorApplicationVersion Property Members

        /// <summary>
        /// Identifies the <see cref="CreatorApplicationVersion"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatorApplicationVersionProperty = DependencyProperty.Register(nameof(CreatorApplicationVersion), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnCreatorApplicationVersionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CreatorApplicationVersion { get => GetValue(CreatorApplicationVersionProperty) as string; set => SetValue(CreatorApplicationVersionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CreatorApplicationVersion"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CreatorApplicationVersion"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CreatorApplicationVersion"/> property.</param>
        private void OnCreatorApplicationVersionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region DateReleased Property Members

        /// <summary>
        /// Identifies the <see cref="DateReleased"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateReleasedProperty = DependencyProperty.Register(nameof(DateReleased), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnDateReleasedPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DateReleased { get => GetValue(DateReleasedProperty) as string; set => SetValue(DateReleasedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DateReleased"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DateReleased"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DateReleased"/> property.</param>
        private void OnDateReleasedPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Duration Property Members

        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof(Duration), typeof(ulong?),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnDurationPropertyChanged((ulong?)e.OldValue, (ulong?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ulong? Duration { get => (ulong?)GetValue(DurationProperty); set => SetValue(DurationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Duration"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Duration"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Duration"/> property.</param>
        private void OnDurationPropertyChanged(ulong? oldValue, ulong? newValue) { }

        #endregion
        #region DVDID Property Members

        /// <summary>
        /// Identifies the <see cref="DVDID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DVDIDProperty = DependencyProperty.Register(nameof(DVDID), typeof(string), typeof(MediaPropertiesRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnDVDIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DVDID { get => GetValue(DVDIDProperty) as string; set => SetValue(DVDIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DVDID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DVDID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DVDID"/> property.</param>
        private void OnDVDIDPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region FrameCount Property Members

        /// <summary>
        /// Identifies the <see cref="FrameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameCountProperty = DependencyProperty.Register(nameof(FrameCount), typeof(uint?),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnFrameCountPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? FrameCount { get => (uint?)GetValue(FrameCountProperty); set => SetValue(FrameCountProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameCount"/> property.</param>
        private void OnFrameCountPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region ProtectionType Property Members

        /// <summary>
        /// Identifies the <see cref="ProtectionType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProtectionTypeProperty = DependencyProperty.Register(nameof(ProtectionType), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnProtectionTypePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ProtectionType { get => GetValue(ProtectionTypeProperty) as string; set => SetValue(ProtectionTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProtectionType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProtectionType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProtectionType"/> property.</param>
        private void OnProtectionTypePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ProviderRating Property Members

        /// <summary>
        /// Identifies the <see cref="ProviderRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderRatingProperty = DependencyProperty.Register(nameof(ProviderRating), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnProviderRatingPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ProviderRating { get => GetValue(ProviderRatingProperty) as string; set => SetValue(ProviderRatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProviderRating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProviderRating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProviderRating"/> property.</param>
        private void OnProviderRatingPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ProviderStyle Property Members

        /// <summary>
        /// Identifies the <see cref="ProviderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderStyleProperty = DependencyProperty.Register(nameof(ProviderStyle), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnProviderStylePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ProviderStyle { get => GetValue(ProviderStyleProperty) as string; set => SetValue(ProviderStyleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProviderStyle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProviderStyle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProviderStyle"/> property.</param>
        private void OnProviderStylePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Publisher Property Members

        /// <summary>
        /// Identifies the <see cref="Publisher"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PublisherProperty = DependencyProperty.Register(nameof(Publisher), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnPublisherPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Publisher { get => GetValue(PublisherProperty) as string; set => SetValue(PublisherProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Publisher"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Publisher"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Publisher"/> property.</param>
        private void OnPublisherPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Subtitle Property Members

        /// <summary>
        /// Identifies the <see cref="Subtitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(nameof(Subtitle), typeof(string),
            typeof(MediaPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnSubtitlePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Subtitle { get => GetValue(SubtitleProperty) as string; set => SetValue(SubtitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Subtitle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Subtitle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Subtitle"/> property.</param>
        private void OnSubtitlePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Year Property Members

        /// <summary>
        /// Identifies the <see cref="Year"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YearProperty = DependencyProperty.Register(nameof(Year), typeof(uint?), typeof(MediaPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MediaPropertiesRowViewModel<TEntity>)?.OnYearPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? Year { get => (uint?)GetValue(YearProperty); set => SetValue(YearProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Year"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Year"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Year"/> property.</param>
        private void OnYearPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

        public MediaPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            ContentDistributor = entity.ContentDistributor;
            CreatorApplication = entity.CreatorApplication;
            CreatorApplicationVersion = entity.CreatorApplicationVersion;
            DateReleased = entity.DateReleased;
            Duration = entity.Duration;
            DVDID = entity.DVDID;
            FrameCount = entity.FrameCount;
            ProtectionType = entity.ProtectionType;
            ProviderRating = entity.ProviderRating;
            ProviderStyle = entity.ProviderStyle;
            Publisher = entity.Publisher;
            Subtitle = entity.Subtitle;
            Year = entity.Year;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IMediaProperties.ContentDistributor):
                    Dispatcher.CheckInvoke(() => ContentDistributor = Entity.ContentDistributor);
                    break;
                case nameof(IMediaProperties.CreatorApplication):
                    Dispatcher.CheckInvoke(() => CreatorApplication = Entity.CreatorApplication);
                    break;
                case nameof(IMediaProperties.CreatorApplicationVersion):
                    Dispatcher.CheckInvoke(() => CreatorApplicationVersion = Entity.CreatorApplicationVersion);
                    break;
                case nameof(IMediaProperties.DateReleased):
                    Dispatcher.CheckInvoke(() => DateReleased = Entity.DateReleased);
                    break;
                case nameof(IMediaProperties.Duration):
                    Dispatcher.CheckInvoke(() => Duration = Entity.Duration);
                    break;
                case nameof(IMediaProperties.DVDID):
                    Dispatcher.CheckInvoke(() => DVDID = Entity.DVDID);
                    break;
                case nameof(IMediaProperties.FrameCount):
                    Dispatcher.CheckInvoke(() => FrameCount = Entity.FrameCount);
                    break;
                case nameof(IMediaProperties.ProtectionType):
                    Dispatcher.CheckInvoke(() => ProtectionType = Entity.ProtectionType);
                    break;
                case nameof(IMediaProperties.ProviderRating):
                    Dispatcher.CheckInvoke(() => ProviderRating = Entity.ProviderRating);
                    break;
                case nameof(IMediaProperties.ProviderStyle):
                    Dispatcher.CheckInvoke(() => ProviderStyle = Entity.ProviderStyle);
                    break;
                case nameof(IMediaProperties.Publisher):
                    Dispatcher.CheckInvoke(() => Publisher = Entity.Publisher);
                    break;
                case nameof(IMediaProperties.Subtitle):
                    Dispatcher.CheckInvoke(() => Subtitle = Entity.Subtitle);
                    break;
                case nameof(IMediaProperties.Year):
                    Dispatcher.CheckInvoke(() => Year = Entity.Year);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
