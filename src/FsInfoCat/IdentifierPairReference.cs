using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public sealed class IdentifierPairReference<TEntity> : IdentityReference<TEntity>, IIdentityPairReference<TEntity>
        where TEntity : class, IDbEntity
    {
        private readonly Func<TEntity, ValueTuple<Guid, Guid>> _getId;

        public ValueTuple<Guid, Guid> Id => _getId(Entity);

        IEnumerable<Guid> IHasCompoundIdentifier.Id => BaseIdentifiers();

        public IdentifierPairReference([DisallowNull] TEntity entity, [DisallowNull] Func<TEntity, ValueTuple<Guid, Guid>> getId) : base(entity) { _getId = getId; }

        protected override IEnumerable<Guid> BaseIdentifiers()
        {
            ValueTuple<Guid, Guid> id = _getId(Entity);
            yield return id.Item1;
            yield return id.Item2;
        }
    }
}
