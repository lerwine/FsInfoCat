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

            var redundancySets = document.Root.Elements(nameof(BinaryPropertySets)).Select(e => Local.BinaryPropertySet.ImportAsync(this, _logger, e).Result).SelectMany(rs => rs).ToArray();
            foreach (XElement fileSystemElement in document.Root.Elements(nameof(FileSystem)))
                await FileSystem.ImportAsync(this, _logger, fileSystemElement);
            foreach (var (redundantSetId, redundancies) in redundancySets)
                foreach (XElement element in redundancies)
                    await Redundancy.ImportAsync(this, _logger, redundantSetId, element);
        }

        public void ForceDeleteBinaryProperties(BinaryPropertySet target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            EntityEntry<BinaryPropertySet> targetEntry = Entry(target);
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
                SummaryPropertySet[] summaryProperties = files.Select(e => e.Entity.SummaryPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.SummaryProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                DocumentPropertySet[] documentProperties = files.Select(e => e.Entity.DocumentPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.DocumentProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                AudioPropertySet[] audioProperties = files.Select(e => e.Entity.AudioPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.AudioProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                DRMPropertySet[] drmProperties = files.Select(e => e.Entity.DRMPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.DRMProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                GPSPropertySet[] gpsProperties = files.Select(e => e.Entity.GPSPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.GPSProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                ImagePropertySet[] imageProperties = files.Select(e => e.Entity.ImagePropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.ImageProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                MediaPropertySet[] mediaProperties = files.Select(e => e.Entity.MediaPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.MediaProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                MusicPropertySet[] musicProperties = files.Select(e => e.Entity.MusicPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.MusicProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                PhotoPropertySet[] photoProperties = files.Select(e => e.Entity.PhotoPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.PhotoProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                RecordedTVPropertySet[] recordedTVProperties = files.Select(e => e.Entity.RecordedTVPropertySetId.HasValue ?
                    e.GetRelatedReferenceAsync(f => f.RecordedTVProperties).Result : null).Where(p => p is not null).Distinct().ToArray();
                VideoPropertySet[] videoProperties = files.Select(e => e.Entity.VideoPropertySetId.HasValue ?
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
            BinaryPropertySets.Remove(target);
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
            var summaryProperties = target.SummaryPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.SummaryProperties) : null;
            var documentProperties = target.DocumentPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DocumentProperties) : null;
            var audioProperties = target.AudioPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.AudioProperties) : null;
            var drmProperties = target.DRMPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.DRMProperties) : null;
            var gpsProperties = target.GPSPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.GPSProperties) : null;
            var imageProperties = target.ImagePropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.ImageProperties) : null;
            var mediaProperties = target.MediaPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MediaProperties) : null;
            var musicProperties = target.MusicPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.MusicProperties) : null;
            var photoProperties = target.PhotoPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.PhotoProperties) : null;
            var recordedTVProperties = target.RecordedTVPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.RecordedTVProperties) : null;
            var videoProperties = target.VideoPropertySetId.HasValue ? await fileEntry.GetRelatedReferenceAsync(f => f.VideoProperties) : null;
            Files.Remove(target);
            await SaveChangesAsync();

            hasChanges = await RemoveIfNoReferencesAsync(content);
            if (content.Files.Count == 0)
                BinaryPropertySets.Remove(content);
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

        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties)
        {
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets)).Any())
                return false;
            BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties)
        {
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files)).Any())
                return false;
            VideoPropertySets.Remove(videoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties)
        {
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files)).Any())
                return false;
            RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties)
        {
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files)).Any())
                return false;
            PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties)
        {
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files)).Any())
                return false;
            MusicPropertySets.Remove(musicProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties)
        {
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files)).Any())
                return false;
            MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties)
        {
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files)).Any())
                return false;
            ImagePropertySets.Remove(imageProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties)
        {
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files)).Any())
                return false;
            GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties)
        {
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files)).Any())
                return false;
            DRMPropertySets.Remove(drmProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties)
        {
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files)).Any())
                return false;
            AudioPropertySets.Remove(audioProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties)
        {
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files)).Any())
                return false;
            DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties)
        {
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files)).Any())
                return false;
            SummaryPropertySets.Remove(summaryProperties);
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
