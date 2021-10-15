namespace FsInfoCat.Background
{
    public interface IDeleteVolumeBackgroundService : ILongRunningAsyncService<bool>
    {
        IVolume Target { get; }

        bool ForceDeleteFromDb { get; set; }
    }

    public interface IDeleteVolumeBackgroundService<TVolume> : IDeleteVolumeBackgroundService
        where TVolume : DbEntity, IVolume
    {
        new TVolume Target { get; set;  }
    }
}
