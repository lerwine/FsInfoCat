using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : DbEntity, IRecordedTVPropertiesListItem
        where TViewModel : RecordedTVPropertiesListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region ChannelNumber Property Members

        /// <summary>
        /// Identifies the <see cref="ChannelNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ChannelNumberProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ChannelNumber))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ChannelNumber { get => (bool)GetValue(ChannelNumberProperty); set => SetValue(ChannelNumberProperty, value); }

        #endregion
        #region EpisodeName Property Members

        /// <summary>
        /// Identifies the <see cref="EpisodeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EpisodeNameProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(EpisodeName))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool EpisodeName { get => (bool)GetValue(EpisodeNameProperty); set => SetValue(EpisodeNameProperty, value); }

        #endregion
        #region IsDTVContent Property Members

        /// <summary>
        /// Identifies the <see cref="IsDTVContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsDTVContentProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(IsDTVContent))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool IsDTVContent { get => (bool)GetValue(IsDTVContentProperty); set => SetValue(IsDTVContentProperty, value); }

        #endregion
        #region IsHDContent Property Members

        /// <summary>
        /// Identifies the <see cref="IsHDContent"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHDContentProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(IsHDContent))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool IsHDContent { get => (bool)GetValue(IsHDContentProperty); set => SetValue(IsHDContentProperty, value); }

        #endregion
        #region NetworkAffiliation Property Members

        /// <summary>
        /// Identifies the <see cref="NetworkAffiliation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NetworkAffiliationProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(NetworkAffiliation))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool NetworkAffiliation { get => (bool)GetValue(NetworkAffiliationProperty); set => SetValue(NetworkAffiliationProperty, value); }

        #endregion
        #region OriginalBroadcastDate Property Members

        /// <summary>
        /// Identifies the <see cref="OriginalBroadcastDate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginalBroadcastDateProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(OriginalBroadcastDate))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool OriginalBroadcastDate { get => (bool)GetValue(OriginalBroadcastDateProperty); set => SetValue(OriginalBroadcastDateProperty, value); }

        #endregion
        #region ProgramDescription Property Members

        /// <summary>
        /// Identifies the <see cref="ProgramDescription"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgramDescriptionProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ProgramDescription))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ProgramDescription { get => (bool)GetValue(ProgramDescriptionProperty); set => SetValue(ProgramDescriptionProperty, value); }

        #endregion
        #region StationCallSign Property Members

        /// <summary>
        /// Identifies the <see cref="StationCallSign"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StationCallSignProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StationCallSign))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool StationCallSign { get => (bool)GetValue(StationCallSignProperty); set => SetValue(StationCallSignProperty, value); }

        #endregion
        #region StationName Property Members

        /// <summary>
        /// Identifies the <see cref="StationName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StationNameProperty = DependencyPropertyBuilder<RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StationName))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as RecordedTVPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool StationName { get => (bool)GetValue(StationNameProperty); set => SetValue(StationNameProperty, value); }

        #endregion

        protected RecordedTVPropertiesColumnVisibilityOptions() : base() { }
    }
}
