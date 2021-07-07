using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public class FileComparison : LocalDbEntity, ILocalComparison
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

        private readonly IPropertyChangeTracker<Guid> _baselineId;
        private readonly IPropertyChangeTracker<Guid> _correlativeId;
        private readonly IPropertyChangeTracker<bool> _areEqual;
        private readonly IPropertyChangeTracker<DateTime> _comparedOn;
        private readonly IPropertyChangeTracker<DbFile> _baseline;
        private readonly IPropertyChangeTracker<DbFile> _correlative;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the primary key of the baseline file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Baseline" />.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        public virtual Guid BaselineId
        {
            get => _baselineId.GetValue();
            set
            {
                if (_baselineId.SetValue(value))
                {
                    DbFile nav = _baseline.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _baseline.SetValue(null);
                }
            }
        }

        /// <summary>
        /// Gets or sets the primary key of the correlative file in the comparison.
        /// </summary>
        /// <value>The primary key of the <see cref="P:FsInfoCat.IComparison.Correlative" />, which is the new or changed file that is being compared to a <see cref="P:FsInfoCat.IComparison.Baseline" /> file.</value>
        /// <remarks>This is also part of this entity's compound primary key.</remarks>
        public virtual Guid CorrelativeId
        {
            get => _correlativeId.GetValue();
            set
            {
                if (_correlativeId.SetValue(value))
                {
                    DbFile nav = _correlative.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _correlative.SetValue(null);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="P:FsInfoCat.IComparison.Baseline" /> and <see cref="P:FsInfoCat.IComparison.Correlative" /> are identical byte-for-byte.
        /// </summary>
        /// <value><see langword="true" /> if <see cref="P:FsInfoCat.IComparison.Baseline" /> and <see cref="P:FsInfoCat.IComparison.Correlative" /> are identical byte-for-byte; otherwise, <see langword="false" />.</value>
        [Required]
        public virtual bool AreEqual { get => _areEqual.GetValue(); set => _areEqual.SetValue(value); }

        /// <summary>
        /// Gets or sets the date and time when the files were compared.
        /// </summary>
        /// <value>The date and time when <see cref="P:FsInfoCat.IComparison.Baseline" /> was compared to <see cref="P:FsInfoCat.IComparison.Correlative" />.</value>
        [Required]
        public virtual DateTime ComparedOn { get => _comparedOn.GetValue(); set => _comparedOn.SetValue(value); }

        /// <summary>
        /// Gets or sets the baseline file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="T:FsInfoCat.Local.ILocalFile" /> that represents the baseline file in the comparison.</value>
        public virtual DbFile Baseline
        {
            get => _baseline.GetValue();
            set
            {
                if (_baseline.SetValue(value))
                {
                    if (value is null)
                        _baselineId.SetValue(Guid.Empty);
                    else
                        _baselineId.SetValue(value.Id);
                }
            }
        }

        /// <summary>
        /// Gets or sets the correlative file in the comparison.
        /// </summary>
        /// <value>The generic <see cref="T:FsInfoCat.Local.ILocalFile" /> that represents the correlative file, which is the new or changed file in the comparison.</value>
        public virtual DbFile Correlative
        {
            get => _correlative.GetValue();
            set
            {
                if (_correlative.SetValue(value))
                {
                    if (value is null)
                        _correlativeId.SetValue(Guid.Empty);
                    else
                        _correlativeId.SetValue(value.Id);
                }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalComparison.Baseline { get => Baseline; }

        IFile IComparison.Baseline { get => Baseline; }

        ILocalFile ILocalComparison.Correlative { get => Correlative; }

        IFile IComparison.Correlative { get => Correlative; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileComparison"/> class.
        /// </summary>
        public FileComparison()
        {
            _baselineId = AddChangeTracker(nameof(BaselineId), Guid.Empty);
            _correlativeId = AddChangeTracker(nameof(CorrelativeId), Guid.Empty);
            _areEqual = AddChangeTracker(nameof(AreEqual), false);
            _comparedOn = AddChangeTracker(nameof(ComparedOn), CreatedOn);
            _baseline = AddChangeTracker<DbFile>(nameof(Baseline), null);
            _correlative = AddChangeTracker<DbFile>(nameof(Correlative), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(BaselineId), nameof(CorrelativeId));
            builder.HasOne(sn => sn.Baseline).WithMany(d => d.BaselineComparisons).HasForeignKey(nameof(BaselineId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Correlative).WithMany(d => d.CorrelativeComparisons).HasForeignKey(nameof(CorrelativeId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
