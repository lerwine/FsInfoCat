using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that have a compound primary key value comprised of more than one <see cref="Guid"/> value.
    /// </summary>
    public interface IHasCompoundIdentifier
    {
        /// <summary>
        /// Gets the values of the compound identifier.
        /// </summary>
        /// <value>The compound identifier values.</value>
        [NotNull]
        IEnumerable<Guid> Id { get; }
    }
}
