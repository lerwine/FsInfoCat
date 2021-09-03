using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class PhotoPropertiesRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IPhotoProperties
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region CameraManufacturer Property Members

        /// <summary>
        /// Identifies the <see cref="CameraManufacturer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CameraManufacturerProperty = DependencyProperty.Register(nameof(CameraManufacturer), typeof(string),
            typeof(PhotoPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as PhotoPropertiesRowViewModel<TEntity>)?.OnCameraManufacturerPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CameraManufacturer { get => GetValue(CameraManufacturerProperty) as string; set => SetValue(CameraManufacturerProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CameraManufacturer"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CameraManufacturer"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CameraManufacturer"/> property.</param>
        protected void OnCameraManufacturerPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCameraManufacturerPropertyChanged Logic
        }

        #endregion
        #region CameraModel Property Members

        /// <summary>
        /// Identifies the <see cref="CameraModel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CameraModelProperty = DependencyProperty.Register(nameof(CameraModel), typeof(string),
            typeof(PhotoPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as PhotoPropertiesRowViewModel<TEntity>)?.OnCameraModelPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string CameraModel { get => GetValue(CameraModelProperty) as string; set => SetValue(CameraModelProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="CameraModel"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="CameraModel"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="CameraModel"/> property.</param>
        protected void OnCameraModelPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnCameraModelPropertyChanged Logic
        }

        #endregion
        #region DateTaken Property Members

        /// <summary>
        /// Identifies the <see cref="DateTaken"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateTakenProperty = DependencyProperty.Register(nameof(DateTaken), typeof(DateTime?),
            typeof(PhotoPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as PhotoPropertiesRowViewModel<TEntity>)?.OnDateTakenPropertyChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? DateTaken { get => (DateTime?)GetValue(DateTakenProperty); set => SetValue(DateTakenProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DateTaken"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DateTaken"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DateTaken"/> property.</param>
        protected void OnDateTakenPropertyChanged(DateTime? oldValue, DateTime? newValue)
        {
            // TODO: Implement OnDateTakenPropertyChanged Logic
        }

        #endregion
        #region EXIFVersion Property Members

        /// <summary>
        /// Identifies the <see cref="EXIFVersion"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EXIFVersionProperty = DependencyProperty.Register(nameof(EXIFVersion), typeof(string),
            typeof(PhotoPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as PhotoPropertiesRowViewModel<TEntity>)?.OnEXIFVersionPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string EXIFVersion { get => GetValue(EXIFVersionProperty) as string; set => SetValue(EXIFVersionProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="EXIFVersion"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="EXIFVersion"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="EXIFVersion"/> property.</param>
        protected void OnEXIFVersionPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnEXIFVersionPropertyChanged Logic
        }

        #endregion
        #region Orientation Property Members

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(ushort?),
            typeof(PhotoPropertiesRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as PhotoPropertiesRowViewModel<TEntity>)?.OnOrientationPropertyChanged((ushort?)e.OldValue, (ushort?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public ushort? Orientation { get => (ushort?)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Orientation"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Orientation"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Orientation"/> property.</param>
        protected void OnOrientationPropertyChanged(ushort? oldValue, ushort? newValue)
        {
            // TODO: Implement OnOrientationPropertyChanged Logic
        }

        #endregion
        #region OrientationText Property Members

        /// <summary>
        /// Identifies the <see cref="OrientationText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationTextProperty = DependencyProperty.Register(nameof(OrientationText), typeof(string),
            typeof(PhotoPropertiesRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as PhotoPropertiesRowViewModel<TEntity>)?.OnOrientationTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string OrientationText { get => GetValue(OrientationTextProperty) as string; set => SetValue(OrientationTextProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="OrientationText"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="OrientationText"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="OrientationText"/> property.</param>
        protected void OnOrientationTextPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnOrientationTextPropertyChanged Logic
        }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public PhotoPropertiesRowViewModel([DisallowNull] TEntity entity) : base(entity)
        {
            CameraManufacturer = entity.CameraManufacturer;
            CameraModel = entity.CameraModel;
            DateTaken = entity.DateTaken;
            EXIFVersion = entity.EXIFVersion;
            Orientation = entity.Orientation;
            OrientationText = entity.OrientationText;
        }

        internal string CalculateDisplayText()
        {
            // TODO: Calculate value for ListingViewModel<TEntity, TItem, TOptions>.SetItemDisplayText(string)
            throw new NotImplementedException();
        }

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
