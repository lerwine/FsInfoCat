using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : Model.DbEntity, Model.IImagePropertiesListItem
        where TViewModel : ImagePropertiesListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region BitDepth Property Members

        /// <summary>
        /// Identifies the <see cref="BitDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BitDepthProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(BitDepth))
            .DefaultValue(false)
            .AsReadWrite();

        public bool BitDepth { get => (bool)GetValue(BitDepthProperty); set => SetValue(BitDepthProperty, value); }

        #endregion
        #region ColorSpace Property Members

        /// <summary>
        /// Identifies the <see cref="ColorSpace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorSpaceProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ColorSpace))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ColorSpace { get => (bool)GetValue(ColorSpaceProperty); set => SetValue(ColorSpaceProperty, value); }

        #endregion
        #region CompressedBitsPerPixel Property Members

        /// <summary>
        /// Identifies the <see cref="CompressedBitsPerPixel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressedBitsPerPixelProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CompressedBitsPerPixel))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CompressedBitsPerPixel { get => (bool)GetValue(CompressedBitsPerPixelProperty); set => SetValue(CompressedBitsPerPixelProperty, value); }

        #endregion
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Compression))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Compression { get => (bool)GetValue(CompressionProperty); set => SetValue(CompressionProperty, value); }

        #endregion
        #region CompressionText Property Members

        /// <summary>
        /// Identifies the <see cref="CompressionText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionTextProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CompressionText))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CompressionText { get => (bool)GetValue(CompressionTextProperty); set => SetValue(CompressionTextProperty, value); }

        #endregion
        #region HorizontalResolution Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalResolutionProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(HorizontalResolution))
            .DefaultValue(false)
            .AsReadWrite();

        public bool HorizontalResolution { get => (bool)GetValue(HorizontalResolutionProperty); set => SetValue(HorizontalResolutionProperty, value); }

        #endregion
        #region HorizontalSize Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalSizeProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(HorizontalSize))
            .DefaultValue(false)
            .AsReadWrite();

        public bool HorizontalSize { get => (bool)GetValue(HorizontalSizeProperty); set => SetValue(HorizontalSizeProperty, value); }

        #endregion
        #region ImageID Property Members

        /// <summary>
        /// Identifies the <see cref="ImageID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageIDProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ImageID))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ImageID { get => (bool)GetValue(ImageIDProperty); set => SetValue(ImageIDProperty, value); }

        #endregion
        #region ResolutionUnit Property Members

        /// <summary>
        /// Identifies the <see cref="ResolutionUnit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResolutionUnitProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ResolutionUnit))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ResolutionUnit { get => (bool)GetValue(ResolutionUnitProperty); set => SetValue(ResolutionUnitProperty, value); }

        #endregion
        #region VerticalResolution Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalResolutionProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(VerticalResolution))
            .DefaultValue(false)
            .AsReadWrite();

        public bool VerticalResolution { get => (bool)GetValue(VerticalResolutionProperty); set => SetValue(VerticalResolutionProperty, value); }

        #endregion
        #region VerticalSize Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalSizeProperty = DependencyPropertyBuilder<ImagePropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(VerticalSize))
            .DefaultValue(false)
            .AsReadWrite();

        public bool VerticalSize { get => (bool)GetValue(VerticalSizeProperty); set => SetValue(VerticalSizeProperty, value); }

        #endregion

        protected ImagePropertiesColumnVisibilityOptions() : base() { }
    }
}
