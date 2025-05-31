using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalSubdirectory"/>.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamSharedSubdirectoryTag" />
public interface ILocalSharedSubdirectoryTag : ILocalSharedTag, ISharedSubdirectoryTag, ILocalSubdirectoryTag,
    IHasMembershipKeyReference<ILocalSubdirectory, ILocalSharedTagDefinition>, IEquatable<ILocalSharedSubdirectoryTag> { }
