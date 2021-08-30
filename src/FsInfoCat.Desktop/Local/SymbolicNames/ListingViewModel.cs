using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.Local.SymbolicNames
{
    public class ListingViewModel : DependencyObject, INotifyNavigatedTo
    {
        #region ViewOptions Property Members

        private static readonly DependencyPropertyKey ViewOptionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewOptions), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ViewOptions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ViewOptionsProperty = ViewOptionsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the view model for the listing view options.
        /// </summary>
        /// <value>The view model that indicates what items to load into the <see cref="Items"/> collection.</value>
        public ThreeStateViewModel ViewOptions => (ThreeStateViewModel)GetValue(ViewOptionsProperty);

        #endregion
        #region Items Property Members

        private readonly ObservableCollection<ListItemViewModel> _backingItems = new();

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(ReadOnlyObservableCollection<ListItemViewModel>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the items to be displayed in the page listing.
        /// </summary>
        /// <value>The items to be displayed in the page listing.</value>
        public ReadOnlyObservableCollection<ListItemViewModel> Items => (ReadOnlyObservableCollection<ListItemViewModel>)GetValue(ItemsProperty);

        #endregion

        public ListingViewModel()
        {
            SetValue(ViewOptionsPropertyKey, new ThreeStateViewModel());
            SetValue(ItemsPropertyKey, new ReadOnlyObservableCollection<ListItemViewModel>(_backingItems));
        }

        private IAsyncJob ReloadAsync(bool? showActiveOnly)
        {
            IWindowsAsyncJobFactoryService jobFactory = Services.GetRequiredService<IWindowsAsyncJobFactoryService>();
            return jobFactory.StartNew("Loading symbolic names", "Opening database", showActiveOnly, LoadItemsAsync);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(ViewOptions.Value);

        private async Task LoadItemsAsync(bool? showActiveOnly, IWindowsStatusListener statusListener)
        {
            using IServiceScope scope = Services.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            IQueryable<SymbolicNameListItem> items = showActiveOnly.HasValue ?
                (showActiveOnly.Value ? dbContext.SymbolicNameListing.Where(f => !f.IsInactive) : dbContext.SymbolicNameListing.Where(f => f.IsInactive)) :
                dbContext.SymbolicNameListing;
            await items.ForEachAsync(async item => await AddItemAsync(item, statusListener), statusListener.CancellationToken);
        }

        private DispatcherOperation AddItemAsync(SymbolicNameListItem model, IWindowsStatusListener statusListener) => Dispatcher.InvokeAsync(() =>
        {
            ListItemViewModel item = new ListItemViewModel(model);
            _backingItems.Add(item);
        }, DispatcherPriority.Background, statusListener.CancellationToken);
    }
}
