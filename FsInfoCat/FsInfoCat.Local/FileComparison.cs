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

        private readonly IPropertyChangeTracker<Guid> _sourceFileId;
        private readonly IPropertyChangeTracker<Guid> _targetFileId;
        private readonly IPropertyChangeTracker<bool> _areEqual;
        private readonly IPropertyChangeTracker<DateTime> _comparedOn;
        private readonly IPropertyChangeTracker<DbFile> _sourceFile;
        private readonly IPropertyChangeTracker<DbFile> _targetFile;

        #endregion

        #region Properties

        public virtual Guid SourceFileId
        {
            get => _sourceFileId.GetValue();
            set
            {
                if (_sourceFileId.SetValue(value))
                {
                    DbFile nav = _sourceFile.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _sourceFile.SetValue(null);
                }
            }
        }

        public virtual Guid TargetFileId
        {
            get => _targetFileId.GetValue();
            set
            {
                if (_targetFileId.SetValue(value))
                {
                    DbFile nav = _targetFile.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _targetFile.SetValue(null);
                }
            }
        }

        [Required]
        public virtual bool AreEqual { get => _areEqual.GetValue(); set => _areEqual.SetValue(value); }

        [Required]
        public virtual DateTime ComparedOn { get => _comparedOn.GetValue(); set => _comparedOn.SetValue(value); }

        public virtual DbFile SourceFile
        {
            get => _sourceFile.GetValue();
            set
            {
                if (_sourceFile.SetValue(value))
                {
                    if (value is null)
                        _sourceFileId.SetValue(Guid.Empty);
                    else
                        _sourceFileId.SetValue(value.Id);
                }
            }
        }

        public virtual DbFile TargetFile
        {
            get => _targetFile.GetValue();
            set
            {
                if (_targetFile.SetValue(value))
                {
                    if (value is null)
                        _targetFileId.SetValue(Guid.Empty);
                    else
                        _targetFileId.SetValue(value.Id);
                }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFile ILocalComparison.SourceFile { get => SourceFile; set => SourceFile = (DbFile)value; }

        IFile IComparison.SourceFile { get => SourceFile; set => SourceFile = (DbFile)value; }

        ILocalFile ILocalComparison.TargetFile { get => TargetFile; set => TargetFile = (DbFile)value; }

        IFile IComparison.TargetFile { get => TargetFile; set => TargetFile = (DbFile)value; }

        #endregion

        public FileComparison()
        {
            _sourceFileId = AddChangeTracker(nameof(SourceFileId), Guid.Empty);
            _targetFileId = AddChangeTracker(nameof(TargetFileId), Guid.Empty);
            _areEqual = AddChangeTracker(nameof(AreEqual), false);
            _comparedOn = AddChangeTracker(nameof(ComparedOn), CreatedOn);
            _sourceFile = AddChangeTracker<DbFile>(nameof(SourceFile), null);
            _targetFile = AddChangeTracker<DbFile>(nameof(TargetFile), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(SourceFileId), nameof(TargetFileId));
            builder.HasOne(sn => sn.SourceFile).WithMany(d => d.ComparisonSources).HasForeignKey(nameof(SourceFileId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.TargetFile).WithMany(d => d.ComparisonTargets).HasForeignKey(nameof(TargetFileId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        internal static async Task<int> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid fileId, XElement comparisonElement)
        {
            string n = nameof(TargetFileId);
            return await new InsertQueryBuilder(nameof(LocalDbContext.Files), comparisonElement, n).AppendGuid(nameof(SourceFileId), fileId)
                .AppendBoolean(nameof(AreEqual)).AppendDateTime(nameof(ComparedOn)).AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn))
                .AppendDateTime(nameof(LastSynchronizedOn)).AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
        }
    }
}
