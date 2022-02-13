using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{

    public class RedundantSet : RedundantSetRow, ILocalRedundantSet, ISimpleIdentityReference<RedundantSet>
    {
        #region Fields

        private BinaryPropertySet _binaryProperties;
        private HashSet<Redundancy> _redundancies = new();

        #endregion

        #region Properties

        public override Guid BinaryPropertiesId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _binaryProperties?.Id;
                    if (id.HasValue && id.Value != base.BinaryPropertiesId)
                    {
                        base.BinaryPropertiesId = id.Value;
                        return id.Value;
                    }
                    return base.BinaryPropertiesId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _binaryProperties?.Id;
                    if (id.HasValue && id.Value != value)
                        _binaryProperties = null;
                    base.BinaryPropertiesId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_binaryProperties is not null)
                            base.BinaryPropertiesId = Guid.Empty;
                    }
                    else
                    {
                        base.BinaryPropertiesId = value.Id;
                        _binaryProperties = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

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
    }
}
