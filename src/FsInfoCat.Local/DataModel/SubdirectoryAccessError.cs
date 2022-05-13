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

namespace FsInfoCat.Local
{
    public class SubdirectoryAccessError : DbEntity, ILocalSubdirectoryAccessError, IEquatable<SubdirectoryAccessError>
    {
        #region Fields

        private Guid? _id;
        private readonly SubdirectoryReference _target;
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

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_message))]
        public virtual string Message { get => _message; set => _message = value.AsWsNormalizedOrEmpty(); }

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_details))]
        public virtual string Details { get => _details; set => _details = value.EmptyIfNullOrWhiteSpace(); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ErrorCode), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public ErrorCode ErrorCode { get; set; } = ErrorCode.Unexpected;

        public virtual Guid TargetId { get => _target.Id; set => _target.SetId(value); }

        [Required]
        public Subdirectory Target { get => _target.Entity; set => _target.Entity = value; }

        #endregion

        #region Explicit Members

        IDbEntity IAccessError.Target => Target;

        ILocalSubdirectory ILocalSubdirectoryAccessError.Target => Target;

        ILocalDbEntity ILocalAccessError.Target => Target;

        ISubdirectory ISubdirectoryAccessError.Target => Target;

        #endregion

        public SubdirectoryAccessError() { _target = new(SyncRoot); }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(Message):
                    case nameof(FsInfoCat.ErrorCode):
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
            if (dbContext.SubdirectoryAccessErrors.Any(e => e.ErrorCode == errorCode && e.Message == name && id != e.Id))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateMessage, new string[] { nameof(Message) }));
        }

        internal static async Task<int> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid subdirectoryId, XElement accessErrorElement)
        {
            string n = nameof(Id);
            Guid accessErrorId = accessErrorElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(SubdirectoryAccessError)} with Id {{Id}}", accessErrorId);
            return await new InsertQueryBuilder(nameof(LocalDbContext.SubdirectoryAccessErrors), accessErrorElement, n).AppendGuid(nameof(TargetId), subdirectoryId)
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

        internal static void OnBuildEntity(EntityTypeBuilder<SubdirectoryAccessError> builder)
        {
            _ = builder.HasOne(e => e.Target).WithMany(d => d.AccessErrors).HasForeignKey(nameof(TargetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalSubdirectoryAccessError other) => ArePropertiesEqual((ISubdirectoryAccessError)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        protected bool ArePropertiesEqual([DisallowNull] ISubdirectoryAccessError other) => ErrorCode == other.ErrorCode &&
            Message == other.Message &&
            Details == other.Details;

        public bool Equals(SubdirectoryAccessError other)
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

        public bool Equals(ISubdirectoryAccessError other)
        {
            if (other is null) return false;
            if (other is SubdirectoryAccessError subdirctoryAccessError) return Equals(subdirctoryAccessError);
            Guid? id = _id;
            if (id.HasValue) return id.Value.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            return (other is ILocalSubdirectoryAccessError localAccessError) ? _target.Equals(localAccessError) && ArePropertiesEqual(localAccessError) : _target.Equals(other) && ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SubdirectoryAccessError volumeAccessError) return Equals(volumeAccessError);
            if (obj is ISubdirectoryAccessError accessError)
            {
                Guid? id = _id;
                if (id.HasValue) return id.Value.Equals(accessError.Id);
                if (accessError.Id.Equals(Guid.Empty)) return false;
                return (accessError is ILocalSubdirectoryAccessError localAccessError) ? _target.Equals(localAccessError) && ArePropertiesEqual(localAccessError) : _target.Equals(accessError) && ArePropertiesEqual(accessError);
            }
            return false;
        }

        public override int GetHashCode() => _id?.GetHashCode() ?? HashCode.Combine(_message, _details, ErrorCode, _target, CreatedOn, ModifiedOn);

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

        public bool TryGetTargetId(out Guid result) => _target.TryGetId(out result);

        protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>, IEquatable<ILocalSubdirectoryAccessError>, IEquatable<ISubdirectoryAccessError>
        {
            internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

            ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

            ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;

            public bool Equals(ILocalSubdirectoryAccessError other)
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

            public bool Equals(ISubdirectoryAccessError other)
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
        }
    }
}
