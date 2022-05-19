using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : Model.DbEntity, Model.IPhotoPropertiesListItem
        where TViewModel : PhotoPropertiesListItemViewModel<TEntity>
    {
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion
        #region CameraManufacturer Property Members

        /// <summary>
        /// Identifies the <see cref="CameraManufacturer"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CameraManufacturerProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CameraManufacturer))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CameraManufacturer { get => (bool)GetValue(CameraManufacturerProperty); set => SetValue(CameraManufacturerProperty, value); }

        #endregion
        #region CameraModel Property Members

        /// <summary>
        /// Identifies the <see cref="CameraModel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CameraModelProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CameraModel))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CameraModel { get => (bool)GetValue(CameraModelProperty); set => SetValue(CameraModelProperty, value); }

        #endregion
        #region DateTaken Property Members

        /// <summary>
        /// Identifies the <see cref="DateTaken"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DateTakenProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DateTaken))
            .DefaultValue(false)
            .AsReadWrite();

        public bool DateTaken { get => (bool)GetValue(DateTakenProperty); set => SetValue(DateTakenProperty, value); }

        #endregion
        #region Event Property Members

        /// <summary>
        /// Identifies the <see cref="Event"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Event))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Event { get => (bool)GetValue(EventProperty); set => SetValue(EventProperty, value); }

        #endregion
        #region EXIFVersion Property Members

        /// <summary>
        /// Identifies the <see cref="EXIFVersion"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EXIFVersionProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(EXIFVersion))
            .DefaultValue(false)
            .AsReadWrite();

        public bool EXIFVersion { get => (bool)GetValue(EXIFVersionProperty); set => SetValue(EXIFVersionProperty, value); }

        #endregion
        #region Orientation Property Members

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Orientation))
            .DefaultValue(false)
            .AsReadWrite();

        public bool Orientation { get => (bool)GetValue(OrientationProperty); set => SetValue(OrientationProperty, value); }

        #endregion
        #region OrientationText Property Members

        /// <summary>
        /// Identifies the <see cref="OrientationText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationTextProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(OrientationText))
            .DefaultValue(false)
            .AsReadWrite();

        public bool OrientationText { get => (bool)GetValue(OrientationTextProperty); set => SetValue(OrientationTextProperty, value); }

        #endregion
        #region PeopleNames Property Members

        /// <summary>
        /// Identifies the <see cref="PeopleNames"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeopleNamesProperty = DependencyPropertyBuilder<PhotoPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(PeopleNames))
            .DefaultValue(false)
            .AsReadWrite();

        public bool PeopleNames { get => (bool)GetValue(PeopleNamesProperty); set => SetValue(PeopleNamesProperty, value); }

        #endregion

        protected PhotoPropertiesColumnVisibilityOptions() : base() { }
    }
}
