using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class Redundancy : LocalDbEntity, ILocalRedundancy, IIdentityPairReference<Redundancy>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _fileId;
        private readonly IPropertyChangeTracker<Guid> _redundantSetId;
        private readonly IPropertyChangeTracker<string> _reference;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<DbFile> _file;
        private readonly IPropertyChangeTracker<RedundantSet> _redundantSet;

        #endregion

        #region Properties

        [Required]
        public virtual Guid FileId
        {
            get => _fileId.GetValue();
            set
            {
                if (_fileId.SetValue(value))
                {
                    DbFile nav = _file.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _ = _file.SetValue(null);
                }
            }
        }

        [Required]
        public virtual Guid RedundantSetId
        {
            get => _redundantSetId.GetValue();
            set
            {
                if (_redundantSetId.SetValue(value))
                {
                    RedundantSet nav = _redundantSet.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _ = _redundantSet.SetValue(null);
                }
            }
        }

        [Required(AllowEmptyStrings = true)]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Reference { get => _reference.GetValue(); set => _reference.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DbFile File
        {
            get => _file.GetValue();
            set
            {
                if (_file.SetValue(value))
                    _ = _fileId.SetValue(value?.Id ?? Guid.Empty);
            }
        }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RedundantSet RedundantSet
        {
            get => _redundantSet.GetValue();
            set
            {
                if (_redundantSet.SetValue(value))
                    _ = _redundantSetId.SetValue(value?.Id ?? Guid.Empty);
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

        public Redundancy()
        {
            _fileId = AddChangeTracker(nameof(FileId), Guid.Empty);
            _redundantSetId = AddChangeTracker(nameof(RedundantSetId), Guid.Empty);
            _reference = AddChangeTracker(nameof(Reference), "", TrimmedNonNullStringCoersion.Default);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _file = AddChangeTracker<DbFile>(nameof(File), null);
            _redundantSet = AddChangeTracker<RedundantSet>(nameof(RedundantSet), null);
        }

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

        public static async Task<int> DeleteAsync([DisallowNull] Redundancy target, [DisallowNull] LocalDbContext dbContext, [AllowNull] ILogger logger,
            CancellationToken cancellationToken)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            using (logger?.BeginScope(target.Id))
            {
                using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                logger?.LogInformation("Removing Redundancy {{ Id = {Id} }}", target.Id);
                EntityEntry<Redundancy> entry = dbContext.Entry(target);
                EntityEntry<RedundantSet> redundantSet = await entry.GetRelatedTargetEntryAsync(e => e.RedundantSet, cancellationToken);
                _ = dbContext.Redundancies.Remove(target);
                int result = await dbContext.SaveChangesAsync(cancellationToken);
                if ((await redundantSet.GetRelatedCollectionAsync(p => p.Redundancies, cancellationToken)).Count() > 0)
                    return result;
                logger?.LogInformation("Removing empty RedundantSet {{ Id = {Id} }}", redundantSet.Entity.Id);
                _ = dbContext.RedundantSets.Remove(redundantSet.Entity);
                result += await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
        }

        public static async Task<int> DeleteAsync([DisallowNull] Redundancy target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IStatusListener statusListener)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (statusListener is null)
                throw new ArgumentNullException(nameof(statusListener));
            return await DeleteAsync(target, dbContext, statusListener.Logger, statusListener.CancellationToken);
        }

        public static async Task<int> DeleteAsync([DisallowNull] Redundancy target, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken) =>
            await DeleteAsync(target, dbContext, null, cancellationToken);
    }
}
