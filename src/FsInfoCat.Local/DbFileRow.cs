using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local
{
    public abstract class DbFileRow : LocalDbEntity, ILocalFileRow, ISimpleIdentityReference<DbFileRow>, IEquatable<DbFileRow>
    {
        #region Fields

        private Guid? _id;
        private string _name = string.Empty;
        private string _notes = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_id.HasValue)
                    {
                        if (!_id.Value.Equals(value))
                            throw new InvalidOperationException();
                    }
                    else if (value.Equals(Guid.Empty))
                        return;
                    _id = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        [Required]
        public virtual FileCorrelationStatus Status { get; set; } = FileCorrelationStatus.Dissociated;

        [Required]
        public virtual FileCrawlOptions Options { get; set; } = FileCrawlOptions.None;

        [Required]
        public virtual DateTime LastAccessed { get; set; }

        public virtual DateTime? LastHashCalculation { get; set; }

        [Required(AllowEmptyStrings = true)]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        public DateTime CreationTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public virtual Guid ParentId { get; set; }

        public virtual Guid BinaryPropertySetId { get; set; }

        public virtual Guid? SummaryPropertySetId { get; set; }

        public virtual Guid? DocumentPropertySetId { get; set; }

        public virtual Guid? AudioPropertySetId { get; set; }

        public virtual Guid? DRMPropertySetId { get; set; }

        public virtual Guid? GPSPropertySetId { get; set; }

        public virtual Guid? ImagePropertySetId { get; set; }

        public virtual Guid? MediaPropertySetId { get; set; }

        public virtual Guid? MusicPropertySetId { get; set; }

        public virtual Guid? PhotoPropertySetId { get; set; }

        public virtual Guid? RecordedTVPropertySetId { get; set; }

        public virtual Guid? VideoPropertySetId { get; set; }

        #endregion

        protected DbFileRow()
        {
            CreationTime = LastWriteTime = LastAccessed = CreatedOn;
        }

        DbFileRow IIdentityReference<DbFileRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalFileRow other) => ArePropertiesEqual((IFileRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        protected virtual bool ArePropertiesEqual([DisallowNull] IFileRow other) => ArePropertiesEqual(other) && CreatedOn == other.CreatedOn &&
            ModifiedOn == other.ModifiedOn &&
            _name == other.Name &&
            LastAccessed == other.LastAccessed &&
            _notes == other.Notes &&
            CreationTime == other.CreationTime &&
            LastWriteTime == other.LastWriteTime &&
            Options == other.Options &&
            Status == other.Status &&
            LastHashCalculation == other.LastHashCalculation &&
            ParentId.Equals(other.ParentId) &&
            BinaryPropertySetId.Equals(other.BinaryPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(SummaryPropertySetId, other.SummaryPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(DocumentPropertySetId, other.DocumentPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(AudioPropertySetId, other.AudioPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(DRMPropertySetId, other.DRMPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(GPSPropertySetId, other.GPSPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(ImagePropertySetId, other.ImagePropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(MediaPropertySetId, other.MediaPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(MusicPropertySetId, other.MusicPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(PhotoPropertySetId, other.PhotoPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(RecordedTVPropertySetId, other.RecordedTVPropertySetId) &&
            EqualityComparer<Guid?>.Default.Equals(VideoPropertySetId, other.VideoPropertySetId);

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }

        public override int GetHashCode()
        {
            Guid? id = _id;
            if (id.HasValue) return id.Value.GetHashCode();
            HashCode hash = new();
            hash.Add(_name);
            hash.Add(Status);
            hash.Add(Options);
            hash.Add(LastAccessed);
            hash.Add(LastHashCalculation);
            hash.Add(_notes);
            hash.Add(CreationTime);
            hash.Add(LastWriteTime);
            hash.Add(ParentId);
            hash.Add(BinaryPropertySetId);
            hash.Add(SummaryPropertySetId);
            hash.Add(DocumentPropertySetId);
            hash.Add(AudioPropertySetId);
            hash.Add(DRMPropertySetId);
            hash.Add(GPSPropertySetId);
            hash.Add(ImagePropertySetId);
            hash.Add(MediaPropertySetId);
            hash.Add(MusicPropertySetId);
            hash.Add(PhotoPropertySetId);
            hash.Add(RecordedTVPropertySetId);
            hash.Add(VideoPropertySetId);
            hash.Add(UpstreamId);
            hash.Add(LastSynchronizedOn);
            hash.Add(CreatedOn);
            hash.Add(ModifiedOn);
            return hash.ToHashCode();
        }

        public bool TryGetId(out Guid result)
        {
            Guid? id = _id;
            if (id.HasValue)
            {
                result = id.Value;
                return true;
            }
            result = Guid.Empty;
            return false;
        }

        public virtual bool Equals(DbFileRow other) => other is not null && (ReferenceEquals(this, other) || (_id?.Equals(other.Id) ?? !other._id.HasValue && ArePropertiesEqual(other)));

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is DbFileRow fileRow) return Equals(fileRow);
            if (obj is not IFileRow row) return false;
            Guid? id = _id;
            if (id.HasValue) return id.Value.Equals(row.Id);
            if (row.Id.Equals(Guid.Empty)) return false;
            if (row is ILocalFileRow localRow) return ArePropertiesEqual(localRow);
            if (row is IFile file) return ArePropertiesEqual(file);
            return ArePropertiesEqual(row);
        }
    }
}
