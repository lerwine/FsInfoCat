using System;

namespace FsInfoCat
{
    /// <summary>
    /// Interface for entities that have a compound primary key comprised of two <see cref="Guid"/> values.
    /// </summary>
    /// <seealso cref="IHasCompoundIdentifier" />
    public interface IHasIdentifierPair : IHasCompoundIdentifier
    {
        /// <summary>
        /// Gets the compound primary key values.
        /// </summary>
        /// <value>The compound primary key values.</value>
        new ValueTuple<Guid, Guid> Id { get; }
    }
}
