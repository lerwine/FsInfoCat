using M = FsInfoCat.Model;
using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Class SummaryPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalSummaryPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalSummaryPropertySet" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SummaryPropertySet : SummaryPropertiesRow, ILocalSummaryPropertySet, IEquatable<SummaryPropertySet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<DbFile> _files = new();

        // TODO: Document SummaryPropertySet class members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [NotNull]
        [BackingField(nameof(_files))]
        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<M.IFile> M.IPropertySet.Files => Files.Cast<M.IFile>();

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<SummaryPropertySet> builder)
        {
            if (builder is null)
                throw new ArgumentOutOfRangeException(nameof(builder));
            _ = builder.Property(nameof(Author)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Keywords)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(ItemAuthors)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Kind)).HasConversion(MultiStringValue.Converter);
        }

        internal static async Task RefreshAsync([DisallowNull] EntityEntry<DbFile> entry, [DisallowNull] IFileDetailProvider fileDetailProvider,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
            if (fileDetailProvider is null)
                throw new ArgumentNullException(nameof(fileDetailProvider));
            switch (entry.State)
            {
                case EntityState.Detached:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is detached");
                case EntityState.Deleted:
                    throw new ArgumentOutOfRangeException(nameof(entry), $"{nameof(DbFile)} is flagged for deletion");
            }
            if (entry.Context is not LocalDbContext dbContext)
                throw new ArgumentOutOfRangeException(nameof(entry), "Invalid database context");
            DbFile entity;
            SummaryPropertySet oldPropertySet = (entity = entry.Entity).SummaryPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.SummaryProperties, cancellationToken) : null;
            M.ISummaryProperties currentProperties = await fileDetailProvider.GetSummaryPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.SummaryProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.SummaryPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        public bool Equals(SummaryPropertySet other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ILocalSummaryPropertySet other)
        {
            if (other is null) return false;
            if (other is SummaryPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(M.ISummaryPropertySet other)
        {
            if (other is null) return false;
            if (other is SummaryPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSummaryPropertySet local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.ISummaryPropertiesRow other)
        {
            if (other is null) return false;
            if (other is SummaryPropertySet propertySet) return Equals(propertySet);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSummaryPropertiesRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(M.ISummaryProperties other)
        {
            if (other is null) return false;
            if (other is SummaryPropertySet propertySet) return Equals(propertySet);
            if (other is M.ISummaryPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalSummaryPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SummaryPropertySet other) return Equals(other);
            if (obj is M.ISummaryPropertiesRow row)
            {
                if (TryGetId(out Guid id)) return row.TryGetId(out Guid id2) && id.Equals(id2);
                return !row.TryGetId(out _) && (row is ILocalSummaryPropertiesRow localRow) ? ArePropertiesEqual(localRow) : ArePropertiesEqual(row);
            }
            return obj is M.ISummaryProperties properties && ArePropertiesEqual(properties);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
