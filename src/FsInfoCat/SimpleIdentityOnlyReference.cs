using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public sealed class SimpleIdentityOnlyReference<TEntity> : IdentityOnlyReference<TEntity>, ISimpleIdentityReference<TEntity>
        where TEntity : class, IDbEntity
    {
        public Guid Id { get; }

        public SimpleIdentityOnlyReference(Guid id) { Id = id; }

        protected override IEnumerable<Guid> BaseIdentifiers()
        {
            yield return Id;
        }
    }
}
