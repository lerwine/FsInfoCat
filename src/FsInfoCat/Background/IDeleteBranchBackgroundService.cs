namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IDeleteBranchBackgroundService : ILongRunningAsyncService<bool>
    {
        ISubdirectory Target { get; }

        bool ForceDeleteFromDb { get; set; }
    }

    [System.Obsolete("Use FsInfoCat.Activities.*, instead.")]
    public interface IDeleteBranchBackgroundService<TSubdirectory> : IDeleteBranchBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
        new TSubdirectory Target { get; set; }
    }
}
