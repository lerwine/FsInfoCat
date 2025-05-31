using FsInfoCat.Model;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for entities containing extended file properties.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamPropertiesRow" />
public interface ILocalPropertiesRow : ILocalDbEntity, IPropertiesRow { }
