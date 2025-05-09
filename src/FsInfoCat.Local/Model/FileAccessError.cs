using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Database entity for file access errors
    /// </summary>
    /// <seealso cref="DbFile.AccessErrors" />
    /// <seealso cref="LocalDbContext.FileAccessErrors" />
    /// <seealso cref="DbEntity" />
    /// <seealso cref="ILocalFileAccessError" />
    /// <seealso cref="IEquatable{T}" />
    public class FileAccessError : DbEntity, ILocalFileAccessError, IEquatable<FileAccessError>
    {
        #region Fields

        private Guid? _id;
        private readonly FileReference _target;
        private string _message = string.Empty;
        private string _details = string.Empty;

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
        /// Gets the brief error message.
        /// </summary>
        /// <value>The brief error message.</value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_message))]
        public virtual string Message { get => _message; set => _message = value.AsWsNormalizedOrEmpty(); }

        /// <summary>
        /// Gets the error detail text.
        /// </summary>
        /// <value>The error detail text.</value>
        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_details))]
        public virtual string Details { get => _details; set => _details = value.EmptyIfNullOrWhiteSpace(); }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>The <see cref="ErrorCode" /> value that represents the numeric error code.</value>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.ErrorCode), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public ErrorCode ErrorCode { get; set; } = ErrorCode.Unexpected;

        /// <summary>
        /// Gets or sets the unique identifier of the associated volume.
        /// </summary>
        /// <returns>The unique identifier of the associate volume database entity.</returns>
        public virtual Guid TargetId { get => _target.Id; set => _target.SetId(value); }

        /// <summary>
        /// Gets the target entity to which the access error applies.
        /// </summary>
        /// <value>The <see cref="Volume" /> object that this error applies to.</value>
        [Required]
        public DbFile Target { get => _target.Entity; set => _target.Entity = value; }

        #endregion

        #region Explicit Members

        IDbEntity IAccessError.Target => Target;

        ILocalFile ILocalFileAccessError.Target => Target;

        ILocalDbEntity ILocalAccessError.Target => Target;

        IFile IFileAccessError.Target => Target;

        #endregion

        /// <summary>
        /// Creates a new file access error entity.
        /// </summary>
        public FileAccessError() => _target = new(SyncRoot);

        /// <summary>
        /// This gets called whenever the current entity is being validated.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="results">Contains validation results to be returned by the <see cref="DbEntity.Validate(ValidationContext)"/> method.</param>
        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(Message):
                    case nameof(ErrorCode):
                        break;
                    default:
                        return;
                }
            string name = Message;
            LocalDbContext dbContext;
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            if (string.IsNullOrEmpty(name) || (dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is null)
                return;
            Guid id = Id;
            ErrorCode errorCode = ErrorCode;
            if (dbContext.FileAccessErrors.Any(e => e.ErrorCode == errorCode && e.Message == name && id != e.Id))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateMessage, new string[] { nameof(Message) }));
        }

        internal static async Task<int> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid fileId, XElement accessErrorElement)
        {
            string n = nameof(Id);
            Guid accessErrorId = accessErrorElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(FileAccessError)} with Id {{Id}}", accessErrorId);
            return await new InsertQueryBuilder(nameof(LocalDbContext.FileAccessErrors), accessErrorElement, n).AppendGuid(nameof(TargetId), fileId)
                .AppendString(nameof(Message)).AppendInnerText(nameof(Details))
                .AppendEnum<ErrorCode>(nameof(ErrorCode)).AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).ExecuteSqlAsync(dbContext.Database);
        }

        // DEFERRED: Change to async with LocalDbContext
        internal XElement Export(bool includeTargetId = false)
        {
            XElement result = new(LocalDbEntity.ElementName_AccessError,
                new XAttribute(nameof(Id), XmlConvert.ToString(Id)),
                new XAttribute(nameof(Message), Message),
                new XAttribute(nameof(ErrorCode), Enum.GetName(ErrorCode))
            );
            if (includeTargetId)
            {
                Guid targetId = TargetId;
                if (!targetId.Equals(Guid.Empty))
                    result.SetAttributeValue(nameof(TargetId), XmlConvert.ToString(targetId));
            }
            AddExportAttributes(result);
            if (Details.Trim().Length > 0)
                result.Add(new XCData(Details));
            return result;
        }

        internal static void OnBuildEntity(EntityTypeBuilder<FileAccessError> builder)
        {
            _ = builder.HasOne(e => e.Target).WithMany(d => d.AccessErrors).HasForeignKey(nameof(TargetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalFileAccessError" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] ILocalFileAccessError other) => ArePropertiesEqual((IFileAccessError)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IFileAccessError" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected bool ArePropertiesEqual([DisallowNull] IFileAccessError other) => ErrorCode == other.ErrorCode &&
            Message == other.Message &&
            Details == other.Details;

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="FileAccessError" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(FileAccessError other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Monitor.Enter(SyncRoot);
            try
            {
                Monitor.Enter(other.SyncRoot);
                try
                {
                    if (_id.HasValue)
                        return other._id.HasValue && _id.Value == other._id.Value;
                    return !other._id.HasValue && _target.Equals(other._target) && ArePropertiesEqual(other);
                }
                finally { Monitor.Exit(other.SyncRoot); }
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ILocalFileAccessError" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ILocalFileAccessError other)
        {
            if (other is null) return false;
            if (other is FileAccessError fileAccessError) return Equals(fileAccessError);
            Guid? id = _id;
            if (id.HasValue) return id.Value.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            return !other.TryGetId(out _) && _target.Equals(other) && ArePropertiesEqual(other);
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="IFileAccessError" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(IFileAccessError other)
        {
            if (other is null) return false;
            if (other is FileAccessError fileAccessError) return Equals(fileAccessError);
            Guid? id = _id;
            if (id.HasValue) return id.Value.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            return !other.TryGetId(out _) && ((other is ILocalFileAccessError localAccessError) ? _target.Equals(localAccessError) &&
                ArePropertiesEqual(localAccessError) : _target.Equals(other) && ArePropertiesEqual(other));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is FileAccessError volumeAccessError) return Equals(volumeAccessError);
            if (obj is IFileAccessError accessError)
            {
                Guid? id = _id;
                if (id.HasValue) return id.Value.Equals(accessError.Id);
                if (accessError.Id.Equals(Guid.Empty)) return false;
                return (accessError is ILocalFileAccessError localAccessError) ? _target.Equals(localAccessError) && ArePropertiesEqual(localAccessError) : _target.Equals(accessError) && ArePropertiesEqual(accessError);
            }
            return false;
        }

        public override int GetHashCode() => _id?.GetHashCode() ?? HashCode.Combine(_message, _details, ErrorCode, _target, CreatedOn, ModifiedOn);

        public override string ToString() => $@"{{ Id={_id}, ErrorCode={ErrorCode}, TargetId={_target.IdValue},
    Message=""{ExtensionMethods.EscapeCsString(_message)}"",
    CreatedOn={CreatedOn:yyyy-mm-ddTHH:mm:ss.fffffff}, ModifiedOn={ModifiedOn:yyyy-mm-ddTHH:mm:ss.fffffff},
    Details = ""{ExtensionMethods.EscapeCsString(_details)}"" }}";
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

        /// <summary>
        /// Gets the unique identifier of the <see cref="Target" /> entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier of the <see cref="Target" /> entity has been set; otherwise, <see langword="false" />.</returns>
        public bool TryGetTargetId(out Guid result) => _target.TryGetId(out result);

        protected class FileReference : ForeignKeyReference<DbFile>, IForeignKeyReference<ILocalFile>, IForeignKeyReference<IFile>, IEquatable<ILocalFileAccessError>, IEquatable<IFileAccessError>
        {
            internal FileReference(object syncRoot) : base(syncRoot) { }

            ILocalFile IForeignKeyReference<ILocalFile>.Entity => Entity;

            IFile IForeignKeyReference<IFile>.Entity => Entity;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public bool Equals(ILocalFileAccessError other)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (Entity is null)
                    {
                        if (TryGetId(out Guid i))
                            return other.TryGetId(out Guid id) && id.Equals(i);
                        return other.Target is null && !other.TryGetTargetId(out _);
                    }
                    if (Entity.TryGetId(out Guid g))
                        return other.TryGetId(out Guid id) && id.Equals(g);
                    return other.Target is not null && Entity.Equals(other.Target);
                }
                finally { Monitor.Exit(SyncRoot); }
            }

            public bool Equals(IFileAccessError other)
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (Entity is null)
                    {
                        if (TryGetId(out Guid i))
                            return other.TryGetId(out Guid id) && id.Equals(i);
                        return other.Target is null && !other.TryGetTargetId(out _);
                    }
                    if (Entity.TryGetId(out Guid g))
                        return other.TryGetId(out Guid id) && id.Equals(g);
                    return other.Target is not null && Entity.Equals(other.Target);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

            bool IEquatable<IForeignKeyReference<ILocalFile>>.Equals(IForeignKeyReference<ILocalFile> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<IFile>>.Equals(IForeignKeyReference<IFile> other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
