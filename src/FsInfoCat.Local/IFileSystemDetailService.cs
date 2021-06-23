using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Interface for service which retrieves file system details.
    /// </summary>
    public interface IFileSystemDetailService
    {
        /// <summary>
        /// Creates a new file detail provider.
        /// </summary>
        /// <param name="filePath">The path of the target file.</param>
        /// <param name="doNotSaveChanges">if set to <see langword="true"/> new items are not saved to the database; otherwise, <see langword="false"/> to save new items to the database.</param>
        /// <returns>A <see cref="IFileDetailProvider"/> instance for the specified file.</returns>
        IFileDetailProvider CreateFileDetailProvider(string filePath, bool doNotSaveChanges);

        Task<ILogicalDiskInfo> GetLogicalDiskAsync(DirectoryInfo directoryInfo, CancellationToken cancellationToken);

        Task<ILogicalDiskInfo[]> GetLogicalDisksAsync(CancellationToken cancellationToken);
    }
}
