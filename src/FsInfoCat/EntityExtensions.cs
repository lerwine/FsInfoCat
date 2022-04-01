using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        
        /// <summary>
        /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same file length.
        /// </summary>
        /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same file length.</param>
        /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where the file length does not match the file length of any <see cref="FileInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where the file length does not match the file length of any <typeparamref name="TDbFile"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TDbFile}, List{FileInfo}}}"/> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same file length.</returns>
        private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByLength<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs,
            out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
            where TDbFile : class, IFile
        {
            List<TDbFile> udb = new();
            List<(List<TDbFile>, List<FileInfo>)> mm = new();
            long[] values = dbItems.GroupBy(d => d.BinaryProperties.Length).GroupJoin(osItems, g => g.Key, f => f.Length, (g, f) => (Group: g, f.ToList())).Where(a =>
            {
                (IGrouping<long, TDbFile> grp, List<FileInfo> fs) = a;
                List<TDbFile> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                    mm.Add((db, fs));
                else
                {
                    udb.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            unmatchedDb = udb;
            unmatchedFs = ((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.Length))).ToList();
            return mm;
        }

        /// <summary>
        /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same last write time.
        /// </summary>
        /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same file length and last write time.</param>
        /// <param name="partialMatches">Sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share only the same last write time with no matching file length.</param>
        /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the file length nor last write time matches the file length or last write time of any <see cref="FileInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the file length nor last write time matches the file length or last write time of any <typeparamref name="TDbFile"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TDbFile}, List{FileInfo}}}"/> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same file length and last write time.</returns>
        private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByLastWriteTime<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs,
            out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
            where TDbFile : class, IFile
        {
            List<TDbFile> udb1 = new();
            List<FileInfo> ufs1 = new();
            List<(List<TDbFile>, List<FileInfo>)> mm = new();
            List<(List<TDbFile>, List<FileInfo>)> pm = new();
            DateTime[] values = dbItems.GroupBy(d => d.LastWriteTime).GroupJoin(osItems, g => g.Key, f => f.LastWriteTime, (g, f) => (Group: g, f.ToList())).Where(a =>
            {
                (IGrouping<DateTime, TDbFile> grp, List<FileInfo> fs) = a;
                List<TDbFile> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                {
                    mm.AddRange(MatchByLength(db, fs, matchingPairs, out List<TDbFile> udb2, out List<FileInfo> ufs2));
                    // udb2/ufs2 = Matches last write time but not length
                    if (udb2.Count == 1 && ufs2.Count == 1)
                        matchingPairs.Add((udb2[0], ufs2[0]));
                    else if (ufs2.Count > 0)
                        pm.Add((udb2, ufs2));
                    else
                        udb1.AddRange(udb2);
                }
                else
                {
                    udb1.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            unmatchedDb = udb1;
            ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.LastWriteTime)));
            unmatchedFs = ufs1;
            partialMatches = pm;
            return mm;
        }

        /// <summary>
        /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same last write time.
        /// </summary>
        /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS subdirectory objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="matchingPairs">Contains pairs of <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects with the same last write time.</param>
        /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where the last write time does not match the last write time of any <see cref="DirectoryInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where the last write time does not match the last write time of any <typeparamref name="TSubdirectory"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TSubdirectory}, List{DirectoryInfo}}}"/> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same last write time.</returns>
        private static List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> MatchByLastWriteTime<TSubdirectory>(IEnumerable<TSubdirectory> dbItems, IEnumerable<DirectoryInfo> osItems,
            List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
            where TSubdirectory : class, ISubdirectory
        {
            List<TSubdirectory> udb = new();
            List<(List<TSubdirectory>, List<DirectoryInfo>)> mm = new();
            DateTime[] values = dbItems.GroupBy(d => d.LastWriteTime).GroupJoin(osItems, g => g.Key, f => f.LastWriteTime, (g, f) => (Group: g, f.ToList())).Where(a =>
            {
                (IGrouping<DateTime, TSubdirectory> grp, List<DirectoryInfo> fs) = a;
                List<TSubdirectory> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                    mm.Add((db, fs));
                else
                {
                    udb.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            unmatchedDb = udb;
            unmatchedFs = ((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.LastWriteTime))).ToList();
            return mm;
        }

        /// <summary>
        /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same creation time.
        /// </summary>
        /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same file length, last write time, and creation time.</param>
        /// <param name="partialMatches">Sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same creation time and may also share the same last write time.</param>
        /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the file length, last write time, nor creation time matches the file length, last write time or creation time of any <see cref="FileInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the file length, last write time, nor creation time matches the file length, last write time or creation time of any <typeparamref name="TDbFile"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TDbFile}, List{FileInfo}}}"/> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same file length, last write time, and creation time.</returns>
        private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByCreationTime<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs,
            out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
            where TDbFile : class, IFile
        {
            List<TDbFile> udb1 = new();
            List<FileInfo> ufs1 = new();
            List<(List<TDbFile>, List<FileInfo>)> mm = new();
            List<(List<TDbFile>, List<FileInfo>)> pm = new();
            DateTime[] values = dbItems.GroupBy(d => d.CreationTime).GroupJoin(osItems, g => g.Key, f => f.CreationTime, (g, f) => (Group: g, f.ToList())).Where(a =>
            {
                (IGrouping<DateTime, TDbFile> grp, List<FileInfo> fs) = a;
                List<TDbFile> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                {
                    mm.AddRange(MatchByLastWriteTime(db, fs, matchingPairs, out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm2, out List<TDbFile> udb2, out List<FileInfo> ufs2));
                    // pm2 = matches creation time and last write time, but not file length
                    // udb2/ufs2 = matches creation time, but not last write time or file length.
                    if (udb2.Count == 1 && ufs2.Count == 1)
                        matchingPairs.Add((udb2[0], ufs2[0]));
                    else if (ufs2.Count > 0)
                    {

                        pm.Add((udb2, ufs2));
                    }
                    else
                        udb1.AddRange(udb2);
                    pm.AddRange(pm2);
                }
                else
                {
                    udb1.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.CreationTime)));
            if (ufs1.Count > 1 && udb1.Count > 1)
                mm.AddRange(MatchByLength(udb1, ufs1, matchingPairs, out unmatchedDb, out unmatchedFs));
            else
            {
                unmatchedFs = ufs1;
                unmatchedDb = udb1;
            }
            partialMatches = pm;
            return mm;
        }

        /// <summary>
        /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same creation time.
        /// </summary>
        /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="matchingPairs">Contains pairs of <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects with the same last write time and creation time.</param>
        /// <param name="partialMatches">Sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share only the same last write time with no matching creation time.</param>
        /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <see cref="DirectoryInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <typeparamref name="TSubdirectory"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TSubdirectory}, List{DirectoryInfo}}}"/> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same last write time and creation time.</returns>
        private static List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> MatchByCreationTime<TSubdirectory>(IEnumerable<TSubdirectory> dbItems, IEnumerable<DirectoryInfo> osItems, List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs,
            out List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> partialMatches, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
            where TSubdirectory : class, ISubdirectory
        {
            List<TSubdirectory> udb1 = new();
            List<DirectoryInfo> ufs1 = new();
            List<(List<TSubdirectory>, List<DirectoryInfo>)> mm = new();
            List<(List<TSubdirectory>, List<DirectoryInfo>)> pm = new();
            DateTime[] values = dbItems.GroupBy(d => d.CreationTime).GroupJoin(osItems, g => g.Key, f => f.CreationTime, (g, f) => (Group: g, f.ToList())).Where(a =>
            {
                (IGrouping<DateTime, TSubdirectory> grp, List<DirectoryInfo> fs) = a;
                List<TSubdirectory> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                {
                    mm.AddRange(MatchByLastWriteTime(db, fs, matchingPairs, out List<TSubdirectory> udb2, out List<DirectoryInfo> ufs2));
                    if (udb2.Count == 1 && ufs2.Count == 1)
                        matchingPairs.Add((udb2[0], ufs2[0]));
                    else if (ufs2.Count > 0)
                        pm.Add((udb2, ufs2));
                    else
                        udb1.AddRange(udb2);
                }
                else
                {
                    udb1.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            unmatchedDb = udb1;
            ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.CreationTime)));
            unmatchedFs = ufs1;
            partialMatches = pm;
            return mm;
        }

        /// <summary>
        /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same name.
        /// </summary>
        /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="comparer">The name comparer.</param>
        /// <param name="partialMatches">Sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same name and may also share the same creation time or last write time.</param>
        /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the name, file length, last write time, nor creation time matches the name, file length, last write time or creation time of any <see cref="FileInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the name, file length, last write time, nor creation time matches the file name, length, last write time or creation time of any <typeparamref name="TDbFile"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TDbFile}, List{FileInfo}}}"/> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same name, file length, last write time, and creation time.</returns>
        private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByName<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs, StringComparer comparer,
            out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
            where TDbFile : class, IFile
        {
            List<TDbFile> udb1 = new();
            List<FileInfo> ufs1 = new();
            List<(List<TDbFile>, List<FileInfo>)> mm = new();
            List<(List<TDbFile>, List<FileInfo>)> pm = new();
            string[] values = dbItems.GroupBy(d => d.Name ?? "", comparer).GroupJoin(osItems, g => g.Key, f => f.Name, (g, f) => (Group: g, f.ToList()), comparer).Where(a =>
            {
                (IGrouping<string, TDbFile> grp, List<FileInfo> fs) = a;
                List<TDbFile> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                {
                    mm.AddRange(MatchByCreationTime(db, fs, matchingPairs, out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm2, out List<TDbFile> udb2, out List<FileInfo> ufs2));
                    // pm2 = matches name and creation time (may match last write time)
                    // udb2/ufs2 = matches name, but not creation time, file length or last write time
                    if (udb2.Count == 1 && ufs2.Count == 1)
                        matchingPairs.Add((udb2[0], ufs2[0]));
                    else if (ufs2.Count > 0)
                        pm.Add((udb2, ufs2));
                    else
                        udb1.AddRange(udb2);
                    pm.AddRange(pm2);
                }
                else
                {
                    udb1.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.Name ?? "", comparer)));
            if (ufs1.Count > 1 && udb1.Count > 1)
            {
                mm.AddRange(MatchByLastWriteTime(udb1, ufs1, matchingPairs, out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm2, out unmatchedDb, out unmatchedFs));
                pm.AddRange(pm2);
            }
            else
            {
                unmatchedDb = udb1;
                unmatchedFs = ufs1;
            }
            partialMatches = pm;
            return mm;
        }

        /// <summary>
        /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same creation time.
        /// </summary>
        /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
        /// <param name="dbItems">The input database entity objects.</param>
        /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
        /// <param name="comparer">The name comparer.</param>
        /// <param name="matchingPairs">Contains pairs of <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects with the same last write time and creation time.</param>
        /// <param name="partialMatches">Sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same name and may also share the same creation time or last write time.</param>
        /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <see cref="DirectoryInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <typeparamref name="TSubdirectory"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{List{TSubdirectory}, List{DirectoryInfo}}}"/> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same name, last write time and creation time.</returns>
        private static List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> MatchByName<TSubdirectory>(IEnumerable<TSubdirectory> dbItems, IEnumerable<DirectoryInfo> osItems, List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs,
            StringComparer comparer, out List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> partialMatches, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
            where TSubdirectory : class, ISubdirectory
        {
            List<TSubdirectory> udb1 = new();
            List<DirectoryInfo> ufs1 = new();
            List<(List<TSubdirectory>, List<DirectoryInfo>)> mm = new();
            List<(List<TSubdirectory>, List<DirectoryInfo>)> pm = new();
            string[] values = dbItems.GroupBy(d => d.Name ?? "", comparer).GroupJoin(osItems, g => g.Key, f => f.Name, (g, f) => (Group: g, f.ToList()), comparer).Where(a =>
            {
                (IGrouping<string, TSubdirectory> grp, List<DirectoryInfo> fs) = a;
                List<TSubdirectory> db = grp.ToList();
                if (db.Count == 1 && fs.Count == 1)
                    matchingPairs.Add((db[0], fs[0]));
                else if (fs.Count > 0)
                {
                    mm.AddRange(MatchByCreationTime(db, fs, matchingPairs, out List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> pm2, out List<TSubdirectory> udb2, out List<DirectoryInfo> ufs2));
                    if (udb2.Count == 1 && ufs2.Count == 1)
                        matchingPairs.Add((udb2[0], ufs2[0]));
                    else if (ufs2.Count > 0)
                        pm.Add((udb2, ufs2));
                    else
                        udb1.AddRange(udb2);
                    pm.AddRange(pm2);
                }
                else
                {
                    udb1.AddRange(db);
                    return false;
                }
                return true;
            }).Select(a => a.Group.Key).ToArray();
            ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.Name ?? "", comparer)));
            if (ufs1.Count > 1 && udb1.Count > 1)
                mm.AddRange(MatchByLastWriteTime(udb1, ufs1, matchingPairs, out unmatchedDb, out unmatchedFs));
            else
            {
                unmatchedDb = udb1;
                unmatchedFs = ufs1;
            }
            partialMatches = pm;
            return mm;
        }

        /// <summary>
        /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same name and other matching property values.
        /// </summary>
        /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
        /// <param name="source1">The input database entity objects.</param>
        /// <param name="source2">The input OS file objects to match up with corresponding <paramref name="source2"/>.</param>
        /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the length, last write time nor creation time matches the length, last write time or creation time of any <see cref="FileInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the length, last write time nor creation time matches the length, last write time or creation time of any <typeparamref name="TDbFile"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{TDbFile, FileInfo}}"/> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same name, last write time, creation time and length.</returns>
        public static List<(TDbFile DbFile, FileInfo FileInfo)> ToMatchedPairs<TDbFile>(this IEnumerable<TDbFile> source1, IEnumerable<FileInfo> source2, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
            where TDbFile : class, IFile
        {
            List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs = new();
            List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches;
            foreach ((List<TDbFile> multiMatchDb, List<FileInfo> multiMatchFs) in MatchByName(source1, source2, matchingPairs, StringComparer.InvariantCultureIgnoreCase, out partialMatches, out unmatchedDb, out unmatchedFs))
            {
                List<TDbFile> udb;
                List<FileInfo> ufs;
                List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm;
                foreach ((List<TDbFile> mmDb, List<FileInfo> mmFs) in MatchByName(multiMatchDb, multiMatchFs, matchingPairs, StringComparer.InvariantCulture, out pm, out udb, out ufs))
                {
                    if (mmDb.Count > mmFs.Count)
                    {
                        matchingPairs.AddRange(mmDb.Take(mmFs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                        udb.AddRange(mmDb.Skip(mmFs.Count));
                    }
                    else if (mmDb.Count < mmFs.Count)
                    {
                        matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(mmFs.Take(mmDb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                        ufs.AddRange(mmFs.Skip(mmDb.Count));
                    }
                    else
                        matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                }
                partialMatches.AddRange(pm);
                if (udb.Count == 1 && ufs.Count == 1)
                    matchingPairs.Add((udb[0], ufs[0]));
                else if (udb.Count > 0)
                {
                    if (ufs.Count > 0)
                    {
                        if (udb.Count > ufs.Count)
                        {
                            matchingPairs.AddRange(udb.Take(ufs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                            unmatchedDb.AddRange(udb.Skip(ufs.Count));
                        }
                        else if (udb.Count < ufs.Count)
                        {
                            matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(ufs.Take(udb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                            unmatchedFs.AddRange(ufs.Skip(udb.Count));
                        }
                        else
                            matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                    }
                    else
                        unmatchedDb.AddRange(udb);
                }
                else if (ufs.Count > 0)
                    unmatchedFs.AddRange(ufs);
            }
            foreach ((List<TDbFile> db, List<FileInfo> fs) in partialMatches)
            {
                if (db.Count > fs.Count)
                {
                    matchingPairs.AddRange(db.Take(fs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                    unmatchedDb.AddRange(db.Skip(fs.Count));
                }
                else if (db.Count < fs.Count)
                {
                    matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(fs.Take(db.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                    unmatchedFs.AddRange(fs.Skip(db.Count));
                }
                else
                    matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
            }
            return matchingPairs;
        }

        /// <summary>
        /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same name and other matching property values.
        /// </summary>
        /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
        /// <param name="source1">The input database entity objects.</param>
        /// <param name="source2">The input OS subdirectory objects to match up with corresponding <paramref name="source2"/>.</param>
        /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <see cref="DirectoryInfo"/> objects.</param>
        /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <typeparamref name="TSubdirectory"/> objects.</param>
        /// <returns>A <see cref="List{ValueTuple{TSubdirectory, DirectoryInfo}}"/> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same name, last write time and creation time.</returns>
        public static List<(TSubdirectory Subdirectory, DirectoryInfo DirectoryInfo)> ToMatchedPairs<TSubdirectory>(this IEnumerable<TSubdirectory> source1, IEnumerable<DirectoryInfo> source2, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
            where TSubdirectory : class, ISubdirectory
        {
            List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs = new();
            List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> partialMatches;
            foreach ((List<TSubdirectory> multiMatchDb, List<DirectoryInfo> multiMatchFs) in MatchByName(source1, source2, matchingPairs, StringComparer.InvariantCultureIgnoreCase, out partialMatches, out unmatchedDb, out unmatchedFs))
            {
                List<TSubdirectory> udb;
                List<DirectoryInfo> ufs;
                List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> pm;
                foreach ((List<TSubdirectory> mmDb, List<DirectoryInfo> mmFs) in MatchByName(multiMatchDb, multiMatchFs, matchingPairs, StringComparer.InvariantCulture, out pm, out udb, out ufs))
                {
                    if (mmDb.Count > mmFs.Count)
                    {
                        matchingPairs.AddRange(mmDb.Take(mmFs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                        udb.AddRange(mmDb.Skip(mmFs.Count));
                    }
                    else if (mmDb.Count < mmFs.Count)
                    {
                        matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(mmFs.Take(mmDb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                        ufs.AddRange(mmFs.Skip(mmDb.Count));
                    }
                    else
                        matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                }
                partialMatches.AddRange(pm);
                if (udb.Count == 1 && ufs.Count == 1)
                    matchingPairs.Add((udb[0], ufs[0]));
                else if (udb.Count > 0)
                {
                    if (ufs.Count > 0)
                    {
                        if (udb.Count > ufs.Count)
                        {
                            matchingPairs.AddRange(udb.Take(ufs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                            unmatchedDb.AddRange(udb.Skip(ufs.Count));
                        }
                        else if (udb.Count < ufs.Count)
                        {
                            matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(ufs.Take(udb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                            unmatchedFs.AddRange(ufs.Skip(udb.Count));
                        }
                        else
                            matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                    }
                    else
                        unmatchedDb.AddRange(udb);
                }
                else if (ufs.Count > 0)
                    unmatchedFs.AddRange(ufs);
            }
            foreach ((List<TSubdirectory> db, List<DirectoryInfo> fs) in partialMatches)
            {
                if (db.Count > fs.Count)
                {
                    matchingPairs.AddRange(db.Take(fs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                    unmatchedDb.AddRange(db.Skip(fs.Count));
                }
                else if (db.Count < fs.Count)
                {
                    matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(fs.Take(db.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                    unmatchedFs.AddRange(fs.Skip(db.Count));
                }
                else
                    matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
            }
            return matchingPairs;
        }

        [Obsolete("Use ToMatchedPairs(IEnumerable<TSubdirectory>, IEnumerable<DirectoryInfo>, IEnumerable<TSubdirectory>, out IEnumerable<DirectoryInfo>)")]
        public static LinkedList<(TSubdirectory Subdirectory, DirectoryInfo DirectoryInfo)> ToMatchedPairs<TSubdirectory>(this IEnumerable<TSubdirectory> subdirectories, IEnumerable<DirectoryInfo> directoryInfos)
            where TSubdirectory : class, ISubdirectory
        {
            if (subdirectories is null || !(subdirectories = subdirectories.Where(s => s is not null)).Distinct().Any())
            {
                if (directoryInfos is null || !(directoryInfos = directoryInfos.Where(d => d is not null)).Distinct().Any())
                    return new();
                return new(directoryInfos.Select(d => ((TSubdirectory)null, d)));
            }
            if (directoryInfos is null || !(directoryInfos = directoryInfos.Where(d => d is not null)).Distinct().Any())
                return new(subdirectories.Select(s => (s, (DirectoryInfo)null)));

            StringComparer csComparer = StringComparer.InvariantCulture;
            StringComparer ciComparer = StringComparer.InvariantCultureIgnoreCase;

            List<TSubdirectory> unmatchedSubdirectories = new();
            if (directoryInfos is not List<DirectoryInfo> unmatchedDirectoryInfos)
                unmatchedDirectoryInfos = directoryInfos.ToList();
            LinkedList<(TSubdirectory Subdirectory, DirectoryInfo DirectoryInfo)> result = new(subdirectories.Select(Subdirectory =>
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
                }));
            if (unmatchedSubdirectories.Count == 0)
            {
                foreach (DirectoryInfo di in unmatchedDirectoryInfos)
                    result.AddLast(((TSubdirectory)null, di));
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
                            result.AddLast((subdirectoryByCiName.First(), d));
                        }
                        else
                            foreach (TSubdirectory d in subdirectoryByCiName)
                                result.AddLast((d, null));
                    }
                }
                foreach (TSubdirectory s in unmatchedSubdirectories)
                    result.AddLast((s, null));
            }
            return result;
        }

        [Obsolete("Use ToMatchedPairs(IEnumerable<TDbFile>, IEnumerable<FileInfo>, IEnumerable<TDbFile>, out IEnumerable<FileInfo>)")]
        public static LinkedList<(TDbFile DbFile, FileInfo FileInfo)> ToMatchedPairs<TDbFile>(this IEnumerable<TDbFile> dbFiles, IEnumerable<FileInfo> fileInfos)
            where TDbFile : class, IFile
        {
            if (dbFiles is null || !(dbFiles = dbFiles.Where(f => f is not null)).Distinct().Any())
            {
                if (fileInfos is null || !(fileInfos = fileInfos.Where(f => f is not null)).Distinct().Any())
                    return new();
                return new(fileInfos.Select(f => ((TDbFile)null, f)));
            }
            if (fileInfos is null || !(fileInfos = fileInfos.Where(f => f is not null)).Distinct().Any())
                return new(dbFiles.Select(f => (f, (FileInfo)null)));

            StringComparer csComparer = StringComparer.InvariantCulture;
            StringComparer ciComparer = StringComparer.InvariantCultureIgnoreCase;

            List<TDbFile> unmatchedDbFiles = new();
            if (fileInfos is not List<FileInfo> unmatchedFileInfos)
                unmatchedFileInfos = fileInfos.ToList();
            LinkedList<(TDbFile DbFile, FileInfo FileInfo)> result = new(dbFiles.Select(DbFile =>
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
            }));
            if (unmatchedDbFiles.Count == 0)
            {
                foreach (FileInfo fi in unmatchedFileInfos)
                    result.AddLast((null, fi));
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
                            result.AddLast((dbFileByCiName.First(), f));
                        }
                        else
                            foreach (TDbFile f in dbFileByCiName)
                                result.AddLast((f, null));
                    }
                }
                foreach (TDbFile f in unmatchedDbFiles)
                    result.AddLast((f, null));
            }
            return result;
        }

        public static bool NullablesEqual<T>(T? x, T? y) where T : struct, IEquatable<T> => x.HasValue ? (y.HasValue && x.Value.Equals(y.Value)) : !y.HasValue;

        public static int HashGuid(Guid value, int hash, int prime) => value.Equals(Guid.Empty) ? hash * prime : hash * prime + value.GetHashCode();

        public static int HashNullable<T>(T? value, int hash, int prime) where T : struct => value.HasValue ? hash * prime + value.Value.GetHashCode() : hash * prime;

        public static int HashRelatedEntity<T>(T value, Func<Guid> getId, int hash, int prime) where T : class, IHasSimpleIdentifier => (value is null) ? HashGuid(getId(), hash, prime) : hash * prime + value.GetHashCode();

        public static int HashObject<T>(T value, int hash, int prime) where T : class => (value is null) ? hash * prime : hash * prime + value.GetHashCode();
    }
}
