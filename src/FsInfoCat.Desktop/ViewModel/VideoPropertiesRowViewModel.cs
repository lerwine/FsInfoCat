using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VideoPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IVideoProperties
    {
        #region Director Property Members

        /// <summary>
        /// Identifies the <see cref="Director"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DirectorProperty = ColumnPropertyBuilder<string, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.Director))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnDirectorPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Director { get => GetValue(DirectorProperty) as string; set => SetValue(DirectorProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Director"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Director"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Director"/> property.</param>
        private void OnDirectorPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = ColumnPropertyBuilder<string, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.Compression))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Compression { get => GetValue(CompressionProperty) as string; set => SetValue(CompressionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Compression"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Compression"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Compression"/> property.</param>
        protected virtual void OnCompressionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = ColumnPropertyBuilder<uint?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.EncodingBitrate))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnEncodingBitratePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? EncodingBitrate { get => (uint?)GetValue(EncodingBitrateProperty); set => SetValue(EncodingBitrateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="EncodingBitrate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="EncodingBitrate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="EncodingBitrate"/> property.</param>
        protected virtual void OnEncodingBitratePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region FrameHeight Property Members

        /// <summary>
        /// Identifies the <see cref="FrameHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameHeightProperty = ColumnPropertyBuilder<uint?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.FrameHeight))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnFrameHeightPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? FrameHeight { get => (uint?)GetValue(FrameHeightProperty); set => SetValue(FrameHeightProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameHeight"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameHeight"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameHeight"/> property.</param>
        protected virtual void OnFrameHeightPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region FrameRate Property Members

        /// <summary>
        /// Identifies the <see cref="FrameRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty = ColumnPropertyBuilder<uint?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.FrameRate))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnFrameRatePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? FrameRate { get => (uint?)GetValue(FrameRateProperty); set => SetValue(FrameRateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameRate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameRate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameRate"/> property.</param>
        protected virtual void OnFrameRatePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region FrameWidth Property Members

        /// <summary>
        /// Identifies the <see cref="FrameWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameWidthProperty = ColumnPropertyBuilder<uint?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.FrameWidth))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnFrameWidthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? FrameWidth { get => (uint?)GetValue(FrameWidthProperty); set => SetValue(FrameWidthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameWidth"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameWidth"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameWidth"/> property.</param>
        protected virtual void OnFrameWidthPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region HorizontalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="HorizontalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAspectRatioProperty = ColumnPropertyBuilder<uint?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.HorizontalAspectRatio))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnHorizontalAspectRatioPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? HorizontalAspectRatio { get => (uint?)GetValue(HorizontalAspectRatioProperty); set => SetValue(HorizontalAspectRatioProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HorizontalAspectRatio"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HorizontalAspectRatio"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HorizontalAspectRatio"/> property.</param>
        protected virtual void OnHorizontalAspectRatioPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = ColumnPropertyBuilder<ushort?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.StreamNumber))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnStreamNumberPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort? StreamNumber { get => (ushort?)GetValue(StreamNumberProperty); set => SetValue(StreamNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StreamNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StreamNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StreamNumber"/> property.</param>
        protected virtual void OnStreamNumberPropertyChanged(ushort? oldValue, ushort? newValue) { }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = ColumnPropertyBuilder<string, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.StreamName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnStreamNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string StreamName { get => GetValue(StreamNameProperty) as string; set => SetValue(StreamNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StreamName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StreamName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StreamName"/> property.</param>
        protected virtual void OnStreamNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region VerticalAspectRatio Property Members

        /// <summary>
        /// Identifies the <see cref="VerticalAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalAspectRatioProperty = ColumnPropertyBuilder<uint?, VideoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVideoProperties.VerticalAspectRatio))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VideoPropertiesRowViewModel<TEntity>)?.OnVerticalAspectRatioPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? VerticalAspectRatio { get => (uint?)GetValue(VerticalAspectRatioProperty); set => SetValue(VerticalAspectRatioProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VerticalAspectRatio"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VerticalAspectRatio"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VerticalAspectRatio"/> property.</param>
        protected virtual void OnVerticalAspectRatioPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

        public VideoPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            Director = entity.Director;
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

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_Director, Director.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Compression, Compression.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_EncodingBitrate, EncodingBitrate?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_FrameRate, FrameRate?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_FrameWidth, FrameWidth?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_FrameHeight, FrameHeight?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_HorizontalAspectRatio, HorizontalAspectRatio?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_VerticalAspectRatio, VerticalAspectRatio?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_StreamName, StreamName.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_StreamNumber, StreamNumber?.ToString());
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

    }
}
