using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MediaPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IMediaProperties
    {
        #region ContentDistributor Property Members

        /// <summary>
        /// Identifies the <see cref="ContentDistributor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentDistributorProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.ContentDistributor))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnContentDistributorPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ContentDistributor { get => GetValue(ContentDistributorProperty) as string; set => SetValue(ContentDistributorProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ContentDistributor"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ContentDistributor"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ContentDistributor"/> property.</param>
        protected virtual void OnContentDistributorPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region CreatorApplication Property Members

        /// <summary>
        /// Identifies the <see cref="CreatorApplication"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatorApplicationProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.CreatorApplication))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnCreatorApplicationPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string CreatorApplication { get => GetValue(CreatorApplicationProperty) as string; set => SetValue(CreatorApplicationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CreatorApplication"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CreatorApplication"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CreatorApplication"/> property.</param>
        protected virtual void OnCreatorApplicationPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region CreatorApplicationVersion Property Members

        /// <summary>
        /// Identifies the <see cref="CreatorApplicationVersion"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CreatorApplicationVersionProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.CreatorApplicationVersion))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnCreatorApplicationVersionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string CreatorApplicationVersion { get => GetValue(CreatorApplicationVersionProperty) as string; set => SetValue(CreatorApplicationVersionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CreatorApplicationVersion"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CreatorApplicationVersion"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CreatorApplicationVersion"/> property.</param>
        protected virtual void OnCreatorApplicationVersionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region DateReleased Property Members

        /// <summary>
        /// Identifies the <see cref="DateReleased"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateReleasedProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.DateReleased))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnDateReleasedPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string DateReleased { get => GetValue(DateReleasedProperty) as string; set => SetValue(DateReleasedProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DateReleased"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DateReleased"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DateReleased"/> property.</param>
        protected virtual void OnDateReleasedPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Duration Property Members

        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = ColumnPropertyBuilder<TimeSpan?, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.Duration))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnDurationPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public TimeSpan? Duration { get => (TimeSpan?)GetValue(DurationProperty); set => SetValue(DurationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Duration"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Duration"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Duration"/> property.</param>
        protected virtual void OnDurationPropertyChanged(TimeSpan? oldValue, TimeSpan? newValue) { }

        #endregion
        #region DVDID Property Members

        /// <summary>
        /// Identifies the <see cref="DVDID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DVDIDProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.DVDID))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnDVDIDPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string DVDID { get => GetValue(DVDIDProperty) as string; set => SetValue(DVDIDProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DVDID"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DVDID"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DVDID"/> property.</param>
        protected virtual void OnDVDIDPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region FrameCount Property Members

        /// <summary>
        /// Identifies the <see cref="FrameCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FrameCountProperty = ColumnPropertyBuilder<uint?, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.FrameCount))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnFrameCountPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? FrameCount { get => (uint?)GetValue(FrameCountProperty); set => SetValue(FrameCountProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="FrameCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="FrameCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="FrameCount"/> property.</param>
        protected virtual void OnFrameCountPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region ProtectionType Property Members

        /// <summary>
        /// Identifies the <see cref="ProtectionType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProtectionTypeProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.ProtectionType))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnProtectionTypePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ProtectionType { get => GetValue(ProtectionTypeProperty) as string; set => SetValue(ProtectionTypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProtectionType"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProtectionType"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProtectionType"/> property.</param>
        protected virtual void OnProtectionTypePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ProviderRating Property Members

        /// <summary>
        /// Identifies the <see cref="ProviderRating"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderRatingProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.ProviderRating))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnProviderRatingPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ProviderRating { get => GetValue(ProviderRatingProperty) as string; set => SetValue(ProviderRatingProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProviderRating"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProviderRating"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProviderRating"/> property.</param>
        protected virtual void OnProviderRatingPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ProviderStyle Property Members

        /// <summary>
        /// Identifies the <see cref="ProviderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProviderStyleProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.ProviderStyle))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnProviderStylePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ProviderStyle { get => GetValue(ProviderStyleProperty) as string; set => SetValue(ProviderStyleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProviderStyle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProviderStyle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProviderStyle"/> property.</param>
        protected virtual void OnProviderStylePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Publisher Property Members

        /// <summary>
        /// Identifies the <see cref="Publisher"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PublisherProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.Publisher))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnPublisherPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Publisher { get => GetValue(PublisherProperty) as string; set => SetValue(PublisherProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Publisher"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Publisher"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Publisher"/> property.</param>
        protected virtual void OnPublisherPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Subtitle Property Members

        /// <summary>
        /// Identifies the <see cref="Subtitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SubtitleProperty = ColumnPropertyBuilder<string, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.Subtitle))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnSubtitlePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Subtitle { get => GetValue(SubtitleProperty) as string; set => SetValue(SubtitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Subtitle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Subtitle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Subtitle"/> property.</param>
        protected virtual void OnSubtitlePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Year Property Members

        /// <summary>
        /// Identifies the <see cref="Year"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YearProperty = ColumnPropertyBuilder<uint?, MediaPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IMediaProperties.Year))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as MediaPropertiesRowViewModel<TEntity>)?.OnYearPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? Year { get => (uint?)GetValue(YearProperty); set => SetValue(YearProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Year"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Year"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Year"/> property.</param>
        protected virtual void OnYearPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

        public MediaPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            ContentDistributor = entity.ContentDistributor;
            CreatorApplication = entity.CreatorApplication;
            CreatorApplicationVersion = entity.CreatorApplicationVersion;
            DateReleased = entity.DateReleased;
            Duration = Converters.TimeSpanToStringConverter.FromMediaDuration(entity.Duration);
            DVDID = entity.DVDID;
            FrameCount = entity.FrameCount;
            ProtectionType = entity.ProtectionType;
            ProviderRating = entity.ProviderRating;
            ProviderStyle = entity.ProviderStyle;
            Publisher = entity.Publisher;
            Subtitle = entity.Subtitle;
            Year = entity.Year;
        }

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_Subtitle, Subtitle.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Publisher, Publisher.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_Year, Year?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_FrameCount, FrameCount?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_Duration, Duration?.ToString("g"));
            yield return (FsInfoCat.Properties.Resources.DisplayName_DateReleased, DateReleased.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ProtectionType, ProtectionType.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ProviderRating, ProviderRating.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ProviderStyle, ProviderStyle.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ContentDistributor, ContentDistributor.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_CreatorApplication, CreatorApplication.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_CreatorApplicationVersion, CreatorApplicationVersion.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_DVDID, DVDID.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IMediaProperties.ContentDistributor):
                    Dispatcher.CheckInvoke(() => ContentDistributor = Entity.ContentDistributor);
                    break;
                case nameof(IMediaProperties.CreatorApplication):
                    Dispatcher.CheckInvoke(() => CreatorApplication = Entity.CreatorApplication);
                    break;
                case nameof(IMediaProperties.CreatorApplicationVersion):
                    Dispatcher.CheckInvoke(() => CreatorApplicationVersion = Entity.CreatorApplicationVersion);
                    break;
                case nameof(IMediaProperties.DateReleased):
                    Dispatcher.CheckInvoke(() => DateReleased = Entity.DateReleased);
                    break;
                case nameof(IMediaProperties.Duration):
                    Dispatcher.CheckInvoke(() => Duration = Converters.TimeSpanToStringConverter.FromMediaDuration(Entity.Duration));
                    break;
                case nameof(IMediaProperties.DVDID):
                    Dispatcher.CheckInvoke(() => DVDID = Entity.DVDID);
                    break;
                case nameof(IMediaProperties.FrameCount):
                    Dispatcher.CheckInvoke(() => FrameCount = Entity.FrameCount);
                    break;
                case nameof(IMediaProperties.ProtectionType):
                    Dispatcher.CheckInvoke(() => ProtectionType = Entity.ProtectionType);
                    break;
                case nameof(IMediaProperties.ProviderRating):
                    Dispatcher.CheckInvoke(() => ProviderRating = Entity.ProviderRating);
                    break;
                case nameof(IMediaProperties.ProviderStyle):
                    Dispatcher.CheckInvoke(() => ProviderStyle = Entity.ProviderStyle);
                    break;
                case nameof(IMediaProperties.Publisher):
                    Dispatcher.CheckInvoke(() => Publisher = Entity.Publisher);
                    break;
                case nameof(IMediaProperties.Subtitle):
                    Dispatcher.CheckInvoke(() => Subtitle = Entity.Subtitle);
                    break;
                case nameof(IMediaProperties.Year):
                    Dispatcher.CheckInvoke(() => Year = Entity.Year);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
