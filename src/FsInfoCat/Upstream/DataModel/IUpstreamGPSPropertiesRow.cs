namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="Local.ILocalGPSPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Upstream.Model.IUpstreamGPSPropertiesRow")]
    public interface IUpstreamGPSPropertiesRow : IUpstreamPropertiesRow, IGPSPropertiesRow { }
}
