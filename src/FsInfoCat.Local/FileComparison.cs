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

        private readonly ForeignKeyReference<DbFile> _baselineNav;
        private readonly ForeignKeyReference<DbFile> _correlativeNav;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Baseline" />.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        public virtual Guid BaselineId
        {
            get => _baselineNav.Id;
            set => _baselineNav.SetId(value);
        }

        /// <summary>
        /// Gets or sets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Correlative" />, which is the new or changed file that is being compared to a <see cref="P:FsInfoCat.IComparison.Baseline" /> file.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        public virtual Guid CorrelativeId
        {
            get => _correlativeNav.Id;
            set => _correlativeNav.SetId(value);
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
        public virtual DbFile Baseline
        {
            get => _baselineNav.Entity;
            set => _baselineNav.Entity = value;
        }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="T:FsInfoCat.Local.ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        public virtual DbFile Correlative
        {
            get => _correlativeNav.Entity;
            set => _correlativeNav.Entity = value;
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalComparison.Baseline { get => Baseline; }

        IFile IComparison.Baseline { get => Baseline; }

        ILocalFile ILocalComparison.Correlative { get => Correlative; }

        IFile IComparison.Correlative { get => Correlative; }

        #endregion

        public FileComparison()
        {
            ComparedOn = CreatedOn;
            _baselineNav = new(null, SyncRoot);
            _correlativeNav = new(null, SyncRoot);
        }

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
            return _baselineNav.SyncDerive(_correlativeNav, (Guid idX, Guid idY) => HashCode.Combine(idX, idY), () => HashCode.Combine(AreEqual, ComparedOn, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, _baselineNav, _correlativeNav));
        }

        public bool TryGetBaselineId(out Guid baselineId) => _baselineNav.TryGetId(out baselineId);

        public bool TryGetCorrelativeId(out Guid correlativeId) => _correlativeNav.TryGetId(out correlativeId);
    }
}
