namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties for image files.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IImagePropertiesRow" />
    public interface IUpstreamImagePropertiesRow : IUpstreamPropertiesRow, IImagePropertiesRow { }
}
