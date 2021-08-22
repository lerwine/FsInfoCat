using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public sealed class IdentifierPairOnlyReference<TEntity> : IdentityOnlyReference<TEntity>, IIdentityPairReference<TEntity>
        where TEntity : class, IDbEntity
    {
        public ValueTuple<Guid, Guid> Id { get; }

        IEnumerable<Guid> IHasCompoundIdentifier.Id => BaseIdentifiers();

        public IdentifierPairOnlyReference(Guid id1, Guid id2) { Id = new(id1, id2); }

        public IdentifierPairOnlyReference(ValueTuple<Guid, Guid> id) { Id = id; }

        protected override IEnumerable<Guid> BaseIdentifiers()
        {
            yield return Id.Item1;
            yield return Id.Item2;
        }
    }
}
