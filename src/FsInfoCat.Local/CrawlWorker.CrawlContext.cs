using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlWorker
    {
        public class CrawlContext
        {
            public CrawlWorker Worker { get; }

            public int Depth { get; }

            public DirectoryInfo FS { get; }

            public Subdirectory DB { get; }

            internal CrawlContext([DisallowNull] CrawlWorker worker, int depth, [DisallowNull] DirectoryInfo fs, [DisallowNull] Subdirectory db)
            {
                Worker = worker ?? throw new ArgumentNullException(nameof(worker));
                Depth = depth;
                FS = fs ?? throw new ArgumentNullException(nameof(fs));
                DB = db ?? throw new ArgumentNullException(nameof(db));
            }

            internal async Task CrawlAsync([DisallowNull] LocalDbContext dbContext, [AllowNull] CrawlEventReceiver crawlEventReceiver, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Worker._logger.LogInformation("Entering {FullName}", FS.FullName);
                Worker.StatusMessage = $"Crawling {FS.FullName}";
                crawlEventReceiver?.RaiseEnterSubdirectory(this);
                EntityEntry<Subdirectory> dbEntry = dbContext.Entry(DB);
                if (FS.Exists)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    FileInfo[] fsFiles = GetFiles(dbContext, crawlEventReceiver);
                    cancellationToken.ThrowIfCancellationRequested();
                    DB.LastAccessed = DateTime.Now;

                    if (fsFiles is not null)
                    {
                        foreach ((FileInfo FS, DbFile DB) item in fsFiles.Join(await dbEntry.GetRelatedCollectionAsync(d => d.Files, cancellationToken),
                                f => f.Name.ToLower(), f => f.Name.ToLower(), (fs, db) => (FS: fs, DB: db)))
                        {
                            crawlEventReceiver?.RaiseReadingFile(this, item.FS, item.DB);
                            (FileInfo FS, EntityEntry<DbFile> DB) pf = await ProcessFile(dbContext, item.FS, item.DB, cancellationToken);
                            crawlEventReceiver.RaiseFileReadComplete(this, pf.FS, pf.DB.Entity);
                            if (++Worker.TotalItems >= Worker.MaxTotalItems)
                                break;
                        }
                    }

                    if (Depth < Worker.MaxRecursionDepth && Worker.TotalItems <= Worker.MaxTotalItems)
                    {
                        DirectoryInfo[] fsDirectories = GetDirectories(dbContext, crawlEventReceiver);
                        DB.LastAccessed = DateTime.Now;
                        if (fsDirectories is not null)
                        {
                            foreach (var item in fsDirectories.Join(await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken), f => f.Name.ToLower(), f => f.Name.ToLower(),
                                (fs, db) => (FS: fs, DB: db)))
                            {
                                CrawlContext crawlContext = await ProcessSubdirectory(dbContext, item.FS, item.DB, cancellationToken);
                                if (++Worker.TotalItems >= Worker.MaxTotalItems)
                                    break;
                                if (crawlContext is not null)
                                    await crawlContext.CrawlAsync(dbContext, crawlEventReceiver, cancellationToken);
                            }
                        }
                    }
                    if (DB.Status == DirectoryStatus.Incomplete && Worker.TotalItems <= Worker.MaxTotalItems)
                        DB.Status = DirectoryStatus.Complete;
                    Worker._logger.LogInformation("Saving changes to database");
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                    await DB.MarkBranchDeletedAsync(dbContext, cancellationToken);
                Worker._logger.LogInformation("Exiting {FullName}", FS.FullName);
                crawlEventReceiver?.RaiseExitSubdirectory(this);
            }

            private FileInfo[] GetFiles(LocalDbContext dbContext, CrawlEventReceiver crawlEventReceiver)
            {
                try { return FS.GetFiles(); }
                catch (UnauthorizedAccessException exception)
                {
                    Worker._logger.LogError(ErrorCode.UnauthorizedAccess.ToEventId(), exception, "Unauthorized Access while calling GetFiles on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.UnauthorizedAccess, exception, $"Access denied while getting file listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.UnauthorizedAccess, exception);
                }
                catch (SecurityException exception)
                {
                    Worker._logger.LogError(ErrorCode.SecurityException.ToEventId(), exception, "Security Exception while calling GetFiles on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.SecurityException, exception, $"Security violation while getting file listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.SecurityException, exception);
                }
                catch (PathTooLongException exception)
                {
                    Worker._logger.LogError(ErrorCode.PathTooLong.ToEventId(), exception, "PathTooLongException while calling GetFiles on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.PathTooLong, exception, $"Path too long while getting file listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.PathTooLong, exception);
                }
                catch (IOException exception)
                {
                    Worker._logger.LogError(ErrorCode.IOError.ToEventId(), exception, "IO Exception while calling GetFiles on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.IOError, exception, $"I/O error while getting file listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.IOError, exception);
                }
                catch (Exception exception)
                {
                    Worker._logger.LogError(ErrorCode.Unexpected.ToEventId(), exception, "{ExceptionType} while calling GetFiles on {Path}", exception.GetType().FullName, FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.Unspecified, exception, $"Unexpected error while getting file listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.Unspecified, exception);
                }

                return null;
            }

            private DirectoryInfo[] GetDirectories(LocalDbContext dbContext, CrawlEventReceiver crawlEventReceiver)
            {
                if (Depth >= Worker.MaxRecursionDepth)
                    return null;
                try { return FS.GetDirectories(); }
                catch (UnauthorizedAccessException exception)
                {
                    Worker._logger.LogError(ErrorCode.UnauthorizedAccess.ToEventId(), exception, "Unauthorized Access while calling GetDirectories on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.UnauthorizedAccess, exception, $"Access denied while getting sub-directory listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.UnauthorizedAccess, exception);
                }
                catch (SecurityException exception)
                {
                    Worker._logger.LogError(ErrorCode.SecurityException.ToEventId(), exception, "Security Exception while calling GetDirectories on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.SecurityException, exception, $"Security violation while getting sub-directory listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.SecurityException, exception);
                }
                catch (PathTooLongException exception)
                {
                    Worker._logger.LogError(ErrorCode.PathTooLong.ToEventId(), exception, "PathTooLongException while calling GetDirectories on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.PathTooLong, exception, $"Path too long while getting sub-directory listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.PathTooLong, exception);
                }
                catch (IOException exception)
                {
                    Worker._logger.LogError(ErrorCode.IOError.ToEventId(), exception, "IO Exception while calling GetDirectories on {Path}", FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.IOError, exception, $"I/O error while getting sub-directory listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.IOError, exception);
                }
                catch (Exception exception)
                {
                    Worker._logger.LogError(ErrorCode.Unexpected.ToEventId(), exception, "{ExceptionType} while calling GetDirectories on {Path}", exception.GetType().FullName, FS.FullName);
                    DB.SetError(dbContext, AccessErrorCode.Unspecified, exception, $"Unexpected error while getting sub-directory listing for {FS.FullName}");
                    crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.Unspecified, exception);
                }
                return null;
            }

            private async Task<(FileInfo FS, EntityEntry<DbFile> DB)> ProcessFile(LocalDbContext dbContext, FileInfo fileInfo, DbFile dbFile, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (dbFile is null)
                {
                    Worker._logger.LogTrace("Adding file {FileName}", fileInfo.Name);
                    EntityEntry<DbFile> fileEntry = await DbFile.AddNewAsync(dbContext, DB.Id, fileInfo.Name, fileInfo.Length, fileInfo.CreationTime,
                        fileInfo.LastWriteTime, Worker._fileSystemDetailService.CreateFileDetailProvider(fileInfo.FullName, true), true, cancellationToken);
                    Worker._logger.LogTrace("Inserted file (Id={Id}; Name={Name})", fileEntry.Entity.Id, fileEntry.Entity.Name);
                    return (FS: fileInfo, DB: fileEntry);
                }
                if (fileInfo is null)
                {
                    Worker._logger.LogTrace("Marking file deleted (Id={Id}; Name={Name})", dbFile.Id, dbFile.Name);
                    await dbFile.MarkDeletedAsync(dbContext, cancellationToken);
                    return (FS: fileInfo, DB: dbContext.Entry(dbFile));
                }
                Worker._logger.LogTrace("Updating file (Id={Id}; Name={Name})", dbFile.Id, dbFile.Name);
                if (!dbFile.Options.HasFlag(FileCrawlOptions.DoNotCompare))
                    await dbFile.MarkDissociatedAsync(dbContext, cancellationToken);
                return (FS: fileInfo, DB: await dbFile.RefreshAsync(dbContext, fileInfo.Length, fileInfo.CreationTime, fileInfo.LastWriteTime,
                    Worker._fileSystemDetailService.CreateFileDetailProvider(fileInfo.FullName, true), true, cancellationToken));
            }

            private async Task<CrawlContext> ProcessSubdirectory(LocalDbContext dbContext, DirectoryInfo directoryInfo, Subdirectory subdirectory, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (subdirectory is null)
                {
                    Worker._logger.LogTrace("Adding subdirectory {FileName}", FS.Name);
                    // TODO: Add new Subdirectory
                }
                else if (directoryInfo is null)
                {
                    Worker._logger.LogTrace("Marking subdirectory deleted (Id={Id}; Name={Name})", subdirectory.Id, subdirectory.Name);
                    await subdirectory.MarkBranchDeletedAsync(dbContext, cancellationToken);
                    return null;
                }
                else
                {
                    Worker._logger.LogTrace("Updating subdirectory (Id={Id}; Name={Name})", subdirectory.Id, subdirectory.Name);
                    // TODO: Update item
                }
                return new CrawlContext(Worker, Depth + 1, directoryInfo, subdirectory);
            }
        }
    }
}
