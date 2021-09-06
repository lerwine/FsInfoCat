using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class PhotoPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IPhotoProperties
    {
        #region CameraManufacturer Property Members

        /// <summary>
        /// Identifies the <see cref="CameraManufacturer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CameraManufacturerProperty = ColumnPropertyBuilder<string, PhotoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoProperties.CameraManufacturer))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as PhotoPropertiesRowViewModel<TEntity>)?.OnCameraManufacturerPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string CameraManufacturer { get => GetValue(CameraManufacturerProperty) as string; set => SetValue(CameraManufacturerProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CameraManufacturer"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CameraManufacturer"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CameraManufacturer"/> property.</param>
        protected virtual void OnCameraManufacturerPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region CameraModel Property Members

        /// <summary>
        /// Identifies the <see cref="CameraModel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CameraModelProperty = ColumnPropertyBuilder<string, PhotoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoProperties.CameraModel))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as PhotoPropertiesRowViewModel<TEntity>)?.OnCameraModelPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string CameraModel { get => GetValue(CameraModelProperty) as string; set => SetValue(CameraModelProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CameraModel"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CameraModel"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CameraModel"/> property.</param>
        protected virtual void OnCameraModelPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region DateTaken Property Members

        /// <summary>
        /// Identifies the <see cref="DateTaken"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateTakenProperty = ColumnPropertyBuilder<DateTime?, PhotoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoProperties.DateTaken))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as PhotoPropertiesRowViewModel<TEntity>)?.OnDateTakenPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public DateTime? DateTaken { get => (DateTime?)GetValue(DateTakenProperty); set => SetValue(DateTakenProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DateTaken"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DateTaken"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DateTaken"/> property.</param>
        protected virtual void OnDateTakenPropertyChanged(DateTime? oldValue, DateTime? newValue) { }

        #endregion
        #region EXIFVersion Property Members

        /// <summary>
        /// Identifies the <see cref="EXIFVersion"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EXIFVersionProperty = ColumnPropertyBuilder<string, PhotoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoProperties.EXIFVersion))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as PhotoPropertiesRowViewModel<TEntity>)?.OnEXIFVersionPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string EXIFVersion { get => GetValue(EXIFVersionProperty) as string; set => SetValue(EXIFVersionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="EXIFVersion"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="EXIFVersion"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="EXIFVersion"/> property.</param>
        protected virtual void OnEXIFVersionPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Orientation Property Members

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = ColumnPropertyBuilder<ushort?, PhotoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoProperties.Orientation))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as PhotoPropertiesRowViewModel<TEntity>)?.OnOrientationPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public ushort? Orientation { get => (ushort?)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Orientation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Orientation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Orientation"/> property.</param>
        protected virtual void OnOrientationPropertyChanged(ushort? oldValue, ushort? newValue) { }

        #endregion
        #region OrientationText Property Members

        /// <summary>
        /// Identifies the <see cref="OrientationText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationTextProperty = ColumnPropertyBuilder<string, PhotoPropertiesRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IPhotoProperties.OrientationText))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as PhotoPropertiesRowViewModel<TEntity>)?.OnOrientationTextPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

        public string OrientationText { get => GetValue(OrientationTextProperty) as string; set => SetValue(OrientationTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="OrientationText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="OrientationText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="OrientationText"/> property.</param>
        protected virtual void OnOrientationTextPropertyChanged(string oldValue, string newValue) { }

        #endregion

        public PhotoPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            CameraManufacturer = entity.CameraManufacturer;
            CameraModel = entity.CameraModel;
            DateTaken = entity.DateTaken;
            EXIFVersion = entity.EXIFVersion;
            Orientation = entity.Orientation;
            OrientationText = entity.OrientationText;
        }

        public IEnumerable<(string DisplayName, string Value)> GetNameValuePairs()
        {
            yield return (FsInfoCat.Properties.Resources.DisplayName_DateTaken, DateTaken?.ToString());
            string orientation = OrientationText.AsWsNormalizedOrEmpty().TruncateWithElipses(256);
            yield return (FsInfoCat.Properties.Resources.DisplayName_Orientation, (orientation.Length > 0) ? orientation : Orientation?.ToString());
            yield return (FsInfoCat.Properties.Resources.DisplayName_EXIFVersion, EXIFVersion.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_CameraManufacturer, CameraManufacturer.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
            yield return (FsInfoCat.Properties.Resources.DisplayName_CameraModel, CameraModel.AsWsNormalizedOrEmpty().TruncateWithElipses(256));
        }

        internal string CalculateDisplayText(Func<(string DisplayName, string Value), bool> filter = null) => (filter is null) ?
            StringExtensionMethods.ToKeyValueListString(GetNameValuePairs()) : StringExtensionMethods.ToKeyValueListString(GetNameValuePairs().Where(filter));

        protected override void OnEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IPhotoProperties.CameraManufacturer):
                    Dispatcher.CheckInvoke(() => CameraManufacturer = Entity.CameraManufacturer);
                    break;
                case nameof(IPhotoProperties.CameraModel):
                    Dispatcher.CheckInvoke(() => CameraModel = Entity.CameraModel);
                    break;
                case nameof(IPhotoProperties.DateTaken):
                    Dispatcher.CheckInvoke(() => DateTaken = Entity.DateTaken);
                    break;
                case nameof(IPhotoProperties.EXIFVersion):
                    Dispatcher.CheckInvoke(() => EXIFVersion = Entity.EXIFVersion);
                    break;
                case nameof(IPhotoProperties.Orientation):
                    Dispatcher.CheckInvoke(() => Orientation = Entity.Orientation);
                    break;
                case nameof(IPhotoProperties.OrientationText):
                    Dispatcher.CheckInvoke(() => OrientationText = Entity.OrientationText);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    break;
            }
        }
    }
}
