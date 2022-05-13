using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class Redundancy : LocalDbEntity, ILocalRedundancy, IIdentityPairReference<Redundancy>, IEquatable<Redundancy>
    {
        #region Fields

        private Guid? _fileId;
        private Guid? _redundantSetId;
        private string _reference;
        private string _notes;
        private DbFile _file;
        private RedundantSet _redundantSet;

        #endregion

        #region Properties

        [Required]
        [BackingField(nameof(_fileId))]
        public virtual Guid FileId
        {
            get => _file?.Id ?? _fileId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_file is not null)
                    {
                        if (_file.Id.Equals(value)) return;
                        _file = null;
                    }
                    _fileId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required]
        [BackingField(nameof(_redundantSetId))]
        public virtual Guid RedundantSetId
        {
            get => _redundantSet?.Id ?? _redundantSetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_redundantSet is not null)
                    {
                        if (_redundantSet.Id.Equals(value)) return;
                        _redundantSet = null;
                    }
                    _redundantSetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_reference))]
        public virtual string Reference { get => _reference; set => _reference = value.AsWsNormalizedOrEmpty(); }

        [Required(AllowEmptyStrings = true)]
        [NotNull]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.TrimmedOrNullIfWhiteSpace(); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_file))]
        public virtual DbFile File
        {
            get => _file;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _file is not null && ReferenceEquals(value, _file)) return;
                    _fileId = null;
                    _file = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_redundantSet))]
        public virtual RedundantSet RedundantSet
        {
            get => _redundantSet;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if ((value is null) ? _redundantSet is null : ReferenceEquals(value, _redundantSet)) return;
                    _redundantSetId = null;
                    _redundantSet = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalRedundancy.File { get => File; }

        IFile IRedundancy.File { get => File; }

        ILocalRedundantSet ILocalRedundancy.RedundantSet { get => RedundantSet; }

        IRedundantSet IRedundancy.RedundantSet { get => RedundantSet; }

        IEnumerable<Guid> IHasCompoundIdentifier.Id
        {
            get
            {
                ValueTuple<Guid, Guid> id = Id;
                yield return id.Item1;
                yield return id.Item2;
            }
        }

        private ValueTuple<Guid, Guid> Id => (RedundantSetId, FileId);

        (Guid, Guid) IHasIdentifierPair.Id => (RedundantSetId, FileId);

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            ValueTuple<Guid, Guid> id = Id;
            yield return id.Item1;
            yield return id.Item2;
        }

        Redundancy IIdentityReference<Redundancy>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        internal static void OnBuildEntity(EntityTypeBuilder<Redundancy> builder)
        {
            _ = builder.HasKey(nameof(FileId), nameof(RedundantSetId));
            _ = builder.HasOne(sn => sn.File).WithOne(d => d.Redundancy).HasForeignKey<Redundancy>(nameof(FileId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.RedundantSet).WithMany(d => d.Redundancies).HasForeignKey(nameof(RedundantSetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (!string.IsNullOrWhiteSpace(validationContext.MemberName))
                switch (validationContext.MemberName)
                {
                    case nameof(RedundantSet):
                    case nameof(File):
                        break;
                    default:
                        return;
                }
            RedundantSet redundantSet = RedundantSet;
            DbFile file = File;
            if (!(redundantSet is null || file is null || redundantSet.BinaryPropertiesId.Equals(file.BinaryPropertySetId)))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileInRedundantSet, new string[] { nameof(File) }));
        }

        internal static async Task ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid redundantSetId, XElement redundancyElement)
        {
            XName n = nameof(FileId);
            Guid fileId = redundancyElement.GetAttributeGuid(n).Value;
            StringBuilder sql = new StringBuilder("INSERT INTO \"").Append(nameof(LocalDbContext.Redundancies)).Append("\" (\"").Append(nameof(FileId)).Append("\" , \"").Append(nameof(RedundantSetId)).Append('"');
            List<object> values = new();
            values.Add(fileId);
            values.Add(redundantSetId);
            foreach (XAttribute attribute in redundancyElement.Attributes().Where(a => a.Name != n))
            {
                _ = sql.Append(", \"").Append(attribute.Name.LocalName).Append('"');
                switch (attribute.Name.LocalName)
                {
                    case nameof(Reference):
                    case nameof(Notes):
                        values.Add(attribute.Value);
                        break;
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                    case nameof(LastSynchronizedOn):
                        values.Add(XmlConvert.ToDateTime(attribute.Value, XmlDateTimeSerializationMode.RoundtripKind));
                        break;
                    case nameof(UpstreamId):
                        values.Add(XmlConvert.ToGuid(attribute.Value));
                        break;
                    default:
                        throw new NotSupportedException($"Attribute {attribute.Name} is not supported for {nameof(Redundancy)}");
                }
            }
            _ = sql.Append(") Values({0}");
            for (int i = 1; i < values.Count; i++)
                _ = sql.Append(", {").Append(i).Append('}');
            logger.LogInformation($"Inserting {nameof(Redundancy)} with FileId {{FileId}} and RedundantSetId {{RedundantSetId}}", fileId, redundantSetId);
            _ = await dbContext.Database.ExecuteSqlRawAsync(sql.Append(')').ToString(), values.ToArray());
        }

        public static async Task<int> DeleteAsync([DisallowNull] Redundancy target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] ILogger logger, CancellationToken cancellationToken)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            using (logger.BeginScope(target.Id))
            {
                using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                logger.LogInformation("Removing Redundancy {{ Id = {Id} }}", target.Id);
                EntityEntry<Redundancy> entry = dbContext.Entry(target);
                EntityEntry<RedundantSet> redundantSet = await entry.GetRelatedTargetEntryAsync(e => e.RedundantSet, cancellationToken);
                _ = dbContext.Redundancies.Remove(target);
                int result = await dbContext.SaveChangesAsync(cancellationToken);
                if ((await redundantSet.GetRelatedCollectionAsync(p => p.Redundancies, cancellationToken)).Count() > 0)
                    return result;
                logger.LogInformation("Removing empty RedundantSet {{ Id = {Id} }}", redundantSet.Entity.Id);
                _ = dbContext.RedundantSets.Remove(redundantSet.Entity);
                result += await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundancy other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IRedundancy other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Redundancy other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRedundancy other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            Guid? fileId = _file?.Id ?? _fileId;
            Guid? redundantSetId = _redundantSet?.Id ?? _redundantSetId;
            if (fileId.HasValue && redundantSetId.HasValue)
                return HashCode.Combine(fileId.Value, redundantSetId.Value);
            // TODO: Implement GetHashCode()
            throw new NotImplementedException();
        }

        public bool TryGetFileId(out Guid fileId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_file is null)
                {
                    if (_fileId.HasValue)
                    {
                        fileId = _fileId.Value;
                        return true;
                    }
                }
                else
                    return _file.TryGetId(out fileId);
            }
            finally { Monitor.Exit(SyncRoot); }
            fileId = Guid.Empty;
            return false;
        }

        public bool TryGetRedundantSetId(out Guid redundantSetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_redundantSet is null)
                {
                    if (_redundantSetId.HasValue)
                    {
                        redundantSetId = _redundantSetId.Value;
                        return true;
                    }
                }
                else
                    return _redundantSet.TryGetId(out redundantSetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            redundantSetId = Guid.Empty;
            return false;
        }
    }
}
