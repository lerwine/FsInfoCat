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

namespace FsInfoCat.Local
{
    /// <summary>
    /// Class MusicPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalMusicPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalMusicPropertySet" />
    public class MusicPropertySet : MusicPropertiesRow, ILocalMusicPropertySet, ISimpleIdentityReference<MusicPropertySet>, IEquatable<MusicPropertySet>
    {
        private HashSet<DbFile> _files = new();

        public HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        MusicPropertySet IIdentityReference<MusicPropertySet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<MusicPropertySet> builder)
        {
            if (builder is null)
                throw new ArgumentOutOfRangeException(nameof(builder));
            _ = builder.Property(nameof(Artist)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Composer)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Conductor)).HasConversion(MultiStringValue.Converter);
            _ = builder.Property(nameof(Genre)).HasConversion(MultiStringValue.Converter);
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
            MusicPropertySet oldPropertySet = (entity = entry.Entity).MusicPropertySetId.HasValue ?
                await entry.GetRelatedReferenceAsync(f => f.MusicProperties, cancellationToken) : null;
            IMusicProperties currentProperties = await fileDetailProvider.GetMusicPropertiesAsync(cancellationToken);
            if (FilePropertiesComparer.Equals(oldPropertySet, currentProperties))
                return;
            entity.MusicProperties = currentProperties.IsNullOrAllPropertiesEmpty() ? null : await dbContext.GetMatchingAsync(currentProperties, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (oldPropertySet is null)
                return;
            switch (entry.State)
            {
                case EntityState.Unchanged:
                case EntityState.Modified:
                    Guid id = entity.Id;
                    if (!(await dbContext.Entry(oldPropertySet).GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
                        _ = dbContext.MusicPropertySets.Remove(oldPropertySet);
                    cancellationToken.ThrowIfCancellationRequested();
                    break;
            }
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalMusicPropertySet other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IMusicPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(MusicPropertySet other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IMusicPropertySet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IMusicProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 47;
                    hash = hash * 59 + AlbumArtist.GetHashCode();
                    hash = hash * 59 + AlbumTitle.GetHashCode();
                    hash = (Artist is null) ? hash * 59 : hash * 59 + (Artist?.GetHashCode() ?? 0);
                    hash = ChannelCount.HasValue ? hash * 59 + (ChannelCount ?? default).GetHashCode() : hash * 59;
                    hash = (Composer is null) ? hash * 59 : hash * 59 + (Composer?.GetHashCode() ?? 0);
                    hash = (Conductor is null) ? hash * 59 : hash * 59 + (Conductor?.GetHashCode() ?? 0);
                    hash = hash * 59 + DisplayArtist.GetHashCode();
                    hash = (Genre is null) ? hash * 59 : hash * 59 + (Genre?.GetHashCode() ?? 0);
                    hash = hash * 59 + PartOfSet.GetHashCode();
                    hash = hash * 59 + Period.GetHashCode();
                    hash = TrackNumber.HasValue ? hash * 59 + (TrackNumber ?? default).GetHashCode() : hash * 59;
                    hash = UpstreamId.HasValue ? hash * 59 + (UpstreamId ?? default).GetHashCode() : hash * 59;
                    hash = LastSynchronizedOn.HasValue ? hash * 59 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 59;
                    hash = hash * 59 + CreatedOn.GetHashCode();
                    hash = hash * 59 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
