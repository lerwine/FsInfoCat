namespace FsInfoCat
{
    [System.Obsolete("Use IForeignKeyReference, instead")]
    public interface ISimpleIdentityReference : IIdentityReference, IHasSimpleIdentifier
    {
    }
    [System.Obsolete("Use IForeignKeyReference, instead")]
    public interface ISimpleIdentityReference<TEntity> : IIdentityReference<TEntity>, ISimpleIdentityReference
        where TEntity : class, IDbEntity
    {
    }
}
