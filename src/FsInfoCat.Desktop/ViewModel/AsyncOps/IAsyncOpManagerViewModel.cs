namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    internal interface IAsyncOpManagerViewModel
    {
        void CancelAll(bool throwOnFirstException);
        void CancelAll();
    }
}
