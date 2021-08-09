using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class FileSystem : LocalDbEntity, ILocalFileSystem
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<bool> _readOnly;
        private readonly IPropertyChangeTracker<uint> _maxNameLength;
        private readonly IPropertyChangeTracker<DriveType?> _defaultDriveType;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private HashSet<Volume> _volumes = new();
        private HashSet<SymbolicName> _symbolicNames = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, uint.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual uint MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DefaultDriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DriveType? DefaultDriveType { get => _defaultDriveType.GetValue(); set => _defaultDriveType.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

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

        #endregion

        public FileSystem()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _displayName = AddChangeTracker(nameof(DisplayName), "", TrimmedNonNullStringCoersion.Default);
            _readOnly = AddChangeTracker(nameof(ReadOnly), false);
            _maxNameLength = AddChangeTracker(nameof(MaxNameLength), DbConstants.DbColDefaultValue_MaxNameLength);
            _defaultDriveType = AddChangeTracker<DriveType?>(nameof(DefaultDriveType), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _isInactive = AddChangeTracker(nameof(IsInactive), false);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateDriveType(results);
                ValidateDisplayName(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(DefaultDriveType):
                        ValidateDriveType(results);
                        break;
                    case nameof(DisplayName):
                        ValidateDisplayName(validationContext, results);
                        break;
                }
        }

        private void ValidateDriveType(List<ValidationResult> results)
        {
            var driveType = DefaultDriveType;
            if (driveType.HasValue && !Enum.IsDefined(driveType.Value))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, new string[] { nameof(DefaultDriveType) }));
        }

        private void ValidateDisplayName(ValidationContext validationContext, List<ValidationResult> results)
        {
            string displayName = DisplayName;
            if (string.IsNullOrEmpty(displayName))
                return;
            Guid id = Id;
            using IServiceScope serviceScope = validationContext.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            if (dbContext is not null)
            {
                if (dbContext.FileSystems.Any(fs => id != fs.Id && fs.DisplayName == displayName))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateDisplayName, new string[] { nameof(DisplayName) }));
            }
        }

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

                symbolicName = await (from sn in dbContext.SymbolicNames where sn.Name == name select sn).FirstOrDefaultAsync(cancellationToken);
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
                symbolicName = await (from sn in dbContext.SymbolicNames where sn.Name == name select sn).FirstOrDefaultAsync(cancellationToken);
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
