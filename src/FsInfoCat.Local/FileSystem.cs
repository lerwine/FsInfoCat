using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class FileSystem : FileSystemRow, ILocalFileSystem, ISimpleIdentityReference<FileSystem>
    {
        #region Fields

        private HashSet<Volume> _volumes = new();
        private HashSet<SymbolicName> _symbolicNames = new();

        #endregion

        #region Properties

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Volumes), ResourceType = typeof(Properties.Resources))]
        public virtual HashSet<Volume> Volumes
        {
            get => _volumes;
            set => CheckHashSetChanged(_volumes, value, h => _volumes = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_SymbolicNames), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<SymbolicName> SymbolicNames
        {
            get => _symbolicNames;
            set => CheckHashSetChanged(_symbolicNames, value, h => _symbolicNames = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalVolume> ILocalFileSystem.Volumes => _volumes.Cast<ILocalVolume>();

        IEnumerable<ILocalSymbolicName> ILocalFileSystem.SymbolicNames => _volumes.Cast<ILocalSymbolicName>();

        IEnumerable<IVolume> IFileSystem.Volumes => _volumes.Cast<IVolume>();

        IEnumerable<ISymbolicName> IFileSystem.SymbolicNames => _volumes.Cast<ISymbolicName>();

        FileSystem IIdentityReference<FileSystem>.Entity => this;

        #endregion

        public static async Task<(EntityEntry<FileSystem> Entry, SymbolicName SymbolicName)> ImportFileSystemAsync([AllowNull] ILogicalDiskInfo diskInfo, VolumeIdentifier volumeIdentifier, [DisallowNull] LocalDbContext dbContext,
            [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken)
        {
            string name;
            FileSystem fileSystem;
            SymbolicName symbolicName;
            if (diskInfo is null)
            {
                if (!volumeIdentifier.Location.IsUnc)
                    throw new ArgumentOutOfRangeException(nameof(volumeIdentifier));
                (IFileSystemProperties Properties, string SymbolicName) genericNetworkFsType = fileSystemDetailService.GetGenericNetworkShareFileSystem();
                name = genericNetworkFsType.SymbolicName;

                symbolicName = await (from sn in dbContext.SymbolicNames.Include(n => n.FileSystem) where sn.Name == name select sn).FirstOrDefaultAsync(cancellationToken);
                if (symbolicName is not null)
                    return (dbContext.Entry(symbolicName.FileSystem), symbolicName);
                fileSystem = new()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    DisplayName = "Network File System",
                    DefaultDriveType = genericNetworkFsType.Properties.DefaultDriveType,
                    MaxNameLength = genericNetworkFsType.Properties.MaxNameLength,
                    ReadOnly = genericNetworkFsType.Properties.ReadOnly
                };
            }
            else
            {
                name = diskInfo.FileSystemName;
                symbolicName = await (from sn in dbContext.SymbolicNames.Include(n => n.FileSystem) where sn.Name == name select sn).FirstOrDefaultAsync(cancellationToken);
                if (symbolicName is not null)
                    return (dbContext.Entry(symbolicName.FileSystem), symbolicName);
                fileSystem = new()
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    DisplayName = "Network File System",
                    DefaultDriveType = diskInfo.DriveType,
                    MaxNameLength = diskInfo.MaxNameLength,
                    ReadOnly = diskInfo.IsReadOnly
                };
            }
            fileSystem.ModifiedOn = fileSystem.CreatedOn;

            return (dbContext.FileSystems.Add(fileSystem), new()
            {
                Id = Guid.NewGuid(),
                CreatedOn = fileSystem.CreatedOn,
                ModifiedOn = fileSystem.ModifiedOn,
                Name = name,
                FileSystem = fileSystem,
                Priority = 0
            });
        }
    }
}
