using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public abstract class DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : DbEntity, IDRMPropertiesListItem
        where TViewModel : DRMPropertiesListItemViewModel<TEntity>
    {
        #region DatePlayExpires Property Members

        /// <summary>
        /// Identifies the <see cref="DatePlayExpires"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DatePlayExpiresProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DatePlayExpires))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool DatePlayExpires { get => (bool)GetValue(DatePlayExpiresProperty); set => SetValue(DatePlayExpiresProperty, value); }

        #endregion
        #region DatePlayStarts Property Members

        /// <summary>
        /// Identifies the <see cref="DatePlayStarts"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DatePlayStartsProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(DatePlayStarts))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool DatePlayStarts { get => (bool)GetValue(DatePlayStartsProperty); set => SetValue(DatePlayStartsProperty, value); }

        #endregion
        #region Description Property Members

        /// <summary>
        /// Identifies the <see cref="Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(Description))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool Description { get => (bool)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }

        #endregion
        #region ExistingFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="ExistingFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExistingFileCountProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ExistingFileCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool ExistingFileCount { get => (bool)GetValue(ExistingFileCountProperty); set => SetValue(ExistingFileCountProperty, value); }

        #endregion
        #region IsProtected Property Members

        /// <summary>
        /// Identifies the <see cref="IsProtected"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsProtectedProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(IsProtected))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool IsProtected { get => (bool)GetValue(IsProtectedProperty); set => SetValue(IsProtectedProperty, value); }

        #endregion
        #region PlayCount Property Members

        /// <summary>
        /// Identifies the <see cref="PlayCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PlayCountProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(PlayCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool PlayCount { get => (bool)GetValue(PlayCountProperty); set => SetValue(PlayCountProperty, value); }

        #endregion
        #region TotalFileCount Property Members

        /// <summary>
        /// Identifies the <see cref="TotalFileCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TotalFileCountProperty = DependencyPropertyBuilder<DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(TotalFileCount))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as DRMPropertiesColumnVisibilityOptions<TEntity, TViewModel>)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool TotalFileCount { get => (bool)GetValue(TotalFileCountProperty); set => SetValue(TotalFileCountProperty, value); }

        #endregion

        protected DRMPropertiesColumnVisibilityOptions() : base() { }
    }
}
