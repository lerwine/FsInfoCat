using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities which represent a logical file system volume on the local host machine.
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalVolumeRow" />
    public abstract class VolumeRow : LocalDbEntity, ILocalVolumeRow
    {
        #region Fields

        private Guid? _id;
        private string _displayName = string.Empty;
        private string _volumeName = string.Empty;
        private string _notes = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.UniqueIdentifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_id.HasValue)
                    {
                        if (!_id.Value.Equals(value))
                            throw new InvalidOperationException();
                    }
                    else if (value.Equals(Guid.Empty))
                        return;
                    _id = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets the display name of the volume.
        /// </summary>
        /// <value>The display name of the volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_displayName))]
        public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        /// <value>The name of the volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.VolumeName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_volumeName))]
        public virtual string VolumeName { get => _volumeName; set => _volumeName = value.AsNonNullTrimmed(); }

        /// <summary>
        /// Gets the unique volume identifier.
        /// </summary>
        /// <value>The system-independent unique identifier, which identifies the volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.VolumeIdentifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VolumeIdentifier Identifier { get; set; } = VolumeIdentifier.Empty;

        /// <summary>
        /// Gets the volume status.
        /// </summary>
        /// <value>The volume status value.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.VolumeStatus), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required]
        public virtual VolumeStatus Status { get; set; } = VolumeStatus.Unknown;

        /// <summary>
        /// Gets the drive type for this volume.
        /// </summary>
        /// <value>The drive type for this volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required]
        public virtual DriveType Type { get; set; } = DriveType.Unknown;

        /// <summary>
        /// Gets a value indicating whether the current volume is read-only.
        /// </summary>
        /// <value><see langword="true" /> if the current volume is read-only; <see langword="false" /> if it is read/write; otherwise, <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystemProperties.ReadOnly">file system type</see>.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool? ReadOnly { get; set; }

        /// <summary>
        /// Gets the maximum length of file system name components.
        /// </summary>
        /// <value>The maximum length of file system name components or <see langword="null" /> to assume the same value as defined by the <see cref="IFileSystemProperties.MaxNameLength">file system type</see>.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, uint.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual uint? MaxNameLength { get; set; }

        /// <summary>
        /// Gets the custom notes for this volume.
        /// </summary>
        /// <value>The custom notes to associate with this volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.Notes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        /// <summary>
        /// Gets the unique identifier of the entity host file system.
        /// </summary>
        /// <value>The unique identifier of the entity that represents the host file system for the current volume.</value>
        [Required]
        public virtual Guid FileSystemId { get; set; }

        #endregion

        /// <summary>
        /// This gets called whenever the current entity is being validated.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="results">Contains validation results to be returned by the <see cref="DbEntity.Validate(ValidationContext)"/> method.</param>
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
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
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

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalVolumeRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalVolumeRow other) => ArePropertiesEqual((IVolumeRow)other) &&
                   EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
                   LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IVolumeRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] IVolumeRow other) => CreatedOn == other.CreatedOn &&
                   ModifiedOn == other.ModifiedOn &&
                   Identifier.Equals(other.Identifier) &&
                   Status == other.Status &&
                   Type == other.Type &&
                   ReadOnly == other.ReadOnly &&
                   MaxNameLength == other.MaxNameLength &&
                   FileSystemId.Equals(other.FileSystemId) &&
                    _displayName == other.DisplayName &&
                    _volumeName == other.VolumeName &&
                    Notes == other.Notes;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            HashCode hash = new();
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            hash.Add(_displayName);
            hash.Add(_volumeName);
            hash.Add(_notes);
            hash.Add(Identifier);
            hash.Add(Status);
            hash.Add(Type);
            hash.Add(ReadOnly);
            hash.Add(MaxNameLength);
            hash.Add(FileSystemId);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $"Identifier={Identifier}, Status={Status}, Type={Type}, ReadOnly={ReadOnly}, ReadOnly={MaxNameLength}";

        public override string ToString() => $@"{{ Id={_id}, VolumeName=""{ExtensionMethods.EscapeCsString(_volumeName)}"", DisplayName=""{ExtensionMethods.EscapeCsString(_displayName)}"",
    {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId},
    Notes=""{ExtensionMethods.EscapeCsString(_notes)}"" }}";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Gets the unique identifier of the current entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the <see cref="Id" /> property has been set; otherwise, <see langword="false" />.</returns>
        public bool TryGetId(out Guid result)
        {
            Guid? id = _id;
            if (id.HasValue)
            {
                result = id.Value;
                return true;
            }
            result = Guid.Empty;
            return false;
        }
    }
}
