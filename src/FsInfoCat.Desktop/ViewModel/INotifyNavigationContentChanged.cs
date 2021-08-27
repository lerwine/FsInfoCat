namespace FsInfoCat.Desktop.ViewModel
{
    [System.Obsolete("Use INotifyNavigatedTo or INotifyNavigatedFrom")]
    interface INotifyNavigationContentChanged
    {
        void OnNavigatedTo(MainVM mainVM);
        void OnNavigatedFrom(MainVM mainVM);
    }
}
