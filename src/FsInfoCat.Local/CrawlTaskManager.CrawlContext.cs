using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public sealed partial class CrawlTaskManager
    {
        record NameSet(DirectoryInfo[] FsDirectories, FileInfo[] FsFiles, Subdirectory[] DbDirectories, DbFile[] DbFiles);

        public class CrawlContext
        {
            public CrawlTaskManager Worker { get; }

            public int Depth { get; }

            public DirectoryInfo FS { get; }

            public Subdirectory DB { get; }

            internal CrawlContext([DisallowNull] CrawlTaskManager worker, int depth, [DisallowNull] DirectoryInfo fs, [DisallowNull] Subdirectory db)
            {
                Worker = worker ?? throw new ArgumentNullException(nameof(worker));
                Depth = depth;
                FS = fs ?? throw new ArgumentNullException(nameof(fs));
                DB = db ?? throw new ArgumentNullException(nameof(db));
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
                    DirectoryInfo[] fsDirectories = GetDirectories(dbContext, crawlEventReceiver);
                    if (fsFiles is not null && fsDirectories is not null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        DB.LastAccessed = DateTime.Now;

                        // Get file system and db items and group by lower-case name - Lets this work on case-sensitive volumes.
                        var fileSystemInfos = fsFiles.Cast<FileSystemInfo>().Concat(fsDirectories.Cast<FileSystemInfo>()).GroupBy(f => f.Name.ToLower());
                        var dbFsItems = (await dbEntry.GetRelatedCollectionAsync(d => d.Files, cancellationToken)).Cast<ILocalDbFsItem>().Concat((await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken)).Cast<ILocalDbFsItem>()).GroupBy(f => f.Name.ToLower());
                        foreach (NameSet nameSet in fileSystemInfos.Join(dbFsItems, f => f.Key, d => d.Key, (fs, db) => new NameSet(fs.OfType<DirectoryInfo>().ToArray(), fs.OfType<FileInfo>().ToArray(), db.OfType<Subdirectory>().ToArray(), db.OfType<DbFile>().ToArray())))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            Worker._logger.LogTrace("Processing name set {{ FsDirectories: [{FsDirectories}], FsFiles: [{FsFiles}], DbDirectories: [{DbDirectories}], DbFiles: [{DbFiles}] }}", nameSet.FsDirectories.Select(d => d.Name).ToArray(),
                                nameSet.FsFiles.Select(d => d.Name).ToArray(), nameSet.DbDirectories.Select(d => d.Name).ToArray(), nameSet.DbFiles.Select(d => d.Name).ToArray());
                            IEnumerable<(FileInfo FS, DbFile DB)> fileTuples;
                            IEnumerable<(DirectoryInfo FS, Subdirectory DB)> directoryTuples;
                            if (nameSet.FsFiles.Length + nameSet.FsDirectories.Length > 1 || nameSet.DbFiles.Length + nameSet.DbDirectories.Length > 1)
                            {
                                fileTuples = nameSet.FsFiles.Join(nameSet.DbFiles, f => f.Name, d => d.Name, (fs, db) => (FS: fs, DB: db));
                                directoryTuples = nameSet.FsDirectories.Join(nameSet.DbDirectories, f => f.Name, f => f.Name, (fs, db) => (FS: fs, DB: db));
                            }
                            else
                            {
                                fileTuples = nameSet.FsFiles.Join(nameSet.DbFiles, f => f.Name.ToLower(), d => d.Name.ToLower(), (fs, db) => (FS: fs, DB: db));
                                directoryTuples = nameSet.FsDirectories.Join(nameSet.DbDirectories, f => f.Name.ToLower(), f => f.Name.ToLower(), (fs, db) => (FS: fs, DB: db));
                            }

                            foreach ((FileInfo FS, DbFile DB) item in fileTuples)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                (FileInfo FS, EntityEntry<DbFile> DB) pf;
                                // If name is different and file with db name does not exist, assume we're dealing with case-sensitive volume
                                if (item.FS is not null && item.DB is not null && item.FS.Name != DB.Name && !File.Exists(Path.Combine(FS.Parent.FullName, DB.Name)))
                                {
                                    crawlEventReceiver?.RaiseReadingFile(this, item.FS, null);
                                    pf = await ProcessFile(dbContext, item.FS, null, cancellationToken);
                                    crawlEventReceiver.RaiseFileReadComplete(this, pf.FS, pf.DB.Entity);
                                    if (++Worker.TotalItems >= Worker.MaxTotalItems)
                                        break;
                                    crawlEventReceiver?.RaiseReadingFile(this, null, item.DB);
                                    pf = await ProcessFile(dbContext, null, item.DB, cancellationToken);
                                }
                                else
                                {
                                    crawlEventReceiver?.RaiseReadingFile(this, item.FS, item.DB);
                                    pf = await ProcessFile(dbContext, item.FS, item.DB, cancellationToken);
                                }
                                crawlEventReceiver.RaiseFileReadComplete(this, pf.FS, pf.DB.Entity);
                                if (++Worker.TotalItems >= Worker.MaxTotalItems)
                                    break;
                            }
                            if (Worker.TotalItems >= Worker.MaxTotalItems)
                                break;
                            if (Depth < Worker.MaxRecursionDepth && Worker.TotalItems <= Worker.MaxTotalItems)
                            {
                                DB.LastAccessed = DateTime.Now;
                                foreach ((DirectoryInfo FS, Subdirectory DB) item in directoryTuples)
                                {
                                    cancellationToken.ThrowIfCancellationRequested();
                                    CrawlContext crawlContext;
                                    // If name is different and directory with db name does not exist, assume we're dealing with case-sensitive volume
                                    if (item.FS is not null && item.DB is not null && item.FS.Name != DB.Name && FS.Parent is not null && !Directory.Exists(Path.Combine(FS.Parent.FullName, DB.Name)))
                                    {
                                        crawlContext = await ProcessSubdirectory(dbContext, item.FS, null, cancellationToken);
                                        if (++Worker.TotalItems >= Worker.MaxTotalItems)
                                            break;
                                        if (crawlContext is not null)
                                            await crawlContext.CrawlAsync(dbContext, crawlEventReceiver, cancellationToken);
                                        crawlContext = await ProcessSubdirectory(dbContext, null, item.DB, cancellationToken);
                                    }
                                    else
                                        crawlContext = await ProcessSubdirectory(dbContext, item.FS, item.DB, cancellationToken);
                                    if (++Worker.TotalItems >= Worker.MaxTotalItems)
                                        break;
                                    if (crawlContext is not null)
                                        await crawlContext.CrawlAsync(dbContext, crawlEventReceiver, cancellationToken);
                                }
                                if (Worker.TotalItems >= Worker.MaxTotalItems)
                                    break;
                            }
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    if (DB.Status == DirectoryStatus.Incomplete && Worker.TotalItems <= Worker.MaxTotalItems)
                        DB.Status = DirectoryStatus.Complete;
                    Worker._logger.LogInformation("Saving changes to database");
                    _ = await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                    await DB.MarkBranchDeletedAsync(dbContext, cancellationToken);
                Worker._logger.LogInformation("Exiting {FullName}", FS.FullName);
                crawlEventReceiver?.RaiseExitSubdirectory(this);
            }

            private async Task<(FileInfo FS, EntityEntry<DbFile> DB)> ProcessFile(LocalDbContext dbContext, FileInfo fileInfo, DbFile dbFile, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (dbFile is null)
                {
                    Worker._logger.LogTrace("Adding file {FileName}", fileInfo.Name);
                    EntityEntry<DbFile> fileEntry = await DbFile.AddNewAsync(dbContext, DB.Id, fileInfo.Name, fileInfo.Length, fileInfo.CreationTime,
                        fileInfo.LastWriteTime, Worker._fileSystemDetailService.CreateFileDetailProvider(fileInfo.FullName, true), cancellationToken);
                    Worker._logger.LogTrace("Inserted file (Id={Id}; Name={Name})", fileEntry.Entity.Id, fileEntry.Entity.Name);
                    return (FS: fileInfo, DB: fileEntry);
                }
                if (fileInfo is null)
                {
                    Worker._logger.LogTrace("Marking file deleted (Id={Id}; Name={Name})", dbFile.Id, dbFile.Name);
                    await dbFile.SetStatusDeleted(dbContext, cancellationToken);
                    return (FS: fileInfo, DB: dbContext.Entry(dbFile));
                }
                Worker._logger.LogTrace("Updating file (Id={Id}; Name={Name})", dbFile.Id, dbFile.Name);
                return (FS: fileInfo, DB: await dbFile.RefreshAsync(dbContext, fileInfo.Length, fileInfo.CreationTime, fileInfo.LastWriteTime,
                    Worker._fileSystemDetailService.CreateFileDetailProvider(fileInfo.FullName, true), cancellationToken));
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
