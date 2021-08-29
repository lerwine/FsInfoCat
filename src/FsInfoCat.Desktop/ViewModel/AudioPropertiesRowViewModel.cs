using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AudioPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IAudioProperties
    {
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyProperty.Register(nameof(Compression), typeof(string),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        private void OnCompressionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCompressionPropertyChanged Logic
        }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = DependencyProperty.Register(nameof(EncodingBitrate), typeof(uint?),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnEncodingBitratePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

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
        private void OnEncodingBitratePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnEncodingBitratePropertyChanged Logic
        }

        #endregion
        #region Format Property Members

        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string), typeof(AudioPropertiesRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnFormatPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Format { get => GetValue(FormatProperty) as string; set => SetValue(FormatProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Format"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Format"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Format"/> property.</param>
        private void OnFormatPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnFormatPropertyChanged Logic
        }

        #endregion
        #region IsVariableBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="IsVariableBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVariableBitrateProperty = DependencyProperty.Register(nameof(IsVariableBitrate), typeof(bool?),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnIsVariableBitratePropertyChanged((bool?)e.OldValue, (bool?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool? IsVariableBitrate { get => (bool?)GetValue(IsVariableBitrateProperty); set => SetValue(IsVariableBitrateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsVariableBitrate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsVariableBitrate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsVariableBitrate"/> property.</param>
        private void OnIsVariableBitratePropertyChanged(bool? oldValue, bool? newValue)
        {
            // TODO: Implement OnIsVariableBitratePropertyChanged Logic
        }

        #endregion
        #region SampleRate Property Members

        /// <summary>
        /// Identifies the <see cref="SampleRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleRateProperty = DependencyProperty.Register(nameof(SampleRate), typeof(uint?),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnSampleRatePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? SampleRate { get => (uint?)GetValue(SampleRateProperty); set => SetValue(SampleRateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SampleRate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SampleRate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SampleRate"/> property.</param>
        private void OnSampleRatePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnSampleRatePropertyChanged Logic
        }

        #endregion
        #region SampleSize Property Members

        /// <summary>
        /// Identifies the <see cref="SampleSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleSizeProperty = DependencyProperty.Register(nameof(SampleSize), typeof(uint?),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnSampleSizePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? SampleSize { get => (uint?)GetValue(SampleSizeProperty); set => SetValue(SampleSizeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SampleSize"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SampleSize"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SampleSize"/> property.</param>
        private void OnSampleSizePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnSampleSizePropertyChanged Logic
        }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = DependencyProperty.Register(nameof(StreamName), typeof(string),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnStreamNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        private void OnStreamNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStreamNamePropertyChanged Logic
        }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = DependencyProperty.Register(nameof(StreamNumber), typeof(ushort?),
            typeof(AudioPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as AudioPropertiesRowViewModel<TEntity>)?.OnStreamNumberPropertyChanged((ushort?)e.OldValue, (ushort?)e.NewValue)));

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
        private void OnStreamNumberPropertyChanged(ushort? oldValue, ushort? newValue)
        {
            // TODO: Implement OnStreamNumberPropertyChanged Logic
        }

        #endregion

        public AudioPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            Compression = entity.Compression;
            EncodingBitrate = entity.EncodingBitrate;
            Format = entity.Format;
            IsVariableBitrate = entity.IsVariableBitrate;
            SampleRate = entity.SampleRate;
            SampleSize = entity.SampleSize;
            StreamName = entity.StreamName;
            StreamNumber = entity.StreamNumber;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IAudioProperties.Compression):
                    Dispatcher.CheckInvoke(() => Compression = Entity.Compression);
                    break;
                case nameof(IAudioProperties.EncodingBitrate):
                    Dispatcher.CheckInvoke(() => EncodingBitrate = Entity.EncodingBitrate);
                    break;
                case nameof(IAudioProperties.Format):
                    Dispatcher.CheckInvoke(() => Format = Entity.Format);
                    break;
                case nameof(IAudioProperties.IsVariableBitrate):
                    Dispatcher.CheckInvoke(() => IsVariableBitrate = Entity.IsVariableBitrate);
                    break;
                case nameof(IAudioProperties.SampleRate):
                    Dispatcher.CheckInvoke(() => SampleRate = Entity.SampleRate);
                    break;
                case nameof(IAudioProperties.SampleSize):
                    Dispatcher.CheckInvoke(() => SampleSize = Entity.SampleSize);
                    break;
                case nameof(IAudioProperties.StreamName):
                    Dispatcher.CheckInvoke(() => StreamName = Entity.StreamName);
                    break;
                case nameof(IAudioProperties.StreamNumber):
                    Dispatcher.CheckInvoke(() => StreamNumber = Entity.StreamNumber);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
