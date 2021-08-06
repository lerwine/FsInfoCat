using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public static class LocalExtensions
    {
        public static async Task<ILogicalDiskInfo> GetLogicalDiskAsync(this IFileSystemDetailService service, DirectoryInfo directoryInfo, CancellationToken cancellationToken)
        {
            Uri uri = new Uri(((directoryInfo.Parent is null) ? directoryInfo : directoryInfo.Root).FullName);
            string path = uri.AbsoluteUri;
            path = (path.EndsWith('/') ? new Uri(path.Substring(0, path.Length - 1)) : uri).LocalPath;
            StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
            if (uri.IsUnc)
                return (await service.GetLogicalDisksAsync(cancellationToken)).FirstOrDefault(d => comparer.Equals(d.ProviderName, path));
            return (await service.GetLogicalDisksAsync(cancellationToken)).FirstOrDefault(d => comparer.Equals(d.GetRootPath(), path));
        }

        public static string GetRootPath(this ILogicalDiskInfo logicalDisk) => (logicalDisk is null) ? null : string.IsNullOrEmpty(logicalDisk.RootDirectory?.Name) ? logicalDisk.Name : logicalDisk.RootDirectory.Name;

        public static bool TryGetVolumeIdentifier(this ILogicalDiskInfo logicalDiskInfo, out VolumeIdentifier result)
        {
            if (logicalDiskInfo is null)
            {
                result = VolumeIdentifier.Empty;
                return false;
            }

            if (logicalDiskInfo.DriveType == DriveType.Network && (Uri.TryCreate(logicalDiskInfo.ProviderName, UriKind.Absolute, out Uri uri) || Uri.TryCreate(logicalDiskInfo., UriKind.Absolute, out uri)) && uri.IsUnc)
            {
                result = new VolumeIdentifier(uri);
                return true;
            }
            return VolumeIdentifier.TryParse(logicalDiskInfo.VolumeSerialNumber, out result);
        }
        public static async Task<VolumeIdentifier?> TryGetVolumeIdentifierAsync(this DirectoryInfo directoryInfo, CancellationToken cancellationToken)
        {
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            ILogicalDiskInfo disk = await serviceScope.ServiceProvider.GetRequiredService<IFileSystemDetailService>().GetLogicalDiskAsync(directoryInfo, cancellationToken);//mucinex dm
            if (disk is null)
            {
                Uri uri = new Uri(((directoryInfo.Parent is null) ? directoryInfo : directoryInfo.Root).FullName, UriKind.Absolute);
                if (uri.IsUnc)
                    return new VolumeIdentifier(uri);
                return null;
            }
            return disk.TryGetVolumeIdentifier(out VolumeIdentifier result) ? (VolumeIdentifier?)result: null;
        }
    }
}
