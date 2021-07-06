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
        private readonly IPropertyChangeTracker<Guid?> _crawlConfigurationId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<CrawlConfiguration> _crawlConfiguration;
        private readonly IPropertyChangeTracker<Volume> _volume;
        private HashSet<DbFile> _files = new();
        private HashSet<Subdirectory> _subDirectories = new();
        private HashSet<SubdirectoryAccessError> _accessErrors = new();

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

        public Guid? CrawlConfigurationId
        {
            get => _crawlConfigurationId.GetValue();
            set
            {
                if (_crawlConfigurationId.SetValue(value))
                {
                    CrawlConfiguration nav = _crawlConfiguration.GetValue();
                    if (!(nav is null || (value.HasValue && nav.Id.Equals(value.Value))))
                        _crawlConfiguration.SetValue(null);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CrawlConfiguration), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public CrawlConfiguration CrawlConfiguration
        {
            get => _crawlConfiguration.GetValue();
            set
            {
                if (_crawlConfiguration.SetValue(value))
                {
                    if (value is null)
                        _crawlConfigurationId.SetValue(Guid.Empty);
                    else
                        _crawlConfigurationId.SetValue(value.Id);
                }
            }
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

        #endregion

        #region Explicit Members

        ILocalSubdirectory ILocalSubdirectory.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ISubdirectory IDbFsItem.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ILocalVolume ILocalSubdirectory.Volume { get => Volume; set => Volume = (Volume)value; }

        IVolume ISubdirectory.Volume { get => Volume; set => Volume = (Volume)value; }

        IEnumerable<ILocalFile> ILocalSubdirectory.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> ISubdirectory.Files => Files.Cast<IFile>();

        IEnumerable<ILocalSubdirectory> ILocalSubdirectory.SubDirectories => SubDirectories.Cast<ILocalSubdirectory>();

        IEnumerable<ISubdirectory> ISubdirectory.SubDirectories => SubDirectories.Cast<ISubdirectory>();

        IEnumerable<IAccessError<ILocalSubdirectory>> ILocalSubdirectory.AccessErrors => AccessErrors.Cast<IAccessError<ILocalSubdirectory>>();

        IEnumerable<IAccessError<ISubdirectory>> ISubdirectory.AccessErrors => AccessErrors.Cast<IAccessError<ISubdirectory>>();

        IEnumerable<IAccessError<ILocalDbFsItem>> ILocalDbFsItem.AccessErrors => AccessErrors.Cast<IAccessError<ILocalDbFsItem>>();

        ILocalCrawlConfiguration ILocalSubdirectory.CrawlConfiguration { get => CrawlConfiguration; set => CrawlConfiguration = (CrawlConfiguration)value; }

        ICrawlConfiguration ISubdirectory.CrawlConfiguration { get => CrawlConfiguration; set => CrawlConfiguration = (CrawlConfiguration)value; }

        IEnumerable<IAccessError> IDbFsItem.AccessErrors => throw new NotImplementedException();

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
            _crawlConfigurationId = AddChangeTracker<Guid?>(nameof(CrawlConfigurationId), null);
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

        public async Task MarkBranchIncompleteAsync(LocalDbContext dbContext, CancellationToken cancellationToken, bool doNotSaveChanges = false)
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

        public static async Task<List<((string, Guid), T)>> LoadFullNamesAsync<T>(IEnumerable<T> source, Func<T, Subdirectory> factory, LocalDbContext dbContext) => await Task.Run(() =>
        {
            Dictionary<Guid, string> fullNames = new();
            return source.Select(t =>
            {
                Subdirectory subdirectory = factory(t);
                if (subdirectory is null)
                    return (((string)null, Guid.Empty), t);
                return ((LookupFullName(fullNames, subdirectory, dbContext), subdirectory.Id), t);
            }).ToList();
        });

        public static async Task<string> LookupFullNameAsync([DisallowNull] Subdirectory subdirectory, LocalDbContext dbContext = null)
        {
            if (subdirectory is null)
                throw new ArgumentNullException(nameof(subdirectory));
            if (dbContext is null)
            {
                using LocalDbContext context = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
                return await LookupFullNameAsync(subdirectory, context);
            }
            Guid? parentId = subdirectory.ParentId;
            string path = subdirectory.Name;
            while (parentId.HasValue)
            {
                Subdirectory parent = subdirectory.Parent;
                if (parent is null && (subdirectory.Parent = parent = await dbContext.Subdirectories.FindAsync(parentId.Value)) is null)
                    break;
                path = $"{parent.Name}/{path}";
                parentId = (subdirectory = parent).ParentId;
            }
            return path;
        }

        private static string LookupFullName(Dictionary<Guid, string> fullNames, Subdirectory subdirectory, LocalDbContext dbContext)
        {
            if (fullNames.ContainsKey(subdirectory.Id))
                return fullNames[subdirectory.Id];
            Guid? parentId = subdirectory.ParentId;
            if (parentId.HasValue)
            {
                Subdirectory parent = subdirectory.Parent ?? dbContext.Find<Subdirectory>(parentId.Value);
                if (parent is not null)
                {
                    string path = $"{LookupFullName(fullNames, parent, dbContext)}/{subdirectory.Name}";
                    fullNames.Add(subdirectory.Id, path);
                    return path;
                }
            }
            fullNames.Add(subdirectory.Id, subdirectory.Name);
            return subdirectory.Name;
        }

        public static async Task<Subdirectory> FindByCrawlConfigurationAsync(Guid crawlConfigurationId) => await Task.Run(() =>
        {
            LocalDbContext dbContext = Services.ServiceProvider.GetRequiredService<LocalDbContext>();
            return FindByCrawlConfiguration(crawlConfigurationId, dbContext);
        });

        public static Subdirectory FindByCrawlConfiguration(Guid crawlConfigurationId, LocalDbContext dbContext)
        {
            return dbContext.Subdirectories.Where(s => s.CrawlConfigurationId == crawlConfigurationId).FirstOrDefault();
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        internal static void BuildEntity(EntityTypeBuilder<Subdirectory> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.SubDirectories).HasForeignKey(nameof(ParentId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Volume).WithOne(d => d.RootDirectory).HasForeignKey<Subdirectory>(nameof(VolumeId)).OnDelete(DeleteBehavior.Restrict);//.HasPrincipalKey<Volume>(nameof(Local.Volume.Id));
            builder.HasOne(s => s.CrawlConfiguration).WithOne(c => c.Root).HasForeignKey<CrawlConfiguration>(nameof(CrawlConfigurationId)).OnDelete(DeleteBehavior.Restrict);
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
            if (parent is null)
            {
                if (volume is null)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_VolumeOrParentRequired, new string[] { nameof(Parent) }));
                else if ((dbContext = validationContext.GetService<LocalDbContext>()) is not null)
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
                else if ((dbContext = validationContext.GetService<LocalDbContext>()) is not null)
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

        public static async Task<Subdirectory> ImportBranchAsync(DirectoryInfo directoryInfo, LocalDbContext dbContext, bool markNewAsCompleted = false)
        {
            Subdirectory result;
            if (directoryInfo.Parent is null)
            {
                throw new NotImplementedException();
                //Volume volume = await Volume.ImportAsync(directoryInfo, dbContext);
                //if ((result = volume.RootDirectory) is null)
                //{
                //    result = new Subdirectory
                //    {
                //        Id = Guid.NewGuid(),
                //        CreationTime = directoryInfo.CreationTime,
                //        LastWriteTime = directoryInfo.LastWriteTime,
                //        Name = directoryInfo.Name,
                //        Volume = volume,
                //        VolumeId = volume.Id
                //    };
                //    if (markNewAsCompleted)
                //        result.Status = DirectoryStatus.Complete;
                //    dbContext.Subdirectories.Add(result);
                //    volume.RootDirectory = result;
                //    await dbContext.SaveChangesAsync();
                //}
            }
            else
            {
                Subdirectory parent = await ImportBranchAsync(directoryInfo.Parent, dbContext, true);
                string name = directoryInfo.Name;
                if ((result = parent.SubDirectories.FirstOrDefault(s => s.Name == name)) is null)
                {
                    result = new Subdirectory
                    {
                        Id = Guid.NewGuid(),
                        CreationTime = directoryInfo.CreationTime,
                        LastWriteTime = directoryInfo.LastWriteTime,
                        Name = directoryInfo.Name,
                        Parent = parent,
                        ParentId = parent.Id
                    };
                    if (markNewAsCompleted)
                        result.Status = DirectoryStatus.Complete;
                    dbContext.Subdirectories.Add(result);
                    await dbContext.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
