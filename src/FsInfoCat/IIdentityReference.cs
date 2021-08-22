using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public interface IIdentityReference
    {
        IEnumerable<Guid> GetIdentifiers();

        [MaybeNull]
        IDbEntity Entity { get; }
    }

    public interface IIdentityReference<TEntity> : IIdentityReference
        where TEntity : class, IDbEntity
    {
        [MaybeNull]
        new TEntity Entity { get; }
    }
}
