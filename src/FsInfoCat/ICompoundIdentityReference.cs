namespace FsInfoCat
{
    public interface ICompoundIdentityReference : IIdentityReference, IHasCompoundIdentifier
    {
    }

    public interface ICompoundIdentityReference<TEntity> : IIdentityReference<TEntity>, ICompoundIdentityReference
        where TEntity : class, IDbEntity
    {
    }
}
