namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IDeleteBranchBackgroundService : ILongRunningAsyncService<bool>
    {
        ISubdirectory Target { get; }

        bool ForceDeleteFromDb { get; set; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IDeleteBranchBackgroundService<TSubdirectory> : IDeleteBranchBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
        new TSubdirectory Target { get; set; }
    }
}
