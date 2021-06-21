using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext
    {
        public class Interceptor : DbCommandInterceptor
        {
            private readonly ILogger<Interceptor> _logger;

            public static Interceptor Instance { get; } = new Interceptor();

            public Interceptor() { _logger = Services.ServiceProvider.GetRequiredService<ILogger<Interceptor>>(); }

            public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
            {
                _logger.LogInformation("NonQueryExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
                CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("NonQueryExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
            {
                _logger.LogInformation("ReaderExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("ReaderExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
            {
                _logger.LogInformation("ScalarExecuting: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecuting(command, eventData, result);
            }

            public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
            {
                _logger.LogInformation("ScalarExecutingAsync: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            }

            public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
            {
                _logger.LogInformation("CommandCreated: {CommandText}; ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    result.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode);
                return base.CommandCreated(eventData, result);
            }

            public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
            {
                _logger.LogError(eventData.Exception, "CommandFailed: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                base.CommandFailed(command, eventData);
            }

            public override Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default)
            {
                _logger.LogError(eventData.Exception, "CommandFailedAsync: {Message}; CommandText={CommandText}, ConnectionId={ConnectionId}, CommandId={CommandId}, EventId={EventId},EventIdCode= {EventIdCode}",
                    command.CommandText, eventData.ConnectionId, eventData.CommandId, eventData.EventId, eventData.EventIdCode, eventData.Exception.Message);
                return base.CommandFailedAsync(command, eventData, cancellationToken);
            }
        }

        public async Task ImportAsync(XDocument document)
        {
            if (document is null)
                throw new ArgumentNullException(nameof(document));

            var redundancySets = document.Root.Elements(nameof(BinaryProperties)).Select(e => Local.BinaryProperties.ImportAsync(this, _logger, e).Result).SelectMany(rs => rs).ToArray();
            foreach (XElement fileSystemElement in document.Root.Elements(nameof(FileSystem)))
                await FileSystem.ImportAsync(this, _logger, fileSystemElement);
            foreach (var (redundantSetId, redundancies) in redundancySets)
                foreach (XElement element in redundancies)
                    await Redundancy.ImportAsync(this, _logger, redundantSetId, element);
        }

        public void ForceDeleteBinaryProperties(BinaryProperties target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            EntityEntry<BinaryProperties> targetEntry = Entry(target);
            var redundantSets = (targetEntry.GetRelatedCollectionAsync(t => t.RedundantSets).Result).ToArray();
            if (redundantSets.Length > 0)
            {
                foreach (var r in redundantSets)
                    ForceDeleteRedundantSet(r);
                SaveChanges();
            }
            var files = target.Files.AsEnumerable().Select(f => Entry(f)).ToArray();
            if (files.Length > 0)
            {
                SummaryProperties[] summaryProperties = files.Select(e => e.Entity.SummaryPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.SummaryProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                DocumentProperties[] documentProperties = files.Select(e => e.Entity.DocumentPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.DocumentProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                AudioProperties[] audioProperties = files.Select(e => e.Entity.AudioPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.AudioProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                DRMProperties[] drmProperties = files.Select(e => e.Entity.DRMPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.DRMProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                GPSProperties[] gpsProperties = files.Select(e => e.Entity.GPSPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.GPSProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                ImageProperties[] imageProperties = files.Select(e => e.Entity.ImagePropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.ImageProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                MediaProperties[] mediaProperties = files.Select(e => e.Entity.MediaPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.MediaProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                MusicProperties[] musicProperties = files.Select(e => e.Entity.MusicPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.MusicProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                PhotoProperties[] photoProperties = files.Select(e => e.Entity.PhotoPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.PhotoProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                RecordedTVProperties[] recordedTVProperties = files.Select(e => e.Entity.RecordedTVPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.RecordedTVProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                VideoProperties[] videoProperties = files.Select(e => e.Entity.VideoPropertiesId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.VideoProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                FileComparison[] comparisons = files.SelectMany(e => e.GetRelatedCollectionAsync(f => f.ComparisonSources).Result
                    .Concat(e.GetRelatedCollectionAsync(f => f.ComparisonTargets).Result)).Distinct().ToArray();
                FileAccessError[] accessErrors = files.SelectMany(e => e.GetRelatedCollectionAsync(f => f.AccessErrors).Result).ToArray();
                bool hasChanges = comparisons.Length > 0;
                if (hasChanges)
                    Comparisons.RemoveRange(comparisons);
                if (accessErrors.Length > 0)
                {
                    FileAccessErrors.RemoveRange(accessErrors);
                    hasChanges = true;
                }
                if (hasChanges)
                    SaveChanges();
                Files.RemoveRange(files.Select(f => f.Entity));
                SaveChanges();
#pragma warning disable CA1827 // Do not use Count() or LongCount() when Any() can be used
                hasChanges = summaryProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0;
                if (documentProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (audioProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (drmProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (gpsProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (imageProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (mediaProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (musicProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (photoProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (recordedTVProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
                if (videoProperties.Count(p => RemoveIfNoReferencesAsync(p).Result) > 0)
                    hasChanges = true;
#pragma warning restore CA1827 // Do not use Count() or LongCount() when Any() can be used
                if (hasChanges)
                    SaveChanges();
            }
            BinaryProperties.Remove(target);
        }

        public void ForceDeleteRedundantSet(RedundantSet target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundancies = target.Redundancies.AsEnumerable().ToArray();
            if (redundancies.Length > 0)
            {
                Redundancies.RemoveRange(redundancies);
                SaveChanges();
            }
            RedundantSets.Remove(target);
        }

        public void ForceDeleteFileSystem(FileSystem target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var symbolicNames = target.SymbolicNames.AsEnumerable().ToArray();
            if (symbolicNames.Length > 0)
            {
                SymbolicNames.RemoveRange(symbolicNames);
                SaveChanges();
            }
            var volumes = target.Volumes.AsEnumerable().ToArray();
            if (volumes.Length > 0)
            {
                foreach (var v in volumes)
                    ForceDeleteVolume(v);
                SaveChanges();
            }
            FileSystems.Remove(target);
        }

        private void ForceDeleteVolume(Volume target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target.RootDirectory is not null)
            {
                ForceDeleteSubdirectory(target.RootDirectory);
                SaveChanges();
            }
            var accessErrors = target.AccessErrors.AsEnumerable().ToArray();
            if (accessErrors.Length > 0)
            {
                VolumeAccessErrors.RemoveRange(accessErrors);
                SaveChanges();
            }
            Volumes.Remove(target);
        }

        private void ForceDeleteSubdirectory(Subdirectory target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var subdirectories = target.SubDirectories.AsEnumerable().ToArray();
            if (subdirectories.Length > 0)
            {
                foreach (var s in subdirectories)
                    ForceDeleteSubdirectory(s);
                SaveChanges();
            }
            var files = target.Files.AsEnumerable().ToArray();
            if (files.Length > 0)
            {
                foreach (var f in files)
                    ForceDeleteFileAsync(f).Wait();
                SaveChanges();
            }
            var accessErrors = target.AccessErrors.AsEnumerable().ToArray();
            if (accessErrors.Length > 0)
            {
                SubdirectoryAccessErrors.RemoveRange(accessErrors);
                SaveChanges();
            }
            Subdirectories.Remove(target);
        }

        private async Task ForceDeleteFileAsync(DbFile target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target.Redundancy is not null)
            {
                ForceDeleteRedundancy(target.Redundancy);
                SaveChanges();
            }
            EntityEntry<DbFile> fileEntry = Entry(target);
            var comparisons = (await fileEntry.GetRelatedCollectionAsync(p => p.ComparisonSources)).ToArray();
            bool hasChanges = comparisons.Length > 0;
            if (hasChanges)
                Comparisons.RemoveRange(comparisons);

            var accessErrors = (await fileEntry.GetRelatedCollectionAsync(p => p.AccessErrors)).ToArray();
            if (accessErrors.Length > 0)
            {
                FileAccessErrors.RemoveRange(accessErrors);
                hasChanges = true;
            }
            if (hasChanges)
                await SaveChangesAsync();
            var content = await fileEntry.GetRelatedReferenceAsync(f => f.BinaryProperties);
            var summaryProperties = target.SummaryPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.SummaryProperties) : null;
            var documentProperties = target.DocumentPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DocumentProperties) : null;
            var audioProperties = target.AudioPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.AudioProperties) : null;
            var drmProperties = target.DRMPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DRMProperties) : null;
            var gpsProperties = target.GPSPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.GPSProperties) : null;
            var imageProperties = target.ImagePropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.ImageProperties) : null;
            var mediaProperties = target.MediaPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MediaProperties) : null;
            var musicProperties = target.MusicPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MusicProperties) : null;
            var photoProperties = target.PhotoPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.PhotoProperties) : null;
            var recordedTVProperties = target.RecordedTVPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.RecordedTVProperties) : null;
            var videoProperties = target.VideoPropertiesId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.VideoProperties) : null;
            Files.Remove(target);
            await SaveChangesAsync();

            hasChanges = await RemoveIfNoReferencesAsync(content);
            if (content.Files.Count == 0)
                BinaryProperties.Remove(content);
            if (summaryProperties is not null && await RemoveIfNoReferencesAsync(summaryProperties))
                hasChanges = true;
            if (documentProperties is not null && await RemoveIfNoReferencesAsync(documentProperties))
                hasChanges = true;
            if (audioProperties is not null && await RemoveIfNoReferencesAsync(audioProperties))
                hasChanges = true;
            if (drmProperties is not null && await RemoveIfNoReferencesAsync(drmProperties))
                hasChanges = true;
            if (gpsProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (imageProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (mediaProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (musicProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (photoProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (recordedTVProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (videoProperties is not null && await RemoveIfNoReferencesAsync(gpsProperties))
                hasChanges = true;
            if (hasChanges)
                await SaveChangesAsync();
        }

        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([NotNull] TEntity entity, [NotNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression);

        private async Task<bool> RemoveIfNoReferencesAsync(BinaryProperties binaryProperties)
        {
            EntityEntry<BinaryProperties> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets)).Any())
                return false;
            BinaryProperties.Remove(binaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(VideoProperties videoProperties)
        {
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files)).Any())
                return false;
            VideoProperties.Remove(videoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVProperties recordedTVProperties)
        {
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files)).Any())
                return false;
            RecordedTVProperties.Remove(recordedTVProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(PhotoProperties photoProperties)
        {
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files)).Any())
                return false;
            PhotoProperties.Remove(photoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MusicProperties musicProperties)
        {
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files)).Any())
                return false;
            MusicProperties.Remove(musicProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MediaProperties mediaProperties)
        {
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files)).Any())
                return false;
            MediaProperties.Remove(mediaProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(ImageProperties imageProperties)
        {
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files)).Any())
                return false;
            ImageProperties.Remove(imageProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(GPSProperties gpsProperties)
        {
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files)).Any())
                return false;
            GPSProperties.Remove(gpsProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DRMProperties drmProperties)
        {
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files)).Any())
                return false;
            DRMProperties.Remove(drmProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(AudioProperties audioProperties)
        {
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files)).Any())
                return false;
            AudioProperties.Remove(audioProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DocumentProperties documentProperties)
        {
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files)).Any())
                return false;
            DocumentProperties.Remove(documentProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(SummaryProperties summaryProperties)
        {
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files)).Any())
                return false;
            SummaryProperties.Remove(summaryProperties);
            return true;
        }

        private void ForceDeleteRedundancy(Redundancy target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundantSet = target.RedundantSet;
            Redundancies.Remove(target);
            SaveChanges();
            if (redundantSet.Redundancies.Count == 0)
                RedundantSets.Remove(redundantSet);
        }
    }
}
