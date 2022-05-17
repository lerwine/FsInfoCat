namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertiesRow" />
    public interface ILocalGPSPropertiesRow : ILocalPropertiesRow, IGPSPropertiesRow { }
}
