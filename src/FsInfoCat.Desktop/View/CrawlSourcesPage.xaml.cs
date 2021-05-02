using FsInfoCat.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    }
}
