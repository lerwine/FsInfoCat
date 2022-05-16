namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="Local.ILocalGPSPropertiesRow" />
    public interface IUpstreamGPSPropertiesRow : IUpstreamPropertiesRow, IGPSPropertiesRow { }
}
