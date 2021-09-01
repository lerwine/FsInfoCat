using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Local.ImagePropertySets
{
    public class ListingViewModel : ListingViewModel<ImagePropertiesListItem, ListItemViewModel, ListingViewModel.ListOptions>, INotifyNavigatedTo
    {
        private ListOptions _currentOptions;
        private ListOptions _editingOptions;

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<ImagePropertiesListItem> GetQueryableListing(ListOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] ImagePropertiesListItem entity) => new(entity);

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

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] ImagePropertiesListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
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
