using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.RedundantSets
{
    public class ListingViewModel : ListingViewModel<RedundantSetListItem, ListItemViewModel, (long? MinRange, long? MaxRange)>, INotifyNavigatedTo
    {
        private ListingOptions _currentRange;
        private ListingOptions _editingRange;

        public const long KB_MAX = 0x40000000000000L;
        public const long KB_DIV = 0x00000000400L;
        public const long MB_MAX = 0x00100000000000L;
        public const long MB_DIV = 0x00000100000L;
        public const long GB_MAX = 0x00000400000000L;
        public const long GB_DIV = 0x00040000000L;
        public const long TB_MAX = 0x00000001000000L;
        public const long TB_DIV = 0x10000000000L;

        #region MinimumRange Property Members

        /// <summary>
        /// Identifies the <see cref="MinimumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumRangeProperty = DependencyProperty.Register(nameof(MinimumRange), typeof(long?), typeof(ListingViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ListingViewModel)?.OnMinimumRangePropertyChanged((long?)e.OldValue, (long?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public long? MinimumRange { get => (long?)GetValue(MinimumRangeProperty); set => SetValue(MinimumRangeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MinimumRange"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MinimumRange"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MinimumRange"/> property.</param>
        private void OnMinimumRangePropertyChanged(long? oldValue, long? newValue)
        {
            if (HasMinimum)
                _editingRange = _editingRange with { Minimum = newValue };
        }

        #endregion
        #region HasMinimum Property Members

        /// <summary>
        /// Identifies the <see cref="HasMinimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasMinimumProperty = DependencyProperty.Register(nameof(HasMinimum), typeof(bool), typeof(ListingViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ListingViewModel)?.OnHasMinimumPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool HasMinimum { get => (bool)GetValue(HasMinimumProperty); set => SetValue(HasMinimumProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasMinimum"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HasMinimum"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HasMinimum"/> property.</param>
        private void OnHasMinimumPropertyChanged(bool oldValue, bool newValue) => _editingRange = _editingRange with { Minimum = (newValue) ? MinimumRange : null };

        #endregion
        #region MaximumRange Property Members

        /// <summary>
        /// Identifies the <see cref="MaximumRange"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRangeProperty = DependencyProperty.Register(nameof(MaximumRange), typeof(long?), typeof(ListingViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ListingViewModel)?.OnMaximumRangePropertyChanged((long?)e.OldValue, (long?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public long? MaximumRange { get => (long?)GetValue(MaximumRangeProperty); set => SetValue(MaximumRangeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaximumRange"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaximumRange"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaximumRange"/> property.</param>
        private void OnMaximumRangePropertyChanged(long? oldValue, long? newValue)
        {
            if (HasMaximum)
                _editingRange = _editingRange with { Maximum = newValue };
        }

        #endregion
        #region HasMaximum Property Members

        /// <summary>
        /// Identifies the <see cref="HasMaximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HasMaximumProperty = DependencyProperty.Register(nameof(HasMaximum), typeof(bool), typeof(ListingViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ListingViewModel)?.OnHasMaximumPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool HasMaximum { get => (bool)GetValue(HasMaximumProperty); set => SetValue(HasMaximumProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="HasMaximum"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="HasMaximum"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="HasMaximum"/> property.</param>
        private void OnHasMaximumPropertyChanged(bool oldValue, bool newValue) => _editingRange = _editingRange with { Maximum = (newValue) ? MaximumRange : null };

        #endregion
        #region Denomination Property Members

        private static readonly DependencyPropertyKey DenominationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Denomination), typeof(EnumValuePickerVM<RangeDenomination>), typeof(ListingViewModel),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Denomination"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DenominationProperty = DenominationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public EnumValuePickerVM<RangeDenomination> Denomination => (EnumValuePickerVM<RangeDenomination>)GetValue(DenominationProperty);

        private void Denomination_SelectedItemPropertyChanged(object sender, DependencyPropertyChangedEventArgs e) =>
            _editingRange = _editingRange with { Denomination = Denomination.SelectedValue ?? RangeDenomination.Megabytes };

        #endregion

        public static bool TryConvertToDenomination(long value, RangeDenomination denomination, out long result)
        {
            if (value < 0L)
            {
                result = value;
                return false;
            }
            switch (denomination)
            {
                case RangeDenomination.Kilobytes:
                    if (value > KB_MAX)
                    {
                        result = value;
                        return false;
                    }
                    result = value / KB_DIV;
                    break;
                case RangeDenomination.Megabytes:
                    if (value > MB_MAX)
                    {
                        result = value;
                        return false;
                    }
                    result = value / MB_DIV;
                    break;
                case RangeDenomination.Gigabytes:
                    if (value > GB_MAX)
                    {
                        result = value;
                        return false;
                    }
                    result = value / GB_DIV;
                    break;
                case RangeDenomination.Terabytes:
                    if (value > TB_MAX)
                    {
                        result = value;
                        return false;
                    }
                    result = value / TB_DIV;
                    break;
                default:
                    result = value;
                    break;
            }
            return true;
        }

        protected override IQueryable<RedundantSetListItem> GetQueryableListing((long? MinRange, long? MaxRange) options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
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
            if (_currentRange.Minimum.HasValue)
            {
                HasMinimum = true;
                MinimumRange = _currentRange.Minimum.Value;
            }
            else
            {
                HasMinimum = false;
                MinimumRange = null;
            }
            if (_currentRange.Maximum.HasValue)
            {
                HasMaximum = true;
                MaximumRange = _currentRange.Maximum.Value;
            }
            else
            {
                HasMaximum = false;
                MaximumRange = null;
            }
            Denomination.SelectedValue = _currentRange.Denomination;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentRange);

        private IAsyncJob ReloadAsync(ListingOptions currentRange)
        {
            if (currentRange.Minimum.HasValue)
            {
                TryConvertToDenomination(currentRange.Minimum.Value, currentRange.Denomination, out long minimum);
                if (currentRange.Maximum.HasValue)
                {
                    TryConvertToDenomination(currentRange.Maximum.Value, currentRange.Denomination, out long maximum);
                    return ReloadAsync((minimum, maximum));
                }
                return ReloadAsync((minimum, null));
            }
            if (currentRange.Maximum.HasValue)
            {
                TryConvertToDenomination(currentRange.Maximum.Value, currentRange.Denomination, out long maximum);
                return ReloadAsync((null, maximum));
            }
            return ReloadAsync((null, null));
        }

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
            EnumValuePickerVM<RangeDenomination> denomination = new() { SelectedValue = RangeDenomination.Megabytes };
            SetValue(DenominationPropertyKey, denomination);
            denomination.SelectedItemPropertyChanged += Denomination_SelectedItemPropertyChanged;
            _currentRange = new(HasMinimum ? MinimumRange : null, HasMinimum ? MaximumRange : null, Denomination.SelectedValue ?? RangeDenomination.Megabytes);
            _editingRange = _currentRange;
        }

        public record ListingOptions(long? Minimum, long? Maximum, RangeDenomination Denomination);
    }
}
