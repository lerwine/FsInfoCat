using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class RedundantSet : RedundantSetRow, ILocalRedundantSet, ISimpleIdentityReference<RedundantSet>, IEquatable<RedundantSet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        [Obsolete("Use _binaryPropertiesNav")]
        private Guid? _binaryPropertiesId;
        [Obsolete("Use _binaryPropertiesNav")]
        private BinaryPropertySet _binaryProperties;
        private readonly ForeignKeyReference<BinaryPropertySet> _binaryPropertiesNav;
        private HashSet<Redundancy> _redundancies = new();

        #endregion

        #region Properties

        public override Guid BinaryPropertiesId
        {
            get => _binaryPropertiesNav.Id;
            set => _binaryPropertiesNav.SetId(value);
        }

        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryPropertiesNav.Entity;
            set => _binaryPropertiesNav.Entity = value;
        }

        [NotNull]
        [BackingField(nameof(_redundancies))]
        public virtual HashSet<Redundancy> Redundancies { get => _redundancies; set => _redundancies = value ?? new(); }

        #endregion

        #region Explicit Members

        ILocalBinaryPropertySet ILocalRedundantSet.BinaryProperties { get => BinaryProperties; }

        IBinaryPropertySet IRedundantSet.BinaryProperties { get => BinaryProperties; }

        IEnumerable<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<IRedundancy> IRedundantSet.Redundancies => Redundancies.Cast<IRedundancy>();

        RedundantSet IIdentityReference<RedundantSet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        public RedundantSet() => _binaryPropertiesNav = new(null, SyncRoot);

        internal static void OnBuildEntity(EntityTypeBuilder<RedundantSet> builder)
        {
            _ = builder.HasOne(sn => sn.BinaryProperties).WithMany(d => d.RedundantSets).HasForeignKey(nameof(BinaryPropertiesId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        internal static async Task<(Guid redundantSetId, XElement[] redundancies)> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid binaryPropertiesId, XElement redundantSetElement)
        {
            string n = nameof(Id);
            Guid redundantSetId = redundantSetElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(RedundantSet)} with Id {{Id}}", binaryPropertiesId);
            _ = await new InsertQueryBuilder(nameof(LocalDbContext.RedundantSets), redundantSetElement, n).AppendGuid(nameof(BinaryPropertiesId), binaryPropertiesId)
                .AppendString(nameof(Reference)).AppendElementString(nameof(Notes)).AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn))
                .AppendDateTime(nameof(LastSynchronizedOn)).AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            return (redundantSetId, redundantSetElement.Elements(nameof(Redundancy)).ToArray());
        }

        public bool Equals(RedundantSet other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IRedundantSet other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public bool TryGetBinaryPropertiesId(out Guid binaryPropertiesId) => _binaryPropertiesNav.TryGetId(out binaryPropertiesId);
    }
}
