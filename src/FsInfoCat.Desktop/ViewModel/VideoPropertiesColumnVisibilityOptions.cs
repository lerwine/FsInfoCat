using System;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VideoPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region Director Property Members

        /// <summary>
        /// Identifies the <see cref="Director"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectorProperty = DependencyProperty.Register(nameof(Director), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool Director { get => (bool)GetValue(DirectorProperty); set => SetValue(DirectorProperty, value); }

        #endregion
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool Compression { get => (bool)GetValue(CompressionProperty); set => SetValue(CompressionProperty, value); }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool EncodingBitrate { get => (bool)GetValue(EncodingBitrateProperty); set => SetValue(EncodingBitrateProperty, value); }

        #endregion
        #region FrameRate Property Members

        private static readonly DependencyProperty FrameRateProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        public bool FrameRate { get => (bool)GetValue(FrameRateProperty); set => SetValue(FrameRateProperty, value); }

        #endregion
        #region FrameHeight Property Members

        /// <summary>
        /// Identifies the <see cref="FrameHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameHeightProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool FrameHeight { get => (bool)GetValue(FrameHeightProperty); set => SetValue(FrameHeightProperty, value); }

        #endregion
        #region FrameWidth Property Members

        /// <summary>
        /// Identifies the <see cref="FrameWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameWidthProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool FrameWidth { get => (bool)GetValue(FrameWidthProperty); set => SetValue(FrameWidthProperty, value); }

        #endregion
        #region HorizontalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAspectRatioProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool HorizontalAspectRatio { get => (bool)GetValue(HorizontalAspectRatioProperty); set => SetValue(HorizontalAspectRatioProperty, value); }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool StreamNumber { get => (bool)GetValue(StreamNumberProperty); set => SetValue(StreamNumberProperty, value); }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool StreamName { get => (bool)GetValue(StreamNameProperty); set => SetValue(StreamNameProperty, value); }

        #endregion
        #region VerticalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalAspectRatioProperty = DependencyProperty.Register(nameof(Compression), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool VerticalAspectRatio { get => (bool)GetValue(VerticalAspectRatioProperty); set => SetValue(VerticalAspectRatioProperty, value); }

        #endregion
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyProperty.Register(nameof(TotalFileCount), typeof(bool),
            typeof(VideoPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as VideoPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion

        protected VideoPropertiesColumnVisibilityOptions(params DependencyProperty[] properties) : base(properties.Concat(new DependencyProperty[]
        {
            TotalFileCountProperty, DirectorProperty, CompressionProperty, EncodingBitrateProperty, FrameHeightProperty, FrameRateProperty, FrameWidthProperty,
            HorizontalAspectRatioProperty, StreamNumberProperty, StreamNameProperty, VerticalAspectRatioProperty
        })) { }
    }
}
