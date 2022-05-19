using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class GPSPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IGPSProperties
    {
        #region AreaInformation Property Members

        /// <summary>
        /// Identifies the <see cref="AreaInformation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AreaInformationProperty = ColumnPropertyBuilder<string, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.AreaInformation))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnAreaInformationPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string AreaInformation { get => GetValue(AreaInformationProperty) as string; set => SetValue(AreaInformationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="AreaInformation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="AreaInformation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="AreaInformation"/> property.</param>
        protected virtual void OnAreaInformationPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region LatitudeDegrees Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeDegrees"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeDegreesProperty = ColumnPropertyBuilder<double?, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LatitudeDegrees))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeDegreesPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? LatitudeDegrees { get => (double?)GetValue(LatitudeDegreesProperty); set => SetValue(LatitudeDegreesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeDegrees"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeDegrees"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeDegrees"/> property.</param>
        protected virtual void OnLatitudeDegreesPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region LatitudeMinutes Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeMinutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeMinutesProperty = ColumnPropertyBuilder<double?, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LatitudeMinutes))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeMinutesPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? LatitudeMinutes { get => (double?)GetValue(LatitudeMinutesProperty); set => SetValue(LatitudeMinutesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeMinutes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeMinutes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeMinutes"/> property.</param>
        protected virtual void OnLatitudeMinutesPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region LatitudeSeconds Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeSeconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeSecondsProperty = ColumnPropertyBuilder<double?, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LatitudeSeconds))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeSecondsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? LatitudeSeconds { get => (double?)GetValue(LatitudeSecondsProperty); set => SetValue(LatitudeSecondsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeSeconds"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeSeconds"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeSeconds"/> property.</param>
        protected virtual void OnLatitudeSecondsPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region LatitudeRef Property Members

        /// <summary>
        /// Identifies the <see cref="LatitudeRef"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LatitudeRefProperty = ColumnPropertyBuilder<string, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LatitudeRef))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLatitudeRefPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string LatitudeRef { get => GetValue(LatitudeRefProperty) as string; set => SetValue(LatitudeRefProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LatitudeRef"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LatitudeRef"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LatitudeRef"/> property.</param>
        protected virtual void OnLatitudeRefPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region LongitudeDegrees Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeDegrees"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeDegreesProperty = ColumnPropertyBuilder<double?, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LongitudeDegrees))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeDegreesPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? LongitudeDegrees { get => (double?)GetValue(LongitudeDegreesProperty); set => SetValue(LongitudeDegreesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeDegrees"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeDegrees"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeDegrees"/> property.</param>
        protected virtual void OnLongitudeDegreesPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region LongitudeMinutes Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeMinutes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeMinutesProperty = ColumnPropertyBuilder<double?, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LongitudeMinutes))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeMinutesPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? LongitudeMinutes { get => (double?)GetValue(LongitudeMinutesProperty); set => SetValue(LongitudeMinutesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeMinutes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeMinutes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeMinutes"/> property.</param>
        protected virtual void OnLongitudeMinutesPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region LongitudeSeconds Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeSeconds"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeSecondsProperty = ColumnPropertyBuilder<double?, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LongitudeSeconds))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeSecondsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public double? LongitudeSeconds { get => (double?)GetValue(LongitudeSecondsProperty); set => SetValue(LongitudeSecondsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeSeconds"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeSeconds"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeSeconds"/> property.</param>
        protected virtual void OnLongitudeSecondsPropertyChanged(double? oldValue, double? newValue) { }

        #endregion
        #region LongitudeRef Property Members

        /// <summary>
        /// Identifies the <see cref="LongitudeRef"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LongitudeRefProperty = ColumnPropertyBuilder<string, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.LongitudeRef))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnLongitudeRefPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string LongitudeRef { get => GetValue(LongitudeRefProperty) as string; set => SetValue(LongitudeRefProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="LongitudeRef"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="LongitudeRef"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="LongitudeRef"/> property.</param>
        protected virtual void OnLongitudeRefPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region MeasureMode Property Members

        /// <summary>
        /// Identifies the <see cref="MeasureMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MeasureModeProperty = ColumnPropertyBuilder<string, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.MeasureMode))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnMeasureModePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string MeasureMode { get => GetValue(MeasureModeProperty) as string; set => SetValue(MeasureModeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MeasureMode"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MeasureMode"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MeasureMode"/> property.</param>
        protected virtual void OnMeasureModePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region ProcessingMethod Property Members

        /// <summary>
        /// Identifies the <see cref="ProcessingMethod"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProcessingMethodProperty = ColumnPropertyBuilder<string, GPSPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IGPSProperties.ProcessingMethod))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as GPSPropertiesRowViewModel<TEntity>)?.OnProcessingMethodPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string ProcessingMethod { get => GetValue(ProcessingMethodProperty) as string; set => SetValue(ProcessingMethodProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ProcessingMethod"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ProcessingMethod"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ProcessingMethod"/> property.</param>
        protected virtual void OnProcessingMethodPropertyChanged(string oldValue, string newValue) { }

        #endregion

        public GPSPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
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

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_LatitudeDegrees, LatitudeDegrees?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_LatitudeMinutes, LatitudeMinutes?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_LatitudeSeconds, LatitudeSeconds?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_LatitudeRef, LatitudeRef.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_LongitudeDegrees, LongitudeDegrees?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_LongitudeMinutes, LongitudeMinutes?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_LongitudeSeconds, LongitudeSeconds?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_LongitudeRef, LongitudeRef.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_MeasureMode, MeasureMode.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_ProcessingMethod, ProcessingMethod.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_AreaInformation, AreaInformation.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Model.IGPSProperties.AreaInformation):
                    Dispatcher.CheckInvoke(() => AreaInformation = Entity.AreaInformation);
                    break;
                case nameof(Model.IGPSProperties.LatitudeDegrees):
                    Dispatcher.CheckInvoke(() => LatitudeDegrees = Entity.LatitudeDegrees);
                    break;
                case nameof(Model.IGPSProperties.LatitudeMinutes):
                    Dispatcher.CheckInvoke(() => LatitudeMinutes = Entity.LatitudeMinutes);
                    break;
                case nameof(Model.IGPSProperties.LatitudeSeconds):
                    Dispatcher.CheckInvoke(() => LatitudeSeconds = Entity.LatitudeSeconds);
                    break;
                case nameof(Model.IGPSProperties.LatitudeRef):
                    Dispatcher.CheckInvoke(() => LatitudeRef = Entity.LatitudeRef);
                    break;
                case nameof(Model.IGPSProperties.LongitudeDegrees):
                    Dispatcher.CheckInvoke(() => LongitudeDegrees = Entity.LongitudeDegrees);
                    break;
                case nameof(Model.IGPSProperties.LongitudeMinutes):
                    Dispatcher.CheckInvoke(() => LongitudeMinutes = Entity.LongitudeMinutes);
                    break;
                case nameof(Model.IGPSProperties.LongitudeSeconds):
                    Dispatcher.CheckInvoke(() => LongitudeSeconds = Entity.LongitudeSeconds);
                    break;
                case nameof(Model.IGPSProperties.LongitudeRef):
                    Dispatcher.CheckInvoke(() => LongitudeRef = Entity.LongitudeRef);
                    break;
                case nameof(Model.IGPSProperties.MeasureMode):
                    Dispatcher.CheckInvoke(() => MeasureMode = Entity.MeasureMode);
                    break;
                case nameof(Model.IGPSProperties.ProcessingMethod):
                    Dispatcher.CheckInvoke(() => ProcessingMethod = Entity.ProcessingMethod);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
