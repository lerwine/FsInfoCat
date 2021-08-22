using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public static class EntityExtensions
    {
        public static ISimpleIdentityReference<TEntity> ToIdentityReference<TEntity>(this Guid? id) where TEntity : class, IDbEntity =>
            id.HasValue ? IdentityReference<TEntity>.FromId(id.Value) : null;

        public static async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            CollectionEntry<TEntity, TProperty> collectionEntry = entry.Collection(propertyExpression);
            if (!collectionEntry.IsLoaded)
                await collectionEntry.LoadAsync(cancellationToken);
            return collectionEntry.CurrentValue;
        }

        /// <summary>
        /// Indicates whether the entry exists in the target database.
        /// </summary>
        /// <param name="entry">The <see cref="EntityEntry"/> to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="entry"/> is not <see langword="null"/> and its <see cref="EntityEntry.State"/>
        /// is <see cref="EntityState.Unchanged"/> or <see cref="EntityState.Modified"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ExistsInDb(this EntityEntry entry)
        {
            if (entry is null)
                return false;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    return true;
                default:
                    return false;
            }
        }

        public static async Task<TProperty> GetRelatedReferenceAsync<TEntity, TProperty>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, TProperty>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReferenceEntry<TEntity, TProperty> referenceEntry = entry.Reference(propertyExpression);
            if (!referenceEntry.IsLoaded)
                await referenceEntry.LoadAsync(cancellationToken);
            return referenceEntry.CurrentValue;
        }

        public static async Task<EntityEntry<TProperty>> GetRelatedTargetEntryAsync<TEntity, TProperty>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, TProperty>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReferenceEntry<TEntity, TProperty> referenceEntry = entry.Reference(propertyExpression);
            if (!referenceEntry.IsLoaded)
                await referenceEntry.LoadAsync(cancellationToken);
            return referenceEntry.TargetEntry;
        }

        public static void RejectChanges<T>(this DbSet<T> dbSet, Func<T, EntityEntry<T>> getEntry) where T : class, IRevertibleChangeTracking
        {
            if (getEntry is null)
                throw new ArgumentNullException(nameof(getEntry));
            if (dbSet is null)
                return;
            T[] items = dbSet.Local.ToArray();
            foreach (T t in items)
            {
                EntityEntry<T> entry = getEntry(t);
                if (entry is null)
                    t.RejectChanges();
                else
                    RejectChanges(entry);
            }
        }

        public static void RejectChanges(this DbContext dbContext)
        {
            if (dbContext is null)
                return;
            EntityEntry[] entityEntries = dbContext.ChangeTracker.Entries().ToArray();
            foreach (EntityEntry entry in entityEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        dbContext.Remove(entry.Entity);
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IDbEntity dbEntity)
                            dbEntity.RejectChanges();
                        break;
                }
            }
        }

        public static void RejectChanges(this EntityEntry entry)
        {
            if (entry is null)
                return;
            switch (entry.State)
            {
                case EntityState.Added:
                    //if (entry.Entity is IRevertibleChangeTracking rct)
                    //    rct.RejectChanges();
                    entry.Context.Remove(entry.Entity);
                    break;
                case EntityState.Deleted:
                    if (entry.Entity is IRevertibleChangeTracking rct2)
                    {
                        rct2.RejectChanges();
                        entry.State = rct2.IsChanged ? EntityState.Modified : EntityState.Unchanged;
                    }
                    break;
                case EntityState.Unchanged:
                    break;
                default:
                    if (entry.Entity is IRevertibleChangeTracking rct3)
                        rct3.RejectChanges();
                    break;
            }
        }

    }
}
