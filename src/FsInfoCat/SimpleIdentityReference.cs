using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public sealed class SimpleIdentityReference<TEntity> : IdentityReference<TEntity>, ISimpleIdentityReference<TEntity>
        where TEntity : class, IDbEntity
    {
        private readonly Func<TEntity, Guid> _getId;

        public Guid Id => _getId(Entity);

        public SimpleIdentityReference([DisallowNull] TEntity entity, [DisallowNull] Func<TEntity, Guid> getId) : base(entity) { _getId = getId; }

        protected override IEnumerable<Guid> BaseIdentifiers()
        {
            yield return Id;
        }
    }
}
