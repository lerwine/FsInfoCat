using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file GPS information properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="IEquatable{IGPSPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalGPSPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamGPSPropertiesRow" />
    public interface IGPSPropertiesRow : IPropertiesRow, IGPSProperties, IEquatable<IGPSPropertiesRow> { }
}
