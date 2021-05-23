using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class FileComparison : NotifyPropertyChanged, ILocalComparison
    {
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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(Properties.Resources))]
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (_createdOn.GetValue().CompareTo(_modifiedOn.GetValue()) > 0)
                result.Add(new ValidationResult($"{nameof(CreatedOn)} cannot be later than {nameof(ModifiedOn)}.", new string[] { nameof(CreatedOn) }));
            // TODO: Complete validation
            return result;
        }
    }
}
