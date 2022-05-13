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
    public class VolumeAccessError : DbEntity, ILocalVolumeAccessError, ISimpleIdentityReference<VolumeAccessError>, IEquatable<VolumeAccessError>
    {
        #region Fields

        private Guid? _id;
        private string _message = string.Empty;
        private string _details = string.Empty;
        private readonly ForeignKeyReference<Volume> _targetNav;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_id))]
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

        public virtual Guid TargetId
        {
            get => _targetNav.Id;
            set => _targetNav.SetId(value);
        }

        [Required]
        public Volume Target
        {
            get => _targetNav.Entity;
            set => _targetNav.Entity = value;
        }

        #endregion

        #region Explicit Members

        IDbEntity IAccessError.Target => Target;

        ILocalVolume ILocalVolumeAccessError.Target => Target;

        ILocalDbEntity ILocalAccessError.Target => Target;

        IVolume IVolumeAccessError.Target => Target;

        VolumeAccessError IIdentityReference<VolumeAccessError>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        public VolumeAccessError() => _targetNav = new(null, SyncRoot);

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(ErrorCode):
                    case nameof(Message):
                        break;
                    default:
                        return;
                }
            string message = Message;
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            LocalDbContext dbContext;
            if (string.IsNullOrEmpty(Message) || (dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is null)
                return;
            Guid id = Id;
            ErrorCode errorCode = ErrorCode;
            if (dbContext.VolumeAccessErrors.Any(e => e.ErrorCode == errorCode && e.Message == message && id != e.Id))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateMessage, new string[] { nameof(Message) }));
        }

        internal static async Task<int> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid volumeId, XElement accessErrorElement)
        {
            string n = nameof(Id);
            Guid accessErrorId = accessErrorElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(VolumeAccessError)} with Id {{Id}}", accessErrorId);
            return await new InsertQueryBuilder(nameof(LocalDbContext.VolumeAccessErrors), accessErrorElement, n).AppendGuid(nameof(TargetId), volumeId)
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

        internal static void OnBuildEntity(EntityTypeBuilder<VolumeAccessError> builder)
        {
            _ = builder.HasOne(e => e.Target).WithMany(d => d.AccessErrors).HasForeignKey(nameof(TargetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalVolumeAccessError other) => ArePropertiesEqual((IVolumeAccessError)other) &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        protected virtual bool ArePropertiesEqual([DisallowNull] IVolumeAccessError other) => ErrorCode == other.ErrorCode &&
            Message == other.Message &&
            Details == other.Details;

        public bool Equals(VolumeAccessError other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid idX) ? other.TryGetId(out Guid idY) && idX.Equals(idY) :
            !other.TryGetId(out _) && _targetNav.Equals(other._targetNav) && ArePropertiesEqual(this)));

        public bool Equals(IVolumeAccessError other)
        {
            if (other is null) return false;
            if (other is VolumeAccessError volumeAccessError) return Equals(volumeAccessError);
            Guid? id = _id;
            if (id.HasValue) return id.Value.Equals(other.Id);
            if (other.Id.Equals(Guid.Empty)) return false;
            return (other is ILocalVolumeAccessError localAccessError) ? ArePropertiesEqual(localAccessError) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is VolumeAccessError volumeAccessError) return Equals(volumeAccessError);
            if (obj is IVolumeAccessError accessError)
            {
                Guid? id = _id;
                if (id.HasValue) return id.Value.Equals(accessError.Id);
                if (accessError.Id.Equals(Guid.Empty)) return false;
                return (accessError is ILocalVolumeAccessError localAccessError) ? ArePropertiesEqual(localAccessError) : ArePropertiesEqual(accessError);
            }
            return false;
        }

        public override int GetHashCode() => _id?.GetHashCode() ?? HashCode.Combine(_message, _details, ErrorCode, TargetId, CreatedOn, ModifiedOn);

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

        public bool TryGetTargetId(out Guid result) => _targetNav.TryGetId(out result);
    }
}
