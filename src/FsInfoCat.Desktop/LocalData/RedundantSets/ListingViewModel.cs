using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using FsInfoCat.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.RedundantSets
{
    public class ListingViewModel : ListingViewModel<RedundantSetListItem, ListItemViewModel, ListingViewModel.ListingOptions, RedundantSet, ItemEditResult>, INavigatedToNotifiable
    {
        private ListingOptions _currentRange;

        #region MinimumRange Property Members

        private static readonly DependencyPropertyKey MinimumRangePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MinimumRange), typeof(DenominatedLengthViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MinimumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumRangeProperty = MinimumRangePropertyKey.DependencyProperty;

        public DenominatedLengthViewModel MinimumRange { get => (DenominatedLengthViewModel)GetValue(MinimumRangeProperty); private set => SetValue(MinimumRangePropertyKey, value); }

        #endregion
        #region MaximumRange Property Members

        private static readonly DependencyPropertyKey MaximumRangePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaximumRange), typeof(DenominatedLengthViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MaximumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRangeProperty = MaximumRangePropertyKey.DependencyProperty;

        public DenominatedLengthViewModel MaximumRange => (DenominatedLengthViewModel)GetValue(MaximumRangeProperty);

        #endregion
        private void UpdatePageTitle(ListingOptions options)
        {
            (BinaryDenominatedInt64F? minRange, BinaryDenominatedInt64F? maxRange) = options;
            if (minRange.HasValue)
            {
                if (maxRange.HasValue)
                    PageTitle = string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_RedundantSets_MinMax, minRange.Value.ToString(CultureInfo.CurrentUICulture),
                        maxRange.Value.ToString(CultureInfo.CurrentUICulture));
                else
                    PageTitle = string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_RedundantSets_MinOnly, minRange.Value.ToString(CultureInfo.CurrentUICulture));
            }
            else if (maxRange.HasValue)
                PageTitle = string.Format(FsInfoCat.Properties.Resources.FormatDisplayName_RedundantSets_MaxOnly, maxRange.Value.ToString(CultureInfo.CurrentUICulture));
            else
                PageTitle = FsInfoCat.Properties.Resources.DisplayName_RedundantSets_All;
        }


        protected override IAsyncJob ReloadAsync(ListingOptions options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        protected override bool EntityMatchesCurrentFilter([DisallowNull] RedundantSetListItem entity) => _currentRange.MinRange.HasValue ?
                entity.Length >= _currentRange.MinRange.Value && (!_currentRange.MaxRange.HasValue || entity.Length < _currentRange.MaxRange.Value) :
                !_currentRange.MaxRange.HasValue || entity.Length < _currentRange.MaxRange.Value;

        protected override IQueryable<RedundantSetListItem> GetQueryableListing(ListingOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading redundancy sets from database");
            if (options.MinRange.HasValue)
            {
                long minimum = options.MinRange.Value;
                if (options.MaxRange.HasValue)
                {
                    long maximum = options.MaxRange.Value;
                    return dbContext.RedundantSetListing.Where(r => r.Length >= minimum && r.Length < maximum);
                }
                return dbContext.RedundantSetListing.Where(r => r.Length >= minimum);
            }
            if (options.MaxRange.HasValue)
            {
                long maximum = options.MaxRange.Value;
                return dbContext.RedundantSetListing.Where(r => r.Length < maximum);
            }
            return dbContext.RedundantSetListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] RedundantSetListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            ListingOptions newRange = new(MinimumRange.DenominatedValue, MaximumRange.DenominatedValue);
            if (_currentRange.MaxRange != newRange.MaxRange || _currentRange.MinRange != newRange.MinRange)
                ReloadAsync(newRange);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(new(MinimumRange.DenominatedValue, MaximumRange.DenominatedValue));
            MinimumRange.DenominatedValue = _currentRange.MinRange;
            MaximumRange.DenominatedValue = _currentRange.MaxRange;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentRange);

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this redundancy set from the database?",
            "Delete Redundancy Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<EntityEntry> DeleteEntityFromDbContextAsync([DisallowNull] RedundantSetListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            RedundantSet target = await dbContext.RedundantSets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return null;
            EntityEntry entry = dbContext.RedundantSets.Remove(target);
            await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            return entry;
        }

        void INavigatedToNotifiable.OnNavigatedTo() => ReloadAsync(_currentRange);

        protected override void OnReloadTaskCompleted(ListingOptions options) => _currentRange = options;

        protected override void OnReloadTaskFaulted([DisallowNull] Exception exception, ListingOptions options)
        {
            MinimumRange.DenominatedValue = _currentRange.MinRange;
            MaximumRange.DenominatedValue = _currentRange.MaxRange;
            UpdatePageTitle(_currentRange);
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(ListingOptions options)
        {
            MinimumRange.DenominatedValue = _currentRange.MinRange;
            MaximumRange.DenominatedValue = _currentRange.MaxRange;
            UpdatePageTitle(_currentRange);
        }

        protected override PageFunction<ItemEditResult> GetEditPage(RedundantSet args)
        {
            EditViewModel viewModel;
            if (args is null)
                viewModel = new(new RedundantSet(), true);
            else
                viewModel = new EditViewModel(args, false);
            return new EditPage(viewModel);
        }

        protected override async Task<RedundantSet> LoadItemAsync([DisallowNull] RedundantSetListItem item, [DisallowNull] IWindowsStatusListener statusListener)
        {
            using IServiceScope serviceScope = Services.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
            Guid id = item.Id;
            statusListener.SetMessage("Reading data");
            return await dbContext.RedundantSets.FirstOrDefaultAsync(e => e.Id == id, statusListener.CancellationToken);
        }

        protected override void OnEditTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            UpdatePageTitle(_currentRange);
            MinimumRange.DenominatedValue = _currentRange.MinRange;
            MaximumRange.DenominatedValue = _currentRange.MaxRange;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnDeleteTaskFaulted([DisallowNull] Exception exception, [DisallowNull] ListItemViewModel item)
        {
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while deleting the item from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public ListingViewModel()
        {
            DenominatedLengthViewModel minRange = new();
            SetValue(MinimumRangePropertyKey, minRange);
            DenominatedLengthViewModel maxRange = new();
            SetValue(MaximumRangePropertyKey, maxRange);
            _currentRange = new(minRange.DenominatedValue, maxRange.DenominatedValue);
            UpdatePageTitle(_currentRange);
        }

        public record ListingOptions(BinaryDenominatedInt64F? MinRange, BinaryDenominatedInt64F? MaxRange);
    }
}
