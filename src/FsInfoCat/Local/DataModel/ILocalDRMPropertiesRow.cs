namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IDRMPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamDRMPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalDRMPropertiesRow")]
    public interface ILocalDRMPropertiesRow : ILocalPropertiesRow, IDRMPropertiesRow { }
}
