namespace FsInfoCat.Background
{
    public interface IMarkBranchIncompleteBackgroundService : ILongRunningAsyncService<bool>
    {
        ISubdirectory Target { get; }
    }

    public interface IMarkBranchIncompleteBackgroundService<TSubdirectory> : IMarkBranchIncompleteBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
        new TSubdirectory Target { get; set; }
    }
}
