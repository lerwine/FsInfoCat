namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IPropertiesRow" />
    public interface IUpstreamPropertiesRow : IUpstreamDbEntity, IPropertiesRow { }
}
