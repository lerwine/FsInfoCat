using System;

namespace FsInfoCat
{
    public interface IHasMembershipKeyReference : ISynchronizable
    {
        IForeignKeyReference Ref1 { get; }

        IForeignKeyReference Ref2 { get; }
    }

    public interface IHasMembershipKeyReference<TEntity1, TEntity2> : IHasMembershipKeyReference
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        new IForeignKeyReference<TEntity1> Ref1 { get; }

        new IForeignKeyReference<TEntity2> Ref2 { get; }
    }
}
