using FsInfoCat.Desktop.FileSystemDetail;
using FsInfoCat.Local;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    class FileDetailProvider : IFileDetailProvider
    {
        private bool _isDisposed;
        private readonly bool _doNotSaveChanges;
        private readonly string _filePath;
        private readonly object _syncRoot = new();
        private Task<ShellFile> _task;

        public FileDetailProvider(string filePath, bool doNotSaveChanges)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath));
            _doNotSaveChanges = doNotSaveChanges;
            _filePath = filePath;
        }

        internal Task<ShellFile> GetShellFileAsync(CancellationToken cancellationToken)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(FileDetailProvider));
            Monitor.Enter(_syncRoot);
            try
            {
                if (_task is null)
                    _task = Task.Run(() => ShellFile.FromFilePath(_filePath) ?? throw new FileNotFoundException($"File not found: {_filePath}", _filePath), cancellationToken);
            }
            finally { Monitor.Exit(_syncRoot); }
            return _task;
        }

        public async Task<Model.IAudioProperties> GetAudioPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemAudio audioProperties = file.Properties.System.Audio;
            if (audioProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            return new AudioPropertiesRecord
            {
                Compression = defaultProperties.Contains(audioProperties.Compression.CanonicalName) ? audioProperties.Compression.Value.TrimmedOrNullIfWhiteSpace() : null,
                EncodingBitrate = defaultProperties.Contains(audioProperties.EncodingBitrate.CanonicalName) ? audioProperties.EncodingBitrate.Value : null,
                Format = defaultProperties.Contains(audioProperties.Format.CanonicalName) ? audioProperties.Format.Value.TrimmedOrNullIfWhiteSpace() : null,
                IsVariableBitrate = defaultProperties.Contains(audioProperties.IsVariableBitrate.CanonicalName) ? audioProperties.IsVariableBitrate.Value : null,
                SampleRate = defaultProperties.Contains(audioProperties.SampleRate.CanonicalName) ? audioProperties.SampleRate.Value : null,
                SampleSize = defaultProperties.Contains(audioProperties.SampleSize.CanonicalName) ? audioProperties.SampleSize.Value : null,
                StreamName = defaultProperties.Contains(audioProperties.StreamName.CanonicalName) ? audioProperties.StreamName.Value.TrimmedOrNullIfWhiteSpace() : null,
                StreamNumber = defaultProperties.Contains(audioProperties.StreamNumber.CanonicalName) ? audioProperties.StreamNumber.Value : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IDocumentProperties> GetDocumentPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemDocument documentProperties = file.Properties.System.Document;
            if (documentProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string[] contributor = defaultProperties.Contains(documentProperties.Contributor.CanonicalName) ? documentProperties.Contributor.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            return new DocumentPropertiesRecord
            {
                ClientID = defaultProperties.Contains(documentProperties.ClientID.CanonicalName) ? documentProperties.ClientID.Value.TrimmedOrNullIfWhiteSpace() : null,
                Contributor = (contributor is null || contributor.Length == 0) ? null : contributor,
                DateCreated = defaultProperties.Contains(documentProperties.DateCreated.CanonicalName) ? documentProperties.DateCreated.Value : null,
                LastAuthor = defaultProperties.Contains(documentProperties.LastAuthor.CanonicalName) ? documentProperties.LastAuthor.Value.TrimmedOrNullIfWhiteSpace() : null,
                RevisionNumber = defaultProperties.Contains(documentProperties.RevisionNumber.CanonicalName) ? documentProperties.RevisionNumber.Value.TrimmedOrNullIfWhiteSpace() : null,
                Security = defaultProperties.Contains(documentProperties.Security.CanonicalName) ? documentProperties.Security.Value : null,
                Division = defaultProperties.Contains(documentProperties.Division.CanonicalName) ? documentProperties.Division.Value.TrimmedOrNullIfWhiteSpace() : null,
                DocumentID = defaultProperties.Contains(documentProperties.DocumentID.CanonicalName) ? documentProperties.DocumentID.Value.TrimmedOrNullIfWhiteSpace() : null,
                Manager = defaultProperties.Contains(documentProperties.Manager.CanonicalName) ? documentProperties.Manager.Value.TrimmedOrNullIfWhiteSpace() : null,
                PresentationFormat = defaultProperties.Contains(documentProperties.PresentationFormat.CanonicalName) ? documentProperties.PresentationFormat.Value.TrimmedOrNullIfWhiteSpace() : null,
                Version = defaultProperties.Contains(documentProperties.Version.CanonicalName) ? documentProperties.Version.Value.TrimmedOrNullIfWhiteSpace() : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IDRMProperties> GetDRMPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemDRM drmProperties = file.Properties.System.DRM;
            if (drmProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            return new DRMPropertiesRecord
            {
                DatePlayExpires = defaultProperties.Contains(drmProperties.DatePlayExpires.CanonicalName) ? drmProperties.DatePlayExpires.Value : null,
                DatePlayStarts = defaultProperties.Contains(drmProperties.DatePlayStarts.CanonicalName) ? drmProperties.DatePlayStarts.Value : null,
                Description = defaultProperties.Contains(drmProperties.Description.CanonicalName) ? drmProperties.Description.Value.TrimmedOrNullIfWhiteSpace() : null,
                IsProtected = defaultProperties.Contains(drmProperties.IsProtected.CanonicalName) ? drmProperties.IsProtected.Value : null,
                PlayCount = defaultProperties.Contains(drmProperties.PlayCount.CanonicalName) ? drmProperties.PlayCount.Value : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IGPSProperties> GetGPSPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemGPS gpsProperties = file.Properties.System.GPS;
            if (gpsProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            double[] latitude = defaultProperties.Contains(gpsProperties.Latitude.CanonicalName) ? gpsProperties.Latitude.Value : null;
            double[] longitude = defaultProperties.Contains(gpsProperties.Longitude.CanonicalName) ? gpsProperties.Longitude.Value : null;
            byte[] versionID = defaultProperties.Contains(gpsProperties.VersionID.CanonicalName) ? gpsProperties.VersionID.Value : null;
            return new GPSPropertiesRecord
            {
                AreaInformation = defaultProperties.Contains(gpsProperties.AreaInformation.CanonicalName) ? gpsProperties.AreaInformation.Value.TrimmedOrNullIfWhiteSpace() : null,
                LatitudeDegrees = (latitude is null || latitude.Length < 1) ? null : latitude[0],
                LatitudeMinutes = (latitude is null || latitude.Length < 2) ? null : latitude[1],
                LatitudeSeconds = (latitude is null || latitude.Length < 3) ? null : latitude[2],
                LatitudeRef = defaultProperties.Contains(gpsProperties.LatitudeRef.CanonicalName) ? gpsProperties.LatitudeRef.Value.TrimmedOrNullIfWhiteSpace() : null,
                LongitudeDegrees = (longitude is null || longitude.Length < 1) ? null : longitude[0],
                LongitudeMinutes = (longitude is null || longitude.Length < 2) ? null : longitude[1],
                LongitudeSeconds = (longitude is null || longitude.Length < 3) ? null : longitude[2],
                LongitudeRef = defaultProperties.Contains(gpsProperties.LongitudeRef.CanonicalName) ? gpsProperties.LongitudeRef.Value.TrimmedOrNullIfWhiteSpace() : null,
                MeasureMode = defaultProperties.Contains(gpsProperties.MeasureMode.CanonicalName) ? gpsProperties.MeasureMode.Value.TrimmedOrNullIfWhiteSpace() : null,
                ProcessingMethod = defaultProperties.Contains(gpsProperties.ProcessingMethod.CanonicalName) ? gpsProperties.ProcessingMethod.Value.TrimmedOrNullIfWhiteSpace() : null,
                VersionID = (versionID is null || versionID.Length == 0) ? null : versionID
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IImageProperties> GetImagePropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemImage imageProperties = file.Properties.System.Image;
            if (imageProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            return new ImagePropertiesRecord
            {
                BitDepth = defaultProperties.Contains(imageProperties.BitDepth.CanonicalName) ? imageProperties.BitDepth.Value : null,
                ColorSpace = defaultProperties.Contains(imageProperties.ColorSpace.CanonicalName) ? imageProperties.ColorSpace.Value : null,
                CompressedBitsPerPixel = defaultProperties.Contains(imageProperties.CompressedBitsPerPixel.CanonicalName) ? imageProperties.CompressedBitsPerPixel.Value : null,
                Compression = defaultProperties.Contains(imageProperties.Compression.CanonicalName) ? imageProperties.Compression.Value : null,
                CompressionText = defaultProperties.Contains(imageProperties.CompressionText.CanonicalName) ? imageProperties.CompressionText.Value.TrimmedOrNullIfWhiteSpace() : null,
                HorizontalResolution = defaultProperties.Contains(imageProperties.HorizontalResolution.CanonicalName) ? imageProperties.HorizontalResolution.Value : null,
                HorizontalSize = defaultProperties.Contains(imageProperties.HorizontalSize.CanonicalName) ? imageProperties.HorizontalSize.Value : null,
                ImageID = defaultProperties.Contains(imageProperties.ImageID.CanonicalName) ? imageProperties.ImageID.Value.TrimmedOrNullIfWhiteSpace() : null,
                ResolutionUnit = defaultProperties.Contains(imageProperties.ResolutionUnit.CanonicalName) ? imageProperties.ResolutionUnit.Value : null,
                VerticalResolution = defaultProperties.Contains(imageProperties.VerticalResolution.CanonicalName) ? imageProperties.VerticalResolution.Value : null,
                VerticalSize = defaultProperties.Contains(imageProperties.VerticalSize.CanonicalName) ? imageProperties.VerticalSize.Value : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IMediaProperties> GetMediaPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemMedia mediaProperties = file.Properties.System.Media;
            if (mediaProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string[] producer = defaultProperties.Contains(mediaProperties.Producer.CanonicalName) ? mediaProperties.Producer.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            string[] writer = defaultProperties.Contains(mediaProperties.Writer.CanonicalName) ? mediaProperties.Writer.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            return new MediaPropertiesRecord
            {
                ContentDistributor = defaultProperties.Contains(mediaProperties.ContentDistributor.CanonicalName) ? mediaProperties.ContentDistributor.Value.TrimmedOrNullIfWhiteSpace() : null,
                CreatorApplication = defaultProperties.Contains(mediaProperties.CreatorApplication.CanonicalName) ? mediaProperties.CreatorApplication.Value.TrimmedOrNullIfWhiteSpace() : null,
                CreatorApplicationVersion = defaultProperties.Contains(mediaProperties.CreatorApplicationVersion.CanonicalName) ? mediaProperties.CreatorApplicationVersion.Value.TrimmedOrNullIfWhiteSpace() : null,
                DateReleased = defaultProperties.Contains(mediaProperties.DateReleased.CanonicalName) ? mediaProperties.DateReleased.Value.TrimmedOrNullIfWhiteSpace() : null,
                FrameCount = defaultProperties.Contains(mediaProperties.FrameCount.CanonicalName) ? mediaProperties.FrameCount.Value : null,
                Producer = (producer is null || producer.Length == 0) ? null : producer,
                ProtectionType = defaultProperties.Contains(mediaProperties.ProtectionType.CanonicalName) ? mediaProperties.ProtectionType.Value.TrimmedOrNullIfWhiteSpace() : null,
                ProviderRating = defaultProperties.Contains(mediaProperties.ProviderRating.CanonicalName) ? mediaProperties.ProviderRating.Value.TrimmedOrNullIfWhiteSpace() : null,
                ProviderStyle = defaultProperties.Contains(mediaProperties.ProviderStyle.CanonicalName) ? mediaProperties.ProviderStyle.Value.TrimmedOrNullIfWhiteSpace() : null,
                Publisher = defaultProperties.Contains(mediaProperties.Publisher.CanonicalName) ? mediaProperties.Publisher.Value.TrimmedOrNullIfWhiteSpace() : null,
                Subtitle = defaultProperties.Contains(mediaProperties.Subtitle.CanonicalName) ? mediaProperties.Subtitle.Value.TrimmedOrNullIfWhiteSpace() : null,
                Writer = (writer is null || writer.Length == 0) ? null : writer,
                Year = defaultProperties.Contains(mediaProperties.Year.CanonicalName) ? mediaProperties.Year.Value : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IMusicProperties> GetMusicPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemMusic musicProperties = file.Properties.System.Music;
            if (musicProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string[] artist = defaultProperties.Contains(musicProperties.Artist.CanonicalName) ? musicProperties.Artist.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            string[] composer = defaultProperties.Contains(musicProperties.Composer.CanonicalName) ? musicProperties.Composer.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            string[] conductor = defaultProperties.Contains(musicProperties.Conductor.CanonicalName) ? musicProperties.Conductor.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            string[] genre = defaultProperties.Contains(musicProperties.Genre.CanonicalName) ? musicProperties.Genre.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            return new MusicPropertiesRecord
            {
                AlbumArtist = defaultProperties.Contains(musicProperties.AlbumArtist.CanonicalName) ? musicProperties.AlbumArtist.Value.TrimmedOrNullIfWhiteSpace() : null,
                AlbumTitle = defaultProperties.Contains(musicProperties.AlbumTitle.CanonicalName) ? musicProperties.AlbumTitle.Value.TrimmedOrNullIfWhiteSpace() : null,
                Artist = (artist is null || artist.Length == 0) ? null : artist,
                Composer = (composer is null || composer.Length == 0) ? null : composer,
                Conductor = (conductor is null || conductor.Length == 0) ? null : conductor,
                DisplayArtist = defaultProperties.Contains(musicProperties.DisplayArtist.CanonicalName) ? musicProperties.DisplayArtist.Value.TrimmedOrNullIfWhiteSpace() : null,
                Genre = (genre is null || genre.Length == 0) ? null : genre,
                PartOfSet = defaultProperties.Contains(musicProperties.PartOfSet.CanonicalName) ? musicProperties.PartOfSet.Value.TrimmedOrNullIfWhiteSpace() : null,
                Period = defaultProperties.Contains(musicProperties.Period.CanonicalName) ? musicProperties.Period.Value.TrimmedOrNullIfWhiteSpace() : null,
                TrackNumber = defaultProperties.Contains(musicProperties.TrackNumber.CanonicalName) ? musicProperties.TrackNumber.Value : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IPhotoProperties> GetPhotoPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemPhoto photoProperties = file.Properties.System.Photo;
            if (photoProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string[] evt = defaultProperties.Contains(photoProperties.Event.CanonicalName) ? photoProperties.Event.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            string[] peopleNames = defaultProperties.Contains(photoProperties.PeopleNames.CanonicalName) ? photoProperties.PeopleNames.Value?
                .NonEmptyTrimmedElements().ToArray() : null;
            return new PhotoPropertiesRecord
            {
                CameraManufacturer = defaultProperties.Contains(photoProperties.CameraManufacturer.CanonicalName) ? photoProperties.CameraManufacturer.Value.TrimmedOrNullIfWhiteSpace() : null,
                CameraModel = defaultProperties.Contains(photoProperties.CameraModel.CanonicalName) ? photoProperties.CameraModel.Value.TrimmedOrNullIfWhiteSpace() : null,
                DateTaken = defaultProperties.Contains(photoProperties.DateTaken.CanonicalName) ? photoProperties.DateTaken.Value : null,
                Event = (evt is null || evt.Length == 0) ? null : evt,
                EXIFVersion = defaultProperties.Contains(photoProperties.EXIFVersion.CanonicalName) ? photoProperties.EXIFVersion.Value.TrimmedOrNullIfWhiteSpace() : null,
                Orientation = defaultProperties.Contains(photoProperties.Orientation.CanonicalName) ? photoProperties.Orientation.Value : null,
                OrientationText = defaultProperties.Contains(photoProperties.OrientationText.CanonicalName) ? photoProperties.OrientationText.Value.TrimmedOrNullIfWhiteSpace() : null,
                PeopleNames = (peopleNames is null || peopleNames.Length == 0) ? null : peopleNames
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IRecordedTVProperties> GetRecordedTVPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemRecordedTV recordedTVProperties = file.Properties.System.RecordedTV;
            if (recordedTVProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            return new RecordedTVPropertiesRecord
            {
                ChannelNumber = defaultProperties.Contains(recordedTVProperties.ChannelNumber.CanonicalName) ? recordedTVProperties.ChannelNumber.Value : null,
                EpisodeName = defaultProperties.Contains(recordedTVProperties.EpisodeName.CanonicalName) ? recordedTVProperties.EpisodeName.Value.TrimmedOrNullIfWhiteSpace() : null,
                IsDTVContent = defaultProperties.Contains(recordedTVProperties.IsDTVContent.CanonicalName) ? recordedTVProperties.IsDTVContent.Value : null,
                IsHDContent = defaultProperties.Contains(recordedTVProperties.IsHDContent.CanonicalName) ? recordedTVProperties.IsHDContent.Value : null,
                NetworkAffiliation = defaultProperties.Contains(recordedTVProperties.NetworkAffiliation.CanonicalName) ? recordedTVProperties.NetworkAffiliation.Value.TrimmedOrNullIfWhiteSpace() : null,
                OriginalBroadcastDate = defaultProperties.Contains(recordedTVProperties.OriginalBroadcastDate.CanonicalName) ? recordedTVProperties.OriginalBroadcastDate.Value : null,
                ProgramDescription = defaultProperties.Contains(recordedTVProperties.ProgramDescription.CanonicalName) ? recordedTVProperties.ProgramDescription.Value.TrimmedOrNullIfWhiteSpace() : null,
                StationCallSign = defaultProperties.Contains(recordedTVProperties.StationCallSign.CanonicalName) ? recordedTVProperties.StationCallSign.Value.TrimmedOrNullIfWhiteSpace() : null,
                StationName = defaultProperties.Contains(recordedTVProperties.StationName.CanonicalName) ? recordedTVProperties.StationName.Value.TrimmedOrNullIfWhiteSpace() : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.ISummaryProperties> GetSummaryPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystem summaryProperties = file.Properties.System;
            ShellProperties.PropertySystemSoftware softwareProperties = file.Properties.System.Software;
            if (summaryProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string[] author = defaultProperties.Contains(summaryProperties.Author.CanonicalName) ? summaryProperties.Author.Value?
                    .NonEmptyTrimmedElements().ToArray() : null;
            string[] keywords = defaultProperties.Contains(summaryProperties.Keywords.CanonicalName) ? summaryProperties.Keywords.Value?
                    .NonEmptyTrimmedElements().ToArray() : null;
            string[] itemAuthors = defaultProperties.Contains(summaryProperties.ItemAuthors.CanonicalName) ? summaryProperties.ItemAuthors.Value?
                    .NonEmptyTrimmedElements().ToArray() : null;
            string[] kind = defaultProperties.Contains(summaryProperties.Kind.CanonicalName) ? summaryProperties.Kind.Value?
                    .NonEmptyTrimmedElements().ToArray() : null;
            return new SummaryPropertiesRecord
            {
                ApplicationName = defaultProperties.Contains(summaryProperties.ApplicationName.CanonicalName) ? summaryProperties.ApplicationName.Value.TrimmedOrNullIfWhiteSpace() : null,
                Author = (author is null || author.Length == 0) ? null : author,
                Comment = defaultProperties.Contains(summaryProperties.Comment.CanonicalName) ? summaryProperties.Comment.Value.TrimmedOrNullIfWhiteSpace() : null,
                Keywords = (keywords is null || keywords.Length == 0) ? null : keywords,
                Subject = defaultProperties.Contains(summaryProperties.Subject.CanonicalName) ? summaryProperties.Subject.Value.TrimmedOrNullIfWhiteSpace() : null,
                Title = defaultProperties.Contains(summaryProperties.Title.CanonicalName) ? summaryProperties.Title.Value.TrimmedOrNullIfWhiteSpace() : null,
                FileDescription = defaultProperties.Contains(summaryProperties.FileDescription.CanonicalName) ? summaryProperties.FileDescription.Value.TrimmedOrNullIfWhiteSpace() : null,
                FileVersion = defaultProperties.Contains(summaryProperties.FileVersion.CanonicalName) ? summaryProperties.FileVersion.Value.TrimmedOrNullIfWhiteSpace() : null,
                Company = defaultProperties.Contains(summaryProperties.Company.CanonicalName) ? summaryProperties.Company.Value.TrimmedOrNullIfWhiteSpace() : null,
                ContentType = defaultProperties.Contains(summaryProperties.ContentType.CanonicalName) ? summaryProperties.ContentType.Value.TrimmedOrNullIfWhiteSpace() : null,
                Copyright = defaultProperties.Contains(summaryProperties.Copyright.CanonicalName) ? summaryProperties.Copyright.Value.TrimmedOrNullIfWhiteSpace() : null,
                ParentalRating = defaultProperties.Contains(summaryProperties.ParentalRating.CanonicalName) ? summaryProperties.ParentalRating.Value.TrimmedOrNullIfWhiteSpace() : null,
                Rating = defaultProperties.Contains(summaryProperties.Rating.CanonicalName) ? summaryProperties.Rating.Value : null,
                ItemAuthors = (itemAuthors is null || itemAuthors.Length == 0) ? null : itemAuthors,
                ItemType = defaultProperties.Contains(summaryProperties.ItemType.CanonicalName) ? summaryProperties.ItemType.Value.TrimmedOrNullIfWhiteSpace() : null,
                ItemTypeText = defaultProperties.Contains(summaryProperties.ItemTypeText.CanonicalName) ? summaryProperties.ItemTypeText.Value.TrimmedOrNullIfWhiteSpace() : null,
                Kind = (kind is null || kind.Length == 0) ? null : kind,
                MIMEType = defaultProperties.Contains(summaryProperties.MIMEType.CanonicalName) ? summaryProperties.MIMEType.Value.TrimmedOrNullIfWhiteSpace() : null,
                ParentalRatingReason = defaultProperties.Contains(summaryProperties.ParentalRatingReason.CanonicalName) ? summaryProperties.ParentalRatingReason.Value.TrimmedOrNullIfWhiteSpace() : null,
                ParentalRatingsOrganization = defaultProperties.Contains(summaryProperties.ParentalRatingsOrganization.CanonicalName) ? summaryProperties.ParentalRatingsOrganization.Value.TrimmedOrNullIfWhiteSpace() : null,
                Sensitivity = defaultProperties.Contains(summaryProperties.Sensitivity.CanonicalName) ? summaryProperties.Sensitivity.Value : null,
                SensitivityText = defaultProperties.Contains(summaryProperties.SensitivityText.CanonicalName) ? summaryProperties.SensitivityText.Value.TrimmedOrNullIfWhiteSpace() : null,
                SimpleRating = defaultProperties.Contains(summaryProperties.SimpleRating.CanonicalName) ? summaryProperties.SimpleRating.Value : null,
                Trademarks = defaultProperties.Contains(summaryProperties.Trademarks.CanonicalName) ? summaryProperties.Trademarks.Value.TrimmedOrNullIfWhiteSpace() : null,
                ProductName = defaultProperties.Contains(softwareProperties.ProductName.CanonicalName) ? softwareProperties.ProductName.Value.TrimmedOrNullIfWhiteSpace() : null
            }.NullIfPropertiesEmpty();
        }

        public async Task<Model.IVideoProperties> GetVideoPropertiesAsync(CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemVideo videoProperties = file.Properties.System.Video;
            if (videoProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string[] director = defaultProperties.Contains(videoProperties.Director.CanonicalName) ? videoProperties.Director.Value?
                    .NonEmptyTrimmedElements().ToArray() : null;
            return new VideoPropertiesRecord
            {
                Compression = defaultProperties.Contains(videoProperties.Compression.CanonicalName) ? videoProperties.Compression.Value.TrimmedOrNullIfWhiteSpace() : null,
                Director = (director is null || director.Length == 0) ? null : director,
                EncodingBitrate = defaultProperties.Contains(videoProperties.EncodingBitrate.CanonicalName) ? videoProperties.EncodingBitrate.Value : null,
                FrameHeight = defaultProperties.Contains(videoProperties.FrameHeight.CanonicalName) ? videoProperties.FrameHeight.Value : null,
                FrameRate = defaultProperties.Contains(videoProperties.FrameRate.CanonicalName) ? videoProperties.FrameRate.Value : null,
                FrameWidth = defaultProperties.Contains(videoProperties.FrameWidth.CanonicalName) ? videoProperties.FrameWidth.Value : null,
                HorizontalAspectRatio = defaultProperties.Contains(videoProperties.HorizontalAspectRatio.CanonicalName) ? videoProperties.HorizontalAspectRatio.Value : null,
                StreamName = defaultProperties.Contains(videoProperties.StreamName.CanonicalName) ? videoProperties.StreamName.Value.TrimmedOrNullIfWhiteSpace() : null,
                StreamNumber = defaultProperties.Contains(videoProperties.StreamNumber.CanonicalName) ? videoProperties.StreamNumber.Value : null,
                VerticalAspectRatio = defaultProperties.Contains(videoProperties.VerticalAspectRatio.CanonicalName) ? videoProperties.VerticalAspectRatio.Value : null,
            }.NullIfPropertiesEmpty();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    _ = _task.ContinueWith(t =>
                      {
                          if (t.IsCompletedSuccessfully)
                              t.Result.Dispose();
                      });
                _isDisposed = true;
            }
        }

        ~FileDetailProvider()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
