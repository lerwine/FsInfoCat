using M = FsInfoCat.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    // TODO: Document LocalDbExtensions class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class LocalDbExtensions
    {
        [Obsolete("Use GetDirectoryInfoAsync(IFileSystemDetailService, Model.Volume, CancellationToken)")]
        private static async Task<DirectoryInfo> GetDirectoryInfoAsync([DisallowNull] IFileSystemDetailService fileSystemDetailService, Volume target, CancellationToken cancellationToken)
        {
            if (target is null)
                return null;

            VolumeIdentifier volumeIdentifier = target.Identifier;
            string path = (await (fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService))).GetLogicalDisksAsync(cancellationToken)).FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volumeIdentifier))?.GetRootPath();
            return string.IsNullOrEmpty(path) ? null : new DirectoryInfo(path);
        }

        private static async Task<DirectoryInfo> GetDirectoryInfoAsync([DisallowNull] IFileSystemDetailService fileSystemDetailService, Model.Volume target, CancellationToken cancellationToken)
        {
            if (target is null)
                return null;

            M.VolumeIdentifier volumeIdentifier = target.Identifier;
            string path = (await (fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService))).GetLogicalDisksAsync(cancellationToken)).FirstOrDefault(d => d.TryGetVolumeIdentifier(out VolumeIdentifier vid) && vid.Equals(volumeIdentifier))?.GetRootPath();
            return string.IsNullOrEmpty(path) ? null : new DirectoryInfo(path);
        }

        [Obsolete("Use GetDirectoryInfoAsync(Model.Volume, IFileSystemDetailService, CancellationToken)")]
        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this Volume target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken) =>
            (target is null) ? null : await GetDirectoryInfoAsync(fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), target, cancellationToken);

        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this Model.Volume target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken) =>
            (target is null) ? null : await GetDirectoryInfoAsync(fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), target, cancellationToken);

        [Obsolete("Use GetDirectoryInfoAsync(Model.Subdirectory, IFileSystemDetailService, Model.LocalDbContext)")]
        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this Subdirectory target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] LocalDbContext dbContext,
            CancellationToken cancellationToken) =>
                (target is null) ? null : await GetDirectoryInfoAsync(dbContext.Entry(target), fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), cancellationToken);

        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this Model.Subdirectory target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] Model.LocalDbContext dbContext,
            CancellationToken cancellationToken) =>
                (target is null) ? null : await GetDirectoryInfoAsync(dbContext.Entry(target), fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), cancellationToken);

        [Obsolete("Use GetDirectoryInfoAsync(EntityEntry<Model.Subdirectory> target, IFileSystemDetailService, CancellationToken")]
        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this EntityEntry<Subdirectory> target, [DisallowNull] IFileSystemDetailService fileSystemDetailService, CancellationToken cancellationToken) =>
            (target is null) ? null : await GetDirectoryInfoAsync(target, fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), cancellationToken);

        public static async Task<DirectoryInfo> GetDirectoryInfoAsync(this EntityEntry<Model.Subdirectory> target, [DisallowNull] IFileSystemDetailService fileSystemDetailService,
            CancellationToken cancellationToken) =>
                (target is null) ? null : await GetDirectoryInfoAsync(target, fileSystemDetailService ?? throw new ArgumentNullException(nameof(fileSystemDetailService)), cancellationToken);

        [Obsolete("Use GetDirectoryInfoAsync(IFileSystemDetailService, EntityEntry<Model.Subdirectory>, CancellationToken")]
        private static async Task<DirectoryInfo> GetDirectoryInfoAsync([DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] EntityEntry<Subdirectory> target,
            CancellationToken cancellationToken)
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

        private static async Task<DirectoryInfo> GetDirectoryInfoAsync([DisallowNull] IFileSystemDetailService fileSystemDetailService, [DisallowNull] EntityEntry<Model.Subdirectory> target,
            CancellationToken cancellationToken)
        {
            EntityEntry<Model.Subdirectory> parent = await target.GetRelatedTargetEntryAsync(d => d.Parent, cancellationToken);
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
