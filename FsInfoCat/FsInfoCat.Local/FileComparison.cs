using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class FileComparison : NotifyPropertyChanged, ILocalComparison
    {
        /*
    "SourceFileId" UNIQUEIDENTIFIER NOT NULL,
    "TargetFileId" UNIQUEIDENTIFIER NOT NULL,
    "AreEqual" BIT NOT NULL DEFAULT 0,
    "UpstreamId" UNIQUEIDENTIFIER DEFAULT NULL,
    "LastSynchronizedOn" DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now','localtime')),
	CONSTRAINT "PK_Comparisons" PRIMARY KEY("SourceFileId","TargetFileId"),
	CONSTRAINT "FK_ComparisonSourceFile" FOREIGN KEY("SourceFileId") REFERENCES "Files"("Id"),
	CONSTRAINT "FK_ComparisonTargetFile" FOREIGN KEY("TargetFileId") REFERENCES "Files"("Id"),
    CHECK(CreatedOn<=ModifiedOn AND SourceFileId<>TargetFileId AND
        (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL))
         */
        #region Fields

        public const string TABLE_NAME = "Comparisons";

        private readonly IPropertyChangeTracker<Guid> _sourceFileId;
        private readonly IPropertyChangeTracker<Guid> _targetFileId;
        private readonly IPropertyChangeTracker<bool> _areEqual;
        private readonly IPropertyChangeTracker<DateTime> _comparedOn;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<DbFile> _sourceFile;
        private readonly IPropertyChangeTracker<DbFile> _targetFile;

        #endregion

        #region Properties

        /// <remarks>UNIQUEIDENTIFIER NOT NULL</remarks>
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

        /// <remarks>UNIQUEIDENTIFIER NOT NULL</remarks>
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

        /// <remarks>UNIQUEIDENTIFIER DEFAULT NULL</remarks>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// DATETIME NOT NULL DEFAULT (datetime('now','localtime'))
        /// </remarks>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// DATETIME NOT NULL DEFAULT (datetime('now','localtime'))
        /// </remarks>
        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

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
            _sourceFileId = CreateChangeTracker(nameof(SourceFileId), Guid.Empty);
            _targetFileId = CreateChangeTracker(nameof(TargetFileId), Guid.Empty);
            _areEqual = CreateChangeTracker(nameof(AreEqual), false);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _comparedOn = CreateChangeTracker(nameof(ComparedOn), _createdOn.GetValue());
            _sourceFile = CreateChangeTracker<DbFile>(nameof(SourceFile), null);
            _targetFile = CreateChangeTracker<DbFile>(nameof(TargetFile), null);
        }

        public bool IsNew() => !(_sourceFileId.IsSet & _targetFileId.IsSet);

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) :
            (other is ILocalComparison entity && SourceFileId.Equals(entity.SourceFileId) && TargetFileId.Equals(entity.TargetFileId));

        internal static void BuildEntity(EntityTypeBuilder<FileComparison> builder)
        {
            builder.HasKey(nameof(SourceFileId), nameof(TargetFileId));
            builder.HasOne(sn => sn.SourceFile).WithMany(d => d.ComparisonSources).HasForeignKey(nameof(SourceFileId)).IsRequired();
            builder.HasOne(sn => sn.TargetFile).WithMany(d => d.ComparisonTargets).HasForeignKey(nameof(TargetFileId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnBeforeValidate, OnValidate);

        private void OnBeforeValidate(EntityEntry<FileComparison> entityEntry, LocalDbContext dbContext)
        {
            // TODO: Finish validation
            throw new NotImplementedException();
        }

        private void OnValidate(EntityEntry<FileComparison> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            // TODO: Finish validation
            throw new NotImplementedException();
        }
    }
}
