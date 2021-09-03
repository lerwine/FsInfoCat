using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ImagePropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IImageProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region BitDepth Property Members

        /// <summary>
        /// Identifies the <see cref="BitDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BitDepthProperty = DependencyProperty.Register(nameof(BitDepth), typeof(uint?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnBitDepthPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? BitDepth { get => (uint?)GetValue(BitDepthProperty); set => SetValue(BitDepthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="BitDepth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="BitDepth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="BitDepth"/> property.</param>
        protected void OnBitDepthPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnBitDepthPropertyChanged Logic
        }

        #endregion
        #region ColorSpace Property Members

        /// <summary>
        /// Identifies the <see cref="ColorSpace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorSpaceProperty = DependencyProperty.Register(nameof(ColorSpace), typeof(uint?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnColorSpacePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? ColorSpace { get => (uint?)GetValue(ColorSpaceProperty); set => SetValue(ColorSpaceProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ColorSpace"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ColorSpace"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ColorSpace"/> property.</param>
        protected void OnColorSpacePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnColorSpacePropertyChanged Logic
        }

        #endregion
        #region CompressedBitsPerPixel Property Members

        /// <summary>
        /// Identifies the <see cref="CompressedBitsPerPixel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressedBitsPerPixelProperty = DependencyProperty.Register(nameof(CompressedBitsPerPixel), typeof(double?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnCompressedBitsPerPixelPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? CompressedBitsPerPixel { get => (double?)GetValue(CompressedBitsPerPixelProperty); set => SetValue(CompressedBitsPerPixelProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CompressedBitsPerPixel"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CompressedBitsPerPixel"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CompressedBitsPerPixel"/> property.</param>
        protected void OnCompressedBitsPerPixelPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnCompressedBitsPerPixelPropertyChanged Logic
        }

        #endregion
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = DependencyProperty.Register(nameof(Compression), typeof(ushort?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged((ushort?)e.OldValue, (ushort?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ushort? Compression { get => (ushort?)GetValue(CompressionProperty); set => SetValue(CompressionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Compression"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Compression"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Compression"/> property.</param>
        protected void OnCompressionPropertyChanged(ushort? oldValue, ushort? newValue)
        {
            // TODO: Implement OnCompressionPropertyChanged Logic
        }

        #endregion
        #region CompressionText Property Members

        /// <summary>
        /// Identifies the <see cref="CompressionText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionTextProperty = DependencyProperty.Register(nameof(CompressionText), typeof(string),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnCompressionTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CompressionText { get => GetValue(CompressionTextProperty) as string; set => SetValue(CompressionTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CompressionText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CompressionText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CompressionText"/> property.</param>
        protected void OnCompressionTextPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCompressionTextPropertyChanged Logic
        }

        #endregion
        #region HorizontalResolution Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalResolutionProperty = DependencyProperty.Register(nameof(HorizontalResolution), typeof(double?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnHorizontalResolutionPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? HorizontalResolution { get => (double?)GetValue(HorizontalResolutionProperty); set => SetValue(HorizontalResolutionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HorizontalResolution"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HorizontalResolution"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HorizontalResolution"/> property.</param>
        protected void OnHorizontalResolutionPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnHorizontalResolutionPropertyChanged Logic
        }

        #endregion
        #region HorizontalSize Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalSizeProperty = DependencyProperty.Register(nameof(HorizontalSize), typeof(uint?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnHorizontalSizePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? HorizontalSize { get => (uint?)GetValue(HorizontalSizeProperty); set => SetValue(HorizontalSizeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HorizontalSize"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HorizontalSize"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HorizontalSize"/> property.</param>
        protected void OnHorizontalSizePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnHorizontalSizePropertyChanged Logic
        }

        #endregion
        #region ImageID Property Members

        /// <summary>
        /// Identifies the <see cref="ImageID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageIDProperty = DependencyProperty.Register(nameof(ImageID), typeof(string),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as ImagePropertiesRowViewModel<TEntity>)?.OnImageIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ImageID { get => GetValue(ImageIDProperty) as string; set => SetValue(ImageIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ImageID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ImageID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ImageID"/> property.</param>
        protected void OnImageIDPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnImageIDPropertyChanged Logic
        }

        #endregion
        #region ResolutionUnit Property Members

        /// <summary>
        /// Identifies the <see cref="ResolutionUnit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResolutionUnitProperty = DependencyProperty.Register(nameof(ResolutionUnit), typeof(short?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnResolutionUnitPropertyChanged((short?)e.OldValue, (short?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public short? ResolutionUnit { get => (short?)GetValue(ResolutionUnitProperty); set => SetValue(ResolutionUnitProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ResolutionUnit"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ResolutionUnit"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ResolutionUnit"/> property.</param>
        protected void OnResolutionUnitPropertyChanged(short? oldValue, short? newValue)
        {
            // TODO: Implement OnResolutionUnitPropertyChanged Logic
        }

        #endregion
        #region VerticalResolution Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalResolutionProperty = DependencyProperty.Register(nameof(VerticalResolution), typeof(double?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnVerticalResolutionPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? VerticalResolution { get => (double?)GetValue(VerticalResolutionProperty); set => SetValue(VerticalResolutionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VerticalResolution"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VerticalResolution"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VerticalResolution"/> property.</param>
        protected void OnVerticalResolutionPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnVerticalResolutionPropertyChanged Logic
        }

        #endregion
        #region VerticalSize Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalSizeProperty = DependencyProperty.Register(nameof(VerticalSize), typeof(uint?),
            typeof(ImagePropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as ImagePropertiesRowViewModel<TEntity>)?.OnVerticalSizePropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? VerticalSize { get => (uint?)GetValue(VerticalSizeProperty); set => SetValue(VerticalSizeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VerticalSize"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VerticalSize"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VerticalSize"/> property.</param>
        protected void OnVerticalSizePropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnVerticalSizePropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public ImagePropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            BitDepth = entity.BitDepth;
            ColorSpace = entity.ColorSpace;
            CompressedBitsPerPixel = entity.CompressedBitsPerPixel;
            Compression = entity.Compression;
            CompressionText = entity.CompressionText;
            HorizontalResolution = entity.HorizontalResolution;
            HorizontalSize = entity.HorizontalSize;
            ImageID = entity.ImageID;
            ResolutionUnit = entity.ResolutionUnit;
            VerticalResolution = entity.VerticalResolution;
            VerticalSize = entity.VerticalSize;
        }

        internal string CalculateDisplayText()
        {
            // TODO: Calculate value for ListingViewModel<TEntity, TItem, TOptions>.SetItemDisplayText(string)
            throw new System.NotImplementedException();
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IImageProperties.BitDepth):
                    Dispatcher.CheckInvoke(() => BitDepth = Entity.BitDepth);
                    break;
                case nameof(IImageProperties.ColorSpace):
                    Dispatcher.CheckInvoke(() => ColorSpace = Entity.ColorSpace);
                    break;
                case nameof(IImageProperties.CompressedBitsPerPixel):
                    Dispatcher.CheckInvoke(() => CompressedBitsPerPixel = Entity.CompressedBitsPerPixel);
                    break;
                case nameof(IImageProperties.Compression):
                    Dispatcher.CheckInvoke(() => Compression = Entity.Compression);
                    break;
                case nameof(IImageProperties.CompressionText):
                    Dispatcher.CheckInvoke(() => CompressionText = Entity.CompressionText);
                    break;
                case nameof(IImageProperties.HorizontalResolution):
                    Dispatcher.CheckInvoke(() => HorizontalResolution = Entity.HorizontalResolution);
                    break;
                case nameof(IImageProperties.HorizontalSize):
                    Dispatcher.CheckInvoke(() => HorizontalSize = Entity.HorizontalSize);
                    break;
                case nameof(IImageProperties.ImageID):
                    Dispatcher.CheckInvoke(() => ImageID = Entity.ImageID);
                    break;
                case nameof(IImageProperties.ResolutionUnit):
                    Dispatcher.CheckInvoke(() => ResolutionUnit = Entity.ResolutionUnit);
                    break;
                case nameof(IImageProperties.VerticalResolution):
                    Dispatcher.CheckInvoke(() => VerticalResolution = Entity.VerticalResolution);
                    break;
                case nameof(IImageProperties.VerticalSize):
                    Dispatcher.CheckInvoke(() => VerticalSize = Entity.VerticalSize);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
