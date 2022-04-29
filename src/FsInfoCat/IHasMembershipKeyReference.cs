namespace FsInfoCat
{
    public interface IHasMembershipKeyReference : ISynchronizable, IHasIdentifierPair
    {
        IForeignKeyReference Ref1 { get; }

        IForeignKeyReference Ref2 { get; }
    }

    public interface IHasMembershipKeyReference<TEntity1, TEntity2> : IHasMembershipKeyReference
        where TEntity1 : IHasSimpleIdentifier
        where TEntity2 : IHasSimpleIdentifier
    {
        new IForeignKeyReference<TEntity1> Ref1 { get; }

        new IForeignKeyReference<TEntity2> Ref2 { get; }
    }
}
