namespace FsInfoCat.Desktop
{
    public interface IHasAsyncWindowsBackgroundOperationManager : IHasAsyncBackgroundOperationManager
    {
        new IAsyncWindowsBackgroundOperationManager GetAsyncBackgroundOperationManager();
    }
}
