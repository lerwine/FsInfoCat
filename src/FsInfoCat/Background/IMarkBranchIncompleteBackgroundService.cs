namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IMarkBranchIncompleteBackgroundService : ILongRunningAsyncService<bool>
    {
        ISubdirectory Target { get; }
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IMarkBranchIncompleteBackgroundService<TSubdirectory> : IMarkBranchIncompleteBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
        new TSubdirectory Target { get; set; }
    }
}
