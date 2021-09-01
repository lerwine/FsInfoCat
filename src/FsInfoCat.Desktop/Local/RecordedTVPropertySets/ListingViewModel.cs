using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Local.RecordedTVPropertySets
{
    public class ListingViewModel : ListingViewModel<RecordedTVPropertiesListItem, ListItemViewModel, ListingViewModel.ListOptions>, INotifyNavigatedTo
    {
        private ListOptions _currentOptions;
        private ListOptions _editingOptions;

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<RecordedTVPropertiesListItem> GetQueryableListing(ListOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] RecordedTVPropertiesListItem entity) => new(entity);

        protected override void OnSaveFilterOptionsCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnRefreshCommand(object parameter) => ReloadAsync(_currentOptions);

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ConfirmItemDelete([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] RecordedTVPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        public record ListOptions(string Name);
    }
}
