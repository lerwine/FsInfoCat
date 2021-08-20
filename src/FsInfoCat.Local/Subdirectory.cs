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
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class Subdirectory : LocalDbEntity, ILocalSubdirectory
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
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<CrawlConfiguration> _crawlConfiguration;
        private readonly IPropertyChangeTracker<Volume> _volume;
        private HashSet<DbFile> _files = new();
        private HashSet<Subdirectory> _subDirectories = new();
        private HashSet<SubdirectoryAccessError> _accessErrors = new();
        private HashSet<PersonalSubdirectoryTag> _personalTags = new();
        private HashSet<SharedSubdirectoryTag> _sharedTags = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

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
                if (_parentId.SetValue(value))
                {
                    Subdirectory nav = _parent.GetValue();
                    if (!(nav is null || (value.HasValue && nav.Id.Equals(value.Value))))
                        _parent.SetValue(null);
                }
            }
        }

        public virtual Guid? VolumeId
        {
            get => _volumeId.GetValue();
            set
            {
                if (_volumeId.SetValue(value))
                {
                    Volume nav = _volume.GetValue();
                    if (!(nav is null || (value.HasValue && nav.Id.Equals(value.Value))))
                        _volume.SetValue(null);
                }
            }
        }

        public virtual Subdirectory Parent
        {
            get => _parent.GetValue();
            set
            {
                if (_parent.SetValue(value))
                {
                    if (value is null)
                        _parentId.SetValue(null);
                    else
                        _parentId.SetValue(value.Id);
                }
            }
        }

        public virtual Volume Volume
        {
            get => _volume.GetValue();
            set
            {
                if (_volume.SetValue(value))
                {
                    if (value is null)
                        _volumeId.SetValue(null);
                    else
                        _volumeId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlConfiguration CrawlConfiguration
        {
            get => _crawlConfiguration.GetValue();
            set => _crawlConfiguration.SetValue(value);
        }

        public virtual HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        public virtual HashSet<Subdirectory> SubDirectories
        {
            get => _subDirectories;
            set => CheckHashSetChanged(_subDirectories, value, h => _subDirectories = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<SubdirectoryAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
        }

        public HashSet<PersonalSubdirectoryTag> PersonalTags
        {
            get => _personalTags;
            set => CheckHashSetChanged(_personalTags, value, h => _personalTags = h);
        }

        public HashSet<SharedSubdirectoryTag> SharedTags
        {
            get => _sharedTags;
            set => CheckHashSetChanged(_sharedTags, value, h => _sharedTags = h);
        }

        #endregion

        #region Explicit Members

        ISubdirectory IDbFsItem.Parent { get => Parent; }

        ILocalVolume ILocalSubdirectory.Volume { get => Volume; }

        IVolume ISubdirectory.Volume { get => Volume; }

        IEnumerable<ILocalFile> ILocalSubdirectory.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> ISubdirectory.Files => Files.Cast<IFile>();

        IEnumerable<ILocalSubdirectory> ILocalSubdirectory.SubDirectories => SubDirectories.Cast<ILocalSubdirectory>();

        IEnumerable<ISubdirectory> ISubdirectory.SubDirectories => SubDirectories.Cast<ISubdirectory>();

        ILocalCrawlConfiguration ILocalSubdirectory.CrawlConfiguration { get => CrawlConfiguration; }

        ICrawlConfiguration ISubdirectory.CrawlConfiguration { get => CrawlConfiguration; }

        IEnumerable<ILocalSubdirectoryAccessError> ILocalSubdirectory.AccessErrors => AccessErrors.Cast<ILocalSubdirectoryAccessError>();

        IEnumerable<ISubdirectoryAccessError> ISubdirectory.AccessErrors => AccessErrors.Cast<ISubdirectoryAccessError>();

        ILocalSubdirectory ILocalDbFsItem.Parent => Parent;

        IEnumerable<ILocalAccessError> ILocalDbFsItem.AccessErrors => AccessErrors.Cast<ILocalAccessError>();

        IEnumerable<IAccessError> IDbFsItem.AccessErrors => AccessErrors.Cast<IAccessError>();

        ISubdirectory ISubdirectory.Parent => Parent;

        IEnumerable<ILocalPersonalSubdirectoryTag> ILocalSubdirectory.PersonalTags => PersonalTags.Cast<ILocalPersonalSubdirectoryTag>();

        IEnumerable<ILocalPersonalTag> ILocalDbFsItem.PersonalTags => PersonalTags.Cast<ILocalPersonalTag>();

        IEnumerable<IPersonalSubdirectoryTag> ISubdirectory.PersonalTags => PersonalTags.Cast<IPersonalSubdirectoryTag>();

        IEnumerable<IPersonalTag> IDbFsItem.PersonalTags => PersonalTags.Cast<IPersonalTag>();

        IEnumerable<ILocalSharedSubdirectoryTag> ILocalSubdirectory.SharedTags => SharedTags.Cast<ILocalSharedSubdirectoryTag>();

        IEnumerable<ILocalSharedTag> ILocalDbFsItem.SharedTags => SharedTags.Cast<ILocalSharedTag>();

        IEnumerable<ISharedSubdirectoryTag> ISubdirectory.SharedTags => SharedTags.Cast<ISharedSubdirectoryTag>();

        IEnumerable<ISharedTag> IDbFsItem.SharedTags => SharedTags.Cast<ISharedTag>();

        #endregion

        public Subdirectory()
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
            _parent = AddChangeTracker<Subdirectory>(nameof(Parent), null);
            _volume = AddChangeTracker<Volume>(nameof(Volume), null);
            _crawlConfiguration = AddChangeTracker<CrawlConfiguration>(nameof(CrawlConfiguration), null);
        }

        public static async Task MarkBranchIncompleteAsync(EntityEntry<Subdirectory> dbEntry, CancellationToken cancellationToken)
        {
            if (dbEntry.Context is not LocalDbContext dbContext)
                throw new InvalidOperationException();
            dbEntry.Entity.Status = DirectoryStatus.Incomplete;
            SubdirectoryAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(d => d.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.SubdirectoryAccessErrors.RemoveRange(accessErrors);
            foreach (Subdirectory subdirectory in await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken))
                await MarkBranchIncompleteAsync(dbContext.Entry(subdirectory), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkBranchIncompleteAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<Subdirectory> dbEntry = dbContext.Entry(this);
            switch (dbEntry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    throw new InvalidOperationException();
            }
            switch (Status)
            {
                case DirectoryStatus.Deleted:
                case DirectoryStatus.Incomplete:
                    return;
            }
            if (dbContext.Database.CurrentTransaction is null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await MarkBranchIncompleteAsync(dbEntry, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await transaction.CommitAsync(cancellationToken);
            }
            else
                await MarkBranchIncompleteAsync(dbEntry, cancellationToken);
        }

        public async Task MarkBranchDeletedAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            if (Status == DirectoryStatus.Deleted)
                return;
            EntityEntry<Subdirectory> dbEntry = dbContext.Entry(this);
            switch (dbEntry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    throw new InvalidOperationException();
            }
            if (dbContext.Database.CurrentTransaction is null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await MarkBranchDeletedAsync(dbContext, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await transaction.CommitAsync(cancellationToken);
            }
            else
            {
                Status = DirectoryStatus.Deleted;
                SubdirectoryAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(d => d.AccessErrors, cancellationToken)).ToArray();
                if (accessErrors.Length > 0)
                    dbContext.SubdirectoryAccessErrors.RemoveRange(accessErrors);
                foreach (Subdirectory subdirectory in await dbEntry.GetRelatedCollectionAsync(d => d.SubDirectories, cancellationToken))
                    await subdirectory.MarkBranchDeletedAsync(dbContext, cancellationToken);
                foreach (DbFile file in await dbEntry.GetRelatedCollectionAsync(d => d.Files, cancellationToken))
                    await file.SetStatusDeleted(dbContext, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Asynchronously expunges the current <see cref="DbFile"/> from the database if the <see cref="Status"/> is <see cref="DirectoryStatus.Deleted"/>
        /// and <see cref="LocalDbEntity.UpstreamId"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        ///   <para>A <see cref="Task{TResult}">Task&lt;System.Boolean&gt;</see> returning a <span class="keyword"><span class="languageSpecificText"><span class="cs">true</span><span class="vb">True</span><span class="cpp">true</span></span></span><span class="nu"><span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)</span> if the subdirectory was deleted; otherwise
        /// <span class="keyword"><span class="languageSpecificText"><span class="cs">false</span><span class="vb">False</span><span class="cpp">false</span></span></span><span class="nu"><span class="keyword">false</span> (<span class="keyword">False</span> in Visual Basic)</span> if any of the following conditions occurred:</para>
        ///   <list type="bullet">
        ///     <item><see cref="Status"/> of the current or any nested subdirectory was not <see cref="DirectoryStatus.Deleted"/>.</item>
        ///     <item><see cref="DbFile.Status"/> of one or more nested files is not <see cref="FileCorrelationStatus.Deleted"/>.</item>
        ///     <item><see cref="LocalDbEntity.UpstreamId"/> of current subdirectory or any nested file or subdirectory is not null.</item>
        ///   </list>
        /// </returns>
        public async Task<bool> ExpungeAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (dbContext.Database.CurrentTransaction is null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                bool result = await ExpungeAsync(dbContext, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if (result)
                    await transaction.CommitAsync(cancellationToken);
                else
                    await transaction.RollbackAsync(cancellationToken);
                return result;
            }
            EntityEntry<Subdirectory> dbEntry = dbContext.Entry(this);
            if (Status != DirectoryStatus.Deleted || UpstreamId.HasValue || !dbEntry.ExistsInDb())
                return false;
            foreach (Subdirectory dir in (await dbEntry.GetRelatedCollectionAsync(f => f.SubDirectories, cancellationToken)).ToArray())
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!await dir.ExpungeAsync(dbContext, cancellationToken))
                    return false;
            }
            foreach (DbFile file in (await dbEntry.GetRelatedCollectionAsync(f => f.Files, cancellationToken)).ToArray())
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!await file.ExpungeAsync(dbContext, cancellationToken))
                    return false;
            }
            EntityEntry<CrawlConfiguration> oldCrawlConfiguration = await dbEntry.GetRelatedTargetEntryAsync(f => f.CrawlConfiguration, cancellationToken);
            if (oldCrawlConfiguration.ExistsInDb())
            {
                cancellationToken.ThrowIfCancellationRequested();
                dbContext.CrawlConfigurations.Remove(oldCrawlConfiguration.Entity);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            SubdirectoryAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (accessErrors.Length > 0)
                dbContext.SubdirectoryAccessErrors.RemoveRange(accessErrors);
            Guid id = Id;
            cancellationToken.ThrowIfCancellationRequested();
            await dbContext.SaveChangesAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return true;
        }

        public void SetError(LocalDbContext dbContext, ErrorCode errorCode, Exception exception, string message = null)
        {
            SetError(dbContext, errorCode.ToAccessErrorCode(), exception, message.GetDefaultIfNullOrWhiteSpace(() =>
                errorCode.TryGetDescription(out string m) ? m : errorCode.GetDisplayName()));
        }

        public void SetError(LocalDbContext dbContext, AccessErrorCode errorCode, Exception exception, string message = null)
        {
            dbContext.SubdirectoryAccessErrors.Add(new SubdirectoryAccessError()
            {
                ErrorCode = errorCode,
                Message = message.GetDefaultIfNullOrWhiteSpace(() => $"An unexpected {exception.GetType().Name} has occurred."),
                Details = exception.ToString(),
                TargetId = Id
            });
            Status = DirectoryStatus.AccessError;
        }

        internal void SetUnauthorizedAccessError([DisallowNull] LocalDbContext dbContext, [DisallowNull] UnauthorizedAccessException exception) =>
            SetError(dbContext, AccessErrorCode.UnauthorizedAccess, exception);

        internal void SetSecurityError([DisallowNull] LocalDbContext dbContext, [DisallowNull] SecurityException exception) =>
            SetError(dbContext, AccessErrorCode.SecurityException, exception);

        internal void SetPathTooLongError([DisallowNull] LocalDbContext dbContext, [DisallowNull] PathTooLongException exception) =>
            SetError(dbContext, AccessErrorCode.PathTooLong, exception);

        internal void SetIOError([DisallowNull] LocalDbContext dbContext, [DisallowNull] IOException exception) =>
            SetError(dbContext, AccessErrorCode.IOError, exception);

        internal void SetUnspecifiedError([DisallowNull] LocalDbContext dbContext, [DisallowNull] Exception exception) =>
            SetError(dbContext, AccessErrorCode.Unspecified, exception);

        public record CrawlConfigWithFullRootPath<T>(string FullName, Guid SubdirectoryId, T Source);

        public static async Task<List<CrawlConfigWithFullRootPath<T>>> BuildFullNamesAsync<T>(IEnumerable<T> source, Func<T, Subdirectory> factory, LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            List<CrawlConfigWithFullRootPath<T>> result = new();
            foreach (T t in source)
            {
                Subdirectory subdirectory = factory(t);
                if (subdirectory is null)
                    result.Add(new CrawlConfigWithFullRootPath<T>("", Guid.Empty, t));
                else
                    result.Add(new CrawlConfigWithFullRootPath<T>(await LookupFullNameAsync(subdirectory, cancellationToken, dbContext), subdirectory.Id, t));
            }
            return result;
        }

        public static async Task<string> LookupFullNameAsync([DisallowNull] Subdirectory subdirectory, CancellationToken cancellationToken, LocalDbContext dbContext = null)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            if (dbContext is null)
            {
                using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
                using LocalDbContext context = serviceScope.ServiceProvider.GetRequiredService<LocalDbContext>();
                return await LookupFullNameAsync(subdirectory, cancellationToken, context);
            }
            Guid? parentId = subdirectory.ParentId;
            if (!parentId.HasValue)
                return subdirectory.Name;
            Stack<string> segments = new();
            segments.Push(subdirectory.Name);
            while (parentId.HasValue)
            {
                Subdirectory parent = subdirectory.Parent;
                if (parent is null && (subdirectory.Parent = parent = await dbContext.Subdirectories.FindAsync(new object[] { parentId.Value }, cancellationToken)) is null)
                    break;
                segments.Push(parent.Name);
                parentId = (subdirectory = parent).ParentId;
            }
            return Path.Combine(segments.ToArray());
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        internal static void OnBuildEntity(EntityTypeBuilder<Subdirectory> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Volume).WithOne(d => d.RootDirectory).HasForeignKey<Subdirectory>(nameof(VolumeId)).OnDelete(DeleteBehavior.Restrict);//.HasPrincipalKey<Volume>(nameof(Local.Volume.Id));
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateOptions(results);
                ValidateParentAndVolume(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(Options):
                        ValidateOptions(results);
                        break;
                    case nameof(Parent):
                    case nameof(Volume):
                    case nameof(Name):
                        ValidateParentAndVolume(validationContext, results);
                        break;
                }
        }

        private void ValidateParentAndVolume(ValidationContext validationContext, List<ValidationResult> results)
        {
            Subdirectory parent = Parent;
            Volume volume = Volume;
            Guid id = Id;
            LocalDbContext dbContext;
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            if (parent is null)
            {
                if (volume is null)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, new string[] { nameof(Parent) }));
                else if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
                {
                    Guid volumeId = volume.Id;
                    var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.VolumeId == volumeId select sn;
                    if (entities.Any())
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeHasRoot, new string[] { nameof(Volume) }));
                }
            }
            else if (volume is null)
            {
                if (Id.Equals(parent.Id))
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                else if ((dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is not null)
                {
                    string name = Name;
                    if (string.IsNullOrEmpty(name))
                        results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired, new string[] { nameof(Name) }));
                    else
                    {
                        Guid parentId = parent.Id;
                        var entities = from sn in dbContext.Subdirectories where id != sn.Id && sn.ParentId == parentId && sn.Name == name select sn;
                        if (entities.Any())
                            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
                        else
                            while (parent.ParentId.HasValue)
                            {
                                if (parent.ParentId.Value.Equals(Id))
                                {
                                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_CircularReference, new string[] { nameof(Name) }));
                                    break;
                                }
                                if ((parent = dbContext.Subdirectories.Find(parent.ParentId)) is null)
                                    break;
                            }
                    }
                }
            }
            else
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeAndParent, new string[] { nameof(Volume) }));
        }

        private void ValidateOptions(List<ValidationResult> results)
        {
            if (!Enum.IsDefined(Options))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidDirectoryCrawlOption, new string[] { nameof(Options) }));
        }

        public static async Task<EntityEntry<Subdirectory>> ImportBranchAsync([DisallowNull] DirectoryInfo directoryInfo, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken, bool markNewAsCompleted = false)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            Subdirectory result;
            if (directoryInfo.Parent is null)
            {
                EntityEntry<Volume> parentVolume = await Volume.ImportVolumeAsync(directoryInfo, dbContext, cancellationToken);
                result = await parentVolume.GetRelatedReferenceAsync(v => v.RootDirectory, cancellationToken);
                if (parentVolume.State == EntityState.Added)
                {
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Name = directoryInfo.Name,
                        CreatedOn = parentVolume.Entity.CreatedOn,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        CreationTime = directoryInfo.CreationTime,
                        Volume = parentVolume.Entity,
                        Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                    };
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    if (result is not null)
                        return dbContext.Entry(result);
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Name = directoryInfo.Name,
                        CreatedOn = DateTime.Now,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        CreationTime = directoryInfo.CreationTime,
                        Volume = parentVolume.Entity,
                        Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                    };
                }
            }
            else
            {
                EntityEntry<Subdirectory> parent = await ImportBranchAsync(directoryInfo.Parent, dbContext, cancellationToken, true);
                if (parent.State == EntityState.Added)
                {
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        Parent = parent.Entity,
                        Name = directoryInfo.Name,
                        CreatedOn = parent.Entity.CreatedOn,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        CreationTime = directoryInfo.CreationTime,
                        Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                    };
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    string name = directoryInfo.Name;
                    Guid parentId = parent.Entity.Id;
                    DbFile[] files = await (from f in dbContext.Files where f.ParentId == parentId && f.Name == name && f.Status != FileCorrelationStatus.Deleted select f).ToArrayAsync(cancellationToken);
                    if (files.Length > 0)
                    {
                        string[] names = directoryInfo.GetFiles().Select(f => f.Name).ToArray();
                        if (names.Length == 0 || (files = files.Where(f => !names.Contains(f.Name)).ToArray()).Length > 0)
                        {
                            foreach (DbFile f in files)
                                f.Status = FileCorrelationStatus.Deleted;
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                    Subdirectory[] subdirectories = await (from d in dbContext.Subdirectories where d.ParentId == parentId && d.Name == name select d).ToArrayAsync(cancellationToken);
                    if (subdirectories.Length == 1)
                    {
                        result = subdirectories[0];
                        if (result.Name == name && result.CreationTime == directoryInfo.CreationTime && result.LastWriteTime == directoryInfo.LastWriteTime)
                            return dbContext.Entry(result);
                        result.ModifiedOn = result.LastAccessed = DateTime.Now;
                    }
                    else if (subdirectories.Length > 1)
                    {
                        string[] names = directoryInfo.GetFiles().Select(f => f.Name).ToArray();
                        if ((result = subdirectories.FirstOrDefault(d => d.Name == name)) is not null)
                        {
                            if ((subdirectories = subdirectories.Where(d => d.Status != DirectoryStatus.Deleted && !(ReferenceEquals(d, result) || names.Contains(d.Name))).ToArray()).Length > 0)
                            {
                                foreach (Subdirectory d in subdirectories)
                                    await d.MarkBranchDeletedAsync(dbContext, cancellationToken);
                                await dbContext.SaveChangesAsync(cancellationToken);
                            }
                            if (result.Status == DirectoryStatus.Deleted)
                                result.Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete;
                            else if (result.CreationTime == directoryInfo.CreationTime && result.LastWriteTime == directoryInfo.LastWriteTime)
                                return dbContext.Entry(result);
                            result.ModifiedOn = result.LastAccessed = DateTime.Now;
                        }
                    }
                    else
                        result = null;
                    if (result is null)
                        result = new()
                        {
                            Id = Guid.NewGuid(),
                            Parent = parent.Entity,
                            Name = name,
                            CreationTime = directoryInfo.CreationTime,
                            LastWriteTime = directoryInfo.LastWriteTime,
                            CreatedOn = DateTime.Now,
                            Status = markNewAsCompleted ? DirectoryStatus.Complete : DirectoryStatus.Incomplete
                        };
                    else
                    {
                        result.Name = name;
                        result.CreationTime = directoryInfo.CreationTime;
                        result.LastWriteTime = directoryInfo.LastWriteTime;
                        result.ModifiedOn = result.LastAccessed = DateTime.Now;
                        return dbContext.Update(result);
                    }
                }
            }
            result.ModifiedOn = result.LastAccessed = result.CreatedOn;
            return dbContext.Subdirectories.Add(result);
        }
    }
}
