using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel> : ColumnVisibilityOptionsViewModel<TEntity, TViewModel>
        where TEntity : Model.DbEntity, Model.ICrawlJobListItem
        where TViewModel : CrawlJobListItemViewModel<TEntity>
    {
        #region ConfigurationDisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="ConfigurationDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConfigurationDisplayNameProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(ConfigurationDisplayName))
            .DefaultValue(false)
            .AsReadWrite();

        public bool ConfigurationDisplayName { get => (bool)GetValue(ConfigurationDisplayNameProperty); set => SetValue(ConfigurationDisplayNameProperty, value); }

        #endregion
        #region CrawlEnd Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlEnd"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlEndProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CrawlEnd))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CrawlEnd { get => (bool)GetValue(CrawlEndProperty); set => SetValue(CrawlEndProperty, value); }

        #endregion
        #region CrawlStart Property Members

        /// <summary>
        /// Identifies the <see cref="CrawlStart"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CrawlStartProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(CrawlStart))
            .DefaultValue(false)
            .AsReadWrite();

        public bool CrawlStart { get => (bool)GetValue(CrawlStartProperty); set => SetValue(CrawlStartProperty, value); }

        #endregion
        #region RootPath Property Members

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(RootPath))
            .DefaultValue(false)
            .AsReadWrite();

        public bool RootPath { get => (bool)GetValue(RootPathProperty); set => SetValue(RootPathProperty, value); }

        #endregion
        #region StatusCode Property Members

        /// <summary>
        /// Identifies the <see cref="StatusCode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusCodeProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StatusCode))
            .DefaultValue(false)
            .AsReadWrite();

        public bool StatusCode { get => (bool)GetValue(StatusCodeProperty); set => SetValue(StatusCodeProperty, value); }

        #endregion
        #region StatusDetail Property Members

        /// <summary>
        /// Identifies the <see cref="StatusDetail"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusDetailProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StatusDetail))
            .DefaultValue(false)
            .AsReadWrite();

        public bool StatusDetail { get => (bool)GetValue(StatusDetailProperty); set => SetValue(StatusDetailProperty, value); }

        #endregion
        #region StatusMessage Property Members

        /// <summary>
        /// Identifies the <see cref="StatusMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusMessageProperty = DependencyPropertyBuilder<CrawlJobPropertiesColumnVisibilityOptions<TEntity, TViewModel>, bool>
            .Register(nameof(StatusMessage))
            .DefaultValue(false)
            .AsReadWrite();

        public bool StatusMessage { get => (bool)GetValue(StatusMessageProperty); set => SetValue(StatusMessageProperty, value); }

        #endregion

        protected CrawlJobPropertiesColumnVisibilityOptions() : base() { }
    }
}
