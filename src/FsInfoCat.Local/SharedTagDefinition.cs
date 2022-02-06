using FsInfoCat.Activities;
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
    public class SharedTagDefinition : SharedTagDefinitionRow, ILocalSharedTagDefinition
    {
        private HashSet<SharedFileTag> _fileTags = new();
        private HashSet<SharedSubdirectoryTag> _subdirectoryTags = new();
        private HashSet<SharedVolumeTag> _volumeTags = new();

        public HashSet<SharedFileTag> FileTags
        {
            get => _fileTags;
            set => CheckHashSetChanged(_fileTags, value, h => _fileTags = h);
        }

        public HashSet<SharedSubdirectoryTag> SubdirectoryTags
        {
            get => _subdirectoryTags;
            set => CheckHashSetChanged(_subdirectoryTags, value, h => _subdirectoryTags = h);
        }

        public HashSet<SharedVolumeTag> VolumeTags
        {
            get => _volumeTags;
            set => CheckHashSetChanged(_volumeTags, value, h => _volumeTags = h);
        }

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
    }
}
