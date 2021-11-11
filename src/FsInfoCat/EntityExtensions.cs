using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public static class EntityExtensions
    {
        public static readonly StringComparer FileNameComparer = StringComparer.InvariantCultureIgnoreCase;

        public static string ToVersionString(this Collections.ByteValues value)
        {
            if (value is null || value.Count == 0)
                return "";
            return string.Join('.', value.Select(b => b.ToString()));
        }

        public static async Task<bool> RemoveRelatedEntitiesAsync<TEntity, TProperty>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, [DisallowNull] DbSet<TProperty> dbSet, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            TProperty[] related = (await entry.GetRelatedCollectionAsync(propertyExpression, cancellationToken)).ToArray();
            if (related.Length > 0)
            {
                dbSet.RemoveRange(related);
                return true;
            }
            return false;
        }

        public static string AncestorNamesToPath(string ancestorNames)
        {
            if (string.IsNullOrWhiteSpace(ancestorNames))
                return "";
            string[] segments = ancestorNames.Split('/').Where(s => s.Length > 0).ToArray();
            if (segments.Length < 2)
                return ancestorNames;
            return System.IO.Path.Combine(segments.Reverse().ToArray());
        }

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

        public static async Task<TProperty[]> QueryRelatedCollectionAsync<TEntity, TProperty>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, [DisallowNull] Expression<Func<TProperty, bool>> predicate, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            CollectionEntry<TEntity, TProperty> collectionEntry = entry.Collection(propertyExpression);
            return await collectionEntry.Query().Where(predicate).ToArrayAsync(cancellationToken);
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
            return entry.State switch
            {
                EntityState.Unchanged or EntityState.Modified => true,
                _ => false,
            };
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
                        _ = dbContext.Remove(entry.Entity);
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
                    _ = entry.Context.Remove(entry.Entity);
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

        public static List<(TSubdirectory Subdirectory, DirectoryInfo DirectoryInfo)> ToMatchedPairs<TSubdirectory>(this IEnumerable<TSubdirectory> subdirectories, IEnumerable<DirectoryInfo> directoryInfos)
            where TSubdirectory : class, ISubdirectory
        {
            if (subdirectories is null || !(subdirectories = subdirectories.Where(s => s is not null)).Distinct().Any())
            {
                if (directoryInfos is null || !(directoryInfos = directoryInfos.Where(d => d is not null)).Distinct().Any())
                    return new();
                return directoryInfos.Select(d => ((TSubdirectory)null, d)).ToList();
            }
            if (directoryInfos is null || !(directoryInfos = directoryInfos.Where(d => d is not null)).Distinct().Any())
                return subdirectories.Select(s => (s, (DirectoryInfo)null)).ToList();

            // BUG: Case-sensitive comparer is not being used.
            StringComparer csComparer = StringComparer.InvariantCulture;
            StringComparer ciComparer = StringComparer.InvariantCultureIgnoreCase;

            List<TSubdirectory> unmatchedSubdirectories = new();
            if (directoryInfos is not List<DirectoryInfo> unmatchedDirectoryInfos)
                unmatchedDirectoryInfos = directoryInfos.ToList();
            List<(TSubdirectory Subdirectory, DirectoryInfo DirectoryInfo)> result = subdirectories.Select(Subdirectory =>
            {
                string n = Subdirectory.Name;
                for (int i = 0; i < unmatchedDirectoryInfos.Count; i++)
                {
                    DirectoryInfo d = unmatchedDirectoryInfos[i];
                    if (ciComparer.Equals(n, d.Name))
                    {
                        unmatchedDirectoryInfos.RemoveAt(i);
                        return (Subdirectory, DirectoryInfo: d);
                    }
                }
                return (Subdirectory, DirectoryInfo: (DirectoryInfo)null);
            }).Where(t =>
                {
                    if (t.DirectoryInfo is null)
                    {
                        unmatchedSubdirectories.Add(t.Subdirectory);
                        return false;
                    }
                    return true;
                }).ToList();
            if (unmatchedSubdirectories.Count == 0)
            {
                foreach (DirectoryInfo di in unmatchedDirectoryInfos)
                    result.Add(new(null, di));
            }
            else
            {
                if (unmatchedDirectoryInfos.Count > 0)
                {
                    Dictionary<string, DirectoryInfo[]> directoryInfosByCiName = unmatchedDirectoryInfos.GroupBy(d => d.Name, ciComparer).ToDictionary(k => k.Key, v => v.ToArray(), ciComparer);
                    foreach (IGrouping<string, TSubdirectory> subdirectoryByCiName in unmatchedSubdirectories.GroupBy(s => s.Name, ciComparer))
                    {
                        string n = subdirectoryByCiName.Key;
                        if (!subdirectoryByCiName.Skip(1).Any() && directoryInfosByCiName.TryGetValue(n, out DirectoryInfo[] di) && di.Length == 1)
                        {
                            DirectoryInfo d = di[0];
                            unmatchedDirectoryInfos.Remove(d);
                            result.Add((subdirectoryByCiName.First(), d));
                        }
                        else
                            result.AddRange(subdirectoryByCiName.Select(s => (s, (DirectoryInfo)null)));
                    }
                }
                foreach (TSubdirectory s in unmatchedSubdirectories)
                    result.Add(new(s, null));
            }
            return result;
        }

        public static List<(TDbFile DbFile, FileInfo FileInfo)> ToMatchedPairs<TDbFile>(this IEnumerable<TDbFile> dbFiles, IEnumerable<FileInfo> fileInfos)
            where TDbFile : class, IFile
        {
            if (dbFiles is null || !(dbFiles = dbFiles.Where(f => f is not null)).Distinct().Any())
            {
                if (fileInfos is null || !(fileInfos = fileInfos.Where(f => f is not null)).Distinct().Any())
                    return new();
                return fileInfos.Select(f => ((TDbFile)null, f)).ToList();
            }
            if (fileInfos is null || !(fileInfos = fileInfos.Where(f => f is not null)).Distinct().Any())
                return dbFiles.Select(f => (f, (FileInfo)null)).ToList();

            // BUG: Case-sensitive comparer is not being used.
            StringComparer csComparer = StringComparer.InvariantCulture;
            StringComparer ciComparer = StringComparer.InvariantCultureIgnoreCase;

            List<TDbFile> unmatchedDbFiles = new();
            if (fileInfos is not List<FileInfo> unmatchedFileInfos)
                unmatchedFileInfos = fileInfos.ToList();
            List<(TDbFile DbFile, FileInfo FileInfo)> result = dbFiles.Select(DbFile =>
            {
                string n = DbFile.Name;
                for (int i = 0; i < unmatchedFileInfos.Count; i++)
                {
                    FileInfo f = unmatchedFileInfos[i];
                    if (ciComparer.Equals(n, f.Name))
                    {
                        unmatchedFileInfos.RemoveAt(i);
                        return (DbFile, FileInfo: f);
                    }
                }
                return (DbFile, FileInfo: (FileInfo)null);
            }).Where(t =>
            {
                if (t.FileInfo is null)
                {
                    unmatchedDbFiles.Add(t.DbFile);
                    return false;
                }
                return true;
            }).ToList();
            if (unmatchedDbFiles.Count == 0)
            {
                foreach (FileInfo fi in unmatchedFileInfos)
                    result.Add(new(null, fi));
            }
            else
            {
                if (unmatchedFileInfos.Count > 0)
                {
                    Dictionary<string, FileInfo[]> fileInfosByCiName = unmatchedFileInfos.GroupBy(f => f.Name, ciComparer).ToDictionary(k => k.Key, v => v.ToArray(), ciComparer);
                    foreach (IGrouping<string, TDbFile> dbFileByCiName in unmatchedDbFiles.GroupBy(f => f.Name, ciComparer))
                    {
                        string n = dbFileByCiName.Key;
                        if (!dbFileByCiName.Skip(1).Any() && fileInfosByCiName.TryGetValue(n, out FileInfo[] fi) && fi.Length == 1)
                        {
                            FileInfo f = fi[0];
                            unmatchedFileInfos.Remove(f);
                            result.Add((dbFileByCiName.First(), f));
                        }
                        else
                            result.AddRange(dbFileByCiName.Select(f => (f, (FileInfo)null)));
                    }
                }
                foreach (TDbFile f in unmatchedDbFiles)
                    result.Add(new(f, null));
            }
            return result;
        }
    }
}
