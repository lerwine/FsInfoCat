using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Interface ILocalSubdirectoryTag
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamSubdirectoryTag" />
public interface ILocalSubdirectoryTag : ILocalItemTag, ISubdirectoryTag, IHasMembershipKeyReference<ILocalSubdirectory, ILocalTagDefinition>, IEquatable<ILocalSubdirectoryTag>
{
    /// <summary>
    /// Gets the tagged subdirectory.
    /// </summary>
    /// <value>The tagged <see cref="ILocalSubdirectory"/>.</value>
    new ILocalSubdirectory Tagged { get; }
}
