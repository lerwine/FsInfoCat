using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class SubdirectoryRow : LocalDbEntity, ILocalSubdirectoryRow, ISimpleIdentityReference<SubdirectoryRow>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<DirectoryCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DirectoryStatus> _status;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<DateTime> _creationTime;
        private readonly IPropertyChangeTracker<DateTime> _lastWriteTime;
        private readonly IPropertyChangeTracker<Guid?> _parentId;
        private readonly IPropertyChangeTracker<Guid?> _volumeId;

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
            get => _id.GetValue();
            set
            {
                if (_id.IsSet)
                {
                    Guid id = _id.GetValue();
                    if (id.Equals(value))
                        return;
                    if (!id.Equals(Guid.Empty))
                        throw new InvalidOperationException();
                }
                _id.SetValue(value);
            }
        }

        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual DirectoryCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public DirectoryStatus Status { get => _status.GetValue(); set => _status.SetValue(value); }

        public DateTime CreationTime { get => _creationTime.GetValue(); set => _creationTime.SetValue(value); }

        public DateTime LastWriteTime { get => _lastWriteTime.GetValue(); set => _lastWriteTime.SetValue(value); }

        public virtual Guid? ParentId
        {
            get => _parentId.GetValue();
            set
            {
                Monitor.Enter(_parentId);
                try
                {
                    if (_parentId.SetValue(value))
                        OnParentIdChanged(value);
                }
                finally { Monitor.Exit(_parentId); }
            }
        }

        protected virtual void OnParentIdChanged(Guid? value) { }

        public virtual Guid? VolumeId
        {
            get => _volumeId.GetValue();
            set
            {
                Monitor.Enter(_parentId);
                try
                {
                    if (_volumeId.SetValue(value))
                        OnVolumeIdChanged(value);
                }
                finally { Monitor.Exit(_parentId); }
            }
        }

        protected virtual void OnVolumeIdChanged(Guid? value) { }

        SubdirectoryRow IIdentityReference<SubdirectoryRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        protected SubdirectoryRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = AddChangeTracker(nameof(Options), DirectoryCrawlOptions.None);
            _status = AddChangeTracker(nameof(Status), DirectoryStatus.Incomplete);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _creationTime = AddChangeTracker(nameof(CreationTime), CreatedOn);
            _lastWriteTime = AddChangeTracker(nameof(LastWriteTime), CreatedOn);
            _parentId = AddChangeTracker<Guid?>(nameof(ParentId), null);
            _volumeId = AddChangeTracker<Guid?>(nameof(VolumeId), null);
            _lastAccessed = AddChangeTracker(nameof(LastAccessed), CreatedOn);
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

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }
    }
}
