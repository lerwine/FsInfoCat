using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public sealed class CompoundIdentityReference<TEntity> : IdentityReference<TEntity>, ICompoundIdentityReference<TEntity>
        where TEntity : class, IDbEntity
    {
        private readonly Func<TEntity, IEnumerable<Guid>> _getIds;

        public IEnumerable<Guid> Id => _getIds(Entity);

        public CompoundIdentityReference([DisallowNull] TEntity entity, [DisallowNull] Func<TEntity, IEnumerable<Guid>> getIds) : base(entity) { _getIds = getIds; }

        protected override IEnumerable<Guid> BaseIdentifiers() => _getIds(Entity);
    }
}
