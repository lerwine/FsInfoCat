using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class FileSystemRow : LocalDbEntity, ILocalFileSystemRow, ISimpleIdentityReference<FileSystemRow>
    {
        #region Fields

        private Guid? _id;
        private string _displayName = string.Empty;
        private string _notes = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_displayName))]
        public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool ReadOnly { get; set; }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, uint.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual uint MaxNameLength { get; set; } = DbConstants.DbColDefaultValue_MaxNameLength;

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DefaultDriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DriveType? DefaultDriveType { get; set; }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Notes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_IsInactive), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool IsInactive { get; set; }

        FileSystemRow IIdentityReference<FileSystemRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

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

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalFileSystemRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] IFileSystemRow other)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
