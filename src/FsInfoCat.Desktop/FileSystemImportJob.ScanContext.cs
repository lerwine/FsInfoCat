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
                foreach ((FileInfo FS, DbFile DB) item in newFiles.Join((await entityEntry.GetRelatedCollectionAsync(d => d.Files, CancellationToken)),
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
                    foreach ((DirectoryInfo FS, Subdirectory DB) item in newDirectories.Join((await entityEntry.GetRelatedCollectionAsync(d => d.SubDirectories, CancellationToken)),
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
                Subdirectory[] oldDirectories = (await entityEntry.GetRelatedCollectionAsync(d => d.SubDirectories, CancellationToken)).ToArray();
                foreach (FileInfo fileInfo in newFiles)
                {
                    if (CancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    string name = fileInfo.Name;
                    DbFile dbFile = oldFiles.FirstOrDefault(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (dbFile is null)
                    {
                        long length = fileInfo.Length;
                        BinaryPropertySet bps = dbContext.BinaryPropertySets.FirstOrDefault(p => p.Length == length && p.Hash == null);
                        if (bps is null)
                        {
                            bps = new BinaryPropertySet { Length = length };
                            dbContext.BinaryPropertySets.Add(bps);
                            await dbContext.SaveChangesAsync(CancellationToken);
                            if (CancellationToken.IsCancellationRequested)
                                throw new OperationCanceledException();
                        }
                        dbFile = new DbFile
                        {
                            Name = name,
                            BinaryPropertySetId = bps.Id,
                            BinaryProperties = bps,
                            ParentId = DbDirectoryItem.Id,
                            Parent = DbDirectoryItem
                        };
                    }
                    observer.RaiseFileImporting(this, fileInfo, dbFile);
                    EntityEntry<DbFile> fileEntry = dbContext.Entry(dbFile);
                    try
                    {
                        await SummaryPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await DocumentPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await AudioPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await DRMPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await GPSPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await ImagePropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await MediaPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await MusicPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await PhotoPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await RecordedTVPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        await VideoPropertySet.ApplyAsync(fileEntry, dbContext, CancellationToken);
                        switch (fileEntry.State)
                        {
                            case EntityState.Detached:
                                dbContext.Files.Add(dbFile);
                                await dbContext.SaveChangesAsync(CancellationToken);
                                if (CancellationToken.IsCancellationRequested)
                                    throw new OperationCanceledException();
                                break;
                            case EntityState.Modified:
                                await dbContext.SaveChangesAsync(CancellationToken);
                                if (CancellationToken.IsCancellationRequested)
                                    throw new OperationCanceledException();
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        observer.RaiseFileImportError(this, fileInfo, dbFile, error);
                        if (++TotalCount >= Job.MaxTotalItems)
                            break;
                        continue;
                    }
                    observer.RaiseFileImported(this, fileInfo, dbFile);
                    if (++TotalCount >= Job.MaxTotalItems)
                        break;
                }
                if (TotalCount < Job.MaxTotalItems)
                    foreach (DirectoryInfo directoryInfo in newDirectories)
                    {
                        if (CancellationToken.IsCancellationRequested)
                            throw new OperationCanceledException();
                        if (++TotalCount >= Job.MaxTotalItems)
                            break;
                        string name = directoryInfo.Name;
                        Subdirectory subdirectory = oldDirectories.FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.InvariantCultureIgnoreCase));
                        if (subdirectory is null)
                        {
                            subdirectory = new Subdirectory
                            {
                                Name = name,
                                ParentId = DbDirectoryItem.Id,
                                Parent = DbDirectoryItem,
                                Status = DirectoryStatus.Incomplete
                            };
                            dbContext.Subdirectories.Add(subdirectory);
                            await dbContext.SaveChangesAsync(CancellationToken);
                            if (CancellationToken.IsCancellationRequested)
                                throw new OperationCanceledException();
                        }
                        else
                        {
                            EntityEntry<Subdirectory> e = dbContext.Entry(subdirectory);
                            subdirectory.Status = DirectoryStatus.Incomplete;
                            e.DetectChanges();
                            if (e.State == EntityState.Modified)
                            {
                                await dbContext.SaveChangesAsync(CancellationToken);
                                if (CancellationToken.IsCancellationRequested)
                                    throw new OperationCanceledException();
                            }
                        }
                        ScanContext context = new((ushort)(Depth + 1), TotalCount, directoryInfo, subdirectory, CancellationToken);
                        await context.ScanAsync(observer, dbContext);
                        if (CancellationToken.IsCancellationRequested)
                            throw new OperationCanceledException();
                        if ((TotalCount = context.TotalCount) >= Job.MaxTotalItems)
                            break;
                    }
                foreach (DbFile dbFile in oldFiles)
                {
                    if (CancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    string name = dbFile.Name;
                    if (newFiles.Any(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                        continue;
                    EntityEntry<DbFile> e = dbContext.Entry(dbFile);
                    dbFile.Deleted = true;
                    e.DetectChanges();
                    if (e.State == EntityState.Modified)
                    {
                        dbContext.Files.Update(dbFile);
                        await dbContext.SaveChangesAsync(CancellationToken);
                    }
                }
                foreach (Subdirectory subdir in oldDirectories)
                {
                    if (CancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    string name = subdir.Name;
                    if (newDirectories.Any(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                        continue;
                    EntityEntry<Subdirectory> e = dbContext.Entry(subdir);
                    subdir.Status = DirectoryStatus.Deleted;
                    e.DetectChanges();
                    if (e.State == EntityState.Modified)
                    {
                        dbContext.Subdirectories.Update(subdir);
                        await dbContext.SaveChangesAsync(CancellationToken);
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
