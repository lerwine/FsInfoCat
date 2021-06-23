using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public partial class FileSystemImportJob
    {
        [Obsolete("Use FsInfoCat.Local.CrawlWorker, instead")]
        public partial class ScanContext
        {
            internal ushort Depth { get; }

            internal Subdirectory DbDirectoryItem { get; }

            internal DirectoryInfo FsDirectoryInfo { get; }

            internal CancellationToken CancellationToken { get; }

            internal ulong TotalCount { get; private set; }

            internal FileSystemImportJob Job { get; }

            internal static async Task RunAsync([NotNull] CrawlConfiguration configuration, [AllowNull] FileSystemImportObserver observer, CancellationToken cancellationToken)
            {
                using LocalDbContext dbContext = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
                Subdirectory subdirectory = await dbContext.Entry(configuration).GetRelatedReferenceAsync(c => c.Root, cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                if (subdirectory is null)
                    throw new InvalidOperationException($"Unexpected error: {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)} was null.");

                string fullName = await Subdirectory.LookupFullNameAsync(subdirectory, dbContext);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                if (string.IsNullOrEmpty(fullName))
                    throw new InvalidOperationException($"Unexpected error: Could not build full path for {nameof(CrawlConfiguration)}.{nameof(CrawlConfiguration.Root)}.");
                ScanContext context = new(0, 0UL, new DirectoryInfo(fullName), subdirectory, cancellationToken);
                await context.ScanAsync(observer, dbContext);
            }

            private async Task ScanAsync(FileSystemImportObserver observer, LocalDbContext dbContext)
            {
                if (CancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                observer.RaiseDirectoryImporting(this);
                FileInfo[] newFiles;
                DirectoryInfo[] newDirectories;
                try
                {
                    newFiles = FsDirectoryInfo.GetFiles();
                    newDirectories = (Depth < Job.MaxRecursionDepth) ? FsDirectoryInfo.GetDirectories() : null;
                }
                catch (Exception exception)
                {
                    EntityEntry<Subdirectory> e = dbContext.Entry(DbDirectoryItem);
                    DbDirectoryItem.Status = DirectoryStatus.AccessError;
                    SubdirectoryAccessError accessError = new()
                    {
                        TargetId = DbDirectoryItem.Id,
                        Target = DbDirectoryItem,
                        ErrorCode = AccessErrorCode.Unspecified,
                        Message = exception.Message,
                        Details = exception.ToString()
                    };
                    dbContext.SubdirectoryAccessErrors.Add(accessError);
                    DbDirectoryItem.AccessErrors.Add(accessError);
                    observer.RaiseDirectoryImportError(this, exception);
                    await dbContext.SaveChangesAsync(CancellationToken);
                    return;
                }

                EntityEntry<Subdirectory> entityEntry = dbContext.Entry(DbDirectoryItem);
                foreach ((FileInfo FS, DbFile DB) item in newFiles.Join(await entityEntry.GetRelatedCollectionAsync(d => d.Files, CancellationToken),
                    f => f.Name, f => f.Name, (fileInfo, dbFile) => (FS: fileInfo, DB: dbFile)))
                {
                    if (item.DB is null)
                    {
                        long length = item.FS.Length;
                        DbFile file = await DbFile.AddNewAsync(dbContext, DbDirectoryItem.Id, item.FS.Name, item.FS.Length, item.FS.CreationTime, item.FS.LastWriteTime,
                            CancellationToken);
                        
                        // TODO: Add other property sets
                    }
                    else if (item.FS is null)
                    {
                        item.DB.SetDeleted(dbContext);
                        // Mark item deleted
                    }
                    else
                    {
                        // Update item
                    }
                }
                if (newDirectories is not null)
                    foreach ((DirectoryInfo FS, Subdirectory DB) item in newDirectories.Join(await entityEntry.GetRelatedCollectionAsync(d => d.SubDirectories, CancellationToken),
                        f => f.Name, f => f.Name, (directoryInfo, subdirectory) => (FS: directoryInfo, DB: subdirectory)))
                    {
                        if (item.DB is null)
                        {
                            // TODO: Add new DB item
                        }
                        else if (item.FS is null)
                        {
                            // Mark item deleted
                        }
                        else
                        {
                            // Update item
                        }
                    }
                observer.RaiseDirectoryImported(this);
            }
            private ScanContext(ushort depth, ulong totalCount, DirectoryInfo directoryInfo, Subdirectory subdirectory, CancellationToken cancellationToken)
            {
                Depth = depth;
                TotalCount = totalCount;
                DbDirectoryItem = subdirectory;
                FsDirectoryInfo = directoryInfo;
                CancellationToken = cancellationToken;
            }
        }
    }
}
