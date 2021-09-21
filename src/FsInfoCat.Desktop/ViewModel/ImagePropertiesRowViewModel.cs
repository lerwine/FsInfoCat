using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ImagePropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IImageProperties
    {
        #region BitDepth Property Members

        /// <summary>
        /// Identifies the <see cref="BitDepth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BitDepthProperty = ColumnPropertyBuilder<uint?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.BitDepth))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnBitDepthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? BitDepth { get => (uint?)GetValue(BitDepthProperty); set => SetValue(BitDepthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="BitDepth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="BitDepth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="BitDepth"/> property.</param>
        protected virtual void OnBitDepthPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region ColorSpace Property Members

        /// <summary>
        /// Identifies the <see cref="ColorSpace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorSpaceProperty = ColumnPropertyBuilder<ushort?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.ColorSpace))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnColorSpacePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort? ColorSpace { get => (ushort?)GetValue(ColorSpaceProperty); set => SetValue(ColorSpaceProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ColorSpace"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ColorSpace"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ColorSpace"/> property.</param>
        protected virtual void OnColorSpacePropertyChanged(ushort? oldValue, ushort? newValue) { }

        #endregion
        #region CompressedBitsPerPixel Property Members

        /// <summary>
        /// Identifies the <see cref="CompressedBitsPerPixel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressedBitsPerPixelProperty = ColumnPropertyBuilder<double?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.CompressedBitsPerPixel))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnCompressedBitsPerPixelPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? CompressedBitsPerPixel { get => (double?)GetValue(CompressedBitsPerPixelProperty); set => SetValue(CompressedBitsPerPixelProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CompressedBitsPerPixel"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CompressedBitsPerPixel"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CompressedBitsPerPixel"/> property.</param>
        protected virtual void OnCompressedBitsPerPixelPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = ColumnPropertyBuilder<ushort?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.Compression))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort? Compression { get => (ushort?)GetValue(CompressionProperty); set => SetValue(CompressionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Compression"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Compression"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Compression"/> property.</param>
        protected virtual void OnCompressionPropertyChanged(ushort? oldValue, ushort? newValue) { }

        #endregion
        #region CompressionText Property Members

        /// <summary>
        /// Identifies the <see cref="CompressionText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionTextProperty = ColumnPropertyBuilder<string, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.CompressionText))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnCompressionTextPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string CompressionText { get => GetValue(CompressionTextProperty) as string; set => SetValue(CompressionTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CompressionText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CompressionText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CompressionText"/> property.</param>
        protected virtual void OnCompressionTextPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region HorizontalResolution Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalResolutionProperty = ColumnPropertyBuilder<double?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.HorizontalResolution))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnHorizontalResolutionPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? HorizontalResolution { get => (double?)GetValue(HorizontalResolutionProperty); set => SetValue(HorizontalResolutionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HorizontalResolution"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HorizontalResolution"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HorizontalResolution"/> property.</param>
        protected virtual void OnHorizontalResolutionPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region HorizontalSize Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalSizeProperty = ColumnPropertyBuilder<uint?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.HorizontalSize))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnHorizontalSizePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? HorizontalSize { get => (uint?)GetValue(HorizontalSizeProperty); set => SetValue(HorizontalSizeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HorizontalSize"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HorizontalSize"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HorizontalSize"/> property.</param>
        protected virtual void OnHorizontalSizePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region ImageID Property Members

        /// <summary>
        /// Identifies the <see cref="ImageID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageIDProperty = ColumnPropertyBuilder<string, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.ImageID))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnImageIDPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ImageID { get => GetValue(ImageIDProperty) as string; set => SetValue(ImageIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ImageID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ImageID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ImageID"/> property.</param>
        protected virtual void OnImageIDPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ResolutionUnit Property Members

        /// <summary>
        /// Identifies the <see cref="ResolutionUnit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ResolutionUnitProperty = ColumnPropertyBuilder<short?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.ResolutionUnit))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnResolutionUnitPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public short? ResolutionUnit { get => (short?)GetValue(ResolutionUnitProperty); set => SetValue(ResolutionUnitProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ResolutionUnit"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ResolutionUnit"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ResolutionUnit"/> property.</param>
        protected virtual void OnResolutionUnitPropertyChanged(short? oldValue, short? newValue) { }

        #endregion
        #region VerticalResolution Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalResolution"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalResolutionProperty = ColumnPropertyBuilder<double?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.VerticalResolution))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnVerticalResolutionPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? VerticalResolution { get => (double?)GetValue(VerticalResolutionProperty); set => SetValue(VerticalResolutionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VerticalResolution"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VerticalResolution"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VerticalResolution"/> property.</param>
        protected virtual void OnVerticalResolutionPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region VerticalSize Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalSizeProperty = ColumnPropertyBuilder<uint?, ImagePropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IImageProperties.VerticalSize))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as ImagePropertiesRowViewModel<TEntity>)?.OnVerticalSizePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? VerticalSize { get => (uint?)GetValue(VerticalSizeProperty); set => SetValue(VerticalSizeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VerticalSize"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VerticalSize"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VerticalSize"/> property.</param>
        protected virtual void OnVerticalSizePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

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

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_BitDepth, BitDepth?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_ColorSpace, ColorSpace?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_CompressedBitsPerPixel, CompressedBitsPerPixel?.ToString());
            string compression = CompressionText.AsWsNormalizedOrEmpty().TruncateWithElipses(256);
            yield return (FsInfoCat.Properties.Resources.DisplayName_Compression, (compression.Length > 0) ? compression : Compression?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_HorizontalResolution, HorizontalResolution?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_HorizontalSize, HorizontalSize?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_VerticalResolution, VerticalResolution?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_VerticalSize, VerticalSize?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_ResolutionUnit, ResolutionUnit?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_ImageID, ImageID.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

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
