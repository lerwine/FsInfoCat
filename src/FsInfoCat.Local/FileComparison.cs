using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
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

        private Guid? _baselineId;
        private Guid? _correlativeId;
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
            get => _baseline?.Id ?? _baselineId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_baseline is not null)
                    {
                        if (_baseline.Id.Equals(value)) return;
                        _baseline = null;
                    }
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
            get => _correlative?.Id ?? _correlativeId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_correlative is not null)
                    {
                        if (_correlative.Id.Equals(value)) return;
                        _correlative = null;
                    }
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
                    if (value is not null && _baseline is not null && ReferenceEquals(value, _baseline)) return;
                    _baselineId = null;
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
                    if (value is not null && _correlative is not null && ReferenceEquals(value, _correlative)) return;
                    _correlativeId = null;
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalComparison other) => ArePropertiesEqual(other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        protected bool ArePropertiesEqual([DisallowNull] IComparison other) => AreEqual == other.AreEqual &&
            ComparedOn == other.ComparedOn &&
            CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn;

        public bool Equals(FileComparison other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Monitor.Enter(SyncRoot);
            try
            {
                Monitor.Enter(other.SyncRoot);
                try
                {
                    throw new NotImplementedException();
                    //return AreEqual == other.AreEqual &&
                    //    ComparedOn == other.ComparedOn &&
                    //    EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
                    //    LastSynchronizedOn == other.LastSynchronizedOn &&
                    //    CreatedOn == other.CreatedOn &&
                    //    ModifiedOn == other.ModifiedOn;
                }
                finally { Monitor.Exit(other.SyncRoot); }
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public bool Equals(IComparison other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {   
            Guid? baselineId = _baseline?.Id ?? _baselineId;
            Guid? correlativeId = _correlative?.Id ?? _correlativeId;
            if (baselineId.HasValue && correlativeId.HasValue)
                return HashCode.Combine(baselineId.Value, correlativeId.Value);
            return HashCode.Combine(AreEqual, ComparedOn, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn);
        }

        public bool TryGetBaselineId(out Guid baselineId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_baseline is null)
                {
                    if (_baselineId.HasValue)
                    {
                        baselineId = _baselineId.Value;
                        return true;
                    }
                }
                else
                    return _baseline.TryGetId(out baselineId);
            }
            finally { Monitor.Exit(SyncRoot); }
            baselineId = Guid.Empty;
            return false;
        }

        public bool TryGetCorrelativeId(out Guid correlativeId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_correlative is null)
                {
                    if (_correlativeId.HasValue)
                    {
                        correlativeId = _correlativeId.Value;
                        return true;
                    }
                }
                else
                    return _correlative.TryGetId(out correlativeId);
            }
            finally { Monitor.Exit(SyncRoot); }
            correlativeId = Guid.Empty;
            return false;
        }
    }
}
