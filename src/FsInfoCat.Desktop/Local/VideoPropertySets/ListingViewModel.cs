using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Local.VideoPropertySets
{
    public class ListingViewModel : ListingViewModel<VideoPropertiesListItem, ListItemViewModel, ListingViewModel.ListOptions>, INotifyNavigatedTo
    {
        private ListOptions _currentOptions;
        private ListOptions _editingOptions;

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<VideoPropertiesListItem> GetQueryableListing(ListOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] VideoPropertiesListItem entity) => new(entity);

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

        protected override async Task<int> DeleteEntityFromDbContextAsync([DisallowNull] VideoPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override void OnAddNewItemCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        public record ListOptions(bool? ShowOnlyWithExistingFiles);
    }
}
