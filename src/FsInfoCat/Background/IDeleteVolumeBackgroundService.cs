namespace FsInfoCat.Background
{
    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IDeleteVolumeBackgroundService : ILongRunningAsyncService<bool>
    {
        IVolume Target { get; }

        bool ForceDeleteFromDb { get; set; }
    }

    [System.Obsolete("Use FsInfoCat.Services.IBackgroundProgressService and/or FsInfoCat.AsyncOps classes")]
    public interface IDeleteVolumeBackgroundService<TVolume> : IDeleteVolumeBackgroundService
        where TVolume : DbEntity, IVolume
    {
        new TVolume Target { get; set;  }
    }
}
