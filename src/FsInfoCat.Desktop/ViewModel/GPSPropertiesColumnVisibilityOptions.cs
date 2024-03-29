using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : Model.DbEntity, Model.IGPSPropertiesListItem
        where TViewModel : GPSPropertiesListItemViewModel<TEntity>
    {
        #region AreaInformation Property Members

        /// <summary>
        /// Identifies the <see cref="AreaInformation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreaInformationProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(AreaInformation))
            .DefaultValue(false)
            .AsReadWrite();

        public bool AreaInformation { get => (bool)GetValue(AreaInformationProperty); set => SetValue(AreaInformationProperty, value); }

        #endregion
        #region LatitudeDegrees Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeDegrees"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeDegreesProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LatitudeDegrees))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LatitudeDegrees { get => (bool)GetValue(LatitudeDegreesProperty); set => SetValue(LatitudeDegreesProperty, value); }

        #endregion
        #region LatitudeMinutes Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeMinutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeMinutesProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LatitudeMinutes))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LatitudeMinutes { get => (bool)GetValue(LatitudeMinutesProperty); set => SetValue(LatitudeMinutesProperty, value); }

        #endregion
        #region LatitudeRef Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeRef"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeRefProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LatitudeRef))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LatitudeRef { get => (bool)GetValue(LatitudeRefProperty); set => SetValue(LatitudeRefProperty, value); }

        #endregion
        #region LatitudeSeconds Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeSeconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeSecondsProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LatitudeSeconds))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LatitudeSeconds { get => (bool)GetValue(LatitudeSecondsProperty); set => SetValue(LatitudeSecondsProperty, value); }

        #endregion
        #region LongitudeDegrees Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeDegrees"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeDegreesProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LongitudeDegrees))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LongitudeDegrees { get => (bool)GetValue(LongitudeDegreesProperty); set => SetValue(LongitudeDegreesProperty, value); }

        #endregion
        #region LongitudeMinutes Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeMinutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeMinutesProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LongitudeMinutes))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LongitudeMinutes { get => (bool)GetValue(LongitudeMinutesProperty); set => SetValue(LongitudeMinutesProperty, value); }

        #endregion
        #region LongitudeRef Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeRef"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeRefProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LongitudeRef))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LongitudeRef { get => (bool)GetValue(LongitudeRefProperty); set => SetValue(LongitudeRefProperty, value); }

        #endregion
        #region LongitudeSeconds Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeSeconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeSecondsProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(LongitudeSeconds))
            .DefaultValue(false)
            .AsReadWrite();

        public bool LongitudeSeconds { get => (bool)GetValue(LongitudeSecondsProperty); set => SetValue(LongitudeSecondsProperty, value); }

        #endregion
        #region MeasureMode Property Members

        /// <summary>
        /// Identifies the <see cref="MeasureMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasureModeProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(MeasureMode))
            .DefaultValue(false)
            .AsReadWrite();

        public bool MeasureMode { get => (bool)GetValue(MeasureModeProperty); set => SetValue(MeasureModeProperty, value); }

        #endregion
        #region ProcessingMethod Property Members

        /// <summary>
        /// Identifies the <see cref="ProcessingMethod"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProcessingMethodProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ProcessingMethod))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ProcessingMethod { get => (bool)GetValue(ProcessingMethodProperty); set => SetValue(ProcessingMethodProperty, value); }

        #endregion
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region VersionID Property Members

        /// <summary>
        /// Identifies the <see cref="VersionID"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VersionIDProperty = DependencyPropertyBuilder<GPSPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(VersionID))
            .DefaultValue(false)
            .AsReadWrite();

        public bool VersionID { get => (bool)GetValue(VersionIDProperty); set => SetValue(VersionIDProperty, value); }

        #endregion

        protected GPSPropertiesColumnVisibilityOptions() : base() { }
    }
}
