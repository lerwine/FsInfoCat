using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MusicPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IMusicProperties
    {
        #region AlbumArtist Property Members

        /// <summary>
        /// Identifies the <see cref="AlbumArtist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumArtistProperty = ColumnPropertyBuilder<string, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.AlbumArtist))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnAlbumArtistPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string AlbumArtist { get => GetValue(AlbumArtistProperty) as string; set => SetValue(AlbumArtistProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AlbumArtist"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AlbumArtist"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AlbumArtist"/> property.</param>
        protected virtual void OnAlbumArtistPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region AlbumTitle Property Members

        /// <summary>
        /// Identifies the <see cref="AlbumTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlbumTitleProperty = ColumnPropertyBuilder<string, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.AlbumTitle))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnAlbumTitlePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string AlbumTitle { get => GetValue(AlbumTitleProperty) as string; set => SetValue(AlbumTitleProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AlbumTitle"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AlbumTitle"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AlbumTitle"/> property.</param>
        protected virtual void OnAlbumTitlePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ChannelCount Property Members

        /// <summary>
        /// Identifies the <see cref="ChannelCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChannelCountProperty = ColumnPropertyBuilder<uint?, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.ChannelCount))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnChannelCountPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? ChannelCount { get => (uint?)GetValue(ChannelCountProperty); set => SetValue(ChannelCountProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ChannelCount"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ChannelCount"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ChannelCount"/> property.</param>
        protected virtual void OnChannelCountPropertyChanged(uint? oldValue, uint? newValue) => IsStereo = newValue.HasValue ? newValue.Value > 1 : null;

        #endregion
        #region IsStereo Property Members

        private static readonly DependencyPropertyKey IsStereoPropertyKey = ColumnPropertyBuilder<bool?, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IsStereo), nameof(Model.IMusicProperties.ChannelCount))
            .DefaultValue(null)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IsStereo"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsStereoProperty = IsStereoPropertyKey.DependencyProperty;

        public bool? IsStereo { get => (bool?)GetValue(IsStereoProperty); private set => SetValue(IsStereoPropertyKey, value); }

        #endregion
        #region DisplayArtist Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayArtist"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayArtistProperty = ColumnPropertyBuilder<string, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.DisplayArtist))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnDisplayArtistPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string DisplayArtist { get => GetValue(DisplayArtistProperty) as string; set => SetValue(DisplayArtistProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayArtist"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayArtist"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayArtist"/> property.</param>
        protected virtual void OnDisplayArtistPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region PartOfSet Property Members

        /// <summary>
        /// Identifies the <see cref="PartOfSet"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PartOfSetProperty = ColumnPropertyBuilder<string, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.PartOfSet))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnPartOfSetPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string PartOfSet { get => GetValue(PartOfSetProperty) as string; set => SetValue(PartOfSetProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="PartOfSet"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="PartOfSet"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="PartOfSet"/> property.</param>
        protected virtual void OnPartOfSetPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Period Property Members

        /// <summary>
        /// Identifies the <see cref="Period"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeriodProperty = ColumnPropertyBuilder<string, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.Period))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnPeriodPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string Period { get => GetValue(PeriodProperty) as string; set => SetValue(PeriodProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Period"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Period"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Period"/> property.</param>
        protected virtual void OnPeriodPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region TrackNumber Property Members

        /// <summary>
        /// Identifies the <see cref="TrackNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrackNumberProperty = ColumnPropertyBuilder<uint?, MusicPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IMusicProperties.TrackNumber))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as MusicPropertiesRowViewModel<TEntity>)?.OnTrackNumberPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public uint? TrackNumber { get => (uint?)GetValue(TrackNumberProperty); set => SetValue(TrackNumberProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="TrackNumber"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="TrackNumber"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="TrackNumber"/> property.</param>
        protected virtual void OnTrackNumberPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion

        public MusicPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            AlbumArtist = entity.AlbumArtist;
            AlbumTitle = entity.AlbumTitle;
            ChannelCount = entity.ChannelCount;
            DisplayArtist = entity.DisplayArtist;
            PartOfSet = entity.PartOfSet;
            Period = entity.Period;
            TrackNumber = entity.TrackNumber;
        }

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_DisplayArtist, DisplayArtist.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_AlbumArtist, AlbumArtist.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_AlbumTitle, AlbumTitle.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_PartOfSet, PartOfSet.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_TrackNumber, TrackNumber?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_Period, Period.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_IsStereo, Converters.BooleanToStringConverter.Convert(IsStereo));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.IMusicProperties.AlbumArtist):
                    Dispatcher.CheckInvoke(() => AlbumArtist = Entity.AlbumArtist);
                    break;
                case nameof(Model.IMusicProperties.AlbumTitle):
                    Dispatcher.CheckInvoke(() => AlbumTitle = Entity.AlbumTitle);
                    break;
                case nameof(Model.IMusicProperties.ChannelCount):
                    Dispatcher.CheckInvoke(() => ChannelCount = Entity.ChannelCount);
                    break;
                case nameof(Model.IMusicProperties.DisplayArtist):
                    Dispatcher.CheckInvoke(() => DisplayArtist = Entity.DisplayArtist);
                    break;
                case nameof(Model.IMusicProperties.PartOfSet):
                    Dispatcher.CheckInvoke(() => PartOfSet = Entity.PartOfSet);
                    break;
                case nameof(Model.IMusicProperties.Period):
                    Dispatcher.CheckInvoke(() => Period = Entity.Period);
                    break;
                case nameof(Model.IMusicProperties.TrackNumber):
                    Dispatcher.CheckInvoke(() => TrackNumber = Entity.TrackNumber);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
