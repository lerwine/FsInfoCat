using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class GPSPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IGPSProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region AreaInformation Property Members

        /// <summary>
        /// Identifies the <see cref="AreaInformation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreaInformationProperty = DependencyProperty.Register(nameof(AreaInformation), typeof(string),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnAreaInformationPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string AreaInformation { get => GetValue(AreaInformationProperty) as string; set => SetValue(AreaInformationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AreaInformation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AreaInformation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AreaInformation"/> property.</param>
        protected void OnAreaInformationPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnAreaInformationPropertyChanged Logic
        }

        #endregion
        #region LatitudeDegrees Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeDegrees"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeDegreesProperty = DependencyProperty.Register(nameof(LatitudeDegrees), typeof(double?),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeDegreesPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? LatitudeDegrees { get => (double?)GetValue(LatitudeDegreesProperty); set => SetValue(LatitudeDegreesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeDegrees"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeDegrees"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeDegrees"/> property.</param>
        protected void OnLatitudeDegreesPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnLatitudeDegreesPropertyChanged Logic
        }

        #endregion
        #region LatitudeMinutes Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeMinutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeMinutesProperty = DependencyProperty.Register(nameof(LatitudeMinutes), typeof(double?),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeMinutesPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? LatitudeMinutes { get => (double?)GetValue(LatitudeMinutesProperty); set => SetValue(LatitudeMinutesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeMinutes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeMinutes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeMinutes"/> property.</param>
        protected void OnLatitudeMinutesPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnLatitudeMinutesPropertyChanged Logic
        }

        #endregion
        #region LatitudeSeconds Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeSeconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeSecondsProperty = DependencyProperty.Register(nameof(LatitudeSeconds), typeof(double?),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeSecondsPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? LatitudeSeconds { get => (double?)GetValue(LatitudeSecondsProperty); set => SetValue(LatitudeSecondsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeSeconds"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeSeconds"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeSeconds"/> property.</param>
        protected void OnLatitudeSecondsPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnLatitudeSecondsPropertyChanged Logic
        }

        #endregion
        #region LatitudeRef Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeRef"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeRefProperty = DependencyProperty.Register(nameof(LatitudeRef), typeof(string),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeRefPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string LatitudeRef { get => GetValue(LatitudeRefProperty) as string; set => SetValue(LatitudeRefProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeRef"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeRef"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeRef"/> property.</param>
        protected void OnLatitudeRefPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnLatitudeRefPropertyChanged Logic
        }

        #endregion
        #region LongitudeDegrees Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeDegrees"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeDegreesProperty = DependencyProperty.Register(nameof(LongitudeDegrees), typeof(double?),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeDegreesPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? LongitudeDegrees { get => (double?)GetValue(LongitudeDegreesProperty); set => SetValue(LongitudeDegreesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeDegrees"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeDegrees"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeDegrees"/> property.</param>
        protected void OnLongitudeDegreesPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnLongitudeDegreesPropertyChanged Logic
        }

        #endregion
        #region LongitudeMinutes Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeMinutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeMinutesProperty = DependencyProperty.Register(nameof(LongitudeMinutes), typeof(double?),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeMinutesPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? LongitudeMinutes { get => (double?)GetValue(LongitudeMinutesProperty); set => SetValue(LongitudeMinutesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeMinutes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeMinutes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeMinutes"/> property.</param>
        protected void OnLongitudeMinutesPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnLongitudeMinutesPropertyChanged Logic
        }

        #endregion
        #region LongitudeSeconds Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeSeconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeSecondsProperty = DependencyProperty.Register(nameof(LongitudeSeconds), typeof(double?),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeSecondsPropertyChanged((double?)e.OldValue, (double?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public double? LongitudeSeconds { get => (double?)GetValue(LongitudeSecondsProperty); set => SetValue(LongitudeSecondsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeSeconds"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeSeconds"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeSeconds"/> property.</param>
        protected void OnLongitudeSecondsPropertyChanged(double? oldValue, double? newValue)
        {
            // TODO: Implement OnLongitudeSecondsPropertyChanged Logic
        }

        #endregion
        #region LongitudeRef Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeRef"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeRefProperty = DependencyProperty.Register(nameof(LongitudeRef), typeof(string),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeRefPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string LongitudeRef { get => GetValue(LongitudeRefProperty) as string; set => SetValue(LongitudeRefProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeRef"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeRef"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeRef"/> property.</param>
        protected void OnLongitudeRefPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnLongitudeRefPropertyChanged Logic
        }

        #endregion
        #region MeasureMode Property Members

        /// <summary>
        /// Identifies the <see cref="MeasureMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasureModeProperty = DependencyProperty.Register(nameof(MeasureMode), typeof(string),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnMeasureModePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string MeasureMode { get => GetValue(MeasureModeProperty) as string; set => SetValue(MeasureModeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MeasureMode"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MeasureMode"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MeasureMode"/> property.</param>
        protected void OnMeasureModePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnMeasureModePropertyChanged Logic
        }

        #endregion
        #region ProcessingMethod Property Members

        /// <summary>
        /// Identifies the <see cref="ProcessingMethod"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProcessingMethodProperty = DependencyProperty.Register(nameof(ProcessingMethod), typeof(string),
            typeof(GPSPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as GPSPropertiesRowViewModel<TEntity>)?.OnProcessingMethodPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ProcessingMethod { get => GetValue(ProcessingMethodProperty) as string; set => SetValue(ProcessingMethodProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProcessingMethod"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProcessingMethod"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProcessingMethod"/> property.</param>
        protected void OnProcessingMethodPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnProcessingMethodPropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public GPSPropertiesRowViewModel(TEntity entity) : base(entity)
        {
            AreaInformation = entity.AreaInformation;
            LatitudeDegrees = entity.LatitudeDegrees;
            LatitudeMinutes = entity.LatitudeMinutes;
            LatitudeSeconds = entity.LatitudeSeconds;
            LatitudeRef = entity.LatitudeRef;
            LongitudeDegrees = entity.LongitudeDegrees;
            LongitudeMinutes = entity.LongitudeMinutes;
            LongitudeSeconds = entity.LongitudeSeconds;
            LongitudeRef = entity.LongitudeRef;
            MeasureMode = entity.MeasureMode;
            ProcessingMethod = entity.ProcessingMethod;
        }

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IGPSProperties.AreaInformation):
                    Dispatcher.CheckInvoke(() => AreaInformation = Entity.AreaInformation);
                    break;
                case nameof(IGPSProperties.LatitudeDegrees):
                    Dispatcher.CheckInvoke(() => LatitudeDegrees = Entity.LatitudeDegrees);
                    break;
                case nameof(IGPSProperties.LatitudeMinutes):
                    Dispatcher.CheckInvoke(() => LatitudeMinutes = Entity.LatitudeMinutes);
                    break;
                case nameof(IGPSProperties.LatitudeSeconds):
                    Dispatcher.CheckInvoke(() => LatitudeSeconds = Entity.LatitudeSeconds);
                    break;
                case nameof(IGPSProperties.LatitudeRef):
                    Dispatcher.CheckInvoke(() => LatitudeRef = Entity.LatitudeRef);
                    break;
                case nameof(IGPSProperties.LongitudeDegrees):
                    Dispatcher.CheckInvoke(() => LongitudeDegrees = Entity.LongitudeDegrees);
                    break;
                case nameof(IGPSProperties.LongitudeMinutes):
                    Dispatcher.CheckInvoke(() => LongitudeMinutes = Entity.LongitudeMinutes);
                    break;
                case nameof(IGPSProperties.LongitudeSeconds):
                    Dispatcher.CheckInvoke(() => LongitudeSeconds = Entity.LongitudeSeconds);
                    break;
                case nameof(IGPSProperties.LongitudeRef):
                    Dispatcher.CheckInvoke(() => LongitudeRef = Entity.LongitudeRef);
                    break;
                case nameof(IGPSProperties.MeasureMode):
                    Dispatcher.CheckInvoke(() => MeasureMode = Entity.MeasureMode);
                    break;
                case nameof(IGPSProperties.ProcessingMethod):
                    Dispatcher.CheckInvoke(() => ProcessingMethod = Entity.ProcessingMethod);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
