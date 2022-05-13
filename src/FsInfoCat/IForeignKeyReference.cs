using System;

namespace FsInfoCat
{
    /// <summary>
    /// Represents a foreign key identifier and the optional associated nagivation property.
    /// </summary>
    /// <seealso cref="IHasSimpleIdentifier" />
    /// <seealso cref="ISynchronizable" />
    public interface IForeignKeyReference : IHasSimpleIdentifier, ISynchronizable
    {
        /// <summary>
        /// Gets the navigation entity object.
        /// </summary>
        /// <value>The navigation entity object or <see langword="null"/> if the entity has not been set.</value>
        IHasSimpleIdentifier Entity { get; }

        /// <summary>
        /// Determines whether the foreign key value or the primary key of the navigation property has been set.
        /// </summary>
        /// <returns><see langword="true"/> foreign key value has been set or the primary key of the navigation object has been set;
        /// otherwise, <see langword="false"/>.</returns>
        bool HasId();

        /// <summary>
        /// Sets the foreign key value.
        /// </summary>
        /// <param name="id">The foreign key value or <see langword="null"/> to unset the foreign key reference.</param>
        void SetId(Guid? id);
    }

    /// <summary>
    /// Represents a foreign key identifier and the optional associated nagivation property.
    /// </summary>
    /// <typeparam name="TEntity">The type of the navigation object.</typeparam>
    /// <seealso cref="IForeignKeyReference" />
    public interface IForeignKeyReference<TEntity> : IForeignKeyReference, IEquatable<IForeignKeyReference<TEntity>>
        where TEntity : class, IHasSimpleIdentifier, IEquatable<TEntity>
    {
        /// <summary>
        /// Gets the navigation entity object.
        /// </summary>
        /// <value>The navigation entity object or <see langword="null"/> if the entity has not been set.</value>
        new TEntity Entity { get; }
    }
}
