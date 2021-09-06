using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class PersonalTagDefinition : PersonalTagDefinitionRow, ILocalPersonalTagDefinition
    {
        private HashSet<PersonalFileTag> _fileTags = new();
        private HashSet<PersonalSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<PersonalVolumeTag> _volumeTags = new();

        public virtual HashSet<PersonalFileTag> FileTags
        {
            get => _fileTags;
            set => CheckHashSetChanged(_fileTags, value, h => _fileTags = h);
        }

        public virtual HashSet<PersonalSubdirectoryTag> SubdirectoryTags
        {
            get => _subdirectoryTags;
            set => CheckHashSetChanged(_subdirectoryTags, value, h => _subdirectoryTags = h);
        }

        public virtual HashSet<PersonalVolumeTag> VolumeTags
        {
            get => _volumeTags;
            set => CheckHashSetChanged(_volumeTags, value, h => _volumeTags = h);
        }

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

        public static async Task<int> DeleteAsync(PersonalTagDefinition target, LocalDbContext dbContext, IStatusListener statusListener)
        {
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            using IDisposable loggerScope = statusListener.Logger.BeginScope(target.Id);
            statusListener.Logger.LogInformation("Removing PersonalTagDefinition {{ Id = {Id}; Name = \"{Name}\" }}", target.Id, target.Name);
            statusListener.SetMessage($"Removing personal tag definition: {target.Name}");
            EntityEntry<PersonalTagDefinition> entry = dbContext.Entry(target);
            PersonalFileTag[] fileTags = (await entry.GetRelatedCollectionAsync(e => e.FileTags, statusListener.CancellationToken)).ToArray();
            PersonalSubdirectoryTag[] subdirectoryTags = (await entry.GetRelatedCollectionAsync(e => e.SubdirectoryTags, statusListener.CancellationToken)).ToArray();
            PersonalVolumeTag[] volumeTags = (await entry.GetRelatedCollectionAsync(e => e.VolumeTags, statusListener.CancellationToken)).ToArray();
            if (fileTags.Length > 0)
                dbContext.PersonalFileTags.RemoveRange(fileTags);
            if (subdirectoryTags.Length > 0)
                dbContext.PersonalSubdirectoryTags.RemoveRange(subdirectoryTags);
            if (volumeTags.Length > 0)
                dbContext.PersonalVolumeTags.RemoveRange(volumeTags);
            int result = dbContext.ChangeTracker.HasChanges() ? await dbContext.SaveChangesAsync(statusListener.CancellationToken) : 0;
            _ = dbContext.PersonalTagDefinitions.Remove(target);
            result += await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await transaction.CommitAsync(statusListener.CancellationToken);
            return result;
        }
    }
}
