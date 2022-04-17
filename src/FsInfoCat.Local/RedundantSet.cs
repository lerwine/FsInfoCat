using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{

    public class RedundantSet : RedundantSetRow, ILocalRedundantSet, ISimpleIdentityReference<RedundantSet>, IEquatable<RedundantSet>
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

        [BackingField(nameof(_binaryProperties))]
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
                        base.BinaryPropertiesId = value.Id;
                    _binaryProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalRedundantSet other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IRedundantSet other)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            if (Id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 19;
                    hash = (BinaryProperties is null) ? (BinaryPropertiesId.Equals(Guid.Empty) ? hash * 109 : hash * 109 + BinaryPropertiesId.GetHashCode()) : hash * 109 + (BinaryProperties?.GetHashCode() ?? 0);
                    hash = hash * 29 + Reference.GetHashCode();
                    hash = hash * 29 + Status.GetHashCode();
                    hash = hash * 29 + Notes.GetHashCode();
                    hash = UpstreamId.HasValue ? hash * 29 + (UpstreamId ?? default).GetHashCode() : hash * 29;
                    hash = LastSynchronizedOn.HasValue ? hash * 29 + (LastSynchronizedOn ?? default).GetHashCode() : hash * 29;
                    hash = hash * 29 + CreatedOn.GetHashCode();
                    hash = hash * 29 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return Id.GetHashCode();
        }
    }
}
