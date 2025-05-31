using FsInfoCat.Model;
using System;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
/// </summary>
/// <seealso cref="Upstream.Model.IUpstreamSharedFileTag" />
public interface ILocalSharedFileTag : ILocalSharedTag, ISharedFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalSharedTagDefinition>,
    IEquatable<ILocalSharedFileTag> { }
