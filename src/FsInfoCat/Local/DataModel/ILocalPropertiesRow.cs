namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamPropertiesRow" />
    public interface ILocalPropertiesRow : ILocalDbEntity, IPropertiesRow { }
}
