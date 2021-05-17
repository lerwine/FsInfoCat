namespace FsInfoCat.Model.Remote
{
    public interface IRemoteFileComparison : IFileComparison, IRemoteTimeStampedEntity
    {
        new IRemoteFile File1 { get; }
        new IRemoteFile File2 { get; }
    }
}
