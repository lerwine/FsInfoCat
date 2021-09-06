using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        public static readonly DependencyProperty CompressionProperty = ColumnPropertyBuilder<string, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.Compression))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnCompressionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

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
        protected virtual void OnCompressionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region EncodingBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="EncodingBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EncodingBitrateProperty = ColumnPropertyBuilder<uint?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.EncodingBitrate))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnEncodingBitratePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnEncodingBitratePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region Format Property Members

        /// <summary>
        /// Identifies the <see cref="Format"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatProperty = ColumnPropertyBuilder<string, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.Format))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnFormatPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

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
        protected virtual void OnFormatPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region IsVariableBitrate Property Members

        /// <summary>
        /// Identifies the <see cref="IsVariableBitrate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsVariableBitrateProperty = ColumnPropertyBuilder<bool?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.IsVariableBitrate))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnIsVariableBitratePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnIsVariableBitratePropertyChanged(bool? oldValue, bool? newValue) { }

        #endregion
        #region SampleRate Property Members

        /// <summary>
        /// Identifies the <see cref="SampleRate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleRateProperty = ColumnPropertyBuilder<uint?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.SampleRate))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnSampleRatePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnSampleRatePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region SampleSize Property Members

        /// <summary>
        /// Identifies the <see cref="SampleSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SampleSizeProperty = ColumnPropertyBuilder<uint?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.SampleSize))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnSampleSizePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnSampleSizePropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region StreamName Property Members

        /// <summary>
        /// Identifies the <see cref="StreamName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNameProperty = ColumnPropertyBuilder<string, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.StreamName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnStreamNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

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
        protected virtual void OnStreamNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StreamNumber Property Members

        /// <summary>
        /// Identifies the <see cref="StreamNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StreamNumberProperty = ColumnPropertyBuilder<ushort?, AudioPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IAudioProperties.StreamNumber))
            .OnChanged((d, oldValue, newValue) => (d as AudioPropertiesRowViewModel<TEntity>)?.OnStreamNumberPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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

        //internal string CalculateDisplayText(Func<(string PropertyName, string Value), bool> filter = null) => (filter is null) ?
        //    StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

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
