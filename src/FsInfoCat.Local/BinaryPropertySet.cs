using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public class BinaryPropertySet : LocalDbEntity, ILocalBinaryPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<long> _length;
        private readonly IPropertyChangeTracker<MD5Hash?> _hash;
        private HashSet<DbFile> _files = new();
        private HashSet<RedundantSet> _redundantSets = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required]
        public virtual long Length { get => _length.GetValue(); set => _length.SetValue(value); }

        public virtual MD5Hash? Hash { get => _hash.GetValue(); set => _hash.SetValue(value); }

        public virtual HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        public virtual HashSet<RedundantSet> RedundantSets
        {
            get => _redundantSets;
            set => CheckHashSetChanged(_redundantSets, value, h => _redundantSets = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalBinaryPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IBinaryPropertySet.Files => Files.Cast<IFile>();

        IEnumerable<ILocalRedundantSet> ILocalBinaryPropertySet.RedundantSets => RedundantSets.Cast<ILocalRedundantSet>();

        IEnumerable<IRedundantSet> IBinaryPropertySet.RedundantSets => RedundantSets.Cast<IRedundantSet>();

        #endregion

        public BinaryPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _length = AddChangeTracker(nameof(Length), 0L);
            _hash = AddChangeTracker<MD5Hash?>(nameof(Hash), null);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (Length < 0L)
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, new string[] { nameof(Length) }));
        }

        internal static void BuildEntity(EntityTypeBuilder<BinaryPropertySet> obj) => obj.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);

        internal static async Task<(Guid redundantSetId, XElement[] redundancies)[]> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, XElement binaryPropertiesIdElement)
        {
            string n = nameof(Id);
            Guid binaryPropertiesId = binaryPropertiesIdElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(BinaryPropertySet)} with Id {{Id}}", binaryPropertiesId);
            await new InsertQueryBuilder(nameof(LocalDbContext.BinaryPropertySets), binaryPropertiesIdElement, n).AppendInt64(nameof(Length)).AppendMd5Hash(nameof(Hash))
                .AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            return binaryPropertiesIdElement.Elements(nameof(RedundantSet)).Select(e => RedundantSet.ImportAsync(dbContext, logger, binaryPropertiesId, e).Result).ToArray();
        }

        internal static async Task<BinaryPropertySet> GetBinaryPropertySetAsync(LocalDbContext dbContext, long length, CancellationToken cancellationToken)
        {
            BinaryPropertySet bps;
            if (length == 0L)
                bps = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == 0L && p.Hash != null, cancellationToken);
            else
                bps = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == length && p.Hash == null, cancellationToken);

            if (bps is null)
            {
                dbContext.BinaryPropertySets.Add(bps = new()
                {
                    Length = length,
                    Hash = (length == 0L) ? (MD5Hash?)await MD5Hash.CreateAsync(System.IO.Stream.Null, cancellationToken) : null
                });
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return bps;
        }

        internal static async Task<BinaryPropertySet> GetBinaryPropertySetAsync(LocalDbContext dbContext, long length, MD5Hash hash, CancellationToken cancellationToken)
        {
            BinaryPropertySet bps = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == length && p.Hash == hash, cancellationToken);
            if (bps is null)
            {
                dbContext.BinaryPropertySets.Add(bps = new()
                {
                    Length = length,
                    Hash = hash
                });
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return bps;
        }
    }
}
