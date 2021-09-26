using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public abstract class DbFileRow : LocalDbEntity, ILocalFileRow, ISimpleIdentityReference<DbFileRow>
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<FileCorrelationStatus> _status;
        private readonly IPropertyChangeTracker<FileCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<DateTime?> _lastHashCalculation;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<DateTime> _creationTime;
        private readonly IPropertyChangeTracker<DateTime> _lastWriteTime;
        private readonly IPropertyChangeTracker<Guid> _parentId;
        private readonly IPropertyChangeTracker<Guid> _binaryPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _summaryPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _documentPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _audioPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _drmPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _gpsPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _imagePropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _mediaPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _musicPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _photoPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _recordedTVPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _videoPropertySetId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Id), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id.GetValue();
            set
            {
                if (_id.IsSet)
                {
                    Guid id = _id.GetValue();
                    if (id.Equals(value))
                        return;
                    if (!id.Equals(Guid.Empty))
                        throw new InvalidOperationException();
                }
                _id.SetValue(value);
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual FileCorrelationStatus Status { get => _status.GetValue(); set => _status.SetValue(value); }

        [Required]
        public virtual FileCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        public virtual DateTime? LastHashCalculation { get => _lastHashCalculation.GetValue(); set => _lastHashCalculation.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        public DateTime CreationTime { get => _creationTime.GetValue(); set => _creationTime.SetValue(value); }

        public DateTime LastWriteTime { get => _lastWriteTime.GetValue(); set => _lastWriteTime.SetValue(value); }

        public virtual Guid ParentId
        {
            get => _parentId.GetValue();
            set
            {
                if (_parentId.SetValue(value))
                    OnParentIdChanged(value);
            }
        }

        protected virtual void OnParentIdChanged(Guid value) { }

        public virtual Guid BinaryPropertySetId
        {
            get => _binaryPropertySetId.GetValue();
            set
            {
                if (_binaryPropertySetId.SetValue(value))
                    RaiseBinaryPropertySetIdChanged(value);
            }
        }

        protected virtual void RaiseBinaryPropertySetIdChanged(Guid value) { }

        public virtual Guid? SummaryPropertySetId
        {
            get => _summaryPropertySetId.GetValue();
            set
            {
                if (_summaryPropertySetId.SetValue(value))
                    OnSummaryPropertySetIdChanged(value);
            }
        }

        protected virtual void OnSummaryPropertySetIdChanged(Guid? value) { }

        public virtual Guid? DocumentPropertySetId
        {
            get => _documentPropertySetId.GetValue();
            set
            {
                if (_documentPropertySetId.SetValue(value))
                    OnDocumentPropertySetIdChanged(value);
            }
        }

        protected virtual void OnDocumentPropertySetIdChanged(Guid? value) { }

        public virtual Guid? AudioPropertySetId
        {
            get => _audioPropertySetId.GetValue();
            set
            {
                if (_audioPropertySetId.SetValue(value))
                    OnAudioPropertySetIdChanged(value);
            }
        }

        protected virtual void OnAudioPropertySetIdChanged(Guid? value) { }

        public virtual Guid? DRMPropertySetId
        {
            get => _drmPropertySetId.GetValue();
            set
            {
                if (_drmPropertySetId.SetValue(value))
                    OnDRMPropertySetIdChanged(value);
            }
        }

        protected virtual void OnDRMPropertySetIdChanged(Guid? value) { }

        public virtual Guid? GPSPropertySetId
        {
            get => _gpsPropertySetId.GetValue();
            set
            {
                if (_gpsPropertySetId.SetValue(value))
                    OnGPSPropertySetIdChanged(value);
            }
        }

        protected virtual void OnGPSPropertySetIdChanged(Guid? value) { }

        public virtual Guid? ImagePropertySetId
        {
            get => _imagePropertySetId.GetValue();
            set
            {
                if (_imagePropertySetId.SetValue(value))
                    OnImagePropertySetIdChanged(value);
            }
        }

        protected virtual void OnImagePropertySetIdChanged(Guid? value) { }

        public virtual Guid? MediaPropertySetId
        {
            get => _mediaPropertySetId.GetValue();
            set
            {
                if (_mediaPropertySetId.SetValue(value))
                    OnMediaPropertySetIdChanged(value);
            }
        }

        protected virtual void OnMediaPropertySetIdChanged(Guid? value) { }

        public virtual Guid? MusicPropertySetId
        {
            get => _musicPropertySetId.GetValue();
            set
            {
                if (_musicPropertySetId.SetValue(value))
                    OnMusicPropertySetIdChanged(value);
            }
        }

        protected virtual void OnMusicPropertySetIdChanged(Guid? value) { }

        public virtual Guid? PhotoPropertySetId
        {
            get => _photoPropertySetId.GetValue();
            set
            {
                if (_photoPropertySetId.SetValue(value))
                    OnPhotoPropertySetIdChanged(value);
            }
        }

        protected virtual void OnPhotoPropertySetIdChanged(Guid? value) { }

        public virtual Guid? RecordedTVPropertySetId
        {
            get => _recordedTVPropertySetId.GetValue();
            set
            {
                if (_recordedTVPropertySetId.SetValue(value))
                    OnRecordedTVPropertySetIdChanged(value);
            }
        }

        protected virtual void OnRecordedTVPropertySetIdChanged(Guid? value) { }

        public virtual Guid? VideoPropertySetId
        {
            get => _videoPropertySetId.GetValue();
            set
            {
                if (_videoPropertySetId.SetValue(value))
                    OnVideoPropertySetIdChanged(value);
            }
        }

        protected virtual void OnVideoPropertySetIdChanged(Guid? value) { }

        #endregion

        DbFileRow IIdentityReference<DbFileRow>.Entity => this;

        IDbEntity IIdentityReference.Entity => this;

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        protected DbFileRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _status = AddChangeTracker(nameof(FileCorrelationStatus), FileCorrelationStatus.Dissociated);
            _options = AddChangeTracker(nameof(FileCrawlOptions), FileCrawlOptions.None);
            _lastAccessed = AddChangeTracker(nameof(LastAccessed), CreatedOn);
            _lastHashCalculation = AddChangeTracker<DateTime?>(nameof(LastHashCalculation), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _creationTime = AddChangeTracker(nameof(CreationTime), CreatedOn);
            _lastWriteTime = AddChangeTracker(nameof(LastWriteTime), CreatedOn);
            _parentId = AddChangeTracker(nameof(ParentId), Guid.Empty);
            _binaryPropertySetId = AddChangeTracker(nameof(BinaryPropertySetId), Guid.Empty);
            _summaryPropertySetId = AddChangeTracker<Guid?>(nameof(SummaryPropertySetId), null);
            _documentPropertySetId = AddChangeTracker<Guid?>(nameof(DocumentPropertySetId), null);
            _audioPropertySetId = AddChangeTracker<Guid?>(nameof(AudioPropertySetId), null);
            _drmPropertySetId = AddChangeTracker<Guid?>(nameof(DRMPropertySetId), null);
            _gpsPropertySetId = AddChangeTracker<Guid?>(nameof(GPSPropertySetId), null);
            _imagePropertySetId = AddChangeTracker<Guid?>(nameof(ImagePropertySetId), null);
            _mediaPropertySetId = AddChangeTracker<Guid?>(nameof(MediaPropertySetId), null);
            _musicPropertySetId = AddChangeTracker<Guid?>(nameof(MusicPropertySetId), null);
            _photoPropertySetId = AddChangeTracker<Guid?>(nameof(PhotoPropertySetId), null);
            _recordedTVPropertySetId = AddChangeTracker<Guid?>(nameof(RecordedTVPropertySetId), null);
            _videoPropertySetId = AddChangeTracker<Guid?>(nameof(VideoPropertySetId), null);
        }
    }
}
