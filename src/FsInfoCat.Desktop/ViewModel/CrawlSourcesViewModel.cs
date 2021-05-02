using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class CrawlSourcesViewModel : DependencyObject
    {
        public event EventHandler NewCrawlSource;
        public event EventHandler<ItemEventArgs<CrawlSourceItemVM>> EditItem;
        public event EventHandler<ItemEventArgs<CrawlSourceItemVM>> DeleteItem;

        private static readonly DependencyPropertyKey CrawlSourcesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CrawlSources),
            typeof(ObservableCollection<CrawlSourceItemVM>), typeof(CrawlSourcesViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty CrawlSourcesProperty = CrawlSourcesPropertyKey.DependencyProperty;

        public ObservableCollection<CrawlSourceItemVM> CrawlSources => (ObservableCollection<CrawlSourceItemVM>)GetValue(CrawlSourcesProperty);

        private static readonly DependencyPropertyKey NewCrawlSourceCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NewCrawlSourceCommand),
            typeof(Commands.RelayCommand), typeof(CrawlSourcesViewModel), new PropertyMetadata(null));

        public static readonly DependencyProperty NewCrawlSourceCommandProperty = NewCrawlSourceCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand NewCrawlSourceCommand => (Commands.RelayCommand)GetValue(NewCrawlSourceCommandProperty);

        public CrawlSourcesViewModel()
        {
            ObservableCollection<CrawlSourceItemVM> crawlSources = new ObservableCollection<CrawlSourceItemVM>();
            SetValue(CrawlSourcesPropertyKey, crawlSources);
            SetValue(NewCrawlSourceCommandPropertyKey, new Commands.RelayCommand(() => NewCrawlSource?.Invoke(this, EventArgs.Empty)));
            crawlSources.CollectionChanged += CrawlSources_CollectionChanged;
        }

        private void CrawlSources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is IEnumerable<CrawlSourceItemVM> newItems)
                foreach (CrawlSourceItemVM item in newItems)
                {
                    item.ToggleActive += Item_ToggleActive;
                    item.SetActive += Item_SetActive;
                    item.SetInactive += Item_SetInactive;
                    item.Edit += Item_Edit;
                    item.Delete += Item_Delete;
                }
            if (e.OldItems is IEnumerable<CrawlSourceItemVM> oldItems)
                foreach (CrawlSourceItemVM item in oldItems)
                {
                    item.ToggleActive -= Item_ToggleActive;
                    item.SetActive -= Item_SetActive;
                    item.SetInactive -= Item_SetInactive;
                    item.Edit -= Item_Edit;
                    item.Delete -= Item_Delete;
                }
        }

        private void Item_ToggleActive(object sender, EventArgs e)
        {
            // TODO: Implement Item_ToggleActive
        }

        private void Item_SetActive(object sender, EventArgs e)
        {
            // TODO: Implement Item_SetActive
        }

        private void Item_SetInactive(object sender, EventArgs e)
        {
            // TODO: Implement Item_SetInactive
        }

        private void Item_Edit(object sender, EventArgs e)
        {
            if (sender is CrawlSourceItemVM item && CrawlSources.Contains(item))
                EditItem?.Invoke(this, new ItemEventArgs<CrawlSourceItemVM>(item));
        }

        private void Item_Delete(object sender, EventArgs e)
        {
            if (sender is CrawlSourceItemVM item && CrawlSources.Contains(item))
                DeleteItem?.Invoke(this, new ItemEventArgs<CrawlSourceItemVM>(item));
        }
    }
}
