using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="IEquatable{IDRMPropertiesRow}" />
    /// <seealso cref="Local.ILocalDRMPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamDRMPropertiesRow" />
    [System.Obsolete("Use FsInfoCat.Model.IDRMPropertiesRow")]
    public interface IDRMPropertiesRow : IPropertiesRow, IDRMProperties, IEquatable<IDRMPropertiesRow> { }
}
