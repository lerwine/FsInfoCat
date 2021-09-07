using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.SymbolicNames
{
    public class ListingViewModel : ListingViewModel<SymbolicNameListItem, ListItemViewModel, bool?>, INotifyNavigatedTo
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
        #region PageTitle Property Members

        private static readonly DependencyPropertyKey PageTitlePropertyKey = DependencyPropertyBuilder<ListingViewModel, string>
            .Register(nameof(PageTitle))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="PageTitle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PageTitleProperty = PageTitlePropertyKey.DependencyProperty;

        public string PageTitle { get => GetValue(PageTitleProperty) as string; private set => SetValue(PageTitlePropertyKey, value); }

        private void UpdatePageTitle(bool? options) => PageTitle = options.HasValue ?
                    (options.Value ? FsInfoCat.Properties.Resources.DisplayName_SymbolicNames_ActiveOnly :
                    FsInfoCat.Properties.Resources.DisplayName_SymbolicNames_InactiveOnly) :
                    FsInfoCat.Properties.Resources.DisplayName_SymbolicNames_All;

        #endregion

        public ListingViewModel()
        {
            SetValue(StateFilterOptionPropertyKey, new ThreeStateViewModel(_currentStateFilterOption));
            UpdatePageTitle(_currentStateFilterOption);
        }

        protected override IAsyncJob ReloadAsync(bool? options)
        {
            UpdatePageTitle(options);
            return base.ReloadAsync(options);
        }

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentStateFilterOption);

        protected override IQueryable<SymbolicNameListItem> GetQueryableListing(bool? options, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            statusListener.SetMessage("Reading symbolic nams from database");
            return options.HasValue ? (options.Value ? dbContext.SymbolicNameListing.Where(f => !f.IsInactive) : dbContext.SymbolicNameListing.Where(f => f.IsInactive)) :
                dbContext.SymbolicNameListing;
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] SymbolicNameListItem entity) => new(entity);

        protected override void OnApplyFilterOptionsCommand(object parameter)
        {
            if (_currentStateFilterOption.HasValue ? (!StateFilterOption.Value.HasValue || _currentStateFilterOption.Value != StateFilterOption.Value.Value) : StateFilterOption.Value.HasValue)
                _ = ReloadAsync(StateFilterOption.Value);
        }

        protected override void OnCancelFilterOptionsCommand(object parameter)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
            base.OnCancelFilterOptionsCommand(parameter);
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentStateFilterOption);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            // TODO: Implement OnItemEditCommand(object);
        }

        protected override bool ConfirmItemDelete(ListItemViewModel item, object parameter) => MessageBox.Show(Application.Current.MainWindow,
            "This action cannot be undone!\n\nAre you sure you want to remove this symbolic name from the database?",
            "Delete Symbolic Name", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] SymbolicNameListItem entity, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IWindowsStatusListener statusListener)
        {
            SymbolicName target = await dbContext.SymbolicNames.FindAsync(new object[] { entity.Id }, statusListener.CancellationToken);
            if (target is null)
                return 0;
            _ = dbContext.SymbolicNames.Remove(target);
            return await dbContext.SaveChangesAsync(statusListener.CancellationToken);
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            // TODO: Implement OnAddNewItemCommand(object);
        }

        protected override void OnReloadTaskCompleted(bool? options) => _currentStateFilterOption = options;

        protected override void OnReloadTaskFaulted(Exception exception, bool? options)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
            _ = MessageBox.Show(Application.Current.MainWindow,
                ((exception is AsyncOperationFailureException aExc) ? aExc.UserMessage.NullIfWhiteSpace() :
                    (exception as AggregateException)?.InnerExceptions.OfType<AsyncOperationFailureException>().Select(e => e.UserMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)).FirstOrDefault()) ??
                    "There was an unexpected error while loading items from the databse.\n\nSee logs for further information",
                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected override void OnReloadTaskCanceled(bool? options)
        {
            UpdatePageTitle(_currentStateFilterOption);
            StateFilterOption.Value = _currentStateFilterOption;
        }
    }
}
