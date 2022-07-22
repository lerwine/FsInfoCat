using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities that represent a subdirectory.
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalSubdirectoryRow" />
    public abstract class SubdirectoryRow : LocalDbEntity, ILocalSubdirectoryRow
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
        /// Gets the name of the current file system item.
        /// </summary>
        /// <value>The name of the current file system item.</value>
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        /// <summary>
        /// Gets the crawl options for the current subdirectory.
        /// </summary>
        /// <value>The crawl options for the current subdirectory.</value>
        [Required]
        public virtual DirectoryCrawlOptions Options { get; set; } = DirectoryCrawlOptions.None;

        /// <summary>
        /// Gets the date and time last accessed.
        /// </summary>
        /// <value>The last accessed for the purposes of this application.</value>
        [Required]
        public virtual DateTime LastAccessed { get; set; }

        /// <summary>
        /// Gets custom notes to be associated with the current file system item.
        /// </summary>
        /// <value>The custom notes to associate with the current file system item.</value>
        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        /// <summary>
        /// Gets the status of the current subdirectory.
        /// </summary>
        /// <value>The status value for the current subdirectory.</value>
        [Required]
        public DirectoryStatus Status { get; set; } = DirectoryStatus.Incomplete;

        /// <summary>
        /// Gets the file's creation time.
        /// </summary>
        /// <value>The creation time as reported by the host file system.</value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets the date and time the file system item was last written nto.
        /// </summary>
        /// <value>The last write time as reported by the host file system.</value>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets the primary key of the parent <see cref="Subdirectory"/>.
        /// </summary>
        /// <value>The <see cref="Id"/> value of the parent <see cref="Subdirectory"/> or <see langword="null"/> if this has no parent
        /// subdirectory.</value>
        /// <remarks>If this is <see langword="null"/>, then <see cref="VolumeId"/> should have a value.</remarks>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// Gets the primary key of the parent <see cref="Volume"/>.
        /// </summary>
        /// <value>The <see cref="Id"/> value of the parent <see cref="Volume"/> or <see langword="null"/> if this is a nested subdirectory.</value>
        /// <remarks>If this is <see langword="null"/>, then <see cref="ParentId"/> should have a value.</remarks>
        public virtual Guid? VolumeId { get; set; }

        #endregion

        /// <summary>
        /// Creates a new subdirectory row database entity.
        /// </summary>
        protected SubdirectoryRow()
        {
            CreationTime = LastWriteTime = LastAccessed = CreatedOn;
        }

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

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalSubdirectoryRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalSubdirectoryRow other)
        {
            // TODO: Implement ArePropertiesEqual(ILocalSubdirectoryRow)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ISubdirectoryRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ISubdirectoryRow other)
        {
            // TODO: Implement ArePropertiesEqual(ISubdirectoryRow)
            throw new NotImplementedException();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            HashCode hash = new();
            hash.Add(_name);
            hash.Add(Options);
            hash.Add(LastAccessed);
            hash.Add(_notes);
            hash.Add(Status);
            hash.Add(CreationTime);
            hash.Add(LastWriteTime);
            hash.Add(ParentId);
            hash.Add(VolumeId);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }

        protected virtual string PropertiesToString() => $@"CreationTime={CreationTime:yyyy-mm-ddTHH:mm:ss.fffffff}, LastWriteTime={LastWriteTime:yyyy-mm-ddTHH:mm:ss.fffffff}, LastAccessed={LastAccessed:yyyy-mm-ddTHH:mm:ss.fffffff}";

        public override string ToString() => $@"{{ Id={_id}, Name=""{ExtensionMethods.EscapeCsString(_name)}"",
    ParentId={ParentId}, VolumeId={VolumeId}, Status={Status}, Options={Options},
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
