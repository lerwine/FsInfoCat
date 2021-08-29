using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MusicPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IMusicProperties
    {
        #region AlbumArtist Property Members

        /// <summary>
        /// Identifies the <see cref="AlbumArtist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumArtistProperty = DependencyProperty.Register(nameof(AlbumArtist), typeof(string),
            typeof(MusicPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnAlbumArtistPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string AlbumArtist { get => GetValue(AlbumArtistProperty) as string; set => SetValue(AlbumArtistProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AlbumArtist"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AlbumArtist"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AlbumArtist"/> property.</param>
        private void OnAlbumArtistPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region AlbumTitle Property Members

        /// <summary>
        /// Identifies the <see cref="AlbumTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumTitleProperty = DependencyProperty.Register(nameof(AlbumTitle), typeof(string),
            typeof(MusicPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnAlbumTitlePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string AlbumTitle { get => GetValue(AlbumTitleProperty) as string; set => SetValue(AlbumTitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AlbumTitle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AlbumTitle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AlbumTitle"/> property.</param>
        private void OnAlbumTitlePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ChannelCount Property Members

        /// <summary>
        /// Identifies the <see cref="ChannelCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChannelCountProperty = DependencyProperty.Register(nameof(ChannelCount), typeof(uint?), typeof(MusicPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnChannelCountPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? ChannelCount { get => (uint?)GetValue(ChannelCountProperty); set => SetValue(ChannelCountProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ChannelCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ChannelCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ChannelCount"/> property.</param>
        private void OnChannelCountPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region DisplayArtist Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayArtist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayArtistProperty = DependencyProperty.Register(nameof(DisplayArtist), typeof(string),
            typeof(MusicPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnDisplayArtistPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayArtist { get => GetValue(DisplayArtistProperty) as string; set => SetValue(DisplayArtistProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayArtist"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayArtist"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayArtist"/> property.</param>
        private void OnDisplayArtistPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region PartOfSet Property Members

        /// <summary>
        /// Identifies the <see cref="PartOfSet"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PartOfSetProperty = DependencyProperty.Register(nameof(PartOfSet), typeof(string),
            typeof(MusicPropertiesRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnPartOfSetPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string PartOfSet { get => GetValue(PartOfSetProperty) as string; set => SetValue(PartOfSetProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PartOfSet"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PartOfSet"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PartOfSet"/> property.</param>
        private void OnPartOfSetPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Period Property Members

        /// <summary>
        /// Identifies the <see cref="Period"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof(Period), typeof(string),
            typeof(MusicPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnPeriodPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Period { get => GetValue(PeriodProperty) as string; set => SetValue(PeriodProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Period"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Period"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Period"/> property.</param>
        private void OnPeriodPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region TrackNumber Property Members

        /// <summary>
        /// Identifies the <see cref="TrackNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackNumberProperty = DependencyProperty.Register(nameof(TrackNumber), typeof(uint?), typeof(MusicPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as MusicPropertiesRowViewModel<TEntity>)?.OnTrackNumberPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? TrackNumber { get => (uint?)GetValue(TrackNumberProperty); set => SetValue(TrackNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="TrackNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="TrackNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="TrackNumber"/> property.</param>
        private void OnTrackNumberPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

        public MusicPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            AlbumArtist = entity.AlbumArtist;
            AlbumTitle = entity.AlbumTitle;
            ChannelCount = entity.ChannelCount;
            DisplayArtist = entity.DisplayArtist;
            PartOfSet = entity.PartOfSet;
            Period = entity.Period;
            TrackNumber = entity.TrackNumber;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IMusicProperties.AlbumArtist):
                    Dispatcher.CheckInvoke(() => AlbumArtist = Entity.AlbumArtist);
                    break;
                case nameof(IMusicProperties.AlbumTitle):
                    Dispatcher.CheckInvoke(() => AlbumTitle = Entity.AlbumTitle);
                    break;
                case nameof(IMusicProperties.ChannelCount):
                    Dispatcher.CheckInvoke(() => ChannelCount = Entity.ChannelCount);
                    break;
                case nameof(IMusicProperties.DisplayArtist):
                    Dispatcher.CheckInvoke(() => DisplayArtist = Entity.DisplayArtist);
                    break;
                case nameof(IMusicProperties.PartOfSet):
                    Dispatcher.CheckInvoke(() => PartOfSet = Entity.PartOfSet);
                    break;
                case nameof(IMusicProperties.Period):
                    Dispatcher.CheckInvoke(() => Period = Entity.Period);
                    break;
                case nameof(IMusicProperties.TrackNumber):
                    Dispatcher.CheckInvoke(() => TrackNumber = Entity.TrackNumber);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
