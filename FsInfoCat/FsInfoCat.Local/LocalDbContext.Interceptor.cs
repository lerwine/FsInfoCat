using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Linq;
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

            var redundancySets = document.Root.Elements(nameof(ContentInfo)).Select(e => ContentInfo.ImportAsync(this, _logger, e).Result).SelectMany(rs => rs).ToArray();
            foreach (XElement fileSystemElement in document.Root.Elements(nameof(FileSystem)))
                await FileSystem.ImportAsync(this, _logger, fileSystemElement);
            foreach (var (redundantSetId, redundancies) in redundancySets)
                foreach (XElement element in redundancies)
                    await Redundancy.ImportAsync(this, _logger, redundantSetId, element);
        }

        public void ForceDeleteContentInfo(ContentInfo target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundantSets = target.RedundantSets.AsEnumerable().ToArray();
            if (redundantSets.Length > 0)
            {
                foreach (var r in redundantSets)
                    ForceDeleteRedundantSet(r);
                SaveChanges();
            }
            var files = target.Files.AsEnumerable().ToArray();
            if (files.Length > 0)
            {
                var extendedProperties = files.Select(f => f.ExtendedProperties).Distinct().ToArray();
                foreach (var f in files)
                {
                    var comparisons = f.ComparisonSources.AsEnumerable().Concat(f.ComparisonTargets.AsEnumerable()).ToArray();
                    if (comparisons.Length > 0)
                    {
                        Comparisons.RemoveRange(comparisons);
                        SaveChanges();
                    }
                    var accessErrors = f.AccessErrors.AsEnumerable().ToArray();
                    if (accessErrors.Length > 0)
                    {
                        FileAccessErrors.RemoveRange(accessErrors);
                        SaveChanges();
                    }
                    Files.Remove(f);
                }
                SaveChanges();
                extendedProperties = extendedProperties.Where(e => e.Files.Count == 0).ToArray();
                if (extendedProperties.Length > 0)
                {
                    ExtendedProperties.RemoveRange(extendedProperties);
                    SaveChanges();
                }
            }
            ContentInfos.Remove(target);
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
                    ForceDeleteFile(f);
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

        private void ForceDeleteFile(DbFile target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (target.Redundancy is not null)
            {
                ForceDeleteRedundancy(target.Redundancy);
                SaveChanges();
            }
            var comparisons = target.ComparisonSources.AsEnumerable().Concat(target.ComparisonTargets.AsEnumerable()).ToArray();
            if (comparisons.Length > 0)
            {
                Comparisons.RemoveRange(comparisons);
                SaveChanges();
            }

            var accessErrors = target.AccessErrors.AsEnumerable().ToArray();
            if (accessErrors.Length > 0)
            {
                FileAccessErrors.RemoveRange(accessErrors);
                SaveChanges();
            }
            var content = target.Content;
            var extendedProperties = target.ExtendedProperties;
            Files.Remove(target);
            SaveChanges();
            if (content.Files.Count == 0)
                ContentInfos.Remove(content);
            if (extendedProperties is not null && extendedProperties.Files.Count == 0)
                ExtendedProperties.Remove(extendedProperties);
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
