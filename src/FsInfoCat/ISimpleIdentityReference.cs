namespace FsInfoCat
{
    public interface ISimpleIdentityReference : IIdentityReference, IHasSimpleIdentifier
    {
    }
    public interface ISimpleIdentityReference<TEntity> : IIdentityReference<TEntity>, ISimpleIdentityReference
        where TEntity : class, IDbEntity
    {
    }
}
