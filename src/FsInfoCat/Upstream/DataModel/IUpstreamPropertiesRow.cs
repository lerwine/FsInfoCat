namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="Local.ILocalPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamPropertiesRow")]
    public interface IUpstreamPropertiesRow : IUpstreamDbEntity, IPropertiesRow { }
}
