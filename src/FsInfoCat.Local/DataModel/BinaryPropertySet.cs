using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Class BinaryPropertySet.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalBinaryPropertySet" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalBinaryPropertySet" />
    public class BinaryPropertySet : LocalDbEntity, ILocalBinaryPropertySet, IEquatable<BinaryPropertySet>
    {
        #region Fields

        private Guid? _id;
        private HashSet<DbFile> _files = new();
        private HashSet<RedundantSet> _redundantSets = new();

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

    // TODO: Document BinaryPropertySet class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        [Required]
        public virtual long Length { get; set; }

        public virtual MD5Hash? Hash { get; set; }

        [NotNull]
        [BackingField(nameof(_files))]
        public virtual HashSet<DbFile> Files { get => _files; set => _files = value ?? new(); }

        [NotNull]
        [BackingField(nameof(_redundantSets))]
        public virtual HashSet<RedundantSet> RedundantSets { get => _redundantSets; set => _redundantSets = value ?? new(); }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalBinaryPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IBinaryPropertySet.Files => Files.Cast<IFile>();

        IEnumerable<ILocalRedundantSet> ILocalBinaryPropertySet.RedundantSets => RedundantSets.Cast<ILocalRedundantSet>();

        IEnumerable<IRedundantSet> IBinaryPropertySet.RedundantSets => RedundantSets.Cast<IRedundantSet>();

        #endregion

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (Length < 0L)
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, new string[] { nameof(Length) }));
        }

        internal static void OnBuildEntity(EntityTypeBuilder<BinaryPropertySet> obj) => obj.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);

        internal static async Task<(Guid redundantSetId, XElement[] redundancies)[]> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, XElement binaryPropertiesIdElement)
        {
            string n = nameof(Id);
            Guid binaryPropertiesId = binaryPropertiesIdElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(BinaryPropertySet)} with Id {{Id}}", binaryPropertiesId);
            _ = await new InsertQueryBuilder(nameof(LocalDbContext.BinaryPropertySets), binaryPropertiesIdElement, n).AppendInt64(nameof(Length)).AppendMd5Hash(nameof(Hash))
                .AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            return binaryPropertiesIdElement.Elements(nameof(RedundantSet)).Select(e => RedundantSet.ImportAsync(dbContext, logger, binaryPropertiesId, e).Result).ToArray();
        }

        internal static async Task<BinaryPropertySet> GetBinaryPropertySetAsync(LocalDbContext dbContext, long length, CancellationToken cancellationToken)
        {
            BinaryPropertySet bps = length == 0L
                ? await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == 0L && p.Hash != null, cancellationToken)
                : await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == length && p.Hash == null, cancellationToken);
            if (bps is null)
            {
                _ = dbContext.BinaryPropertySets.Add(bps = new()
                {
                    Length = length,
                    Hash = (length == 0L) ? (MD5Hash?)await MD5Hash.CreateAsync(System.IO.Stream.Null, cancellationToken) : null
                });
                _ = await dbContext.SaveChangesAsync(cancellationToken);
            }
            return bps;
        }

        internal static async Task<BinaryPropertySet> GetBinaryPropertySetAsync(LocalDbContext dbContext, long length, MD5Hash hash, CancellationToken cancellationToken)
        {
            BinaryPropertySet bps = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == length && p.Hash == hash, cancellationToken);
            if (bps is null)
            {
                _ = dbContext.BinaryPropertySets.Add(bps = new()
                {
                    Length = length,
                    Hash = hash
                });
                _ = await dbContext.SaveChangesAsync(cancellationToken);
            }
            return bps;
        }

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalBinaryPropertySet other) => ArePropertiesEqual((IBinaryPropertySet)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IBinaryPropertySet other) => CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn &&
            Length == other.Length &&
            EqualityComparer<MD5Hash?>.Default.Equals(Hash, other.Hash);

        public bool Equals(BinaryPropertySet other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Guid? id = _id;
            if (id.HasValue) return EqualityComparer<Guid?>.Default.Equals(id, other._id);
            return !other._id.HasValue && ArePropertiesEqual(other);
        }

        public bool Equals(ILocalBinaryPropertySet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IBinaryPropertySet other)
        {
            if (other is null) return false;
            if (other is BinaryPropertySet binaryPropertySet) return Equals(binaryPropertySet);
            Guid? id = _id;
            if (id.HasValue) return id.Equals(other.Id);
            if (!other.Id.Equals(Guid.Empty)) return false;
            if (other is ILocalBinaryPropertySet localProperties)
                return ArePropertiesEqual(localProperties);
            return ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is BinaryPropertySet binaryPropertySet) return Equals(binaryPropertySet);
            if (obj is IBinaryPropertySet propertySet)
            {
                Guid? id = _id;
                if (id.HasValue) return id.Equals(propertySet.Id);
                if (!propertySet.Id.Equals(Guid.Empty)) return false;
                if (propertySet is ILocalBinaryPropertySet localProperties)
                    return ArePropertiesEqual(localProperties);
                return ArePropertiesEqual(propertySet);
            }
            return false;
        }

        public override int GetHashCode() => _id?.GetHashCode() ?? HashCode.Combine(CreatedOn, ModifiedOn, Length, Hash, UpstreamId, LastSynchronizedOn);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
