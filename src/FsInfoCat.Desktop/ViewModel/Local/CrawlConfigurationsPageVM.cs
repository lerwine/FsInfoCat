using FsInfoCat.Desktop.ViewModel.AsyncOps;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class CrawlConfigurationsPageVM : DbEntityListingPageVM<CrawlConfiguration, CrawlConfigItemVM>
    {
        #region Listing Options Members

        #region ShowActiveCrawlConfigurationsOnly Property Members

        public static readonly DependencyProperty ShowActiveCrawlConfigurationsOnlyProperty = DependencyProperty.Register(nameof(ShowActiveCrawlConfigurationsOnly),
            typeof(bool), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlConfigurationsPageVM).OnShowActiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowActiveCrawlConfigurationsOnly
        {
            get => (bool)GetValue(ShowActiveCrawlConfigurationsOnlyProperty);
            set => SetValue(ShowActiveCrawlConfigurationsOnlyProperty, value);
        }

        protected virtual void OnShowActiveCrawlConfigurationsOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowInactiveCrawlConfigurationsOnly = false;
                ShowAllCrawlConfigurations = false;
                CrawlConfigItemVM selectedCrawlConfig = SelectedItem;
                LoadItemsAsync().ContinueWith(t => OnItemsReloaded(selectedCrawlConfig));
            }
            else if (!(ShowInactiveCrawlConfigurationsOnly || ShowAllCrawlConfigurations))
                ShowInactiveCrawlConfigurationsOnly = true;
        }

        #endregion

        #region ShowInactiveCrawlConfigurationsOnly Property Members

        public static readonly DependencyProperty ShowInactiveCrawlConfigurationsOnlyProperty = DependencyProperty.Register(nameof(ShowInactiveCrawlConfigurationsOnly),
            typeof(bool), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlConfigurationsPageVM).OnShowInactiveCrawlConfigurationsOnlyPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowInactiveCrawlConfigurationsOnly
        {
            get => (bool)GetValue(ShowInactiveCrawlConfigurationsOnlyProperty);
            set => SetValue(ShowInactiveCrawlConfigurationsOnlyProperty, value);
        }

        protected virtual void OnShowInactiveCrawlConfigurationsOnlyPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveCrawlConfigurationsOnly = false;
                ShowAllCrawlConfigurations = false;
                CrawlConfigItemVM selectedCrawlConfig = SelectedItem;
                LoadItemsAsync().ContinueWith(t => OnItemsReloaded(selectedCrawlConfig));
            }
            else if (!(ShowActiveCrawlConfigurationsOnly || ShowAllCrawlConfigurations))
                ShowActiveCrawlConfigurationsOnly = true;
        }

        #endregion

        #region ShowAllCrawlConfigurations Property Members

        public static readonly DependencyProperty ShowAllCrawlConfigurationsProperty = DependencyProperty.Register(nameof(ShowAllCrawlConfigurations), typeof(bool),
            typeof(CrawlConfigurationsPageVM), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlConfigurationsPageVM).OnShowAllCrawlConfigurationsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ShowAllCrawlConfigurations
        {
            get => (bool)GetValue(ShowAllCrawlConfigurationsProperty);
            set => SetValue(ShowAllCrawlConfigurationsProperty, value);
        }

        protected virtual void OnShowAllCrawlConfigurationsPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                ShowActiveCrawlConfigurationsOnly = false;
                ShowInactiveCrawlConfigurationsOnly = false;
                CrawlConfigItemVM selectedCrawlConfig = SelectedItem;
                LoadItemsAsync().ContinueWith(t => OnItemsReloaded(selectedCrawlConfig));
            }
            else if (!(ShowActiveCrawlConfigurationsOnly || ShowInactiveCrawlConfigurationsOnly))
                ShowActiveCrawlConfigurationsOnly = true;
        }

        #endregion

        #endregion

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? loadOptions = ShowAllCrawlConfigurations ? null : ShowActiveCrawlConfigurationsOnly;
            return listener => Task.Run(async () => await LoadItemsAsync(loadOptions, listener));
        }

        private void OnItemsReloaded(CrawlConfigItemVM toSelect)
        {
            Dispatcher.Invoke(() =>
            {
                if (toSelect is null)
                {
                    if (SelectedItem is null && Items.Count > 0)
                        SelectedItem = Items.FirstOrDefault();
                }
                {
                    Guid id = toSelect.Model.Id;
                    if ((toSelect = Items.FirstOrDefault(i => i.Model.Id == id)) is not null)
                        SelectedItem = toSelect;
                }
            });
        }

        private async Task<int> LoadItemsAsync(bool? showActive, IStatusListener statusListener)
        {
            statusListener.CancellationToken.ThrowIfCancellationRequested();
            IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IIncludableQueryable<CrawlConfiguration, Subdirectory> crawlConfigurations = dbContext.CrawlConfigurations.Include(c => c.Root);
            IQueryable<CrawlConfiguration> items;
            if (showActive.HasValue)
            {
                if (showActive.Value)
                    items = from v in crawlConfigurations where v.StatusValue !=  CrawlStatus.Disabled select v;
                else
                    items = from v in crawlConfigurations where v.StatusValue == CrawlStatus.Disabled select v;
            }
            else
                items = from v in crawlConfigurations select v;
            List<Subdirectory.CrawlConfigWithFullRootPath<CrawlConfiguration>> result;
            DispatcherOperation dispatcherOperation = statusListener.BeginSetMessage($"Calculating full paths of configuration items");
            try
            {
                result = await Subdirectory.BuildFullNamesAsync(items, c => c.Root, dbContext, statusListener.CancellationToken);
            }
            finally { await dispatcherOperation; }
            return await OnEntitiesLoaded(result, statusListener, r => new CrawlConfigItemVM(r.Source, r.FullName, r.SubdirectoryId));
        }

        protected override CrawlConfigItemVM CreateItem(CrawlConfiguration entity)
        {
            throw new NotImplementedException();
        }

        protected override CrawlConfiguration InitializeNewEntity()
        {
            throw new NotImplementedException();
        }

        protected override DbSet<CrawlConfiguration> GetDbSet(LocalDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveNewProgressTitle(CrawlConfigItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveExistingProgressTitle(CrawlConfigItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override string GetDeleteProgressTitle(CrawlConfigItemVM item)
        {
            throw new NotImplementedException();
        }

        protected override bool ShowModalItemEditWindow(CrawlConfigItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool PromptItemDeleting(CrawlConfigItemVM item, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
