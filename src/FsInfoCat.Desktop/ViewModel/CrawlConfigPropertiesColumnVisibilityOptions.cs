using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlConfigPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(CrawlConfigPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlConfigPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        /*
         * DbEntityRowViewModel<TEntity>.CreatedOn 
         * CrawlConfigListItemViewModel<TEntity>.Delete 
         * DependencyObject.DependencyObjectType 
         * DispatcherObject.Dispatcher 
         * CrawlConfigurationRowViewModel<TEntity>.DisplayName 
         * CrawlConfigListItemViewModel<TEntity>.Edit 
         * CrawlConfigListItemViewModel<TEntity>.FileSystemDisplayName 
         * CrawlConfigListItemViewModel<TEntity>.FileSystemShortDescription 
         * CrawlConfigListItemViewModel<TEntity>.FileSystemSymbolicName 
         * DependencyObject.IsSealed CrawlConfigListItemViewModel<TEntity>.LastCrawlEnd 
         * CrawlConfigListItemViewModel<TEntity>.LastCrawlStart
         * CrawlConfigurationRowViewModel<TEntity>.MaxDuration
         * CrawlConfigurationRowViewModel<TEntity>.MaxRecursionDepth 
         * CrawlConfigurationRowViewModel<TEntity>.MaxTotalItems 
         * DbEntityRowViewModel<TEntity>.ModifiedOn
         * CrawlConfigListItemViewModel<TEntity>.NextScheduledStart
         * CrawlConfigurationRowViewModel<TEntity>.Notes 
         * CrawlConfigListItemViewModel<TEntity>.Path 
         * CrawlConfigurationRowViewModel<TEntity>.RescheduleAfterFail 
         * CrawlConfigurationRowViewModel<TEntity>.RescheduleFromJobEnd 
         * CrawlConfigListItemViewModel<TEntity>.RescheduleInterval
         * CrawlConfigListItemViewModel<TEntity>.StatusValue 
         * CrawlConfigListItemViewModel<TEntity>.TTL 
         * CrawlConfigListItemViewModel<TEntity>.VolumeDisplayName 
         * CrawlConfigListItemViewModel<TEntity>.VolumeIdentifier 
         * CrawlConfigListItemViewModel<TEntity>.VolumeName 
         * CrawlConfigListItemViewModel<TEntity>.VolumeShortDescription
         */

    }
}
