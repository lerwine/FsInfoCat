using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Class FileComparison.
    /// Implements the <see cref="LocalDbEntity" />
    /// Implements the <see cref="ILocalComparison" />
    /// </summary>
    /// <seealso cref="LocalDbEntity" />
    /// <seealso cref="ILocalComparison" />
    [Table(TABLE_NAME)]
    public class FileComparison : LocalDbEntity, ILocalComparison, IEquatable<FileComparison>
    {
        #region Fields

        /// <summary>
        /// The table name
        /// </summary>
        public const string TABLE_NAME = "Comparisons";

        /// <summary>
        /// The element name
        /// </summary>
        public const string ELEMENT_NAME = "Comparison";

        private Guid _baselineId;
        private Guid _correlativeId;
        private DbFile _baseline;
        private DbFile _correlative;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Baseline" />.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [BackingField(nameof(_baselineId))]
        public virtual Guid BaselineId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _baseline?.Id;
                    if (id.HasValue && id.Value != _baselineId)
                    {
                        _baselineId = id.Value;
                        return id.Value;
                    }
                    return _baselineId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _baseline?.Id;
                    if (id.HasValue && id.Value != value)
                        _baseline = null;
                    _baselineId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets or sets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Correlative" />, which is the new or changed file that is being compared to a <see cref="P:FsInfoCat.IComparison.Baseline" /> file.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        [BackingField(nameof(_correlativeId))]
        public virtual Guid CorrelativeId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _correlative?.Id;
                    if (id.HasValue && id.Value != _correlativeId)
                    {
                        _correlativeId = id.Value;
                        return id.Value;
                    }
                    return _correlativeId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _correlative?.Id;
                    if (id.HasValue && id.Value != value)
                        _correlative = null;
                    _correlativeId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="P:FsInfoCat.IComparison.Baseline" /> and <see cref="P:FsInfoCat.IComparison.Correlative" /> are identical byte-for-byte.
        /// </summary>
        /// <value><see langword="true" /> if <see cref="P:FsInfoCat.IComparison.Baseline" /> and <see cref="P:FsInfoCat.IComparison.Correlative" /> are identical byte-for-byte; otherwise, <see langword="false" />.</value>
        [Required]
        public virtual bool AreEqual { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the files were compared.
        /// </summary>
        /// <value>The date and time when <see cref="P:FsInfoCat.IComparison.Baseline" /> was compared to <see cref="P:FsInfoCat.IComparison.Correlative" />.</value>
        [Required]
        public virtual DateTime ComparedOn { get; set; }

        /// <summary>
        /// Gets or sets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="T:FsInfoCat.Local.ILocalFile" /> that represents the baseline file in the comparison.</value>
        [BackingField(nameof(_baseline))]
        public virtual DbFile Baseline
        {
            get => _baseline;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_baseline is not null)
                            _baselineId = Guid.Empty;
                    }
                    else
                        _baselineId = value.Id;
                    _baseline = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="T:FsInfoCat.Local.ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [BackingField(nameof(_correlative))]
        public virtual DbFile Correlative
        {
            get => _correlative;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_correlative is not null)
                            _correlativeId = Guid.Empty;
                    }
                    else
                        _correlativeId = value.Id;
                    _correlative = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalComparison.Baseline { get => Baseline; }

        IFile IComparison.Baseline { get => Baseline; }

        ILocalFile ILocalComparison.Correlative { get => Correlative; }

        IFile IComparison.Correlative { get => Correlative; }

        #endregion

        public FileComparison() { ComparedOn = CreatedOn; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            _ = builder.HasKey(nameof(BaselineId), nameof(CorrelativeId));
            _ = builder.HasOne(sn => sn.Baseline).WithMany(d => d.BaselineComparisons).HasForeignKey(nameof(BaselineId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.Correlative).WithMany(d => d.CorrelativeComparisons).HasForeignKey(nameof(CorrelativeId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalComparison other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IComparison other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(FileComparison other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            DbFile b1 = Baseline;
            DbFile b2 = other.Baseline;
            if (b1 is null)
            {
                if (b2 is null)
                {
                    if (other.BaselineId.Equals(Guid.Empty))
                        return BaselineId.Equals(Guid.Empty) && ArePropertiesEqual(other);
                    return BaselineId.Equals(other.BaselineId);
                }
                return !BaselineId.Equals(Guid.Empty) && BaselineId.Equals(b2.Id);
            }
            if (b2 is null)
                return !other.BaselineId.Equals(Guid.Empty) && other.BaselineId.Equals(b1.Id);
            if (!b1.Equals(b2))
                return false;
            b1 = Correlative;
            b2 = other.Correlative;
            if (b1 is null)
            {
                if (b2 is null)
                {
                    if (other.BaselineId.Equals(Guid.Empty))
                        return BaselineId.Equals(Guid.Empty) && ArePropertiesEqual(other);
                    return BaselineId.Equals(other.BaselineId);
                }
                return !BaselineId.Equals(Guid.Empty) && BaselineId.Equals(b2.Id);
            }
            if (b2 is null)
                return !other.BaselineId.Equals(Guid.Empty) && other.BaselineId.Equals(b1.Id);
            return b1.Equals(b2);
        }

        public bool Equals(IComparison other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                hash = EntityExtensions.HashRelatedEntity(Baseline, () => BaselineId, hash, 29);
                hash = EntityExtensions.HashRelatedEntity(Correlative, () => CorrelativeId, hash, 29);
                hash = hash * 29 + AreEqual.GetHashCode();
                hash = hash * 29 + ComparedOn.GetHashCode();
                hash = EntityExtensions.HashNullable(UpstreamId, hash, 29);
                hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 29);
                hash = hash * 29 + CreatedOn.GetHashCode();
                hash = hash * 29 + ModifiedOn.GetHashCode();
                return hash;
            }
        }
    }
}
