using FsInfoCat.Desktop.WMI;
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
    public partial class FileSystemImporter
    {
        private readonly object _syncRoot = new();
        private CancellationTokenSource _tokenSource;

        public event EventHandler<FileSystemImportEventArgs> Importing;
        public event EventHandler<FileSystemImportEventArgs> Imported;
        public event EventHandler<FileSystemImportEventArgs> ImportError;
        public event EventHandler<FileImportEventArgs> FileImporting;
        public event EventHandler<FileImportEventArgs> FileImported;
        public event EventHandler<FileImportEventArgs> FileImportError;
        public event EventHandler<DirectoryImportEventArgs> DirectoryImporting;
        public event EventHandler<DirectoryImportEventArgs> DirectoryImported;
        public event EventHandler<DirectoryImportEventArgs> DirectoryImportError;

        public DirectoryInfo DirectoryInfo { get; set; }

        public ushort MaxRecursionDepth { get; set; }

        public ulong MaxTotalItems { get; set; }

        public string DisplayName { get; set; }

        public FileSystemImporter(DirectoryInfo directoryInfo, ushort maxRecursionDepth, ulong maxTotalItems, string displayName)
        {
            DirectoryInfo = directoryInfo;
            MaxRecursionDepth = maxRecursionDepth;
            MaxTotalItems = maxTotalItems;
            DisplayName = displayName;
        }

        internal static async Task<FileSystem> GetFileSystemAsync([AllowNull] string symbolicName, [NotNull] LocalDbContext dbContext, bool doNotSaveChanges = false)
        {
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (string.IsNullOrWhiteSpace(symbolicName))
                return null;
            SymbolicName sn = await dbContext.SymbolicNames.FirstOrDefaultAsync(n => n.Name == symbolicName && !n.IsInactive);
            FileSystem fileSystem;
            if (sn is null)
            {
                fileSystem = new FileSystem
                {
                    Id = Guid.NewGuid(),
                    DisplayName = symbolicName
                };
                dbContext.FileSystems.Add(fileSystem);
                sn = new SymbolicName
                {
                    Id = Guid.NewGuid(),
                    Name = symbolicName,
                    FileSystem = fileSystem
                };
                dbContext.SymbolicNames.Add(sn);
                if (!doNotSaveChanges)
                    await dbContext.SaveChangesAsync();
            }
            else if ((fileSystem = sn.FileSystem) is null)
            {
                await dbContext.Entry(sn).Reference(s => s.FileSystem).LoadAsync();
                if ((fileSystem = sn.FileSystem) is null)
                {
                    fileSystem = new FileSystem
                    {
                        Id = Guid.NewGuid(),
                        DisplayName = symbolicName
                    };
                    dbContext.FileSystems.Add(fileSystem);
                    if (!doNotSaveChanges)
                        await dbContext.SaveChangesAsync();
                }
            }
            return fileSystem;
        }

        internal static async Task<Volume> GetVolumeAsync([AllowNull] Win32_LogicalDisk logicalDisk, [NotNull] LocalDbContext dbContext, bool doNotSaveChanges = false)
        {
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (logicalDisk is null)
                return null;
            VolumeIdentifier volumeIdentifier = (logicalDisk.DriveType == DriveType.Network) ? new VolumeIdentifier(new Uri(logicalDisk.ProviderName)) : new VolumeIdentifier(uint.Parse(logicalDisk.VolumeSerialNumber));
            Volume volume = await dbContext.Volumes.FirstOrDefaultAsync(v => v.Identifier == volumeIdentifier);
            if (volume is null)
            {
                volume = new Volume
                {
                    Id = Guid.NewGuid(),
                    Identifier = volumeIdentifier,
                    VolumeName = logicalDisk.VolumeName,
                    DisplayName = logicalDisk.Name,
                    FileSystem = await GetFileSystemAsync(logicalDisk.FileSystem, dbContext, false),
                    Type = logicalDisk.DriveType
                };
                dbContext.Volumes.Add(volume);
                if (!doNotSaveChanges)
                    await dbContext.SaveChangesAsync();
            }
            return volume;
        }

        internal static async Task<Subdirectory> GetSubdirectoryAsync([AllowNull] DirectoryInfo directoryInfo, [NotNull] LocalDbContext dbContext, bool doNotSaveChanges = false)
        {
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (directoryInfo is null)
                return null;
            Subdirectory result;
            DirectoryInfo parent = directoryInfo.Parent;
            if (parent is null)
            {
                Volume volume = await GetVolumeAsync(await Win32_LogicalDisk.GetLogicalDiskAsync(directoryInfo), dbContext, doNotSaveChanges);
                if (volume is null)
                    return null;
                result = volume.RootDirectory;
                if (result is null)
                {
                    EntityEntry<Volume> entityEntry = dbContext.Entry(volume);
                    if (entityEntry.State != EntityState.Added)
                        await entityEntry.Reference(v => v.RootDirectory).LoadAsync();
                    if ((result = volume.RootDirectory) is null)
                    {
                        DateTime createdOn = DateTime.Now;
                        result = new Subdirectory
                        {
                            Id = Guid.NewGuid(),
                            CreationTime = directoryInfo.CreationTime,
                            LastWriteTime = directoryInfo.LastWriteTime,
                            Name = directoryInfo.Name,
                            Volume = volume
                        };
                        volume.RootDirectory = result;
                        dbContext.Subdirectories.Add(result);
                        if (!doNotSaveChanges)
                            await dbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                Subdirectory ps = await GetSubdirectoryAsync(parent, dbContext, false);
                if (ps is null)
                    return null;
                Guid parentId = ps.Id;
                string name = directoryInfo.Name;
                if (dbContext.Entry(ps).State == EntityState.Added || (result = await dbContext.Subdirectories.FirstOrDefaultAsync(d => d.ParentId == parentId && d.Name == name)) is null)
                {
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Name = directoryInfo.Name,
                        CreationTime = directoryInfo.CreationTime,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        Parent = ps
                    };
                    dbContext.Subdirectories.Add(result);
                    if (!doNotSaveChanges)
                        await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }

        public static async Task<FileSystemImporter> CreateAsync(CrawlConfiguration configuration)
        {
            using LocalDbContext dbContext = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
            Subdirectory subdirectory = configuration.Root;
            if (subdirectory is null)
            {
                await dbContext.Entry(configuration).Reference(c => c.Root).LoadAsync();
                if ((subdirectory = configuration.Root) is null)
                    throw new Exception("Could not get root directory"); // TODO: Throw proper error
            }

            string fullName = await Subdirectory.LookupFullNameAsync(subdirectory, dbContext);
            if (string.IsNullOrEmpty(fullName))
                throw new Exception("Could not determine full name"); // TODO: Throw proper error
            return new FileSystemImporter(new DirectoryInfo(fullName), configuration.MaxRecursionDepth, configuration.MaxTotalItems, configuration.DisplayName);
        }

        public async Task ScanAsync([NotNull] LocalDbContext dbContext)
        {
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            lock (_syncRoot)
            {
                if (_tokenSource is not null)
                    throw new InvalidOperationException();
                _tokenSource = new CancellationTokenSource();
            }
            using (_tokenSource)
            {
                ScanContext context = await ScanContext.CreateAsync(this, dbContext, _tokenSource.Token);
                try { await ScanAsync(context, dbContext); }
                finally
                {
                    lock(_syncRoot)
                        try { _tokenSource.Dispose(); }
                        finally { _tokenSource = null; }
                }
            }
        }

        private async Task ScanAsync(ScanContext context, LocalDbContext dbContext)
        {
            throw new NotImplementedException();
        }
    }
}
