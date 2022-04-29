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
    public class FileComparison : LocalDbEntity, IHasMembershipKeyReference<DbFile, DbFile>, ILocalComparison, IEquatable<FileComparison>
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

        private readonly FileReference _baseline;
        private readonly FileReference _correlative;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Baseline" />.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        public virtual Guid BaselineId { get => _baseline.Id; set => _baseline.SetId(value); }

        /// <summary>
        /// Gets or sets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Correlative" />, which is the new or changed file that is being compared to a <see cref="P:FsInfoCat.IComparison.Baseline" /> file.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        public virtual Guid CorrelativeId { get => _correlative.Id; set => _correlative.SetId(value); }

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
        public virtual DbFile Baseline { get => _baseline.Entity; set => _baseline.Entity = value; }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="T:FsInfoCat.Local.ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        [BackingField(nameof(_correlative))]
        public virtual DbFile Correlative { get => _correlative.Entity; set => _correlative.Entity = value; }

        #endregion

        #region Explicit Members

        ILocalFile ILocalComparison.Baseline { get => Baseline; }

        IFile IComparison.Baseline { get => Baseline; }

        ILocalFile ILocalComparison.Correlative { get => Correlative; }

        IFile IComparison.Correlative { get => Correlative; }

        IForeignKeyReference<DbFile> IHasMembershipKeyReference<DbFile, DbFile>.Ref1 => _baseline;

        IForeignKeyReference<DbFile> IHasMembershipKeyReference<DbFile, DbFile>.Ref2 => _correlative;

        IForeignKeyReference IHasMembershipKeyReference.Ref1 => _baseline;

        IForeignKeyReference IHasMembershipKeyReference.Ref2 => _correlative;

        object ISynchronizable.SyncRoot => SyncRoot;

        (Guid, Guid) IHasIdentifierPair.Id => (_baseline.Id, _correlative.Id);

        IEnumerable<Guid> IHasCompoundIdentifier.Id
        {
            get
            {
                yield return _baseline.Id;
                yield return _correlative.Id;
            }
        }

        IForeignKeyReference<IFile> IHasMembershipKeyReference<IFile, IFile>.Ref1 => _baseline;

        IForeignKeyReference<IFile> IHasMembershipKeyReference<IFile, IFile>.Ref2 => _correlative;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalFile>.Ref1 => _baseline;

        IForeignKeyReference<ILocalFile> IHasMembershipKeyReference<ILocalFile, ILocalFile>.Ref2 => _correlative;

        #endregion

        public FileComparison()
        {
            ComparedOn = CreatedOn;
            _baseline = new(SyncRoot);
            _correlative = new(SyncRoot);
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

        public override int GetHashCode() => this.SyncDerive<DbFile, DbFile, int>((id1, id2) => HashCode.Combine(id1, id2),
            (id, file) => HashCode.Combine(AreEqual, ComparedOn, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, id, file),
            (file, id) => HashCode.Combine(AreEqual, ComparedOn, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, file, id),
            (file1, file2) => HashCode.Combine(AreEqual, ComparedOn, UpstreamId, LastSynchronizedOn, CreatedOn, ModifiedOn, file1, file2));

        public bool TryGetBaselineId(out Guid baselineId) => _baseline.TryGetId(out baselineId);

        public bool TryGetCorrelativeId(out Guid correlativeId) => _correlative.TryGetId(out correlativeId);

        protected class FileReference : ForeignKeyReference<DbFile>, IForeignKeyReference<ILocalFile>, IForeignKeyReference<IFile>
        {
            internal FileReference(object syncRoot) : base(syncRoot) { }

            ILocalFile IForeignKeyReference<ILocalFile>.Entity => Entity;

            IFile IForeignKeyReference<IFile>.Entity => Entity;
        }
    }
}
