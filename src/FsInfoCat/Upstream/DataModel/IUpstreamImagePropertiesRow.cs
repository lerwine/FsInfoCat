namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="Local.ILocalImagePropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamImagePropertiesRow")]
    public interface IUpstreamImagePropertiesRow : IUpstreamPropertiesRow, IImagePropertiesRow { }
}
