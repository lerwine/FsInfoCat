namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="Local.ILocalDRMPropertiesRow" />
    public interface IUpstreamDRMPropertiesRow : IUpstreamPropertiesRow, IDRMPropertiesRow { }
}
