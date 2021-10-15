namespace FsInfoCat.Background
{
    public interface IDeleteBranchBackgroundService : ILongRunningAsyncService<bool>
    {
        ISubdirectory Target { get; }

        bool ForceDeleteFromDb { get; set; }
    }

    public interface IDeleteBranchBackgroundService<TSubdirectory> : IDeleteBranchBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
        new TSubdirectory Target { get; set; }
    }
}
