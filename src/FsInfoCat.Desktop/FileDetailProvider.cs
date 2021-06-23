using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        private readonly object _syncRoot = new object();
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

        public async Task<EntityEntry<AudioPropertySet>> GetAudioPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemAudio audioProperties = file.Properties.System.Audio;
            if (audioProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            string compression = defaultProperties.Contains(audioProperties.Compression.CanonicalName) ? audioProperties.Compression.Value.NullIfWhiteSpace() : null;
            uint? encodingBitrate = defaultProperties.Contains(audioProperties.EncodingBitrate.CanonicalName) ? audioProperties.EncodingBitrate.Value : null;
            string format = defaultProperties.Contains(audioProperties.Format.CanonicalName) ? audioProperties.Format.Value.NullIfWhiteSpace() : null;
            bool? isVariableBitrate = defaultProperties.Contains(audioProperties.IsVariableBitrate.CanonicalName) ? audioProperties.IsVariableBitrate.Value : null;
            uint? sampleRate = defaultProperties.Contains(audioProperties.SampleRate.CanonicalName) ? audioProperties.SampleRate.Value : null;
            uint? sampleSize = defaultProperties.Contains(audioProperties.SampleSize.CanonicalName) ? audioProperties.SampleSize.Value : null;
            string streamName = defaultProperties.Contains(audioProperties.StreamName.CanonicalName) ? audioProperties.StreamName.Value.NullIfWhiteSpace() : null;
            ushort? streamNumber = defaultProperties.Contains(audioProperties.StreamNumber.CanonicalName) ? audioProperties.StreamNumber.Value : null;
            if (compression is null && format is null && streamName is null && !(encodingBitrate.HasValue || isVariableBitrate.HasValue || sampleRate.HasValue || sampleSize.HasValue || streamNumber.HasValue))
                return null;
            AudioPropertySet propertySet = dbContext.AudioPropertySets.FirstOrDefault(a => a.Compression == compression && a.EncodingBitrate == encodingBitrate && a.Format == format && a.IsVariableBitrate == isVariableBitrate &&
                a.SampleRate == sampleRate && a.SampleSize == sampleSize && a.StreamName == streamName && a.StreamNumber == streamNumber);
            if (propertySet is null)
            {
                propertySet = new AudioPropertySet
                {
                    Compression = compression,
                    EncodingBitrate = encodingBitrate,
                    Format = format,
                    IsVariableBitrate = isVariableBitrate,
                    SampleRate = sampleRate,
                    SampleSize = sampleSize,
                    StreamName = streamName,
                    StreamNumber = streamNumber
                };
                EntityEntry<AudioPropertySet> entry = dbContext.AudioPropertySets.Add(propertySet);
                if (!_doNotSaveChanges)
                    await dbContext.SaveChangesAsync(cancellationToken);
                return entry;
            }
            return dbContext.Entry(propertySet);
        }

        public async Task<EntityEntry<DocumentPropertySet>> GetDocumentPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemDocument documentProperties = file.Properties.System.Document;
            if (documentProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetDocumentPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<DRMPropertySet>> GetDRMPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemDRM drmProperties = file.Properties.System.DRM;
            if (drmProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetDRMPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<GPSPropertySet>> GetGPSPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemGPS gpsProperties = file.Properties.System.GPS;
            if (gpsProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetGPSPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<ImagePropertySet>> GetImagePropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemImage imageProperties = file.Properties.System.Image;
            if (imageProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetImagePropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<MediaPropertySet>> GetMediaPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemMedia mediaProperties = file.Properties.System.Media;
            if (mediaProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetMediaPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<MusicPropertySet>> GetMusicPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemMusic musicProperties = file.Properties.System.Music;
            if (musicProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetMusicPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<PhotoPropertySet>> GetPhotoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemPhoto photoProperties = file.Properties.System.Photo;
            if (photoProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetAudioPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<RecordedTVPropertySet>> GetRecordedTVPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemRecordedTV recordedTVProperties = file.Properties.System.RecordedTV;
            if (recordedTVProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetRecordedTVPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<SummaryPropertySet>> GetSummaryPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystem summaryProperties = file.Properties.System;
            if (summaryProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetSummaryPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        public async Task<EntityEntry<VideoPropertySet>> GetVideoPropertySetAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            ShellFile file = await GetShellFileAsync(cancellationToken);
            ShellProperties.PropertySystemVideo videoProperties = file.Properties.System.Video;
            if (videoProperties is null)
                return null;
            ShellPropertyCollection defaultProperties = file.Properties.DefaultPropertyCollection;
            // TODO: Implement GetVideoPropertySetAsync(LocalDbContext, CancellationToken)
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _isDisposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileDetailProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
