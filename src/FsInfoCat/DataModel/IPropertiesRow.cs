using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="IHasSimpleIdentifier" />
    /// <seealso cref="Local.ILocalPropertiesRow" />
    /// <seealso cref="Upstream.IUpstreamPropertiesRow" />
    public interface IPropertiesRow : IDbEntity, IHasSimpleIdentifier { }
}
