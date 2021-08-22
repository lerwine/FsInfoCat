using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public abstract class IdentityReference<TEntity> : IIdentityReference<TEntity>
        where TEntity : class, IDbEntity
    {
        public TEntity Entity { get; }

        IDbEntity IIdentityReference.Entity => Entity;

        protected IdentityReference([DisallowNull] TEntity entity) { Entity = entity ?? throw new ArgumentNullException(nameof(entity)); }

        protected abstract IEnumerable<Guid> BaseIdentifiers();

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() => BaseIdentifiers();

        public static ISimpleIdentityReference<TEntity> FromId(Guid id) => new SimpleIdentityOnlyReference<TEntity>(id);

        public static IIdentityPairReference<TEntity> FromId(Guid id1, Guid id2) => new IdentifierPairOnlyReference<TEntity>(id1, id2);

        public static ISimpleIdentityReference<TEntity> CreateSimple<TSource>([AllowNull] TSource source, [DisallowNull] Func<TSource, TEntity> getRelatedEntity,
            [DisallowNull] Func<TSource, Guid?> getReltedId, [DisallowNull] Func<TEntity, Guid> getId)
            where TSource : class
        {
            if (getRelatedEntity is null)
                throw new ArgumentNullException(nameof(getRelatedEntity));
            if (getReltedId is null)
                throw new ArgumentNullException(nameof(getReltedId));
            if (getId is null)
                throw new ArgumentNullException(nameof(getId));
            if (source is null)
                return null;

            TEntity entity = getRelatedEntity(source);
            if (entity is null)
            {
                Guid? id = getReltedId(source);
                if (id.HasValue)
                    return new SimpleIdentityOnlyReference<TEntity>(id.Value);
                return null;
            }
            return new SimpleIdentityReference<TEntity>(entity, getId);
        }

        public static IIdentityReference<TEntity> CreateCompound<TSource>([AllowNull] TSource source, [DisallowNull] Func<TSource, TEntity> getRelatedEntity,
            [DisallowNull] Func<TSource, Guid[]> getReltedIds, [DisallowNull] Func<TEntity, IEnumerable<Guid>> getIds)
            where TSource : class
        {
            if (source is null)
                return null;
            TEntity entity = getRelatedEntity(source);
            if (entity is null)
            {
                Guid[] ids = getReltedIds(source);
                if ((ids?.Length ?? 0) > 0)
                    return new CompoundIdentityOnlyReference<TEntity>(ids);
                return null;
            }
            return new CompoundIdentityReference<TEntity>(entity, getIds);
        }
    }
}
