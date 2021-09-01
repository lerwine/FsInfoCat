using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using FsInfoCat.Numerics;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.RedundantSets
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

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DenominatedLengthViewModel MinimumRange { get => (DenominatedLengthViewModel)GetValue(MinimumRangeProperty); private set => SetValue(MinimumRangePropertyKey, value); }

        #endregion
        #region MaximumRange Property Members

        private static readonly DependencyPropertyKey MaximumRangePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaximumRange), typeof(DenominatedLengthViewModel), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="MaximumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRangeProperty = MaximumRangePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public DenominatedLengthViewModel MaximumRange => (DenominatedLengthViewModel)GetValue(MaximumRangeProperty);

        #endregion

        protected override IQueryable<RedundantSetListItem> GetQueryableListing(ListingOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
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

        protected override void OnSaveFilterOptionsCommand(object parameter)
        {
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
            throw new NotImplementedException();
        }

        protected override bool ConfirmItemDelete([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] RedundantSetListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void MaximumRange_DenominatedValuePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public record ListingOptions(BinaryDenominatedInt64? MinRange, BinaryDenominatedInt64? MaxRange);
    }
}
