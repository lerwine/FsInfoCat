namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for photo files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IPhotoPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamPhotoPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalPhotoPropertiesRow")]
    public interface ILocalPhotoPropertiesRow : ILocalPropertiesRow, IPhotoPropertiesRow { }
}
