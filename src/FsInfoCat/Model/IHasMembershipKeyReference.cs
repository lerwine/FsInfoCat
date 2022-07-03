using System;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Interface for entities that has a compound identifier comprised of 2 foreign key references.
    /// </summary>
    /// <seealso cref="ISynchronizable" />
    /// <seealso cref="IHasIdentifierPair" />
    public interface IHasMembershipKeyReference : ISynchronizable, IHasIdentifierPair
    {
        /// <summary>
        /// Gets the first foreign key reference.
        /// </summary>
        /// <value>The foreign key reference whose key is the first value of the current entity's compound primary key.</value>
        IForeignKeyReference Ref1 { get; }

        /// <summary>
        /// Gets the second foreign key reference.
        /// </summary>
        /// <value>The foreign key reference whose key is the second value of the current entity's compound primary key.</value>
        IForeignKeyReference Ref2 { get; }
    }

    /// <summary>
    /// Interface for entities that has a compound identifier comprised of 2 foreign key references.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the first entity whose primary key is also the first value of the current entity's compound primary key.</typeparam>
    /// <typeparam name="TEntity2">The type of the second entity whose primary key is also the second value of the current entity's compound primary key.</typeparam>
    /// <seealso cref="IHasMembershipKeyReference" />
    public interface IHasMembershipKeyReference<TEntity1, TEntity2> : IHasMembershipKeyReference
            where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
            where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        /// <summary>
        /// Gets the first foreign key reference.
        /// </summary>
        /// <value>The foreign key reference whose key is the first value of the current entity's compound primary key.</value>
        new IForeignKeyReference<TEntity1> Ref1 { get; }

        /// <summary>
        /// Gets the second foreign key reference.
        /// </summary>
        /// <value>The foreign key reference whose key is the second value of the current entity's compound primary key.</value>
        new IForeignKeyReference<TEntity2> Ref2 { get; }
    }
}
