using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities that represent a structural instance of file.
    /// </summary>
    public abstract class DbFileRow : LocalDbEntity, ILocalFileRow
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

        /// <summary>
        /// Gets the name of the current file system item.
        /// </summary>
        /// <value>The name of the current file system item.</value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_name))]
        public virtual string Name { get => _name; set => _name = value ?? ""; }

        /// <summary>
        /// Gets the correlative status of the current file.
        /// </summary>
        /// <value>A <see cref="FileCorrelationStatus" /> value that indicates the file's correlation status.</value>
        [Required]
        public virtual FileCorrelationStatus Status { get; set; } = FileCorrelationStatus.Dissociated;

        /// <summary>
        /// Gets the visibility and crawl options for the current file.
        /// </summary>
        /// <value>A <see cref="FileCrawlOptions" /> value that contains the crawl options for the current file.</value>
        [Required]
        public virtual FileCrawlOptions Options { get; set; } = FileCrawlOptions.None;

        /// <summary>
        /// Gets the date and time last accessed.
        /// </summary>
        /// <value>The last accessed for the purposes of this application.</value>
        [Required]
        public virtual DateTime LastAccessed { get; set; }

        /// <summary>
        /// Gets the date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file.
        /// </summary>
        /// <value>The date and time that the <see cref="MD5Hash">MD5 hash</see> was calculated for the current file or <see langword="null" />
        /// if no <see cref="MD5Hash">MD5 hash</see> has been calculated, yet.</value>
        public virtual DateTime? LastHashCalculation { get; set; }

        /// <summary>
        /// Gets custom notes to be associated with the current file system item.
        /// </summary>
        /// <value>The custom notes to associate with the current file system item.</value>
        [Required(AllowEmptyStrings = true)]
        [BackingField(nameof(_notes))]
        public virtual string Notes { get => _notes; set => _notes = value.EmptyIfNullOrWhiteSpace(); }

        /// <summary>
        /// Gets the file's creation time.
        /// </summary>
        /// <value>The creation time as reported by the host file system.</value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets the date and time the file system item was last written nto.
        /// </summary>
        /// <value>The last write time as reported by the host file system.</value>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets the unique identifier of the parent subdirectory.
        /// </summary>
        /// <value>The <see cref="SubdirectoryRow.Id" /> of the parent <see cref="Subdirectory" /> entity.</value>
        public virtual Guid ParentId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated binary properties entity.
        /// </summary>
        /// <value>The <see cref="BinaryPropertySet.Id" /> of the <see cref="BinaryPropertySet" /> that has the length and MD5 hash that matches the current file.</value>
        public virtual Guid BinaryPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated summary properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="SummaryPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no summary properties.</value>
        public virtual Guid? SummaryPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated document properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="DocumentPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no document properties.</value>
        public virtual Guid? DocumentPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated audio properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="AudioPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no audio properties.</value>
        public virtual Guid? AudioPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated DRM properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="DRMPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no DRM properties.</value>
        public virtual Guid? DRMPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated GPS properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="GPSPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no GPS properties.</value>
        public virtual Guid? GPSPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated image properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="ImagePropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no image properties.</value>
        public virtual Guid? ImagePropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated media properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="MediaPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no media properties.</value>
        public virtual Guid? MediaPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated music properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="MusicPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no music properties.</value>
        public virtual Guid? MusicPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated photo properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="PhotoPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no photo properties.</value>
        public virtual Guid? PhotoPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated recorded TV properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="RecordedTVPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no recorded TV properties.</value>
        public virtual Guid? RecordedTVPropertySetId { get; set; }

        /// <summary>
        /// Gets unique identifier of the associated video properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="VideoPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no video properties.</value>
        public virtual Guid? VideoPropertySetId { get; set; }

        #endregion

        /// <summary>
        /// Creates a new file row database entity.
        /// </summary>
        protected DbFileRow()
        {
            CreationTime = LastWriteTime = LastAccessed = CreatedOn;
        }

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="ILocalFileRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
        protected virtual bool ArePropertiesEqual([DisallowNull] ILocalFileRow other) => ArePropertiesEqual((IFileRow)other) && EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) && LastSynchronizedOn == other.LastSynchronizedOn;

        /// <summary>
        /// Checks for equality by comparing property values.
        /// </summary>
        /// <param name="other">The other <see cref="IFileRow" /> to compare to.</param>
        /// <returns><see langword="true" /> if properties are equal; otherwise, <see langword="false" />.</returns>
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override int GetHashCode()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

        /// <summary>
        /// Gets the unique identifier of the current entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the <see cref="Id" /> property has been set; otherwise, <see langword="false" />.</returns>
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
    }
}
