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

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class RedundantSet : RedundantSetRow, ILocalRedundantSet, IEquatable<RedundantSet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        private Guid? _binaryPropertiesId;
        private BinaryPropertySet _binaryProperties;
        private HashSet<Redundancy> _redundancies = new();

        #endregion

        #region Properties

        public override Guid BinaryPropertiesId
        {
            get => _binaryProperties?.Id ?? _binaryPropertiesId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_binaryProperties is not null)
                    {
                        if (_binaryProperties.Id.Equals(value)) return;
                        _binaryProperties = null;
                    }
                    _binaryPropertiesId = value;
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
                    if (value is not null && _binaryProperties is not null && ReferenceEquals(value, _binaryProperties)) return;
                    _binaryPropertiesId = null;
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

        public bool TryGetBinaryPropertiesId(out Guid binaryPropertiesId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_binaryProperties is null)
                {
                    if (_binaryPropertiesId.HasValue)
                    {
                        binaryPropertiesId = _binaryPropertiesId.Value;
                        return true;
                    }
                }
                else
                    return _binaryProperties.TryGetId(out binaryPropertiesId);
            }
            finally { Monitor.Exit(SyncRoot); }
            binaryPropertiesId = Guid.Empty;
            return false;
        }
    }
}
