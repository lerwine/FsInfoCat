namespace FsInfoCat
{
    [System.Obsolete("Use IHasMembershipKeyReference, isntead")]
    public interface IIdentityPairReference : ICompoundIdentityReference, IHasIdentifierPair
    {
    }

    [System.Obsolete("Use IHasMembershipKeyReference, isntead")]
    public interface IIdentityPairReference<TEntity> : ICompoundIdentityReference<TEntity>, IIdentityPairReference
        where TEntity : class, IDbEntity
    {
    }
}
