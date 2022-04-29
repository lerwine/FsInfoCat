namespace FsInfoCat
{
    [System.Obsolete("Use IHasMembershipKeyReference, isntead")]
    public interface ICompoundIdentityReference : IIdentityReference, IHasCompoundIdentifier
    {
    }

    [System.Obsolete("Use IHasMembershipKeyReference, isntead")]
    public interface ICompoundIdentityReference<TEntity> : IIdentityReference<TEntity>, ICompoundIdentityReference
        where TEntity : class, IDbEntity
    {
    }
}
