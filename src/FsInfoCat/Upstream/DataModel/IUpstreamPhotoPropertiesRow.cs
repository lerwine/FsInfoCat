namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    public interface IUpstreamPhotoPropertiesRow : IUpstreamPropertiesRow, IPhotoPropertiesRow { }
}
