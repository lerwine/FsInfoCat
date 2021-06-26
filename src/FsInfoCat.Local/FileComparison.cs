using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class FileComparison : LocalDbEntity, ILocalComparison
    {
        #region Fields

        public const string TABLE_NAME = "Comparisons";

        public const string ELEMENT_NAME = "Comparison";

        private readonly IPropertyChangeTracker<Guid> _baselineId;
        private readonly IPropertyChangeTracker<Guid> _correlativeId;
        private readonly IPropertyChangeTracker<bool> _areEqual;
        private readonly IPropertyChangeTracker<DateTime> _comparedOn;
        private readonly IPropertyChangeTracker<DbFile> _baseline;
        private readonly IPropertyChangeTracker<DbFile> _correlative;

        #endregion

        #region Properties

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

        [Required]
        public virtual bool AreEqual { get => _areEqual.GetValue(); set => _areEqual.SetValue(value); }

        [Required]
        public virtual DateTime ComparedOn { get => _comparedOn.GetValue(); set => _comparedOn.SetValue(value); }

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

        ILocalFile ILocalComparison.Baseline { get => Baseline; set => Baseline = (DbFile)value; }

        IFile IComparison.Baseline { get => Baseline; set => Baseline = (DbFile)value; }

        ILocalFile ILocalComparison.Correlative { get => Correlative; set => Correlative = (DbFile)value; }

        IFile IComparison.Correlative { get => Correlative; set => Correlative = (DbFile)value; }

        #endregion

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
            builder.HasOne(sn => sn.Baseline).WithMany(d => d.ComparisonSources).HasForeignKey(nameof(BaselineId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Correlative).WithMany(d => d.ComparisonTargets).HasForeignKey(nameof(CorrelativeId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        internal static async Task<int> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid fileId, XElement comparisonElement)
        {
            string n = nameof(CorrelativeId);
            return await new InsertQueryBuilder(nameof(LocalDbContext.Files), comparisonElement, n).AppendGuid(nameof(BaselineId), fileId)
                .AppendBoolean(nameof(AreEqual)).AppendDateTime(nameof(ComparedOn)).AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn))
                .AppendDateTime(nameof(LastSynchronizedOn)).AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
        }
    }
}
