using FsInfoCat.Local;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public class FileSystemDetailService : IFileSystemDetailService
    {
        [ServiceBuilderHandler]
#pragma warning disable IDE0051 // Remove unused private members
        private static void ConfigureServices(IServiceCollection services) => services.AddSingleton<FileSystemDetailService>();
#pragma warning restore IDE0051 // Remove unused private members

        public IFileDetailProvider CreateFileDetailProvider(string filePath, bool doNotSaveChanges) => new FileDetailProvider(filePath, doNotSaveChanges);

        public async Task<ILogicalDiskInfo> GetLogicalDiskAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken) => await WMI.Win32_LogicalDisk.GetLogicalDiskAsync(directoryInfo, cancellationToken);

        public async Task<ILogicalDiskInfo[]> GetLogicalDisksAsync(CancellationToken cancellationToken) => (await WMI.Win32_LogicalDisk.GetLogicalDisksAsync(cancellationToken)).Cast<ILogicalDiskInfo>().ToArray();
    }
}
