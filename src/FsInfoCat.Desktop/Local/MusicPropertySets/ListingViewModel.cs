using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.Local.MusicPropertySets
{
    public class ListingViewModel : ListingViewModel<MusicPropertiesListItem, ListItemViewModel, ListingViewModel.ListOptions>, INotifyNavigatedTo
    {
        private ListOptions _currentOptions;
        private ListOptions _editingOptions;

        void INotifyNavigatedTo.OnNavigatedTo() => ReloadAsync(_currentOptions);

        protected override IQueryable<MusicPropertiesListItem> GetQueryableListing(ListOptions options, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
        {
            throw new NotImplementedException();
        }

        protected override ListItemViewModel CreateItemViewModel([DisallowNull] MusicPropertiesListItem entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveFilterOptionsCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnRefreshCommand(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void OnItemEditCommand([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override bool ConfirmItemDelete([DisallowNull] ListItemViewModel item, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Task<int> DeleteEntityFromDbContextAsync([DisallowNull] MusicPropertiesListItem entity, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IWindowsStatusListener statusListener)
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
