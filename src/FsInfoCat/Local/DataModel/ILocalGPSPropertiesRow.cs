namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="IGPSPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamGPSPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalGPSPropertiesRow")]
    public interface ILocalGPSPropertiesRow : ILocalPropertiesRow, IGPSPropertiesRow { }
}
