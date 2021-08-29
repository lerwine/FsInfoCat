using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VideoPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IVideoProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyProperty.Register(nameof(Compression), typeof(string),
            typeof(VideoPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Compression { get => GetValue(CompressionProperty) as string; set => SetValue(CompressionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Compression"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Compression"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Compression"/> property.</param>
        protected void OnCompressionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCompressionPropertyChanged Logic
        }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = DependencyProperty.Register(nameof(EncodingBitrate), typeof(uint?),
            typeof(VideoPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnEncodingBitratePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? EncodingBitrate { get => (uint?)GetValue(EncodingBitrateProperty); set => SetValue(EncodingBitrateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="EncodingBitrate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="EncodingBitrate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="EncodingBitrate"/> property.</param>
        protected void OnEncodingBitratePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnEncodingBitratePropertyChanged Logic
        }

        #endregion
        #region FrameHeight Property Members

        /// <summary>
        /// Identifies the <see cref="FrameHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameHeightProperty = DependencyProperty.Register(nameof(FrameHeight), typeof(uint?), typeof(VideoPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnFrameHeightPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? FrameHeight { get => (uint?)GetValue(FrameHeightProperty); set => SetValue(FrameHeightProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameHeight"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameHeight"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameHeight"/> property.</param>
        protected void OnFrameHeightPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnFrameHeightPropertyChanged Logic
        }

        #endregion
        #region FrameRate Property Members

        /// <summary>
        /// Identifies the <see cref="FrameRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty = DependencyProperty.Register(nameof(FrameRate), typeof(uint?), typeof(VideoPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnFrameRatePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? FrameRate { get => (uint?)GetValue(FrameRateProperty); set => SetValue(FrameRateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameRate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameRate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameRate"/> property.</param>
        protected void OnFrameRatePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnFrameRatePropertyChanged Logic
        }

        #endregion
        #region FrameWidth Property Members

        /// <summary>
        /// Identifies the <see cref="FrameWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameWidthProperty = DependencyProperty.Register(nameof(FrameWidth), typeof(uint?), typeof(VideoPropertiesRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnFrameWidthPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? FrameWidth { get => (uint?)GetValue(FrameWidthProperty); set => SetValue(FrameWidthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameWidth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameWidth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameWidth"/> property.</param>
        protected void OnFrameWidthPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnFrameWidthPropertyChanged Logic
        }

        #endregion
        #region HorizontalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAspectRatioProperty = DependencyProperty.Register(nameof(HorizontalAspectRatio), typeof(uint?),
            typeof(VideoPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnHorizontalAspectRatioPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? HorizontalAspectRatio { get => (uint?)GetValue(HorizontalAspectRatioProperty); set => SetValue(HorizontalAspectRatioProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HorizontalAspectRatio"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HorizontalAspectRatio"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HorizontalAspectRatio"/> property.</param>
        protected void OnHorizontalAspectRatioPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnHorizontalAspectRatioPropertyChanged Logic
        }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = DependencyProperty.Register(nameof(StreamNumber), typeof(ushort?),
            typeof(VideoPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnStreamNumberPropertyChanged((ushort?)e.OldValue, (ushort?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ushort? StreamNumber { get => (ushort?)GetValue(StreamNumberProperty); set => SetValue(StreamNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StreamNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StreamNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StreamNumber"/> property.</param>
        protected void OnStreamNumberPropertyChanged(ushort? oldValue, ushort? newValue)
        {
            // TODO: Implement OnStreamNumberPropertyChanged Logic
        }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = DependencyProperty.Register(nameof(StreamName), typeof(string),
            typeof(VideoPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesRowViewModel<TEntity>)?.OnStreamNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string StreamName { get => GetValue(StreamNameProperty) as string; set => SetValue(StreamNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StreamName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StreamName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StreamName"/> property.</param>
        protected void OnStreamNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStreamNamePropertyChanged Logic
        }

        #endregion
        #region VerticalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalAspectRatioProperty = DependencyProperty.Register(nameof(VerticalAspectRatio), typeof(uint?),
            typeof(VideoPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VideoPropertiesRowViewModel<TEntity>)?.OnVerticalAspectRatioPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? VerticalAspectRatio { get => (uint?)GetValue(VerticalAspectRatioProperty); set => SetValue(VerticalAspectRatioProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VerticalAspectRatio"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VerticalAspectRatio"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VerticalAspectRatio"/> property.</param>
        protected void OnVerticalAspectRatioPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnVerticalAspectRatioPropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public VideoPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            Compression = entity.Compression;
            EncodingBitrate = entity.EncodingBitrate;
            FrameHeight = entity.FrameHeight;
            FrameRate = entity.FrameRate;
            FrameWidth = entity.FrameWidth;
            HorizontalAspectRatio = entity.HorizontalAspectRatio;
            StreamNumber = entity.StreamNumber;
            StreamName = entity.StreamName;
            VerticalAspectRatio = entity.VerticalAspectRatio;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IVideoProperties.Compression):
                    Dispatcher.CheckInvoke(() => Compression = Entity.Compression);
                    break;
                case nameof(IVideoProperties.EncodingBitrate):
                    Dispatcher.CheckInvoke(() => EncodingBitrate = Entity.EncodingBitrate);
                    break;
                case nameof(IVideoProperties.FrameHeight):
                    Dispatcher.CheckInvoke(() => FrameHeight = Entity.FrameHeight);
                    break;
                case nameof(IVideoProperties.FrameRate):
                    Dispatcher.CheckInvoke(() => FrameRate = Entity.FrameRate);
                    break;
                case nameof(IVideoProperties.FrameWidth):
                    Dispatcher.CheckInvoke(() => FrameWidth = Entity.FrameWidth);
                    break;
                case nameof(IVideoProperties.HorizontalAspectRatio):
                    Dispatcher.CheckInvoke(() => HorizontalAspectRatio = Entity.HorizontalAspectRatio);
                    break;
                case nameof(IVideoProperties.StreamNumber):
                    Dispatcher.CheckInvoke(() => StreamNumber = Entity.StreamNumber);
                    break;
                case nameof(IVideoProperties.StreamName):
                    Dispatcher.CheckInvoke(() => StreamName = Entity.StreamName);
                    break;
                case nameof(IVideoProperties.VerticalAspectRatio):
                    Dispatcher.CheckInvoke(() => VerticalAspectRatio = Entity.VerticalAspectRatio);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
