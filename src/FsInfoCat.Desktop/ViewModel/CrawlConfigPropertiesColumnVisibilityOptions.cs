using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : Model.DbEntity, Model.ICrawlConfigurationListItem
        where TViewModel : CrawlConfigListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Compression))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Compression { get => (bool)GetValue(CompressionProperty); set => SetValue(CompressionProperty, value); }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(EncodingBitrate))
            .DefaultValue(false)
            .AsReadWrite();

        public bool EncodingBitrate { get => (bool)GetValue(EncodingBitrateProperty); set => SetValue(EncodingBitrateProperty, value); }

        #endregion
        #region Format Property Members

        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Format))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Format { get => (bool)GetValue(FormatProperty); set => SetValue(FormatProperty, value); }

        #endregion
        #region IsVariableBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="IsVariableBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVariableBitrateProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(IsVariableBitrate))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IsVariableBitrate { get => (bool)GetValue(IsVariableBitrateProperty); set => SetValue(IsVariableBitrateProperty, value); }

        #endregion
        #region SampleRate Property Members

        /// <summary>
        /// Identifies the <see cref="SampleRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleRateProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(SampleRate))
            .DefaultValue(false)
            .AsReadWrite();

        public bool SampleRate { get => (bool)GetValue(SampleRateProperty); set => SetValue(SampleRateProperty, value); }

        #endregion
        #region SampleSize Property Members

        /// <summary>
        /// Identifies the <see cref="SampleSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleSizeProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(SampleSize))
            .DefaultValue(false)
            .AsReadWrite();

        public bool SampleSize { get => (bool)GetValue(SampleSizeProperty); set => SetValue(SampleSizeProperty, value); }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StreamName))
            .DefaultValue(false)
            .AsReadWrite();

        public bool StreamName { get => (bool)GetValue(StreamNameProperty); set => SetValue(StreamNameProperty, value); }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = DependencyPropertyBuilder<CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StreamNumber))
            .DefaultValue(false)
            .AsReadWrite();

        public bool StreamNumber { get => (bool)GetValue(StreamNumberProperty); set => SetValue(StreamNumberProperty, value); }

        #endregion

        protected CrawlConfigPropertiesColumnVisibilityOptions() : base() { }
    }
}
