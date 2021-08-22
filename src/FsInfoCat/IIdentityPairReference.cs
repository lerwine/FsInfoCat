namespace FsInfoCat
{
    public interface IIdentityPairReference : ICompoundIdentityReference, IHasIdentifierPair
    {
    }

    public interface IIdentityPairReference<TEntity> : ICompoundIdentityReference<TEntity>, IIdentityPairReference
        where TEntity : class, IDbEntity
    {
    }
}
