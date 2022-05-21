using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Generic interface for entities containing extended file DRM information properties.
    /// </summary>
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="IEquatable{IDRMPropertiesRow}" />
    /// <seealso cref="Local.Model.ILocalDRMPropertiesRow" />
    /// <seealso cref="Upstream.Model.IUpstreamDRMPropertiesRow" />
    public interface IDRMPropertiesRow : IPropertiesRow, IDRMProperties, IEquatable<IDRMPropertiesRow> { }
}
