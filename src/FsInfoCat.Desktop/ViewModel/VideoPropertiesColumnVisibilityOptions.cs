using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : DbEntity, IVideoPropertiesListItem
        where TViewModel : VideoPropertiesListItemViewModel<TEntity>
    {
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Compression))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Compression { get => (bool)GetValue(CompressionProperty); set => SetValue(CompressionProperty, value); }

        #endregion
        #region Director Property Members

        /// <summary>
        /// Identifies the <see cref="Director"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectorProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Director))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Director { get => (bool)GetValue(DirectorProperty); set => SetValue(DirectorProperty, value); }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(EncodingBitrate))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool EncodingBitrate { get => (bool)GetValue(EncodingBitrateProperty); set => SetValue(EncodingBitrateProperty, value); }

        #endregion
        #region FrameHeight Property Members

        /// <summary>
        /// Identifies the <see cref="FrameHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameHeightProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(FrameHeight))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool FrameHeight { get => (bool)GetValue(FrameHeightProperty); set => SetValue(FrameHeightProperty, value); }

        #endregion
        #region FrameRate Property Members

        /// <summary>
        /// Identifies the <see cref="FrameRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(FrameRate))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool FrameRate { get => (bool)GetValue(FrameRateProperty); set => SetValue(FrameRateProperty, value); }

        #endregion
        #region FrameWidth Property Members

        /// <summary>
        /// Identifies the <see cref="FrameWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameWidthProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(FrameWidth))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool FrameWidth { get => (bool)GetValue(FrameWidthProperty); set => SetValue(FrameWidthProperty, value); }

        #endregion
        #region HorizontalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAspectRatioProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(HorizontalAspectRatio))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool HorizontalAspectRatio { get => (bool)GetValue(HorizontalAspectRatioProperty); set => SetValue(HorizontalAspectRatioProperty, value); }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StreamName))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool StreamName { get => (bool)GetValue(StreamNameProperty); set => SetValue(StreamNameProperty, value); }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StreamNumber))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool StreamNumber { get => (bool)GetValue(StreamNumberProperty); set => SetValue(StreamNumberProperty, value); }

        #endregion
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region VerticalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalAspectRatioProperty = DependencyPropertyBuilder<VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(VerticalAspectRatio))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as VideoPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool VerticalAspectRatio { get => (bool)GetValue(VerticalAspectRatioProperty); set => SetValue(VerticalAspectRatioProperty, value); }

        #endregion

        protected VideoPropertiesColumnVisibilityOptions() : base() { }
    }
}
