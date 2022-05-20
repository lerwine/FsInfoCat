namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="Local.ILocalPhotoPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPhotoPropertiesRow")]
    public interface IUpstreamPhotoPropertiesRow : IUpstreamPropertiesRow, IPhotoPropertiesRow { }
}
