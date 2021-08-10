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
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileSystemDetailService, FileSystemDetailService>();
        }
#pragma warning restore IDE0051 // Remove unused private members

        public IFileDetailProvider CreateFileDetailProvider(string filePath, bool doNotSaveChanges) => new FileDetailProvider(filePath, doNotSaveChanges);

        public (IFileSystemProperties Properties, string SymbolicName) GetGenericNetworkShareFileSystem() => (new FileSystemRecord()
        {
            DisplayName = "Network File Share",
            ReadOnly = Properties.Settings.Default.GenericNfsTypeReadOnly,
            MaxNameLength = Properties.Settings.Default.GenericNfsTypeMaxNameLength,
            DefaultDriveType = DriveType.Network
        }, Properties.Settings.Default.GenericNfsTypeSymboicName);

        public async Task<ILogicalDiskInfo[]> GetLogicalDisksAsync(CancellationToken cancellationToken) => (await WMI.Win32_LogicalDisk.GetLogicalDisksAsync(cancellationToken)).Cast<ILogicalDiskInfo>().ToArray();
    }
}
