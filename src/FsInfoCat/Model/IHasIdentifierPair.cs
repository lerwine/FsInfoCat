using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that have a compound primary key comprised of two <see cref="Guid"/> values.
    /// </summary>
    /// <seealso cref="IHasMembershipKeyReference" />
    /// <seealso cref="IItemTagRow" />
    public interface IHasIdentifierPair : IHasCompoundIdentifier
    {
        /// <summary>
        /// Gets the compound primary key values.
        /// </summary>
        /// <value>The compound primary key values.</value>
        new ValueTuple<Guid, Guid> Id { get; }
    }
}
