using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class Volume : LocalDbEntity, ILocalVolume
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<string> _volumeName;
        private readonly IPropertyChangeTracker<VolumeIdentifier> _identifier;
        private readonly IPropertyChangeTracker<DriveType> _type;
        private readonly IPropertyChangeTracker<bool?> _readOnly;
        private readonly IPropertyChangeTracker<uint?> _maxNameLength;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<VolumeStatus> _status;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;
        private readonly IPropertyChangeTracker<Subdirectory> _rootDirectory;
        private HashSet<VolumeAccessError> _accessErrors = new();
        private HashSet<PersonalVolumeTag> _personalTags = new();
        private HashSet<SharedVolumeTag> _sharedTags = new();

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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string VolumeName { get => _volumeName.GetValue(); set => _volumeName.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Identifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        //[StringLength(DbConstants.DbColMaxLen_Identifier, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength),
        //    ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        //[Column(TypeName = "nvarchar(1024)")]
        public virtual VolumeIdentifier Identifier { get => _identifier.GetValue(); set => _identifier.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VolumeStatus), ResourceType = typeof(Properties.Resources))]
        [Required]
        public virtual VolumeStatus Status { get => _status.GetValue(); set => _status.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DriveType), ResourceType = typeof(Properties.Resources))]
        [Required]
        public virtual DriveType Type { get => _type.GetValue(); set => _type.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool? ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, uint.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual uint? MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Notes), ResourceType = typeof(Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual Guid FileSystemId
        {
            get => _fileSystemId.GetValue();
            set
            {
                if (_fileSystemId.SetValue(value))
                {
                    FileSystem nav = _fileSystem.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _fileSystem.SetValue(null);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem.GetValue();
            set
            {
                if (_fileSystem.SetValue(value))
                {
                    if (value is null)
                        _fileSystemId.SetValue(Guid.Empty);
                    else
                        _fileSystemId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Subdirectory RootDirectory { get => _rootDirectory.GetValue(); set => _rootDirectory.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<VolumeAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
        }

        public HashSet<PersonalVolumeTag> PersonalTags
        {
            get => _personalTags;
            set => CheckHashSetChanged(_personalTags, value, h => _personalTags = h);
        }

        public HashSet<SharedVolumeTag> SharedTags
        {
            get => _sharedTags;
            set => CheckHashSetChanged(_sharedTags, value, h => _sharedTags = h);
        }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalVolume.FileSystem { get => FileSystem; }

        IFileSystem IVolume.FileSystem { get => FileSystem; }

        ILocalSubdirectory ILocalVolume.RootDirectory => RootDirectory;

        ISubdirectory IVolume.RootDirectory => RootDirectory;

        IEnumerable<ILocalVolumeAccessError> ILocalVolume.AccessErrors => AccessErrors.Cast<ILocalVolumeAccessError>();

        IEnumerable<IVolumeAccessError> IVolume.AccessErrors => AccessErrors.Cast<IVolumeAccessError>();

        IEnumerable<ILocalPersonalVolumeTag> ILocalVolume.PersonalTags => PersonalTags.Cast<ILocalPersonalVolumeTag>();

        IEnumerable<IPersonalVolumeTag> IVolume.PersonalTags => PersonalTags.Cast<IPersonalVolumeTag>();

        IEnumerable<ILocalSharedVolumeTag> ILocalVolume.SharedTags => SharedTags.Cast<ILocalSharedVolumeTag>();

        IEnumerable<ISharedVolumeTag> IVolume.SharedTags => SharedTags.Cast<ISharedVolumeTag>();

        #endregion

        public Volume()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _displayName = AddChangeTracker(nameof(DisplayName), "", TrimmedNonNullStringCoersion.Default);
            _volumeName = AddChangeTracker(nameof(VolumeName), "", TrimmedNonNullStringCoersion.Default);
            _identifier = AddChangeTracker<VolumeIdentifier>(nameof(Identifier), default);
            _readOnly = AddChangeTracker<bool?>(nameof(ReadOnly), null);
            _maxNameLength = AddChangeTracker<uint?>(nameof(MaxNameLength), null);
            _type = AddChangeTracker(nameof(Type), DriveType.Unknown);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _status = AddChangeTracker(nameof(Status), VolumeStatus.Unknown);
            _fileSystemId = AddChangeTracker(nameof(FileSystemId), Guid.Empty);
            _fileSystem = AddChangeTracker<FileSystem>(nameof(FileSystem), null);
            _rootDirectory = AddChangeTracker<Subdirectory>(nameof(RootDirectory), null);
        }

        internal async Task<bool> ForceDeleteFromDbAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<Volume> dbEntry = dbContext.Entry(this);
            if (!dbEntry.ExistsInDb())
                return false;
            cancellationToken.ThrowIfCancellationRequested();
            IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            Subdirectory oldSubdirectory = await dbEntry.GetRelatedReferenceAsync(f => f.RootDirectory, cancellationToken);
            if (oldSubdirectory is not null)
                await oldSubdirectory.ExpungeAsync(dbContext, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            VolumeAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.VolumeAccessErrors.RemoveRange(accessErrors);
            Guid id = Id;
            cancellationToken.ThrowIfCancellationRequested();
            await dbContext.SaveChangesAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await transaction.CommitAsync(cancellationToken);
            return true;
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<Volume> builder)
        {
            builder.HasOne(sn => sn.FileSystem).WithMany(d => d.Volumes).HasForeignKey(nameof(FileSystemId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateType(results);
                ValidateStatus(results);
                ValidateIdentifier(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(Type):
                        ValidateType(results);
                        break;
                    case nameof(Status):
                        ValidateStatus(results);
                        break;
                    case nameof(Identifier):
                        ValidateIdentifier(validationContext, results);
                        break;
                }
        }

        private void ValidateIdentifier(ValidationContext validationContext, List<ValidationResult> results)
        {
            VolumeIdentifier identifier = Identifier;
            LocalDbContext dbContext;
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            if (identifier.IsEmpty())
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired, new string[] { nameof(Identifier) }));
            else if (identifier.ToString().Length > DbConstants.DbColMaxLen_Identifier)
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength, new string[] { nameof(Identifier) }));
            else if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
            {
                Guid id = Id;
                if (dbContext.Volumes.Any(v => id != v.Id && v.Identifier == identifier))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateVolumeIdentifier, new string[] { nameof(Identifier) }));
            }
        }

        private void ValidateStatus(List<ValidationResult> results)
        {
            if (!Enum.IsDefined(Status))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidVolumeStatus, new string[] { nameof(Status) }));
        }

        private void ValidateType(List<ValidationResult> results)
        {
            if (!Enum.IsDefined(Type))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, new string[] { nameof(Type) }));
        }

        public static async Task<EntityEntry<Volume>> ImportVolumeAsync([DisallowNull] DirectoryInfo directoryInfo, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            if (directoryInfo.Parent is not null)
                directoryInfo = directoryInfo.Root;

            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            IFileSystemDetailService fileSystemDetailService = serviceScope.ServiceProvider.GetService<IFileSystemDetailService>();
            string name = directoryInfo.Name;
            ILogicalDiskInfo diskInfo = await fileSystemDetailService.GetLogicalDiskAsync(directoryInfo, cancellationToken);
            VolumeIdentifier volumeIdentifier;
            if (diskInfo is null)
            {
                Uri uri = new(((directoryInfo.Parent is null) ? directoryInfo : directoryInfo.Root).FullName, UriKind.Absolute);
                if (uri.IsUnc)
                    volumeIdentifier = new VolumeIdentifier(uri);
                else
                    throw new InvalidOperationException($"Logical disk \"{directoryInfo.FullName}\" not found.");
            }
            else if (!diskInfo.TryGetVolumeIdentifier(out volumeIdentifier))
                throw new InvalidOperationException($"Logical disk \"{diskInfo.Name}\" does not specify a volume identifer.");

            Volume result = await (from v in dbContext.Volumes where v.Identifier == volumeIdentifier select v).FirstOrDefaultAsync(cancellationToken);
            if (result is not null)
                return dbContext.Entry(result);
            (EntityEntry<FileSystem> Entry, SymbolicName SymbolicName) fileSystem = await FileSystem.ImportFileSystemAsync(diskInfo, volumeIdentifier, dbContext, fileSystemDetailService, cancellationToken);
            result = new()
            {
                Id = Guid.NewGuid(),
                Identifier = volumeIdentifier,
                FileSystem = fileSystem.Entry.Entity
            };
            if (diskInfo is null)
            {
                (IFileSystemProperties Properties, string SymbolicName) genericNetworkFsType = fileSystemDetailService.GetGenericNetworkShareFileSystem();
                result.MaxNameLength = genericNetworkFsType.Properties.MaxNameLength;
                result.ReadOnly = genericNetworkFsType.Properties.ReadOnly;
                result.Status = VolumeStatus.Unknown;
                result.Type = DriveType.Network;
                result.DisplayName = $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}";
            }
            else
            {
                result.MaxNameLength = diskInfo.MaxNameLength;
                result.ReadOnly = diskInfo.IsReadOnly;
                result.Status = VolumeStatus.Unknown;
                result.Type = diskInfo.DriveType;
                result.DisplayName = (diskInfo.DriveType == DriveType.Network && diskInfo.DisplayName == directoryInfo.FullName) ? $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}" : diskInfo.DisplayName;
            }
            if (fileSystem.Entry.State == EntityState.Added)
            {
                result.ModifiedOn = result.CreatedOn = fileSystem.Entry.Entity.CreatedOn;
                await dbContext.SaveChangesAsync(cancellationToken);
                dbContext.SymbolicNames.Add(fileSystem.SymbolicName);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            else
                result.ModifiedOn = result.CreatedOn = DateTime.Now;
            return dbContext.Volumes.Add(result);
        }
    }
}
