using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public static class LocalDbExtensions
    {
        public static ISimpleIdentityReference<Subdirectory> GetParentReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.Parent ?? IdentityReference<Subdirectory>.FromId(file.ParentId);

        public static ISimpleIdentityReference<AudioPropertySet> GetAudioPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.AudioProperties ?? file.AudioPropertySetId.ToIdentityReference<AudioPropertySet>();

        public static ISimpleIdentityReference<SummaryPropertySet> GetSummaryPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.SummaryProperties ?? file.SummaryPropertySetId.ToIdentityReference<SummaryPropertySet>();

        public static ISimpleIdentityReference<ImagePropertySet> GetImagePropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.ImageProperties ?? file.ImagePropertySetId.ToIdentityReference<ImagePropertySet>();

        public static ISimpleIdentityReference<MusicPropertySet> GetMusicPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.MusicProperties ?? file.MusicPropertySetId.ToIdentityReference<MusicPropertySet>();

        public static ISimpleIdentityReference<RecordedTVPropertySet> GetRecordedTVPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.RecordedTVProperties ?? file.RecordedTVPropertySetId.ToIdentityReference<RecordedTVPropertySet>();

        public static ISimpleIdentityReference<VideoPropertySet> GetVideoPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.VideoProperties ?? file.VideoPropertySetId.ToIdentityReference<VideoPropertySet>();

        public static ISimpleIdentityReference<PhotoPropertySet> GetPhotoPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.PhotoProperties ?? file.PhotoPropertySetId.ToIdentityReference<PhotoPropertySet>();

        public static ISimpleIdentityReference<MediaPropertySet> GetMediaPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.MediaProperties ?? file.MediaPropertySetId.ToIdentityReference<MediaPropertySet>();

        public static ISimpleIdentityReference<GPSPropertySet> GetGPSPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.GPSProperties ?? file.GPSPropertySetId.ToIdentityReference<GPSPropertySet>();

        public static ISimpleIdentityReference<DRMPropertySet> GetDRMPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.DRMProperties ?? file.DRMPropertySetId.ToIdentityReference<DRMPropertySet>();

        public static ISimpleIdentityReference<DocumentPropertySet> GetDocumentPropertySetReference([AllowNull] this DbFile file) =>
            (file is null) ? null : file.DocumentProperties ?? file.DocumentPropertySetId.ToIdentityReference<DocumentPropertySet>();

        private static async Task<DirectoryInfo> GetDirectoryInfoAsync([DisallowNull] IFileSystemDetailService fileSystemDetailService, Volume target, CancellationToken cancellationToken)
        {
            if (target is null)
                return null;

            VolumeIdentifier volumeIdentifier = target.Identifier;
            string path = (await (fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService))).GetLogicalDisksAsync(cancellationToken)).FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volumeIdentifier))?.GetRootPath();
            return string.IsNullOrEmpty(path) ? null : new DirectoryInfo(path);
        }

        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this Volume target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken) =>
            (target is null) ? null : await GetDirectoryInfoAsync(fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), target, cancellationToken);

        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this Subdirectory target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken) =>
            (target is null) ? null : await GetDirectoryInfoAsync(dbContext.Entry(target), fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), cancellationToken);

        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this EntityEntry<Subdirectory> target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken) =>
            (target is null) ? null : await GetDirectoryInfoAsync(target, fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), cancellationToken);

        private static async Task<DirectoryInfo> GetDirectoryInfoAsync([DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] EntityEntry<Subdirectory> target, CancellationToken cancellationToken)
        {
            EntityEntry<Subdirectory> parent = await target.GetRelatedTargetEntryAsync(d => d.Parent, cancellationToken);
            if (parent is null)
                return await (await target.GetRelatedReferenceAsync(d => d.Volume, cancellationToken)).GetDirectoryInfoAsync(fileSystemDetailService, cancellationToken);
            DirectoryInfo directoryInfo = await GetDirectoryInfoAsync(fileSystemDetailService, parent, cancellationToken);
            if (directoryInfo is null)
                return null;
            string name = target.Entity.Name;
            StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
            DirectoryInfo[] matching = directoryInfo.GetDirectories().Where(d => comparer.Equals(d.Name, name)).ToArray();
            if (matching.Length == 0)
                return new DirectoryInfo(Path.Combine(directoryInfo.FullName, name));
            return (matching.Length > 1) ? (matching.FirstOrDefault(m => m.Name == name) ?? new DirectoryInfo(Path.Combine(directoryInfo.FullName, name))) : matching[0];
        }
    }
}
