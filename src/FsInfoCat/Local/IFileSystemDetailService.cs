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
        /// <param name="doNotSaveChanges">if set to <see langword="true"/> new items are not saved to the database; otherwise, <see langword="false"/> to save new items to
        /// the database.</param>
        /// <returns>An <see cref="IFileDetailProvider"/> instance for the specified file.</returns>
        IFileDetailProvider CreateFileDetailProvider(string filePath, bool doNotSaveChanges);

        /// <summary>
        /// Gets the system logical disks asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Any array of <see cref="ILogicalDiskInfo"/> objects that represent the logical disks found on the current host system.</returns>
        Task<ILogicalDiskInfo[]> GetLogicalDisksAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the generic network share filesystem information.
        /// </summary>
        /// <returns>An <see cref="IVolume"/> object that represents the fallback network share file system type.</returns>
        (IFileSystemProperties Properties, string SymbolicName) GetGenericNetworkShareFileSystem();
    }
}
