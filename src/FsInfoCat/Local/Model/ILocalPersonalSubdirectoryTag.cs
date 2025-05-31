using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for an <see cref="ILocalPersonalTagDefinition"/> that is associated with an <see cref="ILocalSubdirectory"/>.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamPersonalSubdirectoryTag" />
public interface ILocalPersonalSubdirectoryTag : ILocalPersonalTag, IPersonalSubdirectoryTag, ILocalSubdirectoryTag,
    IHasMembershipKeyReference<ILocalSubdirectory, ILocalPersonalTagDefinition>, IEquatable<ILocalPersonalSubdirectoryTag> { }
