using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    // TODO: Document SharedTagDefinition class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    [Obsolete("Use FsInfoCat.Local.Model.SharedTagDefinition")]
    public class SharedTagDefinition : SharedTagDefinitionRow, ILocalSharedTagDefinition, IEquatable<SharedTagDefinition>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<SharedFileTag> _fileTags = new();
        private HashSet<SharedSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<SharedVolumeTag> _volumeTags = new();

        [NotNull]
        [BackingField(nameof(_fileTags))]
        public HashSet<SharedFileTag> FileTags { get => _fileTags; set => _fileTags = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_subdirectoryTags))]
        public HashSet<SharedSubdirectoryTag> SubdirectoryTags { get => _subdirectoryTags; set => _subdirectoryTags = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_volumeTags))]
        public HashSet<SharedVolumeTag> VolumeTags { get => _volumeTags; set => _volumeTags = value ?? new(); }

        IEnumerable<ILocalFileTag> ILocalTagDefinition.FileTags => FileTags.Cast<ILocalFileTag>();

        IEnumerable<ISharedFileTag> ISharedTagDefinition.FileTags => FileTags.Cast<ISharedFileTag>();

        IEnumerable<IFileTag> ITagDefinition.FileTags => FileTags.Cast<IFileTag>();

        IEnumerable<ILocalSharedFileTag> ILocalSharedTagDefinition.FileTags => FileTags.Cast<ILocalSharedFileTag>();

        IEnumerable<ILocalSubdirectoryTag> ILocalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSubdirectoryTag>();

        IEnumerable<ISharedSubdirectoryTag> ISharedTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISharedSubdirectoryTag>();

        IEnumerable<ISubdirectoryTag> ITagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISubdirectoryTag>();

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalSharedTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSharedSubdirectoryTag>();

        IEnumerable<ILocalVolumeTag> ILocalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalVolumeTag>();

        IEnumerable<ISharedVolumeTag> ISharedTagDefinition.VolumeTags => VolumeTags.Cast<ISharedVolumeTag>();

        IEnumerable<IVolumeTag> ITagDefinition.VolumeTags => VolumeTags.Cast<IVolumeTag>();

        IEnumerable<ILocalSharedVolumeTag> ILocalSharedTagDefinition.VolumeTags => VolumeTags.Cast<ILocalSharedVolumeTag>();

        public static async Task<EntityEntry<SharedTagDefinition>> DeleteAsync([DisallowNull] SharedTagDefinition target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress, [DisallowNull] ILogger logger)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
            if (progress is null) throw new ArgumentNullException(nameof(progress));
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            using (logger.BeginScope(target.Id))
            {
                logger.LogInformation("Removing SharedTagDefinition {{ Id = {Id}; Name = \"{Name}\" }}", target.Id, target.Name);
                progress.Report($"Removing shared tag definition: {target.Name}");
                EntityEntry<SharedTagDefinition> entry = dbContext.Entry(target);
                SharedFileTag[] fileTags = (await entry.GetRelatedCollectionAsync(e => e.FileTags, progress.Token)).ToArray();
                SharedSubdirectoryTag[] subdirectoryTags = (await entry.GetRelatedCollectionAsync(e => e.SubdirectoryTags, progress.Token)).ToArray();
                SharedVolumeTag[] volumeTags = (await entry.GetRelatedCollectionAsync(e => e.VolumeTags, progress.Token)).ToArray();
                if (fileTags.Length > 0)
                    dbContext.SharedFileTags.RemoveRange(fileTags);
                if (subdirectoryTags.Length > 0)
                    dbContext.SharedSubdirectoryTags.RemoveRange(subdirectoryTags);
                if (volumeTags.Length > 0)
                    dbContext.SharedVolumeTags.RemoveRange(volumeTags);
                _ = dbContext.ChangeTracker.HasChanges() ? await dbContext.SaveChangesAsync(progress.Token) : 0;
                _ = dbContext.SharedTagDefinitions.Remove(target);
                _ = await dbContext.SaveChangesAsync(progress.Token);
                await transaction.CommitAsync(progress.Token);
                return entry;
            }
        }

        public bool Equals(SharedTagDefinition other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ILocalSharedTagDefinition other)
        {
            if (other is null) return false;
            if (other is SharedTagDefinition tagDefinition) return Equals(tagDefinition);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(ILocalTagDefinition other)
        {
            if (other is null) return false;
            if (other is SharedTagDefinition tagDefinition) return Equals(tagDefinition);
            if (other is not ISharedTagDefinition) return false;
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSharedTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public bool Equals(ISharedTagDefinition other)
        {
            if (other is null) return false;
            if (other is SharedTagDefinition tagDefinition) return Equals(tagDefinition);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSharedTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public bool Equals(ITagDefinition other)
        {
            if (other is null) return false;
            if (other is SharedTagDefinition tagDefinition) return Equals(tagDefinition);
            if (other is not ISharedTagDefinition sharedTag) return false;
            if (TryGetId(out Guid id)) return sharedTag.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalSharedTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SharedTagDefinition tagDefinition) return Equals(tagDefinition);
            return obj is ISharedTagDefinition row && (TryGetId(out Guid id1) ? row.TryGetId(out Guid id2) && id1.Equals(id2) :
                (!row.TryGetId(out _) && ((row is ILocalSharedTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(row))));
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
