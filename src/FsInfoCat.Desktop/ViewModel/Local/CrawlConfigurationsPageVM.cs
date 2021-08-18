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
        #region SelectedItem Property Members

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(CrawlConfigItemVM), typeof(CrawlConfigurationsPageVM), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public CrawlConfigItemVM SelectedItem { get => (CrawlConfigItemVM)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

        #endregion
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(CrawlConfigurationsPageVM),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion

        public CrawlConfigurationsPageVM()
        {
            ThreeStateViewModel viewOptions = new(true);
            SetValue(ViewOptionsPropertyKey, viewOptions);
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            viewOptions.ValuePropertyChanged += (s, e) =>
            {
                CrawlConfigItemVM selectedCrawlConfig = SelectedItem;
                LoadItemsAsync().ContinueWith(t => OnItemsReloaded(selectedCrawlConfig));
            };
        }

        protected override Func<IStatusListener, Task<int>> GetItemsLoaderFactory()
        {
            bool? viewOptions = ViewOptions.Value;
            return listener => Task.Run(async () => await LoadItemsAsync(viewOptions, listener));
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
