using FsInfoCat.Activities;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.SymbolicNames
{
    public class ListingViewModel : ListingViewModel<SymbolicNameListItem, ListItemViewModel, bool?>, INavigatedToNotifiable
    {
        private bool? _currentStateFilterOption = true;

        #region StateFilterOption Property Members

        private static readonly DependencyPropertyKey StateFilterOptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StateFilterOption), typeof(ThreeStateViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="StateFilterOption"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StateFilterOptionProperty = StateFilterOptionPropertyKey.DependencyProperty;

        public ThreeStateViewModel StateFilterOption => (ThreeStateViewModel)GetValue(StateFilterOptionProperty);

        #endregion

        public ListingViewModel()
        {
            SetValue(StateFilterOptionPropertyKey, new ThreeStateViewModel(_currentStateFilterOption));
            UpdatePageTitle(_currentStateFilterOption);
        }

        protected override IAsyncAction<IActivityEvent> RefreshAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.RefreshAsync(options);
        }

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_SymbolicNames_ActiveOnly :
                    FsInfoCat.Properties.Resources.DisplayName_SymbolicNames_InactiveOnly) :
                    FsInfoCat.Properties.Resources.DisplayName_SymbolicNames_All;

        void INavigatedToNotifiable.OnNavigatedTo() => RefreshAsync(_currentStateFilterOption);

        protected override IQueryable<SymbolicNameListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            progress.Report("Reading symbolic nams from database");
            return options.HasValue ? (options.Value ? dbContext.SymbolicNameListing.Where(f => !f.IsInactive) : dbContext.SymbolicNameListing.Where(f => f.IsInactive)) :
                dbContext.SymbolicNameListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] SymbolicNameListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentStateFilterOption.HasValue ? (!StateFilterOption.Value.HasValue || _currentStateFilterOption.Value != StateFilterOption.Value.Value) : StateFilterOption.Value.HasValue)
                _ = RefreshAsync(StateFilterOption.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => RefreshAsync(_currentStateFilterOption);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this symbolic name from the database?",
            "Delete Symbolic Name", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] SymbolicNameListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IActivityProgress progress)
        {
            SymbolicName target = await dbContext.SymbolicNames.FindAsync(new object[] { entity.Id }, progress.Token);
            if (target is null)
                return null;
            EntityEntry<SymbolicName> entityEntry = dbContext.SymbolicNames.Remove(target);
            await dbContext.SaveChangesAsync(progress.Token);
            return entityEntry;
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentStateFilterOption = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, bool? options)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(bool? options)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] SymbolicNameListItem entity) => !_currentStateFilterOption.HasValue || _currentStateFilterOption.Value != entity.IsInactive;

        protected override Task<PageFunction<ItemFunctionResultEventArgs>> GetDetailPageAsync([DisallowNull] ListItemViewModel item, [DisallowNull] IActivityProgress progress) => GetEditPageAsync(item, progress);

        protected async override Task<PageFunction<ItemFunctionResultEventArgs>> GetEditPageAsync(ListItemViewModel item, [DisallowNull] IActivityProgress progress)
        {
            if (item is null)
                return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(new(), null)));
            using IServiceScope serviceScope = Hosting.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Entity.Id;
            SymbolicName fs = await dbContext.SymbolicNames.FirstOrDefaultAsync(f => f.Id == id, progress.Token);
            if (fs is null)
            {
                await Dispatcher.ShowMessageBoxAsync("Item not found in database. Click OK to refresh listing.", "Security Exception", MessageBoxButton.OK, MessageBoxImage.Error, progress.Token);
                RefreshAsync(_currentStateFilterOption);
                return null;
            }
            return await Dispatcher.InvokeAsync<PageFunction<ItemFunctionResultEventArgs>>(() => new EditPage(new(fs, item.Entity)));
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, ListItemViewModel item)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is ActivityException aExc) ? aExc.ToString().NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<ActivityException>().Select(e => e.ToString())
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the database.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
