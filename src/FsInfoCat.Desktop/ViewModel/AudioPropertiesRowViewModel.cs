using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AudioPropertiesListItemViewModel<TEntity> : AudioPropertiesRowViewModel<TEntity>, ICrudEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IAudioPropertiesListItem
    {
        #region Edit Property Members

        /// <summary>
        /// Occurs when the <see cref="Edit">Edit Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> EditCommand;

        private static readonly DependencyPropertyKey EditPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Edit),
            typeof(Commands.RelayCommand), typeof(AudioPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Edit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EditProperty = EditPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
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
            typeof(Commands.RelayCommand), typeof(AudioPropertiesListItemViewModel<TEntity>), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Delete"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DeleteProperty = DeletePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Delete => (Commands.RelayCommand)GetValue(DeleteProperty);

        /// <summary>
        /// Called when the Delete event is raised by <see cref="Delete" />.
        /// </summary>
        /// <param name="parameter">The parameter value that was passed to the <see cref="System.Windows.Input.ICommand.Execute(object)"/> method on <see cref="Delete" />.</param>
        protected virtual void RaiseDeleteCommand(object parameter) => DeleteCommand?.Invoke(this, new(parameter));

        #endregion
        #region FileCount Property Members

        private static readonly DependencyPropertyKey FileCountPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileCount), typeof(long), typeof(AudioPropertiesListItemViewModel<TEntity>),
                new PropertyMetadata(0L));

        /// <summary>
        /// Identifies the <see cref="FileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileCountProperty = FileCountPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public long FileCount { get => (long)GetValue(FileCountProperty); private set => SetValue(FileCountPropertyKey, value); }

        #endregion

        public AudioPropertiesListItemViewModel(TEntity entity) : base(entity)
        {
            FileCount = entity.FileCount;
            SetValue(EditPropertyKey, new Commands.RelayCommand(RaiseEditCommand));
            SetValue(DeletePropertyKey, new Commands.RelayCommand(RaiseDeleteCommand));
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(FileCount))
                Dispatcher.CheckInvoke(() => FileCount = Entity.FileCount);
        }
    }
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
        protected void OnCompressionPropertyChanged(string oldValue, string newValue) { }

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
        protected void OnEncodingBitratePropertyChanged(uint? oldValue, uint? newValue)
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
        protected void OnFormatPropertyChanged(string oldValue, string newValue)
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
        protected void OnIsVariableBitratePropertyChanged(bool? oldValue, bool? newValue)
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
        protected void OnSampleRatePropertyChanged(uint? oldValue, uint? newValue)
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
        protected void OnSampleSizePropertyChanged(uint? oldValue, uint? newValue)
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
        protected void OnStreamNamePropertyChanged(string oldValue, string newValue)
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
        protected void OnStreamNumberPropertyChanged(ushort? oldValue, ushort? newValue)
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
