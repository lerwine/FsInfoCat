namespace FsInfoCat.Desktop.ViewModel
{
    internal interface INotifyNavigationContentChanged
    {
        void OnNavigatedTo(MainVM mainVM);
        void OnNavigatedFrom(MainVM mainVM);
    }
}
