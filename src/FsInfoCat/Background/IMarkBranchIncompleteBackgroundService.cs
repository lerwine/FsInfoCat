namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IMarkBranchIncompleteBackgroundService : ILongRunningAsyncService<bool>
    {
        ISubdirectory Target { get; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IMarkBranchIncompleteBackgroundService<TSubdirectory> : IMarkBranchIncompleteBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
        new TSubdirectory Target { get; set; }
    }
}
