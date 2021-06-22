using FsInfoCat.Desktop.ViewModel;
using System;
using System.Windows.Controls;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Interaction logic for CrawlSourcesPage.xaml
    /// </summary>
    public partial class CrawlSourcesPage : Page
    {
        public CrawlSourcesPage()
        {
            InitializeComponent();
        }

        private void CrawlSourcesViewModel_NewCrawlSource(object sender, EventArgs e)
        {
            //if (sender is CrawlSourcesViewModel crawlSources)
            // TODO: Implement CrawlSourcesViewModel_NewCrawlSource
        }

        private void CrawlSourcesViewModel_EditItem(object sender, ItemEventArgs<CrawlSourceItemVM> e)
        {
            //if (sender is CrawlSourcesViewModel crawlSources)
            // TODO: Implement CrawlSourcesViewModel_EditItem
        }

        private void CrawlSourcesViewModel_DeleteItem(object sender, ItemEventArgs<CrawlSourceItemVM> e)
        {
            //if (sender is CrawlSourcesViewModel crawlSources)
            // TODO: Implement CrawlSourcesViewModel_DeleteItem
        }
    }
}
