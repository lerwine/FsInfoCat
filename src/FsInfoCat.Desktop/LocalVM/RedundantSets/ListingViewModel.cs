using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using FsInfoCat.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.RedundantSets
{
    public class ListingViewModel : ListingViewModel<RedundantSetListItem, ListItemViewModel, ListingViewModel.ListingOptions>, INotifyNavigatedTo
    {
        private ListingOptions _currentRange;
        private ListingOptions _editingRange;

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
            // BUG: Need to fix logic
            _currentRange = _editingRange;
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            _editingRange = _currentRange;
            MinimumRange.DenominatedValue = _currentRange.MinRange;
            MaximumRange.DenominatedValue = _currentRange.MaxRange;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentRange);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(App.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this redundancy set from the database?",
            "Delete Redundancy Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] RedundantSetListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            RedundantSet target = await dbContext.RedundantSets.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            dbContext.RedundantSets.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentRange);

        public ListingViewModel()
        {
            DenominatedLengthViewModel minRange = new();
            SetValue(MinimumRangePropertyKey, minRange);
            DenominatedLengthViewModel maxRange = new();
            SetValue(MaximumRangePropertyKey, maxRange);
            minRange.DenominatedValuePropertyChanged += MinimumRange_DenominatedValuePropertyChanged;
            maxRange.DenominatedValuePropertyChanged += MaximumRange_DenominatedValuePropertyChanged;
            _currentRange = new(minRange.DenominatedValue, maxRange.DenominatedValue);
            _editingRange = _currentRange;
        }

        private void MinimumRange_DenominatedValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Implement MinimumRange_DenominatedValuePropertyChanged(object, e)
            throw new NotImplementedException();
        }

        private void MaximumRange_DenominatedValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Implement MaximumRange_DenominatedValuePropertyChanged(object, e)
            throw new NotImplementedException();
        }

        public record ListingOptions(BinaryDenominatedInt64? MinRange, BinaryDenominatedInt64? MaxRange);
    }
}
