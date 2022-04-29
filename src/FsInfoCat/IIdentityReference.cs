using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    /// <summary>
    /// Represents an object which referest to a database entity.
    /// </summary>
    [Obsolete("Use IForeignKeyReference, instead")]
    public interface IIdentityReference
    {
        /// <summary>
        /// Gets the values of the entity's primary key.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Guid}"/> that iterates through an entity's primary key.</returns>
        IEnumerable<Guid> GetIdentifiers();

        /// <summary>
        /// Gets the referenced entity object if, if present.
        /// </summary>
        /// <value>The referenced entity object or <see langword="null"/> if the current object only refers to the entity through the primary key values.</value>
        [MaybeNull]
        IDbEntity Entity { get; }
    }

    /// <summary>
    /// Represents an object that refers to a database entity of a specific type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the database entity.</typeparam>
    /// <seealso cref="IIdentityReference" />
    [Obsolete("Use IForeignKeyReference<TEntity>, instead")]
    public interface IIdentityReference<TEntity> : IIdentityReference
        where TEntity : class, IDbEntity
    {
        /// <summary>
        /// Gets the referenced entity object if, if present.
        /// </summary>
        /// <value>The referenced entity object or <see langword="null"/> if the current object only refers to the entity through the primary key values.</value>
        [MaybeNull]
        new TEntity Entity { get; }
    }
}
