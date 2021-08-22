using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public abstract class IdentityOnlyReference<TEntity> : IIdentityReference<TEntity>
        where TEntity : class, IDbEntity
    {
        public TEntity Entity => null;

        IDbEntity IIdentityReference.Entity => Entity;

        protected abstract IEnumerable<Guid> BaseIdentifiers();

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() => BaseIdentifiers();
    }
}
