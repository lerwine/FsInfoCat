using System;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Generic interface for an <see cref="ILocalSharedTagDefinition"/> that is associated with an <see cref="ILocalFile"/>.
    /// </summary>
    /// <seealso cref="ILocalSharedTag" />
    /// <seealso cref="ISharedFileTag" />
    /// <seealso cref="ILocalFileTag" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalFile, ILocalSharedTagDefinition}" />
    /// <seealso cref="IEquatable{ILocalSharedFileTag}" />
    /// <seealso cref="Upstream.IUpstreamSharedFileTag" />
    [System.Obsolete("Use FsInfoCat.Local.Model.ILocalSharedFileTag")]
    public interface ILocalSharedFileTag : ILocalSharedTag, ISharedFileTag, ILocalFileTag, IHasMembershipKeyReference<ILocalFile, ILocalSharedTagDefinition>,
        IEquatable<ILocalSharedFileTag> { }
}
