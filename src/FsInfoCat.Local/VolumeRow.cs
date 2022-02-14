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
    public abstract class VolumeRow : LocalDbEntity, ILocalVolumeRow, ISimpleIdentityReference<VolumeRow>
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
        public virtual string DisplayName { get => _displayName; set => _displayName = value.AsWsNormalizedOrEmpty(); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string VolumeName { get => _volumeName; set => _volumeName = value.AsNonNullTrimmed(); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Identifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VolumeIdentifier Identifier { get; set; } = VolumeIdentifier.Empty;

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VolumeStatus), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required]
        public virtual VolumeStatus Status { get; set; } = VolumeStatus.Unknown;

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DriveType), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required]
        public virtual DriveType Type { get; set; } = DriveType.Unknown;

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool? ReadOnly { get; set; }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, uint.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual uint? MaxNameLength { get; set; }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Notes), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        [Required]
        public virtual Guid FileSystemId { get; set; }

        VolumeRow IIdentityReference<VolumeRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

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

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalVolumeRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] IVolumeRow other)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
