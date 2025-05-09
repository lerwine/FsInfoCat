using FsInfoCat.Model;
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

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// DB entity for a personal (private) tag that can be associated with <see cref="DbFile"/>, <see cref="Subdirectory"/> or <see cref="Volume"/> entities.
    /// </summary>
    /// <seealso cref="PersonalTagDefinitionListItem" />
    /// <seealso cref="LocalDbContext.PersonalTagDefinitions" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class PersonalTagDefinition : PersonalTagDefinitionRow, ILocalPersonalTagDefinition, IEquatable<PersonalTagDefinition>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private HashSet<PersonalFileTag> _fileTags = [];
        private HashSet<PersonalSubdirectoryTag> _subdirectoryTags = [];
        private HashSet<PersonalVolumeTag> _volumeTags = [];

        [NotNull]
        [BackingField(nameof(_fileTags))]
        public virtual HashSet<PersonalFileTag> FileTags { get => _fileTags; set => _fileTags = value ?? []; }

        [NotNull]
        [BackingField(nameof(_subdirectoryTags))]
        public virtual HashSet<PersonalSubdirectoryTag> SubdirectoryTags { get => _subdirectoryTags; set => _subdirectoryTags = value ?? []; }

        [NotNull]
        [BackingField(nameof(_volumeTags))]
        public virtual HashSet<PersonalVolumeTag> VolumeTags { get => _volumeTags; set => _volumeTags = value ?? []; }

        IEnumerable<ILocalFileTag> ILocalTagDefinition.FileTags => FileTags.Cast<ILocalFileTag>();

        IEnumerable<IPersonalFileTag> IPersonalTagDefinition.FileTags => FileTags.Cast<IPersonalFileTag>();

        IEnumerable<IFileTag> ITagDefinition.FileTags => FileTags.Cast<IFileTag>();

        IEnumerable<ILocalPersonalFileTag> ILocalPersonalTagDefinition.FileTags => FileTags.Cast<ILocalPersonalFileTag>();

        IEnumerable<ILocalSubdirectoryTag> ILocalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalSubdirectoryTag>();

        IEnumerable<IPersonalSubdirectoryTag> IPersonalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<IPersonalSubdirectoryTag>();

        IEnumerable<ISubdirectoryTag> ITagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ISubdirectoryTag>();

        IEnumerable<ILocalPersonalSubdirectoryTag> ILocalPersonalTagDefinition.SubdirectoryTags => SubdirectoryTags.Cast<ILocalPersonalSubdirectoryTag>();

        IEnumerable<ILocalVolumeTag> ILocalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalVolumeTag>();

        IEnumerable<IPersonalVolumeTag> IPersonalTagDefinition.VolumeTags => VolumeTags.Cast<IPersonalVolumeTag>();

        IEnumerable<IVolumeTag> ITagDefinition.VolumeTags => VolumeTags.Cast<IVolumeTag>();

        IEnumerable<ILocalPersonalVolumeTag> ILocalPersonalTagDefinition.VolumeTags => VolumeTags.Cast<ILocalPersonalVolumeTag>();

        public static async Task<int> DeleteAsync(PersonalTagDefinition target, LocalDbContext dbContext, IActivityProgress progress, ILogger logger)
        {
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            using (logger.BeginScope(target.Id))
            {
                logger.LogInformation("Removing PersonalTagDefinition {{ Id = {Id}; Name = \"{Name}\" }}", target.Id, target.Name);
                progress.Report($"Removing personal tag definition: {target.Name}");
                EntityEntry<PersonalTagDefinition> entry = dbContext.Entry(target);
                PersonalFileTag[] fileTags = (await entry.GetRelatedCollectionAsync(e => e.FileTags, progress.Token)).ToArray();
                PersonalSubdirectoryTag[] subdirectoryTags = (await entry.GetRelatedCollectionAsync(e => e.SubdirectoryTags, progress.Token)).ToArray();
                PersonalVolumeTag[] volumeTags = (await entry.GetRelatedCollectionAsync(e => e.VolumeTags, progress.Token)).ToArray();
                if (fileTags.Length > 0)
                    dbContext.PersonalFileTags.RemoveRange(fileTags);
                if (subdirectoryTags.Length > 0)
                    dbContext.PersonalSubdirectoryTags.RemoveRange(subdirectoryTags);
                if (volumeTags.Length > 0)
                    dbContext.PersonalVolumeTags.RemoveRange(volumeTags);
                int result = dbContext.ChangeTracker.HasChanges() ? await dbContext.SaveChangesAsync(progress.Token) : 0;
                _ = dbContext.PersonalTagDefinitions.Remove(target);
                result += await dbContext.SaveChangesAsync(progress.Token);
                await transaction.CommitAsync(progress.Token);
                return result;
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(PersonalTagDefinition other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ILocalPersonalTagDefinition other)
        {
            if (other is null) return false;
            if (other is PersonalTagDefinition tagDefinition) return Equals(tagDefinition);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        public bool Equals(ILocalTagDefinition other)
        {
            if (other is null) return false;
            if (other is PersonalTagDefinition tagDefinition) return Equals(tagDefinition);
            if (other is not IPersonalTagDefinition) return false;
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalPersonalTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public bool Equals(IPersonalTagDefinition other)
        {
            if (other is null) return false;
            if (other is PersonalTagDefinition tagDefinition) return Equals(tagDefinition);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalPersonalTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public bool Equals(ITagDefinition other)
        {
            if (other is null) return false;
            if (other is PersonalTagDefinition tagDefinition) return Equals(tagDefinition);
            if (other is not IPersonalTagDefinition personalTag) return false;
            if (TryGetId(out Guid id)) return personalTag.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalPersonalTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is PersonalTagDefinition tagDefinition) return Equals(tagDefinition);
            return obj is IPersonalTagDefinition row && (TryGetId(out Guid id1) ? row.TryGetId(out Guid id2) && id1.Equals(id2) :
                (!row.TryGetId(out _) && ((row is ILocalPersonalTagDefinition local) ? ArePropertiesEqual(local) : ArePropertiesEqual(row))));
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
