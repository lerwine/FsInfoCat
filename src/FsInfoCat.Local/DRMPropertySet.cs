using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Class DRMPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalDRMPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalDRMPropertySet" />
    public class DRMPropertySet : LocalDbEntity, ILocalDRMPropertySet, ISimpleIdentityReference<DRMPropertySet>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<DateTime?> _datePlayExpires;
        private readonly IPropertyChangeTracker<DateTime?> _datePlayStarts;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<bool?> _isProtected;
        private readonly IPropertyChangeTracker<uint?> _playCount;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public DateTime? DatePlayExpires { get => _datePlayExpires.GetValue(); set => _datePlayExpires.SetValue(value); }
        public DateTime? DatePlayStarts { get => _datePlayStarts.GetValue(); set => _datePlayStarts.SetValue(value); }
        public string Description { get => _description.GetValue(); set => _description.SetValue(value); }
        public bool? IsProtected { get => _isProtected.GetValue(); set => _isProtected.SetValue(value); }
        public uint? PlayCount { get => _playCount.GetValue(); set => _playCount.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        DRMPropertySet IIdentityReference<DRMPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        public DRMPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _datePlayExpires = AddChangeTracker<DateTime?>(nameof(DatePlayExpires), null);
            _datePlayStarts = AddChangeTracker<DateTime?>(nameof(DatePlayStarts), null);
            _description = AddChangeTracker(nameof(Description), null, FilePropertiesComparer.StringValueCoersion);
            _isProtected = AddChangeTracker<bool?>(nameof(IsProtected), null);
            _playCount = AddChangeTracker<uint?>(nameof(PlayCount), null);
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
            DRMPropertySet oldPropertySet = (entity = entry.Entity).DRMPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.DRMProperties, cancellationToken) : null;
            IDRMProperties currentProperties = await fileDetailProvider.GetDRMPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            if (currentProperties.IsNullOrAllPropertiesEmpty())
                entity.DRMProperties = null;
            else
                entity.DRMProperties = await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        dbContext.DRMPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
