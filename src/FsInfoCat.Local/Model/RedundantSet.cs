using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local.Model;

/// <summary>
/// Represents a set of files that have the same size, Hash and remediation status.
/// </summary>
/// <seealso cref="RedundantSetListItem" />
/// <seealso cref="LocalDbContext.RedundantSets" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    // CodeQL [cs/inconsistent-equals-and-gethashcode]: GetHashCode() of base class is sufficient
public partial class RedundantSet : RedundantSetRow, ILocalRedundantSet, IEquatable<RedundantSet>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    #region Fields

    private readonly BinaryPropertiesReference _binaryProperties;
    private HashSet<Redundancy> _redundancies = [];

    #endregion

    #region Properties

    public override Guid BinaryPropertiesId
    {
        get => _binaryProperties.Id;
        set => _binaryProperties.SetId(value);
    }

    public virtual BinaryPropertySet BinaryProperties
    {
        get => _binaryProperties.Entity;
        set => _binaryProperties.Entity = value;
    }

    [NotNull]
    [BackingField(nameof(_redundancies))]
    public virtual HashSet<Redundancy> Redundancies { get => _redundancies; set => _redundancies = value ?? []; }

    #endregion

    #region Explicit Members

    ILocalBinaryPropertySet ILocalRedundantSet.BinaryProperties { get => BinaryProperties; }

    IBinaryPropertySet IRedundantSet.BinaryProperties { get => BinaryProperties; }

    IEnumerable<ILocalRedundancy> ILocalRedundantSet.Redundancies => Redundancies.Cast<ILocalRedundancy>();

    IEnumerable<IRedundancy> IRedundantSet.Redundancies => Redundancies.Cast<IRedundancy>();

    #endregion

    public RedundantSet() => _binaryProperties = new(SyncRoot);

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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(RedundantSet other) => other is not null && (ReferenceEquals(this, other) ||
        (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

    public bool Equals(ILocalRedundantSet other)
    {
        if (other is null) return false;
        if (other is RedundantSet redundantSet) return Equals(redundantSet);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && ArePropertiesEqual(other);
    }

    public bool Equals(IRedundantSet other)
    {
        if (other is null) return false;
        if (other is RedundantSet redundantSet) return Equals(redundantSet);
        if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
        return !other.TryGetId(out _) && (other is ILocalRedundantSet local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (obj is RedundantSet redundantSet) return Equals(redundantSet);
        if (obj is IRedundantSet other)
        {
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalRedundantSet local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }
        return false;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    public bool TryGetBinaryPropertiesId(out Guid binaryPropertiesId) => _binaryProperties.TryGetId(out binaryPropertiesId);
}
