namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamImagePropertiesRow" />
    public interface ILocalImagePropertiesRow : ILocalPropertiesRow, IImagePropertiesRow { }
}
