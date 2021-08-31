using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CrawlJobListItemViewModel<TEntity> : CrawlJobRowViewModel<TEntity>
        where TEntity : DbEntity, ICrawlJobListItem
    {
        #region ConfigurationDisplayName Property Members

        private static readonly DependencyPropertyKey ConfigurationDisplayNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(ConfigurationDisplayName), typeof(string),
            typeof(CrawlJobListItemViewModel<TEntity>), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="ConfigurationDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ConfigurationDisplayNameProperty = ConfigurationDisplayNamePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string ConfigurationDisplayName { get => GetValue(ConfigurationDisplayNameProperty) as string; private set => SetValue(ConfigurationDisplayNamePropertyKey, value); }

        #endregion

        public CrawlJobListItemViewModel(TEntity entity) : base(entity)
        {

        }
    }
}
