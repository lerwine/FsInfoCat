namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="Local.ILocalDRMPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamDRMPropertiesRow")]
    public interface IUpstreamDRMPropertiesRow : IUpstreamPropertiesRow, IDRMPropertiesRow { }
}
