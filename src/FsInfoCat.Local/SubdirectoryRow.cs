using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class SubdirectoryRow : LocalDbEntity, ILocalSubdirectoryRow, ISimpleIdentityReference<SubdirectoryRow>
    {
        #region Fields

        private Guid? _id;
        private string _name = string.Empty;
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

        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        [Required]
        public virtual DirectoryCrawlOptions Options { get; set; } = DirectoryCrawlOptions.None;

        [Required]
        public virtual DateTime LastAccessed { get; set; }

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        [Required]
        public DirectoryStatus Status { get; set; } = DirectoryStatus.Incomplete;

        public DateTime CreationTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public virtual Guid? ParentId { get; set; }

        public virtual Guid? VolumeId { get; set; }

        SubdirectoryRow IIdentityReference<SubdirectoryRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        protected SubdirectoryRow()
        {
            CreationTime = LastWriteTime = LastAccessed = CreatedOn;
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateOptions(results);
                ValidateParentAndVolumeId(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(Options):
                        ValidateOptions(results);
                        break;
                    case nameof(ParentId):
                    case nameof(VolumeId):
                    case nameof(Name):
                        ValidateParentAndVolumeId(validationContext, results);
                        break;
                }
        }

        private void ValidateParentAndVolumeId(ValidationContext validationContext, List<ValidationResult> results)
        {
            Guid? parent = ParentId;
            Guid? volume = VolumeId;
            Guid id = Id;
            LocalDbContext dbContext;
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            if (parent.HasValue)
            {
                if (volume.HasValue)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeAndParent, new string[] { nameof(Volume) }));
                else
                {
                    Guid parentId = parent.Value;
                    if (Id.Equals(parentId))
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                    else if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
                    {
                        string name = Name;
                        if (string.IsNullOrEmpty(name))
                            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, new string[] { nameof(Name) }));
                        else
                        {
                            var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.ParentId == parentId && sn.Name == name select sn;
                            if (entities.Any())
                                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
                            else
                                while (parent.HasValue)
                                {
                                    if (parent.Value.Equals(Id))
                                    {
                                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                                        break;
                                    }
                                    parent = dbContext.Subdirectories.Find(parent.Value)?.ParentId;
                                }
                        }
                    }
                }
            }
            else if (volume.HasValue)
            {
                if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
                {
                    Guid volumeId = volume.Value;
                    var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.VolumeId == volumeId select sn;
                    if (entities.Any())
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeHasRoot, new string[] { nameof(VolumeId) }));
                }
            }
            else
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, new string[] { nameof(ParentId) }));
        }

        private void ValidateOptions(List<ValidationResult> results)
        {
            if (!Enum.IsDefined(Options))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidDirectoryCrawlOption, new string[] { nameof(Options) }));
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalSubdirectoryRow other)
        {
            throw new NotImplementedException();
        }

        protected virtual bool ArePropertiesEqual([DisallowNull] ISubdirectoryRow other)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }
    }
}
