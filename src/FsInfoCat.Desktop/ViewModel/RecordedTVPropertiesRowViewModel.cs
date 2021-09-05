using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RecordedTVPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IRecordedTVProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region ChannelNumber Property Members

        /// <summary>
        /// Identifies the <see cref="ChannelNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChannelNumberProperty = ColumnPropertyBuilder<uint?, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.ChannelNumber))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnChannelNumberPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? ChannelNumber { get => (uint?)GetValue(ChannelNumberProperty); set => SetValue(ChannelNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ChannelNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ChannelNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ChannelNumber"/> property.</param>
        protected virtual void OnChannelNumberPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region EpisodeName Property Members

        /// <summary>
        /// Identifies the <see cref="EpisodeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EpisodeNameProperty = ColumnPropertyBuilder<string, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.EpisodeName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnEpisodeNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string EpisodeName { get => GetValue(EpisodeNameProperty) as string; set => SetValue(EpisodeNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="EpisodeName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="EpisodeName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="EpisodeName"/> property.</param>
        protected virtual void OnEpisodeNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region IsDTVContent Property Members

        /// <summary>
        /// Identifies the <see cref="IsDTVContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDTVContentProperty = ColumnPropertyBuilder<bool?, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.IsDTVContent))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnIsDTVContentPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool? IsDTVContent { get => (bool?)GetValue(IsDTVContentProperty); set => SetValue(IsDTVContentProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsDTVContent"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsDTVContent"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsDTVContent"/> property.</param>
        protected virtual void OnIsDTVContentPropertyChanged(bool? oldValue, bool? newValue) { }

        #endregion
        #region IsHDContent Property Members

        /// <summary>
        /// Identifies the <see cref="IsHDContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHDContentProperty = ColumnPropertyBuilder<bool?, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.IsHDContent))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnIsHDContentPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool? IsHDContent { get => (bool?)GetValue(IsHDContentProperty); set => SetValue(IsHDContentProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="IsHDContent"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="IsHDContent"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="IsHDContent"/> property.</param>
        protected virtual void OnIsHDContentPropertyChanged(bool? oldValue, bool? newValue) { }

        #endregion
        #region NetworkAffiliation Property Members

        /// <summary>
        /// Identifies the <see cref="NetworkAffiliation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NetworkAffiliationProperty = ColumnPropertyBuilder<string, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.NetworkAffiliation))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnNetworkAffiliationPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string NetworkAffiliation { get => GetValue(NetworkAffiliationProperty) as string; set => SetValue(NetworkAffiliationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="NetworkAffiliation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="NetworkAffiliation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="NetworkAffiliation"/> property.</param>
        protected virtual void OnNetworkAffiliationPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region OriginalBroadcastDate Property Members

        /// <summary>
        /// Identifies the <see cref="OriginalBroadcastDate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginalBroadcastDateProperty = ColumnPropertyBuilder<DateTime?, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.OriginalBroadcastDate))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnOriginalBroadcastDatePropertyChanged(oldValue, newValue))
            .AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? OriginalBroadcastDate { get => (DateTime?)GetValue(OriginalBroadcastDateProperty); set => SetValue(OriginalBroadcastDateProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="OriginalBroadcastDate"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="OriginalBroadcastDate"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="OriginalBroadcastDate"/> property.</param>
        protected virtual void OnOriginalBroadcastDatePropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region ProgramDescription Property Members

        /// <summary>
        /// Identifies the <see cref="ProgramDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgramDescriptionProperty = ColumnPropertyBuilder<string, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.ProgramDescription))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnProgramDescriptionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ProgramDescription { get => GetValue(ProgramDescriptionProperty) as string; set => SetValue(ProgramDescriptionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProgramDescription"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProgramDescription"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProgramDescription"/> property.</param>
        protected virtual void OnProgramDescriptionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StationCallSign Property Members

        /// <summary>
        /// Identifies the <see cref="StationCallSign"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StationCallSignProperty = ColumnPropertyBuilder<string, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.StationCallSign))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnStationCallSignPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string StationCallSign { get => GetValue(StationCallSignProperty) as string; set => SetValue(StationCallSignProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StationCallSign"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StationCallSign"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StationCallSign"/> property.</param>
        protected virtual void OnStationCallSignPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region StationName Property Members

        /// <summary>
        /// Identifies the <see cref="StationName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StationNameProperty = ColumnPropertyBuilder<string, RecordedTVPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IRecordedTVProperties.StationName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnStationNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string StationName { get => GetValue(StationNameProperty) as string; set => SetValue(StationNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="StationName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="StationName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="StationName"/> property.</param>
        protected virtual void OnStationNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public RecordedTVPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            ChannelNumber = entity.ChannelNumber;
            EpisodeName = entity.EpisodeName;
            IsDTVContent = entity.IsDTVContent;
            IsHDContent = entity.IsHDContent;
            NetworkAffiliation = entity.NetworkAffiliation;
            OriginalBroadcastDate = entity.OriginalBroadcastDate;
            ProgramDescription = entity.ProgramDescription;
            StationCallSign = entity.StationCallSign;
            StationName = entity.StationName;
        }

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_EpisodeName, EpisodeName.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ProgramDescription, ProgramDescription.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_OriginalBroadcastDate, OriginalBroadcastDate?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_StationCallSign, StationCallSign.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_StationName, StationName.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_NetworkAffiliation, NetworkAffiliation.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ChannelNumber, ChannelNumber?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_IsDTVContent, Converters.BooleanToStringConverter.Convert(IsDTVContent));
            yield return (FsInfoCat.Properties.Resources.DisplayName_IsHDContent, Converters.BooleanToStringConverter.Convert(IsHDContent));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IRecordedTVProperties.ChannelNumber):
                    Dispatcher.CheckInvoke(() => ChannelNumber = Entity.ChannelNumber);
                    break;
                case nameof(IRecordedTVProperties.EpisodeName):
                    Dispatcher.CheckInvoke(() => EpisodeName = Entity.EpisodeName);
                    break;
                case nameof(IRecordedTVProperties.IsDTVContent):
                    Dispatcher.CheckInvoke(() => IsDTVContent = Entity.IsDTVContent);
                    break;
                case nameof(IRecordedTVProperties.IsHDContent):
                    Dispatcher.CheckInvoke(() => IsHDContent = Entity.IsHDContent);
                    break;
                case nameof(IRecordedTVProperties.NetworkAffiliation):
                    Dispatcher.CheckInvoke(() => NetworkAffiliation = Entity.NetworkAffiliation);
                    break;
                case nameof(IRecordedTVProperties.OriginalBroadcastDate):
                    Dispatcher.CheckInvoke(() => OriginalBroadcastDate = Entity.OriginalBroadcastDate);
                    break;
                case nameof(IRecordedTVProperties.ProgramDescription):
                    Dispatcher.CheckInvoke(() => ProgramDescription = Entity.ProgramDescription);
                    break;
                case nameof(IRecordedTVProperties.StationCallSign):
                    Dispatcher.CheckInvoke(() => StationCallSign = Entity.StationCallSign);
                    break;
                case nameof(IRecordedTVProperties.StationName):
                    Dispatcher.CheckInvoke(() => StationName = Entity.StationName);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
