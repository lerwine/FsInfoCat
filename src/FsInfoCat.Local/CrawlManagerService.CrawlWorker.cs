using FsInfoCat.Local.Crawling;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlManagerService
    {
        internal class CrawlWorker : ILocalCrawlConfiguration
        {
            private readonly CancellationToken _cancellationToken;
            private readonly CrawlJob _crawlJob;
            private bool? _isTimedOut;

            internal ICurrentItem CurrentItem { get; private set; }

            internal string Message { get; private set; }

            internal StatusMessageLevel MessageLevel { get; private set; }

            internal TimeSpan Duration { get; private set; }

            internal ulong ItemsProcessed { get; private set; }

            public Guid RootId { get; }

            public string DisplayName => _crawlJob.CrawlConfiguration.DisplayName;

            public ushort MaxRecursionDepth { get; }

            public ulong? MaxTotalItems { get; }

            public long? TTL { get; }

            public Guid Id => _crawlJob.CrawlConfiguration.Id;

            ISubdirectory ICrawlConfiguration.Root => _crawlJob.CrawlConfiguration.Root;

            IEnumerable<ICrawlJobLog> ICrawlConfiguration.Logs => _crawlJob.CrawlConfiguration.Logs;

            ILocalSubdirectory ILocalCrawlConfiguration.Root => _crawlJob.CrawlConfiguration.Root;

            IEnumerable<ILocalCrawlJobLog> ILocalCrawlConfiguration.Logs => _crawlJob.CrawlConfiguration.Logs;

            Guid? ILocalDbEntity.UpstreamId => _crawlJob.CrawlConfiguration.UpstreamId;

            DateTime? ILocalDbEntity.LastSynchronizedOn => _crawlJob.CrawlConfiguration.LastSynchronizedOn;

            string ICrawlConfigurationRow.Notes => _crawlJob.CrawlConfiguration.Notes;

            CrawlStatus ICrawlConfigurationRow.StatusValue => _crawlJob.CrawlConfiguration.StatusValue;

            DateTime? ICrawlConfigurationRow.LastCrawlStart => _crawlJob.CrawlConfiguration.LastCrawlStart;

            DateTime? ICrawlConfigurationRow.LastCrawlEnd => _crawlJob.CrawlConfiguration.LastCrawlEnd;

            DateTime? ICrawlConfigurationRow.NextScheduledStart => _crawlJob.CrawlConfiguration.NextScheduledStart;

            long? ICrawlConfigurationRow.RescheduleInterval => _crawlJob.CrawlConfiguration.RescheduleInterval;

            bool ICrawlConfigurationRow.RescheduleFromJobEnd => _crawlJob.CrawlConfiguration.RescheduleFromJobEnd;

            bool ICrawlConfigurationRow.RescheduleAfterFail => _crawlJob.CrawlConfiguration.RescheduleAfterFail;

            DateTime IDbEntity.CreatedOn { get => _crawlJob.CrawlConfiguration.CreatedOn; set => throw new NotImplementedException(); }

            DateTime IDbEntity.ModifiedOn { get => _crawlJob.CrawlConfiguration.ModifiedOn; set => throw new NotImplementedException(); }

            bool IChangeTracking.IsChanged => _crawlJob.CrawlConfiguration.IsChanged;

            public DateTime? StopAt { get; private set; }

            internal CrawlWorker(CrawlJob crawlJob, DateTime? stopAt, CancellationToken cancellationToken)
            {
                _crawlJob = crawlJob ?? throw new ArgumentNullException(nameof(crawlJob));
                MaxRecursionDepth = _crawlJob.CrawlConfiguration.MaxRecursionDepth;
                MaxTotalItems = _crawlJob.CrawlConfiguration.MaxTotalItems;
                TTL = _crawlJob.CrawlConfiguration.TTL;
                StopAt = stopAt;
                _cancellationToken = cancellationToken;
                RootId = _crawlJob.CrawlConfiguration.RootId;
            }

            private async Task CrawlAsync(LocalDbContext dbContext, Context parent, bool isNew)
            {
                if (ItemsProcessed >= MaxTotalItems.Value && !_isTimedOut.HasValue)
                {
                    _crawlJob.Cancel(true);
                    _isTimedOut = false;
                }
                _cancellationToken.ThrowIfCancellationRequested();
                _crawlJob.Service.RaiseDirectoryCrawling(new DirectoryCrawlEventArgs(this, parent));
                EntityEntry<Subdirectory> entry = dbContext.Entry(parent.Subdirectory);
                List<Context> subdirectories, files;
                using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(_cancellationToken))
                {
                    if (!isNew)
                    {
                        await entry.RemoveRelatedEntitiesAsync(d => d.AccessErrors, dbContext.SubdirectoryAccessErrors, _cancellationToken);
                        parent.Subdirectory.Status = DirectoryStatus.Incomplete;
                    }
                    try
                    {
                        if (isNew)
                        {
                            files = await parent.GetFilesAsync(dbContext, _cancellationToken);
                            if (parent.Depth > MaxRecursionDepth)
                                subdirectories = null;
                            else if (parent.Subdirectory.Options.HasFlag(DirectoryCrawlOptions.SkipSubdirectories))
                                subdirectories = new();
                            else
                                subdirectories = await parent.GetDirectoriesAsync(dbContext, _cancellationToken);
                        }
                        else
                        {
                            _cancellationToken.ThrowIfCancellationRequested();
                            files = parent.GetFiles().ToList();
                            if (parent.Depth > MaxRecursionDepth)
                                subdirectories = null;
                            else if (parent.Subdirectory.Options.HasFlag(DirectoryCrawlOptions.SkipSubdirectories))
                                subdirectories = new();
                            else
                            {
                                _cancellationToken.ThrowIfCancellationRequested();
                                subdirectories = parent.GetDirectories(dbContext).ToList();
                            }
                        }
                        parent.Subdirectory.LastAccessed = parent.Subdirectory.ModifiedOn = DateTime.Now;
                    }
                    catch (System.Security.SecurityException secExc)
                    {
                        SubdirectoryAccessError subdirectoryAccessError = new()
                        {
                            Message = secExc.Message,
                            Details = secExc.ToString(),
                            ErrorCode = AccessErrorCode.SecurityException,
                            Target = parent.Subdirectory
                        };
                        dbContext.SubdirectoryAccessErrors.Add(subdirectoryAccessError);
                        await dbContext.SaveChangesAsync(_cancellationToken);
                        try { await transaction.CommitAsync(_cancellationToken); }
                        finally { _crawlJob.Service.RaiseDirectoryCrawled(new DirectoryCrawlEventArgs(this, parent, subdirectoryAccessError)); }
                        return;
                    }
                    catch (UnauthorizedAccessException uaExc)
                    {
                        SubdirectoryAccessError subdirectoryAccessError = new()
                        {
                            Message = uaExc.Message,
                            Details = uaExc.ToString(),
                            ErrorCode = AccessErrorCode.UnauthorizedAccess,
                            Target = parent.Subdirectory
                        };
                        dbContext.SubdirectoryAccessErrors.Add(subdirectoryAccessError);
                        await dbContext.SaveChangesAsync(_cancellationToken);
                        try { await transaction.CommitAsync(_cancellationToken); }
                        finally { _crawlJob.Service.RaiseDirectoryCrawled(new DirectoryCrawlEventArgs(this, parent, subdirectoryAccessError)); }
                        return;
                    }
                    catch (PathTooLongException pExc)
                    {
                        SubdirectoryAccessError subdirectoryAccessError = new()
                        {
                            Message = pExc.Message,
                            Details = pExc.ToString(),
                            ErrorCode = AccessErrorCode.PathTooLong,
                            Target = parent.Subdirectory
                        };
                        dbContext.SubdirectoryAccessErrors.Add(subdirectoryAccessError);
                        await dbContext.SaveChangesAsync(_cancellationToken);
                        try { await transaction.CommitAsync(_cancellationToken); }
                        finally { _crawlJob.Service.RaiseDirectoryCrawled(new DirectoryCrawlEventArgs(this, parent, subdirectoryAccessError)); }
                        return;
                    }
                    catch (IOException ioExc)
                    {
                        SubdirectoryAccessError subdirectoryAccessError = new()
                        {
                            Message = ioExc.Message,
                            Details = ioExc.ToString(),
                            ErrorCode = AccessErrorCode.IOError,
                            Target = parent.Subdirectory
                        };
                        dbContext.SubdirectoryAccessErrors.Add(subdirectoryAccessError);
                        await dbContext.SaveChangesAsync(_cancellationToken);
                        try { await transaction.CommitAsync(_cancellationToken); }
                        finally { _crawlJob.Service.RaiseDirectoryCrawled(new DirectoryCrawlEventArgs(this, parent, subdirectoryAccessError)); }
                        return;
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception exc)
                    {
                        SubdirectoryAccessError subdirectoryAccessError = new()
                        {
                            Message = exc.Message,
                            Details = exc.ToString(),
                            ErrorCode = AccessErrorCode.Unspecified,
                            Target = parent.Subdirectory
                        };
                        dbContext.SubdirectoryAccessErrors.Add(subdirectoryAccessError);
                        await dbContext.SaveChangesAsync(_cancellationToken);
                        try { await transaction.CommitAsync(_cancellationToken); }
                        finally { _crawlJob.Service.RaiseDirectoryCrawled(new DirectoryCrawlEventArgs(this, parent, subdirectoryAccessError)); }
                        return;
                    }
                    await dbContext.SaveChangesAsync(_cancellationToken);
                    await transaction.CommitAsync(_cancellationToken);
                }

                List<DbFile> deletedFiles = new();
                for (int i = 0; i < files.Count; i++)
                {
                    Context fc = files[i];
                    if (fc.FileInfo is null)
                    {
                        DbFile dbFile = fc.DbFile;
                        if (dbFile.Status != FileCorrelationStatus.Deleted)
                            deletedFiles.Add(dbFile);
                        files.RemoveAt(i--);
                    }
                }
                foreach (Context fc in files)
                {
                    DbFile dbFile = fc.DbFile;
                    FileInfo fileInfo = fc.FileInfo;
                    if (dbFile is null)
                    {
                        // TODO: Add new file
                    }
                    else if (fileInfo is null)
                    {
                        // TODO: Delete file
                    }
                    else
                    {
                        // TODO: Update file
                    }
                }
                if (deletedFiles.Count > 0)
                {
                    foreach (DbFile dbFile in deletedFiles)
                    {
                        dbFile.Status = FileCorrelationStatus.Deleted;
                        dbFile.LastAccessed = dbFile.ModifiedOn = DateTime.Now;
                    }
                    await dbContext.SaveChangesAsync();
                }
                List<Context> newSubdirectories = new();
                List<Subdirectory> deletedSubdirectories = new();
                for (int i = 0; i < subdirectories.Count; i++)
                {
                    Context dc = subdirectories[i];
                    Subdirectory subdirectory = dc.Subdirectory;
                    DirectoryInfo directoryInfo = dc.DirectoryInfo;
                    if (subdirectory is null)
                    {
                        newSubdirectories.Add(dc with
                        {
                            Subdirectory = subdirectory = new()
                            {
                                CreationTime = directoryInfo.CreationTime,
                                LastWriteTime = directoryInfo.LastWriteTime,
                                Name = directoryInfo.Name,
                                Parent = parent.Subdirectory
                            }
                        });
                        subdirectories.RemoveAt(i--);
                    }
                    else if (directoryInfo is null)
                    {
                        if (subdirectory.Status != DirectoryStatus.Deleted)
                            deletedSubdirectories.Add(subdirectory);
                        subdirectories.RemoveAt(i--);
                    }
                    else if (subdirectory.Options.HasFlag(DirectoryCrawlOptions.Skip))
                        subdirectories.RemoveAt(i--);
                }
                using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(_cancellationToken))
                {
                    if (deletedSubdirectories.Count > 0)
                    {
                        foreach (Subdirectory subdirectory in deletedSubdirectories)
                            await Subdirectory.DeleteAsync(subdirectory, dbContext, _cancellationToken);
                        await dbContext.SaveChangesAsync(_cancellationToken);
                    }
                    if (newSubdirectories.Count > 0)
                    {
                        foreach (Context context in newSubdirectories)
                            dbContext.Subdirectories.Add(context.Subdirectory);
                    }
                    else if (subdirectories.Count == 0)
                        return;
                    foreach (Context context in subdirectories)
                    {
                        Subdirectory subdirectory = context.Subdirectory;
                        DirectoryInfo directoryInfo = context.DirectoryInfo;
                        subdirectory.ModifiedOn = subdirectory.LastAccessed = DateTime.Now;
                        subdirectory.Name = directoryInfo.Name;
                        subdirectory.CreationTime = directoryInfo.CreationTime;
                        subdirectory.LastWriteTime = directoryInfo.LastWriteTime;
                        subdirectory.Status = DirectoryStatus.Incomplete;
                    }
                    await dbContext.SaveChangesAsync(_cancellationToken);
                    await transaction.CommitAsync(_cancellationToken);
                }
                foreach (Context context in subdirectories)
                    await CrawlAsync(dbContext, context, false);
                foreach (Context context in newSubdirectories)
                    await CrawlAsync(dbContext, context, true);
                _crawlJob.Service.RaiseDirectoryCrawled(new DirectoryCrawlEventArgs(this, parent));
            }

            private async Task RunAsync(IServiceScope serviceScope)
            {
                _cancellationToken.ThrowIfCancellationRequested();
                using LocalDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                Subdirectory subdirectory = await dbContext.Subdirectories.FindAsync(new object[] { RootId }, _cancellationToken);
                if (subdirectory is null)
                    throw new Exception("Could not find crawl root subdirectory record");
                await CrawlAsync(dbContext, await Context.CreateAsync(dbContext, subdirectory, _crawlJob.Service._fileSystemDetailService, _cancellationToken), false);
            }

            internal async Task RunAsync()
            {
                TimeSpan? ttl = TTL.HasValue ? TimeSpan.FromSeconds(TTL.Value) : null;
                if (StopAt.HasValue)
                {
                    TimeSpan t = StopAt.Value.Subtract(DateTime.Now);
                    if (!(ttl.HasValue && ttl.Value < t))
                        ttl = t;
                }
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                if (ttl.HasValue)
                {
                    if (ttl.Value > TimeSpan.Zero)
                    {
                        using Timer timer = new(o =>
                        {
                            if (!_isTimedOut.HasValue)
                                _isTimedOut = true;
                            _crawlJob.Cancel(true);
                        }, null, ttl.Value, Timeout.InfiniteTimeSpan);
                        await RunAsync(serviceScope);
                        return;
                    }
                    _isTimedOut = true;
                    _crawlJob.Cancel(true);
                }
                await RunAsync(serviceScope);
            }

            IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext) => _crawlJob.CrawlConfiguration.Validate(validationContext);

            void IRevertibleChangeTracking.RejectChanges() => throw new NotSupportedException();

            void IChangeTracking.AcceptChanges() => throw new NotSupportedException();
        }
    }
}
