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
    public class Redundancy : LocalDbEntity, IHasMembershipKeyReference<RedundantSet, DbFile>, ILocalRedundancy, IEquatable<Redundancy>
    {
        #region Fields

        private string _reference;
        private string _notes;
        private readonly FileReference _file;
        private readonly RedundantSetReference _redundantSet;

        #endregion

        #region Properties

        [Required]
        public virtual Guid FileId { get => _file.Id; set => _file.SetId(value); }

        [Required]
        public virtual Guid RedundantSetId { get => _redundantSet.Id; set => _redundantSet.SetId(value); }

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
        public virtual DbFile File { get => _file.Entity; set => _file.Entity = value; }

        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_RedundantSetRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RedundantSet RedundantSet { get => _redundantSet.Entity; set => _redundantSet.Entity = value; }

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
                yield return _redundantSet.Id;
                yield return _file.Id;
            }
        }

        (Guid, Guid) IHasIdentifierPair.Id => (_redundantSet.Id, _file.Id);

        IForeignKeyReference<RedundantSet> IHasMembershipKeyReference<RedundantSet, DbFile>.Ref1 => _redundantSet;

        IForeignKeyReference<DbFile> IHasMembershipKeyReference<RedundantSet, DbFile>.Ref2 => _file;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _redundantSet;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _file;

        object ISynchronizable.SyncRoot => SyncRoot;

        IForeignKeyReference<IRedundantSet> IHasMembershipKeyReference<IRedundantSet, IFile>.Ref1 => _redundantSet;

        IForeignKeyReference<IFile> IHasMembershipKeyReference<IRedundantSet, IFile>.Ref2 => _file;

        IForeignKeyReference<ILocalRedundantSet> IHasMembershipKeyReference<ILocalRedundantSet, ILocalFile>.Ref1 => _redundantSet;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalRedundantSet, ILocalFile>.Ref2 => _file;

        #endregion

        public Redundancy()
        {
            _redundantSet = new(SyncRoot);
            _file = new(SyncRoot);
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

        public static async Task<int> DeleteAsync([DisallowNull] Redundancy target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] ILogger logger, CancellationToken cancellationToken)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            using (logger.BeginScope((target._redundantSet.Id, target._file.Id)))
            {
                using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                logger.LogInformation("Removing Redundancy {{ Id = {Id} }}", (target._redundantSet.Id, target._file.Id));
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
            // TODO: Implement ArePropertiesEqual(ILocalRedundancy)
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IRedundancy other)
        {
            // TODO: Implement ArePropertiesEqual(IRedundancy)
            throw new NotImplementedException();
        }

        public bool Equals(Redundancy other)
        {
            // TODO: Implement Equals(Redundancy)
            throw new NotImplementedException();
        }

        public bool Equals(ILocalRedundancy other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRedundancy other)
        {
            // TODO: Implement Equals(IRedundancy)
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode() => this.SyncDerive<RedundantSet, DbFile, int>((id1, id2) => HashCode.Combine(id1, id2),
            (id, file) => HashCode.Combine(_reference, _notes, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, id, file),
            (rs, id) => HashCode.Combine(_reference, _notes, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, rs, id),
            (rs, file) => HashCode.Combine(_reference, _notes, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, rs, file));

        public bool TryGetFileId(out Guid fileId) => _file.TryGetId(out fileId);

        public bool TryGetRedundantSetId(out Guid redundantSetId) => _redundantSet.TryGetId(out redundantSetId);

        protected class FileReference : ForeignKeyReference<DbFile>, IForeignKeyReference<ILocalFile>, IForeignKeyReference<IFile>
        {
            internal FileReference(object syncRoot) : base(syncRoot) { }

            ILocalFile IForeignKeyReference<ILocalFile>.Entity => Entity;

            IFile IForeignKeyReference<IFile>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IFile>>.Equals(IForeignKeyReference<IFile> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalFile>>.Equals(IForeignKeyReference<ILocalFile> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class RedundantSetReference : ForeignKeyReference<RedundantSet>, IForeignKeyReference<ILocalRedundantSet>, IForeignKeyReference<IRedundantSet>
        {
            internal RedundantSetReference(object syncRoot) : base(syncRoot) { }

            ILocalRedundantSet IForeignKeyReference<ILocalRedundantSet>.Entity => Entity;

            IRedundantSet IForeignKeyReference<IRedundantSet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IRedundantSet>>.Equals(IForeignKeyReference<IRedundantSet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalRedundantSet>>.Equals(IForeignKeyReference<ILocalRedundantSet> other)
            {
                throw new NotImplementedException();
            }
        }
    }
}