using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                Worker.StatusMessage = $"Crawling {FS.FullName}";
                crawlEventReceiver?.RaiseEnterSubdirectory(this);
                EntityEntry<Subdirectory> dbEntry = dbContext.Entry(DB);
                if (FS.Exists)
                {
                    DB.LastAccessed = DateTime.Now;
                    FileInfo[] fsFiles = null;
                    try { fsFiles = FS.GetFiles(); }
                    catch (UnauthorizedAccessException exception)
                    {
                        DB.SetUnauthorizedAccessError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.UnauthorizedAccess, exception);
                    }
                    catch (SecurityException exception)
                    {
                        DB.SetSecurityError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.SecurityException, exception);
                    }
                    catch (PathTooLongException exception)
                    {
                        DB.SetPathTooLongError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.PathTooLong, exception);
                    }
                    catch (IOException exception)
                    {
                        DB.SetIOError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.IOError, exception);
                    }
                    catch (Exception exception)
                    {
                        DB.SetUnspecifiedError(dbContext, exception);
                        crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.Unspecified, exception);
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                    DirectoryInfo[] fsDirectories = null;
                    if (Depth < Worker.MaxRecursionDepth)
                    {
                        try
                        {
                            DB.LastAccessed = DateTime.Now;
                            fsDirectories = FS.GetDirectories();
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.Status = DirectoryStatus.Complete;
                        }
                        catch (UnauthorizedAccessException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetUnauthorizedAccessError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.UnauthorizedAccess, exception);
                        }
                        catch (SecurityException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetSecurityError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.SecurityException, exception);
                        }
                        catch (PathTooLongException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetPathTooLongError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.PathTooLong, exception);
                        }
                        catch (IOException exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetIOError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.IOError, exception);
                        }
                        catch (Exception exception)
                        {
                            if (DB.Status == DirectoryStatus.Incomplete)
                                DB.SetUnspecifiedError(dbContext, exception);
                            crawlEventReceiver?.RaiseSubdirectoryAccessError(this, AccessErrorCode.Unspecified, exception);
                        }
                    }

                    if (fsFiles is not null)
                    {
                        foreach ((FileInfo FS, DbFile DB) item in fsFiles.Join(await dbEntry.GetRelatedCollectionAsync(d => d.Files, cancellationToken), f => f.Name.ToLower(), f => f.Name.ToLower(),
                            (fs, db) => (FS: fs, DB: db)))
                        {
                            if (item.DB is null)
                            {
                                // TODO: Add new FsFile
                            }
                            else if (item.FS is null)
                                await item.DB.MarkDeletedAsync(dbContext, cancellationToken);
                            else
                            {
                                // TODO: Update item
                            }
                        }
                    }

                    if (Depth >= Worker.MaxRecursionDepth)
                    {
                        if (DB.Status == DirectoryStatus.Incomplete && fsFiles is not null)
                        {
                            DB.Status = DirectoryStatus.Complete;
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    else if (fsDirectories is not null)
                    {
                        foreach (var item in fsDirectories.Join(await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken), f => f.Name.ToLower(), f => f.Name.ToLower(),
                            (fs, db) => (FS: fs, DB: db)))
                        {
                            if (item.DB is null)
                            {
                                // TODO: Add new Subdirectory
                            }
                            else if (item.FS is null)
                            {
                                await item.DB.MarkBranchDeletedAsync(dbContext, cancellationToken);
                                continue;
                            }
                            else
                            {
                                // TODO: Update item
                            }
                            CrawlContext context = new(Worker, Depth + 1, item.FS, item.DB);
                            await context.CrawlAsync(dbContext, crawlEventReceiver, cancellationToken);
                        }
                        if (DB.Status == DirectoryStatus.Incomplete && fsFiles is not null)
                        {
                            DB.Status = DirectoryStatus.Complete;
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
                else
                    await DB.MarkBranchDeletedAsync(dbContext, cancellationToken);
            }
        }
    }
}
