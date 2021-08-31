using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Windows;

namespace FsInfoCat.Desktop.Local.CrawlLogs
{
    public class ListItemViewModel : CrawlJobListItemViewModel<CrawlJobLogListItem>, ILocalCrudEntityRowViewModel<CrawlJobLogListItem>
    {
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?), typeof(ListItemViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion

        public ListItemViewModel(CrawlJobLogListItem entity) : base(entity)
        {
            LastSynchronizedOn = entity.LastSynchronizedOn;
        }
    }
}
