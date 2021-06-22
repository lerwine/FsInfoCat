using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public partial class FileSystemImportJob
    {
        public class ScanContext
        {
            internal ushort Depth { get; }
            internal Subdirectory DbDirectoryItem { get; }
            internal DirectoryInfo FsDirectoryInfo { get; }
            internal CancellationToken CancellationToken { get; }
            internal ulong TotalCount { get; private set; }
            internal FileSystemImportJob Job { get; }

            internal static async Task Create(CrawlConfiguration configuration, FileSystemImportObserver observer, CancellationToken cancellationToken)
            {
                using LocalDbContext dbContext = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
                ReferenceEntry<CrawlConfiguration, Subdirectory> rootEntry = dbContext.Entry(configuration).Reference(c => c.Root);
                if (!rootEntry.IsLoaded)
                    await rootEntry.LoadAsync(cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                Subdirectory subdirectory = rootEntry.CurrentValue;
                if (subdirectory is null)
                    throw new Exception("Could not get root directory"); // TODO: Throw proper error

                string fullName = await Subdirectory.LookupFullNameAsync(subdirectory, dbContext);
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                if (string.IsNullOrEmpty(fullName))
                    throw new Exception("Could not determine full name"); // TODO: Throw proper error
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
                    newDirectories = (Depth < Job.MaxRecursionDepth) ? FsDirectoryInfo.GetDirectories() : Array.Empty<DirectoryInfo>();
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
                if (CancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
                EntityEntry<Subdirectory> entityEntry = dbContext.Entry(DbDirectoryItem);
                CollectionEntry<Subdirectory, DbFile> filesEntry = entityEntry.Collection(d => d.Files);
                if (!filesEntry.IsLoaded)
                {
                    await filesEntry.LoadAsync(CancellationToken);
                    if (CancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                }
                DbFile[] oldFiles = filesEntry.CurrentValue.ToArray();
                CollectionEntry<Subdirectory, Subdirectory> subdirectoriesEntry = entityEntry.Collection(d => d.SubDirectories);
                if (!subdirectoriesEntry.IsLoaded)
                {
                    await subdirectoriesEntry.LoadAsync(CancellationToken);
                    if (CancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                }
                Subdirectory[] oldDirectories = subdirectoriesEntry.CurrentValue.ToArray();
                foreach (FileInfo fileInfo in newFiles)
                {
                    if (CancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();
                    string name = fileInfo.Name;
                    DbFile dbFile = filesEntry.CurrentValue.FirstOrDefault(f => f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
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
                        Subdirectory subdirectory = subdirectoriesEntry.CurrentValue.FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.InvariantCultureIgnoreCase));
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
