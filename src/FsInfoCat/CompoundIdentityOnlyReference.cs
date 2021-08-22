using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FsInfoCat
{
    public sealed class CompoundIdentityOnlyReference<TEntity> : IdentityOnlyReference<TEntity>, ICompoundIdentityReference<TEntity>
        where TEntity : class, IDbEntity
    {
        public ReadOnlyCollection<Guid> Id { get; }

        IEnumerable<Guid> IHasCompoundIdentifier.Id => Id;

        public CompoundIdentityOnlyReference(params Guid[] id) { Id = new(new Collection<Guid>(id)); }

        protected override IEnumerable<Guid> BaseIdentifiers() => Id;
    }
}
