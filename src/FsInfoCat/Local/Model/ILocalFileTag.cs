using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for an <see cref="ILocalTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamFileTag" />
public interface ILocalFileTag : ILocalItemTag, IFileTag, IHasMembershipKeyReference<ILocalFile, ILocalTagDefinition>, IEquatable<ILocalFileTag>
{
    /// <summary>
    /// Gets the tagged file.
    /// </summary>
    /// <value>The tagged <see cref="ILocalFile"/>.</value>
    new ILocalFile Tagged { get; }
}
