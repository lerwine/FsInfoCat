using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{

    public class RedundantSet : RedundantSetRow, ILocalRedundantSet, ISimpleIdentityReference<RedundantSet>
    {
        #region Fields

        private readonly IPropertyChangeTracker<BinaryPropertySet> _binaryProperties;
        private HashSet<Redundancy> _redundancies = new();

        #endregion

        #region Properties

        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties.GetValue();
            set
            {
                if (_binaryProperties.SetValue(value))
                    BinaryPropertiesId = value?.Id ?? Guid.Empty;
            }
        }

        public virtual HashSet<Redundancy> Redundancies
        {
            get => _redundancies;
            set => CheckHashSetChanged(_redundancies, value, h => _redundancies = h);
        }

        #endregion

        #region Explicit Members

        ILocalBinaryPropertySet ILocalRedundantSet.BinaryProperties { get => BinaryProperties; }

        IBinaryPropertySet IRedundantSet.BinaryProperties { get => BinaryProperties; }

        IEnumerable<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<IRedundancy> IRedundantSet.Redundancies => Redundancies.Cast<IRedundancy>();

        RedundantSet IIdentityReference<RedundantSet>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        #endregion

        public RedundantSet()
        {
            _binaryProperties = AddChangeTracker<BinaryPropertySet>(nameof(BinaryProperties), null);
        }

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

        protected override void OnBinaryPropertiesIdChanged(Guid value)
        {
            BinaryPropertySet nav = _binaryProperties.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _ = _binaryProperties.SetValue(null);
        }
    }
}
