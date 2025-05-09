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
    /// Generic interface for file system entities.
    /// </summary>
    /// <seealso cref="FileSystemListItem" />
    /// <seealso cref="FileSystem" />
    public abstract class FileSystemRow : LocalDbEntity, ILocalFileSystemRow
    {
        #region Fields

        private Guid? _id;
        private string _displayName = string.Empty;
        private string _notes = string.Empty;

        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_displayName))]
        public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool ReadOnly { get; set; }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, uint.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual uint MaxNameLength { get; set; } = DbConstants.DbColDefaultValue_MaxNameLength;

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DefaultDriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DriveType? DefaultDriveType { get; set; }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.Notes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get; set; }

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
            if (dbContext is not null && dbContext.FileSystems.Any(fs => id != fs.Id && fs.DisplayName == displayName))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateDisplayName, new string[] { nameof(DisplayName) }));
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalFileSystemRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalFileSystemRow other) => ArePropertiesEqual((IFileSystemRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IFileSystemRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] IFileSystemRow other) => CreatedOn == other.CreatedOn && ModifiedOn == other.ModifiedOn && _notes == other.Notes && IsInactive == other.IsInactive && _displayName == other.DisplayName && ReadOnly == other.ReadOnly &&
                MaxNameLength == other.MaxNameLength && DefaultDriveType == other.DefaultDriveType;

        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            HashCode hash = new();
            hash.Add(_displayName);
            hash.Add(ReadOnly);
            hash.Add(MaxNameLength);
            hash.Add(DefaultDriveType);
            hash.Add(_notes);
            hash.Add(IsInactive);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $@"ReadOnly={ReadOnly}, MaxNameLength={MaxNameLength}, DefaultDriveType={DefaultDriveType}, IsInactive={IsInactive}";

        public override string ToString() => $@"{{ Id={_id}, DisplayName=""{ExtensionMethods.EscapeCsString(_displayName)}"",
    {PropertiesToString()},
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, LastSynchronizedOn={LastSynchronizedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, UpstreamId={UpstreamId},
    Notes=""{ExtensionMethods.EscapeCsString(_notes)}"" }}";

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
