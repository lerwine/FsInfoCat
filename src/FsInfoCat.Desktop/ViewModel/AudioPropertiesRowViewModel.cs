using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AudioPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IAudioProperties
    {
        #region Compression Property Members

        /// <summary>
        /// Identifies the <see cref="Compression"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CompressionProperty = ColumnPropertyBuilder<string, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.Compression))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

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
        public static readonly DependencyProperty EncodingBitrateProperty = ColumnPropertyBuilder<uint?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.EncodingBitrate))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnEncodingBitratePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? EncodingBitrate { get => (uint?)GetValue(EncodingBitrateProperty); set => SetValue(EncodingBitrateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="EncodingBitrate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="EncodingBitrate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="EncodingBitrate"/> property.</param>
        protected virtual void OnEncodingBitratePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region Format Property Members

        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatProperty = ColumnPropertyBuilder<string, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.Format))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnFormatPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string Format { get => GetValue(FormatProperty) as string; set => SetValue(FormatProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Format"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Format"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Format"/> property.</param>
        protected virtual void OnFormatPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region IsVariableBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="IsVariableBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVariableBitrateProperty = ColumnPropertyBuilder<bool?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.IsVariableBitrate))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnIsVariableBitratePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public bool? IsVariableBitrate { get => (bool?)GetValue(IsVariableBitrateProperty); set => SetValue(IsVariableBitrateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsVariableBitrate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsVariableBitrate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsVariableBitrate"/> property.</param>
        protected virtual void OnIsVariableBitratePropertyChanged(bool? oldValue, bool? newValue) { }

        #endregion
        #region SampleRate Property Members

        /// <summary>
        /// Identifies the <see cref="SampleRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleRateProperty = ColumnPropertyBuilder<uint?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.SampleRate))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnSampleRatePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? SampleRate { get => (uint?)GetValue(SampleRateProperty); set => SetValue(SampleRateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SampleRate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SampleRate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SampleRate"/> property.</param>
        protected virtual void OnSampleRatePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region SampleSize Property Members

        /// <summary>
        /// Identifies the <see cref="SampleSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleSizeProperty = ColumnPropertyBuilder<uint?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.SampleSize))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnSampleSizePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? SampleSize { get => (uint?)GetValue(SampleSizeProperty); set => SetValue(SampleSizeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="SampleSize"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="SampleSize"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="SampleSize"/> property.</param>
        protected virtual void OnSampleSizePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = ColumnPropertyBuilder<string, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.StreamName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnStreamNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string StreamName { get => GetValue(StreamNameProperty) as string; set => SetValue(StreamNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StreamName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StreamName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StreamName"/> property.</param>
        protected virtual void OnStreamNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = ColumnPropertyBuilder<ushort?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IAudioProperties.StreamNumber))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnStreamNumberPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort? StreamNumber { get => (ushort?)GetValue(StreamNumberProperty); set => SetValue(StreamNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StreamNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StreamNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StreamNumber"/> property.</param>
        protected virtual void OnStreamNumberPropertyChanged(ushort? oldValue, ushort? newValue) { }

        #endregion

        public AudioPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
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

        public virtual IEnumerable<(string PropertyName, string Value)> GetNameValuePairs()
        {
            yield return (nameof(Compression), Compression.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (nameof(EncodingBitrate), EncodingBitrate?.ToString());
            yield return (nameof(Format), Format.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (nameof(IsVariableBitrate), Converters.BooleanToStringConverter.Convert(IsVariableBitrate));
            yield return (nameof(SampleRate), SampleRate?.ToString());
            yield return (nameof(SampleSize), SampleSize?.ToString());
            yield return (nameof(StreamName), StreamName.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (nameof(StreamNumber), StreamNumber?.ToString());
        }


        /// <summary>
        /// Calculates the display text.
        /// </summary>
        /// <param name="columns">The columns to be included.</param>
        /// <returns>The display text.</returns>
        internal string CalculateSummaryText()
        {
            IEnumerable<ColumnProperty> columns = ColumnProperty.GetOrderedProperties(GetType());
            Dictionary<string, string> nvp = GetNameValuePairs().ToDictionary(k => k.PropertyName, v => v.Value);
            return StringExtensionMethods.ToKeyValueListString(columns.Select(Col => (Col, Success: nvp.TryGetValue(Col.Name, out string Value), Value)).Where(t => t.Success).Select(t =>
                (string.IsNullOrWhiteSpace(t.Col.ShortName) ? (string.IsNullOrWhiteSpace(t.Col.DisplayName) ? t.Col.Name : t.Col.DisplayName) : t.Col.ShortName, t.Value)));
        }
    }
}
