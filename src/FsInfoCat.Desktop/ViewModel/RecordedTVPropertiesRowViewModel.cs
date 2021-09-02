using System;
using System.Diagnostics.CodeAnalysis;
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
        public static readonly DependencyProperty ChannelNumberProperty = DependencyProperty.Register(nameof(ChannelNumber), typeof(uint?),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnChannelNumberPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

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
        protected void OnChannelNumberPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnChannelNumberPropertyChanged Logic
        }

        #endregion
        #region EpisodeName Property Members

        /// <summary>
        /// Identifies the <see cref="EpisodeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EpisodeNameProperty = DependencyProperty.Register(nameof(EpisodeName), typeof(string),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnEpisodeNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        protected void OnEpisodeNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnEpisodeNamePropertyChanged Logic
        }

        #endregion
        #region IsDTVContent Property Members

        /// <summary>
        /// Identifies the <see cref="IsDTVContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDTVContentProperty = DependencyProperty.Register(nameof(IsDTVContent), typeof(bool?),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnIsDTVContentPropertyChanged((bool?)e.OldValue, (bool?)e.NewValue)));

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
        protected void OnIsDTVContentPropertyChanged(bool? oldValue, bool? newValue)
        {
            // TODO: Implement OnIsDTVContentPropertyChanged Logic
        }

        #endregion
        #region IsHDContent Property Members

        /// <summary>
        /// Identifies the <see cref="IsHDContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHDContentProperty = DependencyProperty.Register(nameof(IsHDContent), typeof(bool?),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnIsHDContentPropertyChanged((bool?)e.OldValue, (bool?)e.NewValue)));

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
        protected void OnIsHDContentPropertyChanged(bool? oldValue, bool? newValue)
        {
            // TODO: Implement OnIsHDContentPropertyChanged Logic
        }

        #endregion
        #region NetworkAffiliation Property Members

        /// <summary>
        /// Identifies the <see cref="NetworkAffiliation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NetworkAffiliationProperty = DependencyProperty.Register(nameof(NetworkAffiliation), typeof(string),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnNetworkAffiliationPropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        protected void OnNetworkAffiliationPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNetworkAffiliationPropertyChanged Logic
        }

        #endregion
        #region OriginalBroadcastDate Property Members

        /// <summary>
        /// Identifies the <see cref="OriginalBroadcastDate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginalBroadcastDateProperty = DependencyProperty.Register(nameof(OriginalBroadcastDate), typeof(DateTime?),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnOriginalBroadcastDatePropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

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
        protected void OnOriginalBroadcastDatePropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnOriginalBroadcastDatePropertyChanged Logic
        }

        #endregion
        #region ProgramDescription Property Members

        /// <summary>
        /// Identifies the <see cref="ProgramDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgramDescriptionProperty = DependencyProperty.Register(nameof(ProgramDescription), typeof(string),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnProgramDescriptionPropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        protected void OnProgramDescriptionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnProgramDescriptionPropertyChanged Logic
        }

        #endregion
        #region StationCallSign Property Members

        /// <summary>
        /// Identifies the <see cref="StationCallSign"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StationCallSignProperty = DependencyProperty.Register(nameof(StationCallSign), typeof(string),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnStationCallSignPropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        protected void OnStationCallSignPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStationCallSignPropertyChanged Logic
        }

        #endregion
        #region StationName Property Members

        /// <summary>
        /// Identifies the <see cref="StationName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StationNameProperty = DependencyProperty.Register(nameof(StationName), typeof(string),
            typeof(RecordedTVPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as RecordedTVPropertiesRowViewModel<TEntity>)?.OnStationNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

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
        protected void OnStationNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStationNamePropertyChanged Logic
        }

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
