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
    public class PersonalTagDefinition : PersonalTagDefinitionRow, ILocalPersonalTagDefinition, IEquatable<PersonalTagDefinition>
    {
        private HashSet<PersonalFileTag> _fileTags = new();
        private HashSet<PersonalSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<PersonalVolumeTag> _volumeTags = new();

        [NotNull]
        [BackingField(nameof(_fileTags))]
        public virtual HashSet<PersonalFileTag> FileTags { get => _fileTags; set => _fileTags = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_subdirectoryTags))]
        public virtual HashSet<PersonalSubdirectoryTag> SubdirectoryTags { get => _subdirectoryTags; set => _subdirectoryTags = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_volumeTags))]
        public virtual HashSet<PersonalVolumeTag> VolumeTags { get => _volumeTags; set => _volumeTags = value ?? new(); }


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

        protected bool ArePropertiesEqual([DisallowNull] ILocalPersonalTagDefinition other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IPersonalTagDefinition other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(PersonalTagDefinition other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPersonalTagDefinition other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = hash * 19 + Name.GetHashCode();
            hash = hash * 19 + Description.GetHashCode();
            hash = EntityExtensions.HashNullable(UpstreamId, hash, 19);
            hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 19);
            hash = hash * 19 + CreatedOn.GetHashCode();
            hash = hash * 19 + ModifiedOn.GetHashCode();
            return hash;
        }
    }
}
